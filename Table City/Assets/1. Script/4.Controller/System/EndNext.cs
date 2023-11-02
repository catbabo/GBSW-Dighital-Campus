using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndNext : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] endingName;

    private string endingNameString;

    private void OnEnable()
    {
        switch (Managers.Game.endingType)
        {
            case 0:
                endingNameString = "부유 도시";
                break;
            case 1:
                endingNameString = "현대 도시";
                break;
            case 2:
                endingNameString = "우주 도시";
                break;
            case 3:
                endingNameString = "멸망 도시";
                break;
            case 4:
                endingNameString = "노말 도시";
                break;
        }
        for (int i =0; i < endingName.Length; i++)
        {
            endingName[i].text = "엔딩 : "+ endingNameString;
        }
    }

    public void Exit()
    {
        Managers.Network.OutRoom_GoMain();
    }
}
