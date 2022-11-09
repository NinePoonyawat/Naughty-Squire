using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [Header("Essentials")]
    public ItemData itemData;
    public enum ItemType {NORMAL, WEAPON, MAGAZINE, CONSUMABLE}
    public ItemType itemType;

    [Header("Weapon")]
    public MagazineData equippedMagazine = null;

    [Header("Ammo")]
    public int ammoCapacity = 0;
    public int ammoRemained = 0;

    public int HEIGHT
    {
        get {
            if (rotated == false)
            {
                return itemData.height;
            }
            return itemData.width;
        }
    }

    public int WIDTH
    {
        get {
            if (rotated == false)
            {
                return itemData.width;
            }
            return itemData.height;
        }
    }

    public int onGridPositionX;
    public int onGridPositionY;

    public bool rotated = false;

    internal void Set(ItemData itemData)
    {
        this.itemData = itemData;

        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = WIDTH * ItemGrid.tileSizeWidth;
        size.y = HEIGHT * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;

        MagazineData magData = itemData as MagazineData;
        if (magData != null)
        {
            ammoCapacity = magData.ammoCapacity;
        }
    }

    internal void Rotate()
    {
        rotated = !rotated;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rotation = Quaternion.Euler(0, 0, rotated == true ? 90f : 0f);
    }

    public void EquipMagazine(InventoryItem magazine)
    {
        equippedMagazine = magazine.itemData as MagazineData;
        ammoCapacity = magazine.ammoCapacity;
        ammoRemained = magazine.ammoRemained;
    }

    public MagazineData GetMagazine()
    {
        MagazineData temp = equippedMagazine;
        equippedMagazine = null;
        return temp;
    }

    public bool HaveMagazine()
    {
        if (equippedMagazine != null) return true;
        return false;
    }
}
