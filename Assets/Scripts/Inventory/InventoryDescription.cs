using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDescription : MonoBehaviour
{
    [SerializeField] Image descriptionGO;
    [SerializeField] RectTransform description;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text usesText;

    void Awake()
    {
        description = descriptionGO.rectTransform;
    }

    public void Show(bool b)
    {
        description.gameObject.SetActive(b);
    }

    public void SetText(InventoryItem targetItem)
    {
        ItemData itemData = targetItem.itemData;
        nameText.text = itemData.name;
        descriptionText.text = itemData.description;
        usesText.text = targetItem.durable.ToString();
        if (targetItem.durable == 0) usesText.text = "";
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        SetParent(targetGrid);

        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY
            );
        
        description.localPosition = pos;
    }

    public void SetParent(ItemGrid targetGrid)
    {
        description.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            posX,
            posY
            );

        description.localPosition = pos;
    }
}
