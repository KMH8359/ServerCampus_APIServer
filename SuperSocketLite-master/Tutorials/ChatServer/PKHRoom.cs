using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessagePack;

using CSBaseLib;


namespace ChatServer
{
    public class PKHRoom : PKHandler
    {
        List<Room> RoomList = null;
        int StartRoomNumber;
        
        public void SetRoomList(List<Room> roomList)
        {
            RoomList = roomList;
            StartRoomNumber = roomList[0].Number;
        }

        public void RegistPacketHandler(Dictionary<int, Action<ServerPacketData>> packetHandlerMap)
        {
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_ENTER, RequestRoomEnter);
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_LEAVE, RequestLeave);
            // NTF_IN_ROOM_LEAVE의 용도 : 방에서 나가지 않았는데 접속을 끊으면 방에서 내보내줘야함. 서버에서 이를 감지한다.
            // 네트워크 처리 로직과 패킷 처리 로직을 두 개의 스레드로 분리하기 위해 NTF_IN_ROOM_LEAVE라는 별도의 패킷을 줘서 패킷 처리로 내보내는 것
            packetHandlerMap.Add((int)PACKETID.NTF_IN_ROOM_LEAVE, NotifyLeaveInternal); 
            packetHandlerMap.Add((int)PACKETID.REQ_ROOM_CHAT, RequestChat);
        }


        Room GetRoom(int roomNumber)
        {
            var index = roomNumber - StartRoomNumber;

            if( index < 0 || index >= RoomList.Count())
            {
                return null;
            }

            return RoomList[index];
        }
                
        (bool, Room, RoomUser) CheckRoomAndRoomUser(string userNetSessionID)    // 방을 찾았는지 여부, 방 객체, 그 방에 속한 유저 총 3개를 튜플로 반환한다. 이들은 item1, item2, item3 필드로 접근할 수 있음
        {
            var user = UserMgr.GetUser(userNetSessionID);
            if (user == null)
            {
                return (false, null, null);
            }

            var roomNumber = user.RoomNumber;
            var room = GetRoom(roomNumber);

            if(room == null)
            {
                return (false, null, null);
            }

            var roomUser = room.GetUserByNetSessionId(userNetSessionID);    // room의 멤버 함수를 통해 해당 유저가 그 룸 안에 있는지 다시 한번 검증

            if (roomUser == null)
            {
                return (false, room, null);
            }

            return (true, room, roomUser);
        }



        public void RequestRoomEnter(ServerPacketData packetData)   
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("RequestRoomEnter");

            try
            {
                var user = UserMgr.GetUser(sessionID);
                // User 유효성 검증
                if (user == null || user.IsConfirm(sessionID) == false)
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_USER, sessionID);
                    return;
                }

                if (user.IsStateRoom())
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_STATE, sessionID);
                    return;
                }

                var reqData = MessagePackSerializer.Deserialize<PKTReqRoomEnter>(packetData.BodyData);
                
                var room = GetRoom(reqData.RoomNumber);
                // room 유효성 검증
                if (room == null)
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_INVALID_ROOM_NUMBER, sessionID);
                    return;
                }

                if (room.AddUser(user.ID(), sessionID) == false)
                {
                    ResponseEnterRoomToClient(ERROR_CODE.ROOM_ENTER_FAIL_ADD_USER, sessionID);
                    return;
                }


                user.EnteredRoom(reqData.RoomNumber);   // User에 해당 Room넘버 내부필드로 등록

                room.NotifyPacketUserList(sessionID);                  // 이미 방 안에 있던 유저들 정보를 신규 유저에게 보내줘야 함
                room.NofifyPacketNewUser(sessionID, user.ID());     // 이미 방 안에 있던 유저들에게 신규 유저의 정보를 보내줘야 함

                ResponseEnterRoomToClient(ERROR_CODE.NONE, sessionID);  // 룸 입장이 성공했음을 알리는 패킷

                MainServer.MainLogger.Debug("RequestEnterInternal - Success");
            }
            catch (Exception ex)
            {
                MainServer.MainLogger.Error(ex.ToString());
            }
        }

        void ResponseEnterRoomToClient(ERROR_CODE errorCode, string sessionID)
        {
            var resRoomEnter = new PKTResRoomEnter()
            {
                Result = (short)errorCode
            };

            var bodyData = MessagePackSerializer.Serialize(resRoomEnter);
            var sendData = PacketToBytes.Make(PACKETID.RES_ROOM_ENTER, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void RequestLeave(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("로그아웃 요청 받음");

            try
            {
                var user = UserMgr.GetUser(sessionID);
                if(user == null)
                {
                    return;
                }

                if(LeaveRoomUser(sessionID, user.RoomNumber) == false)
                {
                    return;
                }

                user.LeaveRoom();

                ResponseLeaveRoomToClient(sessionID);

                MainServer.MainLogger.Debug("Room RequestLeave - Success");
            }
            catch (Exception ex)
            {
                MainServer.MainLogger.Error(ex.ToString());
            }
        }

        bool LeaveRoomUser(string sessionID, int roomNumber)
        {
            MainServer.MainLogger.Debug($"LeaveRoomUser. SessionID:{sessionID}");

            var room = GetRoom(roomNumber);
            if (room == null)
            {
                return false;
            }

            var roomUser = room.GetUserByNetSessionId(sessionID);
            if (roomUser == null)
            {
                return false;
            }
                        
            var userID = roomUser.UserID;
            room.RemoveUser(roomUser);

            room.NotifyPacketLeaveUser(userID);
            return true;
        }

        void ResponseLeaveRoomToClient(string sessionID)
        {
            var resRoomLeave = new PKTResRoomLeave()
            {
                Result = (short)ERROR_CODE.NONE
            };

            var bodyData = MessagePackSerializer.Serialize(resRoomLeave);
            var sendData = PacketToBytes.Make(PACKETID.RES_ROOM_LEAVE, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void NotifyLeaveInternal(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug($"NotifyLeaveInternal. SessionID: {sessionID}");

            var reqData = MessagePackSerializer.Deserialize<PKTInternalNtfRoomLeave>(packetData.BodyData);            
            LeaveRoomUser(sessionID, reqData.RoomNumber);
        }
                
        public void RequestChat(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("Room RequestChat");

            try
            {
                var roomObject = CheckRoomAndRoomUser(sessionID);

                if(roomObject.Item1 == false)
                {
                    return;
                }


                var reqData = MessagePackSerializer.Deserialize<PKTReqRoomChat>(packetData.BodyData);

                var notifyPacket = new PKTNtfRoomChat()
                {
                    UserID = roomObject.Item3.UserID,
                    ChatMessage = reqData.ChatMessage
                };

                var Body = MessagePackSerializer.Serialize(notifyPacket);
                var sendData = PacketToBytes.Make(PACKETID.NTF_ROOM_CHAT, Body);

                roomObject.Item2.Broadcast("", sendData);

                MainServer.MainLogger.Debug("Room RequestChat - Success");
            }
            catch (Exception ex)
            {
                MainServer.MainLogger.Error(ex.ToString());
            }
        }
       

        

    }
}
