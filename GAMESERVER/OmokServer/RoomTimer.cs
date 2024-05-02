using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PvPGameServer;

public class RoomTimer
{
    Timer turnTimer;

    public event EventHandler TurnTimeOut;

    public int RoomGroupIndex;

    public RoomTimer(double interval)
    {
        // 타이머 초기화
        turnTimer = new Timer(interval);
        turnTimer.Elapsed += TurnTimerElapsed;
        turnTimer.AutoReset = true;
        RoomGroupIndex = 0;
    }
    public void StartTurnTimer()
    {
        turnTimer.Start();
    }

    public void RestartTurnTimer() 
    {
        turnTimer.Stop();
        turnTimer.Start();
    }

    public void StopTurnTimer()
    {
        turnTimer.Stop();
    }

    public void TurnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        RoomGroupIndex = (RoomGroupIndex + 1) % 4;
        TurnTimeOut?.Invoke(this, new GroupIndexEventArgs(RoomGroupIndex));
    }

}

public class GroupIndexEventArgs : EventArgs
{
    public int RoomGroupIndex { get; set; }

    public GroupIndexEventArgs(int roomGroupIndex)
    {
        RoomGroupIndex = roomGroupIndex;
    }
}