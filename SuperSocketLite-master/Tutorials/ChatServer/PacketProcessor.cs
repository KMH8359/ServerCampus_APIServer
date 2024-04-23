using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks.Dataflow;

using CSBaseLib;

namespace ChatServer
{
    class PacketProcessor
    {
        bool IsThreadRunning = false;
        System.Threading.Thread ProcessThread = null;

        //receive쪽에서 처리하지 않아도 Post에서 블럭킹 되지 않는다. 
        //BufferBlock<T>(DataflowBlockOptions) 에서 DataflowBlockOptions의 BoundedCapacity로 버퍼 가능 수 지정. BoundedCapacity 보다 크게 쌓이면 블럭킹 된다
        // BufferBlock<T>의 경우 concurrent_queue와 비슷한 기능을 제공함
        // 일반적인 큐는 멀티스레드 환경에서 사용할 때 텅 빈 큐를 반복해서 훑지 않도록 따로 sleep을 해야하는 문제가 있지만 BufferBlock은 내부적으로 알아서 Sleep을 해준다.
        BufferBlock<ServerPacketData> MsgBuffer = new BufferBlock<ServerPacketData>();  

        UserManager UserMgr = new UserManager();

        Tuple<int,int> RoomNumberRange = new Tuple<int, int>(-1, -1);
        List<Room> RoomList = new List<Room>();

        Dictionary<int, Action<ServerPacketData>> PacketHandlerMap = new Dictionary<int, Action<ServerPacketData>>();           // Action<T> - T를 인자로 받는 함수를 가리키는 델리게이트. 대충 함수 포인터와 유사함. 패킷의 ID에 대응하는 패킷처리 함수를 저장한다.
        PKHCommon CommonPacketHandler = new PKHCommon();
        PKHRoom RoomPacketHandler = new PKHRoom();
                

        //TODO MainServer를 인자로 주지말고, func을 인자로 넘겨주는 것이 좋다
        public void CreateAndStart(List<Room> roomList, MainServer mainServer)
        {
            var maxUserCount = MainServer.ServerOption.RoomMaxCount * MainServer.ServerOption.RoomMaxUserCount;
            UserMgr.Init(maxUserCount);

            RoomList = roomList;
            var minRoomNum = RoomList[0].Number; // 시작방 번호
            var maxRoomNum = RoomList[0].Number + RoomList.Count() - 1; // 끝방 번호
            RoomNumberRange = new Tuple<int, int>(minRoomNum, maxRoomNum);  // 방 번호들을 튜플로 저장
            
            RegistPacketHandler(mainServer);    // 패킷 핸들러 등록

            IsThreadRunning = true;
            ProcessThread = new System.Threading.Thread(this.Process);
            ProcessThread.Start();
        }
        
        public void Destroy()
        {
            IsThreadRunning = false;
            MsgBuffer.Complete();
        }
              
        public void InsertPacket(ServerPacketData data)     // 클라이언트의 입력 없이 서버 단위에서 인위적으로 서버에게 패킷을 송신한다. -> 내부적인 로직 처리를 위해서 
        {
            MsgBuffer.Post(data);
        }

        
        void RegistPacketHandler(MainServer serverNetwork) // 패킷 ID에 대응하는 함수를 저장한다. ID가 로그인이면 로그인 처리, ID가 채팅이면 채팅 처리 함수가 호출되도록
        {            
            // 일반적인 패킷
            CommonPacketHandler.Init(serverNetwork, UserMgr);
            CommonPacketHandler.RegistPacketHandler(PacketHandlerMap);                
            
            // 게임룸 내부에서 오고가는 패킷
            RoomPacketHandler.Init(serverNetwork, UserMgr);
            RoomPacketHandler.SetRoomList(RoomList);
            RoomPacketHandler.RegistPacketHandler(PacketHandlerMap);
        }

        void Process()  // 스레드의 동작
        {
            while (IsThreadRunning)
            {
                //System.Threading.Thread.Sleep(64); //테스트 용
                try
                {
                    var packet = MsgBuffer.Receive();      

                    if (PacketHandlerMap.ContainsKey(packet.PacketID))  // 핸들러 맵에 매핑된 패킷ID가 발견되면 대응하는 함수를 호출하여 로직 처리
                    {
                        PacketHandlerMap[packet.PacketID](packet);
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("세션 번호 {0}, PacketID {1}, 받은 데이터 크기: {2}", packet.SessionID, packet.PacketID, packet.BodyData.Length);
                    }
                }
                catch (Exception ex)
                {
                    IsThreadRunning.IfTrue(() => MainServer.MainLogger.Error(ex.ToString()));
                }
            }
        }


    }
}
