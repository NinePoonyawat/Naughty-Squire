using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumnSlider;
    [SerializeField] TMP_Text volumeValueText;
    // Start is called before the first frame update
    public void Start()
    {
        AudioListener.volume = 1;
        volumnSlider.value = 1;
        volumeValueText.SetText("100");
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumnSlider.value;
        volumeValueText.SetText("" + ((int) (volumnSlider.value * 100)));
    }
}
