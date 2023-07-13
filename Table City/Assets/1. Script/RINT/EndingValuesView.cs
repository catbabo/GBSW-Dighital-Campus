using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingValuesView : MonoBehaviour
{
    [SerializeField]
    private Slider[] slider;
    [SerializeField]
    private TextMeshProUGUI[] sliderText;
    [SerializeField]
    private TextMeshProUGUI[] valueView;
    private GameManager system;
    private void Start()
    {
        system = Managers.system;
    }

    void Update()
    {
        int allEndingValue =0;
        for (int i =0; i< system.endingValues.Length; i++)
        {
            allEndingValue += system.endingValues[i];
        }

        for (int i = 0; i < slider.Length; i++)
        {
            slider[i].value = (float)allEndingValue / 100;
            sliderText[i].text = "¿£µù : " + allEndingValue + "%";
            valueView[i].text = $"<color=\"red\">{system.endingValues[0]}</color>/<color=\"yellow\">{system.endingValues[1]}</color>/<color=\"green\">{system.endingValues[2]}</color>/<color=\"blue\">{system.endingValues[3]}</color>";
        }
    }
}
