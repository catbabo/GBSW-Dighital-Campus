using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndNext : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] endingName;

    private string eningNameString;
    private void OnEnable()
    {
        switch (Managers._game.endingType)
        {
            case 0:
                eningNameString = "부유 도시";
                break;
            case 1:
                eningNameString = "현대 도시";
                break;
            case 2:
                eningNameString = "우주 도시";
                break;
            case 3:
                eningNameString = "멸망 도시";
                break;
            case 4:
                eningNameString = "노말 도시";
                break;
        }
        for (int i =0; i < endingName.Length; i++)
        {
            endingName[i].text = "엔딩 : "+ name;
        }

    }
    public void Exit()
    {
        Managers._network.OutRoom_GoMain();
    }
}
