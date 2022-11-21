using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindingObjective : Objective
{
    private InventoryController inventoryController;
    private ItemData findingItem;
    private int levelRank;
    private int[] quantity = new int[3];
    private int levelFinding;
    private int count;

    // Start is called before the first frame update
    void Awake()
    {
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        inventoryController.OnPickUpItem += OnPickUpItem;
        inventoryController.OnPlacingItem += OnPlacingItem;
    }
    void Start()
    {
        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 100;
            score[1] = 150;
            score[2] = 200;
        }
        if (quantity[0] == 0 && quantity[1] == 0 && quantity[2] == 0)
        {
            quantity[0] = 1;
            quantity[1] = 2;
            quantity[2] = 3;
        }
        count = 0;
        levelRank = 0;
        levelFinding = quantity[levelRank];

        UpdateText();
    }

    public void OnPickUpItem(ItemData itemData)
    {
        if (findingItem.equals(itemData))
        {
            count--;
            Relagations();
            levelRank--;
            levelFinding = quantity[levelRank];
            UpdateText();
        }
    }

    public void OnPlacingItem(ItemData itemData)
    {
        Debug.Log("enter1");
        if (findingItem.equals(itemData))
        {
            Debug.Log("enter2");
            count++;
            Promotions();
            levelRank++;
            levelFinding = quantity[levelRank];
            UpdateText();
        }
    }

    public int GetCount()
    {
        return count;
    }

    public void SetItem(ItemData itemData)
    {
        findingItem = itemData;
    }

    public Texture getTexture()
    {
        return findingItem.itemIcon.texture;
    }

    public override void UpdateText()
    {
        uiText.text = "find "  + levelFinding + " of " + findingItem.name + "<color=" + color + "> ( " + count + " / " + levelFinding + " )</color>";
    }
}
