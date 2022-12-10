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
    public int durable;

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

        if (itemData == null) return;
        Debug.Log("not null");
        GetComponent<Image>().sprite = itemData.itemIcon;

        Vector2 size = new Vector2();
        size.x = WIDTH * ItemGrid.tileSizeWidth;
        size.y = HEIGHT * ItemGrid.tileSizeHeight;
        GetComponent<RectTransform>().sizeDelta = size;

        durable = itemData.durable;

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
        GetComponent<Image>().sprite = itemData.alternateIcon;
        equippedMagazine = magazine.itemData as MagazineData;
        ammoCapacity = magazine.ammoCapacity;
        ammoRemained = magazine.ammoRemained;
    }

    public MagazineData GetMagazine()
    {
        GetComponent<Image>().sprite = itemData.itemIcon;
        MagazineData temp = equippedMagazine;
        equippedMagazine = null;
        ammoRemained = 0;
        ammoCapacity = 0;
        return temp;
    }

    public bool HaveMagazine()
    {
        if (equippedMagazine != null) return true;
        return false;
    }

    public void RefillAmmo()
    {
        GetComponent<Image>().sprite = itemData.alternateIcon;
        ammoRemained = ammoCapacity;
        Debug.Log(ammoRemained);
    }

    public bool GetRotate()
    {
        return rotated;
    }
}
