using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetRoomCode : MonoBehaviour, IUpdateSelectedHandler
{
    private InputField field
    {
        get
        {
            if(field == null)
            {
                field = gameObject.GetComponent<InputField>();
            }
            return field;
        }
        set { field = value; }
    }

    public void OnUpdateSelected(BaseEventData eventData)
    {
        Managers.Network.SetRoomCode(field.text);
    }
}
