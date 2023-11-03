using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReadyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Sprite readyImage, readyedImage, startImage;
    [SerializeField]
    private TMP_Text text;
    private bool isReady = false, stopPush = false;

    public void OnPointerEnter(PointerEventData eventData) { }

    public void OnPointerExit(PointerEventData eventData) { }

    public void OnPointerClick(PointerEventData eventData)
    {

        Managers.Event.ExcuteReadyButton(isReady);
    }

    public void Init()
    {
        stopPush = false;
        isReady = false;
        text.text = "READY";
        image.sprite = readyImage;
    }

    public void SwapButton()
    {
        if (stopPush)
            return;

        if (!isReady)
        {
            isReady = true;
            if (!Managers.Network.IsMaster())
            {
                image.sprite = readyedImage;
                stopPush = true;
                return;
            }
            text.text = "START";
            image.sprite = startImage;
        }
    }

    public void UpdateButon()
    {
        if (isReady)
        {
            if (Managers.Network.IsMaster())
            {
                text.text = "START";
                image.sprite = startImage;
                stopPush = false;
            }
        }
    }
}