using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Define;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _header, _subject;

    public void Init()
    {
        Managers.Event.AddCancleButton(CancleButton);
        gameObject.SetActive(false);
    }

    private void CancleButton()
    {
        gameObject.SetActive(false);
    }

    public void Show(string header, string subject)
    {
        _header.text = header;
        _subject.text = subject;
        gameObject.SetActive(true);
    }
}
