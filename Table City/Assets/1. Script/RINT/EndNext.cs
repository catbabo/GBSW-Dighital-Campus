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
                eningNameString = "���� ����";
                break;
            case 1:
                eningNameString = "���� ����";
                break;
            case 2:
                eningNameString = "���� ����";
                break;
            case 3:
                eningNameString = "��� ����";
                break;
            case 4:
                eningNameString = "�븻 ����";
                break;
        }
        for (int i =0; i < endingName.Length; i++)
        {
            endingName[i].text = "���� : "+ name;
        }

    }
    public void Exit()
    {
        Managers._network.OutRoom_GoMain();
    }
}
