using System;
using UnityEngine;
using UnityEngine.UI;

public class GetDefaultValueFromPlayerPref : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("volume");
    }
}
