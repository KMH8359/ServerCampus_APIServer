using CSCommon;
using MemoryPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CA1416

namespace csharp_test_client
{
    public partial class mainForm
    {
        Dictionary<UInt16, Action<byte[]>> PacketFuncDic = new Dictionary<UInt16, Action<byte[]>>();

        void SetPacketHandler()
        {
            //PacketFuncDic.Add(PACKET_ID.PACKET_ID_ERROR_NTF, PacketProcess_ErrorNotify);
            PacketFuncDic.Add(PacketID.RES_LOGIN, PacketProcess_LoginResponse);
            PacketFuncDic.Add(PacketID.HEART_BEAT, PacketProcess_HeartBeat);

            PacketFuncDic.Add(PacketID.RES_ROOM_ENTER, PacketProcess_RoomEnterResponse);
            PacketFuncDic.Add(PacketID.NTF_ROOM_USER_LIST, PacketProcess_RoomUserListNotify);
            PacketFuncDic.Add(PacketID.NTF_ROOM_NEW_USER, PacketProcess_RoomNewUserNotify);
            PacketFuncDic.Add(PacketID.RES_ROOM_LEAVE, PacketProcess_RoomLeaveResponse);
            PacketFuncDic.Add(PacketID.NTF_ROOM_LEAVE_USER, PacketProcess_RoomLeaveUserNotify);
            //PacketFuncDic.Add(PacketID.RES_ROOM_CHAT, PacketProcess_RoomChatResponse);
            PacketFuncDic.Add(PacketID.NTF_ROOM_CHAT, PacketProcess_RoomChatNotify);
            //PacketFuncDic.Add(PacketID.RES_READY_OMOK, PacketProcess_ReadyOmokResponse);
            PacketFuncDic.Add(PacketID.NTF_READY_OMOK, PacketProcess_ReadyOmokNotify);
            PacketFuncDic.Add(PacketID.NTF_START_OMOK, PacketProcess_StartOmokNotify);
            PacketFuncDic.Add(PacketID.RES_PUT_MOK, PacketProcess_PutMokResponse);
            PacketFuncDic.Add(PacketID.NTF_PUT_MOK, PacketProcess_PutMokNotify);
            PacketFuncDic.Add(PacketID.NTF_TIME_OVER, PacketProcess_TimeOverNotify);
            PacketFuncDic.Add(PacketID.NTF_END_MOK, PacketProcess_EndOmokNotify);
        }

        void PacketProcess(byte[] packet)
        {
            var header = new MemoryPackPacketHeaderInfo();
            header.Read(packet);

            var packetID = header.Id;

            if (PacketFuncDic.ContainsKey(packetID))
            {
                PacketFuncDic[packetID](packet);
            }
            else
            {
                DevLog.Write("Unknown Packet Id: " + packetID);
            }
        }


        void PacketProcess_LoginResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResLogin>(packetData);
            DevLog.Write($"로그인 결과: {(ErrorCode)responsePkt.Result}");
        }

        void PacketProcess_HeartBeat(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTHeartBeat>(packetData);
            RespondToHeartbeat();           
        }

        void PacketProcess_RoomEnterResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomEnter>(packetData);
            DevLog.Write($"방 입장 결과:  {(ErrorCode)responsePkt.Result}");
        }

        void PacketProcess_RoomUserListNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomUserList>(packetData);

            for (int i = 0; i < notifyPkt.UserIDList.Count; ++i)
            {
                AddRoomUserList(notifyPkt.UserIDList[i]);
            }

            DevLog.Write($"방의 기존 유저 리스트 받음");
        }

        void PacketProcess_RoomNewUserNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomNewUser>(packetData);

            AddRoomUserList(notifyPkt.UserID);

            DevLog.Write($"방에 새로 들어온 유저 받음");
        }


        void PacketProcess_RoomLeaveResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomLeave>(packetData);

            ClearRoomUserList();
            listBoxRoomChatMsg.Items.Clear();

            DevLog.Write($"방 나가기 결과:  {(ErrorCode)responsePkt.Result}");
        }

        void PacketProcess_RoomLeaveUserNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomLeaveUser>(packetData);

            RemoveRoomUserList(notifyPkt.UserID);

            DevLog.Write($"방에서 나간 유저 받음");
        }


        void PacketProcess_RoomChatResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResRoomChat>(packetData);

            DevLog.Write($"방 채팅 결과:  {(ErrorCode)responsePkt.Result}");
        }


        void PacketProcess_RoomChatNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfRoomChat>(packetData);

            AddRoomChatMessageList(notifyPkt.UserID, notifyPkt.ChatMessage);
        }

        void AddRoomChatMessageList(string userID, string message)
        {
            var msg = $"{userID}:  {message}";

            if (listBoxRoomChatMsg.Items.Count > 512)
            {
                listBoxRoomChatMsg.Items.Clear();
            }

            listBoxRoomChatMsg.Items.Add($"[{userID}]: {message}");
            listBoxRoomChatMsg.SelectedIndex = listBoxRoomChatMsg.Items.Count - 1;
        }

        void PacketProcess_ReadyOmokResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResReadyOmok>(packetData);

            DevLog.Write($"게임 준비 완료 요청 결과:  {(ErrorCode)responsePkt.Result}");
        }

        void PacketProcess_ReadyOmokNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfReadyOmok>(packetData);

            if (notifyPkt.IsReady)
            {
                DevLog.Write($"[{notifyPkt.UserID}]님은 게임 준비 완료");
            }
            else
            {
                DevLog.Write($"[{notifyPkt.UserID}]님이 게임 준비 완료 취소");
            }

        }

        void PacketProcess_StartOmokNotify(byte[] packetData)
        {
            var isMyTurn = false;

            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfStartOmok>(packetData);
            
            if(notifyPkt.FirstUserID == textBoxUserID.Text)
            {
                isMyTurn = true;
            }

            StartGame(isMyTurn, textBoxUserID.Text, GetOtherPlayers(textBoxUserID.Text));

            DevLog.Write($"게임 시작. 흑돌 플레이어: {notifyPkt.FirstUserID}");
        }
        

        void PacketProcess_PutMokResponse(byte[] packetData)
        {
            var responsePkt = MemoryPackSerializer.Deserialize<PKTResPutMok>(packetData);

            DevLog.Write($"오목 놓기 실패: {(ErrorCode)responsePkt.Result}");

        }
        

        void PacketProcess_PutMokNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfPutMok>(packetData);

            플레이어_돌두기(notifyPkt.PosX, notifyPkt.PosY);

            DevLog.Write($"오목 정보: X: {notifyPkt.PosX},  Y: {notifyPkt.PosY}");
        }

        void PacketProcess_TimeOverNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfTimeOver>(packetData);

            IsMyTurn = !IsMyTurn;
            OmokLogic.시간초과();
          

            DevLog.Write($"타임 오버");
        }


        void PacketProcess_EndOmokNotify(byte[] packetData)
        {
            var notifyPkt = MemoryPackSerializer.Deserialize<PKTNtfEndOmok>(packetData);

            EndGame();
            if (notifyPkt.WinUserID == null)
            {
                DevLog.Write($"오목 GameOver: TimeOut");
            }
            else
            {
                DevLog.Write($"오목 GameOver: Win: {notifyPkt.WinUserID}");
            }
        }
    }
}
