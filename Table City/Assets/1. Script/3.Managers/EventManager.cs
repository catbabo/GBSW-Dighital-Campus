using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ManagerBase
{
    public delegate void Function();
    public delegate void FunctionBool(bool trigger);

    public override void Init() { }


    private Function OnGameStart;
    public void AddOnGameStart(Function func)
    { OnGameStart += func; }
    public void ExcuteOnGameStart()
    { OnGameStart(); }


    private Function MatchRoomButton;
    public void AddMatchRoomButton(Function func)
    { MatchRoomButton += func; }
    public void ExcuteMatchRoomButton()
    { MatchRoomButton(); }


    private Function CancleButton;
    public void AddCancleButton(Function func)
    { CancleButton += func; }
    public void ExcuteCancleButton()
    { CancleButton(); }


    private FunctionBool JobButton;
    public void AddJobButton(FunctionBool func)
    { JobButton += func; }
    public void ExcuteJobButton(bool trigger)
    { JobButton(trigger); }
}
