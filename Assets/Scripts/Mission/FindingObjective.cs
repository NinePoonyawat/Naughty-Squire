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
        level = CompleteLevel.Fail;
        
        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 2;
            score[1] = 3;
            score[2] = 4;
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
            if (level != 0 && count < quantity[levelRank - 1])
            {
                Relagations();
                levelRank--;
                if (levelRank >= 0 && levelRank <= 3) levelFinding = quantity[levelRank];
            }
            UpdateText();
        }
    }

    public void OnPlacingItem(ItemData itemData)
    {
        if (findingItem.equals(itemData))
        {
            count++;
            Debug.Log(levelRank);
            if (levelRank != 3 && count >= quantity[levelRank])
            {
                Promotions();
                levelRank++;
                if (levelRank >= 0 && levelRank < 3) levelFinding = quantity[levelRank];
            }
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

    public void SetQuantity()
    {
        quantity[0] = findingItem.quantity[0];
        quantity[1] = findingItem.quantity[1];
        quantity[2] = findingItem.quantity[2];   
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
