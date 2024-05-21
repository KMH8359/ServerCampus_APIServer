using MemoryPack;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PvPGameServer;

class RoomManager
{
    List<Room> _roomsList = new List<Room>();


    public RoomTimer _gameTimer { get; private set; } = null;
    public int maxRoomCount;    
    public int checkingGroupCount;  
    public int groupSize;
    public int roomCheckInterval;
    public int maxGameTimeMinute;

    private PacketProcessor _packetProcessor;

    public void SetPacketProcessor(PacketProcessor packetProcessor)
    {
        _packetProcessor = packetProcessor;
    }
    public void CreateRooms(ServerOption serverOpt)
    {
        maxRoomCount = serverOpt.RoomMaxCount;
        checkingGroupCount = serverOpt.RoomCheckGroupCount;
        groupSize = maxRoomCount / checkingGroupCount;
        roomCheckInterval = serverOpt.RoomCheckInterval;
        maxGameTimeMinute = serverOpt.MaxGameTimeMinute;
        var startNumber = serverOpt.RoomStartNumber;
        var maxUserCount = serverOpt.RoomMaxUserCount;

        for(int i = 0; i < maxRoomCount; ++i)
        {
            var roomNumber = (startNumber + i);
            var room = new Room();
            room.Init(i, roomNumber, maxUserCount);

            _roomsList.Add(room);
        }

        _gameTimer = new RoomTimer(1000);
        _gameTimer.TurnTimeOut += OnTurnTimeOut;
        _gameTimer.StartTurnTimer();
    }

    public Room GetValidRoom()
    {
        foreach (var room in _roomsList)
        {
            if (room.CurrentUserCount() == 0 && room.IsReserved == false)
            {
                return room;
            }
        }
        return null;
    }

    void OnTurnTimeOut(object sender, EventArgs e)
    {
        GroupIndexEventArgs groupArgs = e as GroupIndexEventArgs;

        int groupIndex = groupArgs.RoomGroupIndex;

        var group = _roomsList.GetRange(groupIndex * groupSize, groupIndex * groupSize + groupSize);

        foreach (var room in group)
        {
            if (room.IsReserved && room.CurrentUserCount() == 0)
            {
                room.IsReserved = false;
            }

            if (room.RecentPutMokTime == DateTime.MinValue) continue;

            var nowTime = DateTime.UtcNow;
            if ((nowTime - room.RecentPutMokTime).TotalMilliseconds > roomCheckInterval)
            {
                var packet = InnerPakcetMaker.MakeNTFInTimeOutPacket(room.Index);
                _packetProcessor.InsertPacket(packet);
            }

            if ((nowTime - room.GameStartTime).TotalMinutes > maxGameTimeMinute)
            {
                var packet = InnerPakcetMaker.MakeNTFInTooLongGameRoomPacket(room.Index);
                _packetProcessor.InsertPacket(packet);
            }
        }
    }


    public List<Room> GetRoomsList() 
    { 
        return _roomsList; 
    }

    public Room GetRoombyRoomNumber(int roomNumber)
    {
        return _roomsList[roomNumber];
    }
}
