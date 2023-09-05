using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCountView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countText;
    [SerializeField]
    private Define.Point point;
    void Update()
    {
        countText.text = Managers._game.countText[(int)point];
    }
}
