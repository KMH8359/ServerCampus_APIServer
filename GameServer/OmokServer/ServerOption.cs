
namespace PvPGameServer;

public class ServerOption
{
    public int ServerUniqueID { get; set; }

    public int Port { get; set; }

    public string Name { get; set; }

    public int MaxConnectionNumber { get; set; }

    public int MaxRequestLength { get; set; }

    public int ReceiveBufferSize { get; set; }

    public int SendBufferSize { get; set; }

    public int RoomMaxCount { get; set; } = 0;

    public int RoomMaxUserCount { get; set; } = 0;

    public int RoomStartNumber { get; set; } = 0;

    public int RoomCheckGroupCount { get; set; } = 0;
    public int RoomCheckInterval { get; set; } = 0;
    public int MaxGameTimeMinute { get; set; } = 0;
    public int HeartbeatGroupCount { get; set; } = 0;
    public int HeartbeatInterval { get; set; } = 0;
    public int HeartbeatGroupSize { get; set; } = 0;
    public int HeartbeatTimeOut { get; set; } = 0;

}    
