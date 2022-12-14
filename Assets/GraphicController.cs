using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicController : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown graphicDropdown;

    public void Start()
    {
        graphicDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void SetQuantity(int quantityIndex)
    {
        QualitySettings.SetQualityLevel(quantityIndex);
    }
}
