using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MessagePack;

using CSBaseLib;


namespace ChatServer
{
    public class PKHCommon : PKHandler  // PKHandler - 는 서버 네트워크와 UserManager를 인자로 갖는 객체
    {
        public void RegistPacketHandler(Dictionary<int, Action<ServerPacketData>> packetHandlerMap)
        {            
            packetHandlerMap.Add((int)PACKETID.NTF_IN_CONNECT_CLIENT, NotifyInConnectClient);
            packetHandlerMap.Add((int)PACKETID.NTF_IN_DISCONNECT_CLIENT, NotifyInDisConnectClient);

            packetHandlerMap.Add((int)PACKETID.REQ_LOGIN, RequestLogin);
                                                
        }

        public void NotifyInConnectClient(ServerPacketData requestData) // 세션 하나가 연결됐을 때 호출되는 함수로, 현재 연결된 세션들이 총 얼마인지 반환
        {
            MainServer.MainLogger.Debug($"Current Connected Session Count: {ServerNetwork.SessionCount}");  // SessionCount는 SuperSocket 내부에서 정의된 필드
        }

        public void NotifyInDisConnectClient(ServerPacketData requestData)
        {
            var sessionID = requestData.SessionID;
            var user = UserMgr.GetUser(sessionID);  // 연결을 끊은 사용자 반환
            
            if (user != null)
            {
                var roomNum = user.RoomNumber;

                if (roomNum != PacketDef.INVALID_ROOM_NUMBER)
                {
                    var packet = new PKTInternalNtfRoomLeave()
                    {
                        RoomNumber = roomNum,
                        UserID = user.ID(),
                    };

                    var packetBodyData = MessagePackSerializer.Serialize(packet);   // 패킷 body로 PKTInternalNtfRoomLeave를 조립
                    var internalPacket = new ServerPacketData();
                    internalPacket.Assign(sessionID, (Int16)PACKETID.NTF_IN_ROOM_LEAVE, packetBodyData);    

                    ServerNetwork.Distribute(internalPacket);   // 인위적으로 패킷을 삽입한다. 서버가 서버에게 보내는 패킷
                }

                UserMgr.RemoveUser(sessionID);
            }
                        
            MainServer.MainLogger.Debug($"Current Connected Session Count: {ServerNetwork.SessionCount}");
        }


        public void RequestLogin(ServerPacketData packetData)
        {
            var sessionID = packetData.SessionID;
            MainServer.MainLogger.Debug("로그인 요청 받음");

            try
            {
                if(UserMgr.GetUser(sessionID) != null)
                {
                    ResponseLoginToClient(ERROR_CODE.LOGIN_ALREADY_WORKING, packetData.SessionID);
                    return;
                }
                                
                var reqData = MessagePackSerializer.Deserialize< PKTReqLogin>(packetData.BodyData);
                var errorCode = UserMgr.AddUser(reqData.UserID, sessionID);
                if (errorCode != ERROR_CODE.NONE)
                {
                    ResponseLoginToClient(errorCode, packetData.SessionID);

                    if (errorCode == ERROR_CODE.LOGIN_FULL_USER_COUNT)  // 꽉차서 접속이 안돼요
                    {
                        NotifyMustCloseToClient(ERROR_CODE.LOGIN_FULL_USER_COUNT, packetData.SessionID);
                    }
                    
                    return;
                }

                ResponseLoginToClient(errorCode, packetData.SessionID);

                MainServer.MainLogger.Debug("로그인 요청 답변 보냄");

            }
            catch(Exception ex)
            {
                // 패킷 해제에 의해서 로그가 남지 않도록 로그 수준을 Debug로 한다.
                MainServer.MainLogger.Error(ex.ToString());
            }
        }
                
        public void ResponseLoginToClient(ERROR_CODE errorCode, string sessionID)
        {
            var resLogin = new PKTResLogin()
            {
                Result = (short)errorCode
            };

            var bodyData = MessagePackSerializer.Serialize(resLogin);
            var sendData = PacketToBytes.Make(PACKETID.RES_LOGIN, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }

        public void NotifyMustCloseToClient(ERROR_CODE errorCode, string sessionID)
        {
            var resLogin = new PKNtfMustClose()
            {
                Result = (short)errorCode
            };

            var bodyData = MessagePackSerializer.Serialize(resLogin);
            var sendData = PacketToBytes.Make(PACKETID.NTF_MUST_CLOSE, bodyData);

            ServerNetwork.SendData(sessionID, sendData);
        }


        
                      
    }
}
