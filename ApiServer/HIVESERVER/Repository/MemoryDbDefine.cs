namespace HIVESERVER.Repository;

public enum UserState
{
    Default = 0,
    Login = 1,
    Matching = 2,
    Playing = 3
}

public class RediskeyExpireTime // Redis 데이터 저장 유지시간
{
    public const ushort NxKeyExpireSecond = 3;
    public const ushort RegistKeyExpireSecond = 6000;
    public const ushort LoginKeyExpireMin = 60;
    public const ushort TicketKeyExpireSecond = 6000;  
}