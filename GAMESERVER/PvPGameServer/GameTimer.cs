using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PvPGameServer;

public class GameTimer
{
    const int TurnTimeoutSeconds = 90;
    Timer turnTimer;

    public event EventHandler TurnTimeOut;

    public GameTimer()
    {
        // 타이머 초기화
        turnTimer = new Timer(TurnTimeoutSeconds * 1000);
        turnTimer.Elapsed += TurnTimerElapsed;
        turnTimer.AutoReset = false;
    }
    public void StartTurnTimer()
    {
        turnTimer.Start();
    }

    public void RestartTurnTimer() 
    {
        turnTimer.Stop();
        turnTimer.Interval = TurnTimeoutSeconds * 1000;
        turnTimer.Start();
    }

    public void StopTurnTimer()
    {
        turnTimer.Stop();
    }

    public void TurnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        TurnTimeOut?.Invoke(this, EventArgs.Empty);
    }

}
