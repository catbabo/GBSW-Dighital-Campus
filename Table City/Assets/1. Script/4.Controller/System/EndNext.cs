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
                endingNameString = "���� ����";
                break;
            case 1:
                endingNameString = "���� ����";
                break;
            case 2:
                endingNameString = "���� ����";
                break;
            case 3:
                endingNameString = "��� ����";
                break;
            case 4:
                endingNameString = "�븻 ����";
                break;
        }
        for (int i =0; i < endingName.Length; i++)
        {
            endingName[i].text = "���� : "+ endingNameString;
        }
    }

    public void Exit()
    {
        Managers.Network.OutRoom_GoMain();
    }
}
