using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : ManagerBase
{
    public delegate void Function();

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
}
