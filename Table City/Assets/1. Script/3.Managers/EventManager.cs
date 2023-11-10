using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ManagerBase
{
    public delegate void Function();
    public delegate void FunctionBool(bool trigger);

    public override void Init() { }

    private Function OnGameStartButton;
    public void AddGameStartButton(Function func)
    {
        OnGameStartButton += func;
    }
    public void ExcuteGameStartButton()
    { OnGameStartButton(); }


    private Function MatchRoomButton;
    public void AddMatchRoomButton(Function func)
    {
        MatchRoomButton += func;
    }
    public void ExcuteMatchRoomButton()
    { MatchRoomButton(); }


    private Function CancleButton;
    public void AddCancleButton(Function func)
    {        
        CancleButton += func;
    }
    public void ExcuteCancleButton()
    { CancleButton(); }


    private FunctionBool JobButton;
    public void AddJobButton(FunctionBool func)
    {
        JobButton += func;
    }
    public void ExcuteJobButton(bool trigger)
    { JobButton(trigger); }


    private FunctionBool AllReady;
    public void AddAllReady(FunctionBool func)
    {
        AllReady += func;
    }
    public void ExcuteAllReady(bool isSolo)
    { AllReady(isSolo); }


    private FunctionBool ReadyButton;
    public void AddReadyButton(FunctionBool func) {
        ReadyButton += func;
    }
    public void ExcuteReadyButton(bool isReady)
    { ReadyButton(isReady); }


    private Function LeaveButton;
    public void AddLeaveButton(Function func) {
        LeaveButton += func;
    }
    public void ExcuteLeaveButton()
    { LeaveButton(); }
}
