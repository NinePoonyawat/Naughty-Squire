using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObjective : Objective
{
    private InventoryController inventoryController;
    private ItemData findingItem;
    private int levelRank;
    private int[] inventoryScore = new int[3];
    private int levelScore;
    private int iScore;

    // Start is called before the first frame update
    void Awake()
    {
        inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        inventoryController.OnPickUpItem += OnPickUpItem;
        inventoryController.OnPlacingItem += OnPlacingItem;
        level = CompleteLevel.Fail;
    }

    void Start()
    {

        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 2;
            score[1] = 3;
            score[2] = 4;
        }

        if (inventoryScore[0] == 0 && inventoryScore[1] == 0 && inventoryScore[2] == 0)
        {
            inventoryScore[0] = 1;
            inventoryScore[1] = 2;
            inventoryScore[2] = 3;
        }
        levelRank = 0;
        levelScore = inventoryScore[levelRank];

        UpdateText();
    }

    public void OnPickUpItem(ItemData itemData)
    {
        iScore = inventoryController.GetIScore();
        if (level != 0 && iScore < inventoryScore[levelRank - 1])
        {
            Relagations();
            levelRank--;
            if (levelRank >= 0 && levelRank <= 3) levelScore = inventoryScore[levelRank];
        }
        UpdateText();
    }

    public void OnPlacingItem(ItemData itemData)
    {
        
        iScore = inventoryController.GetIScore();
        if (levelRank != 3 && iScore >= inventoryScore[levelRank])
        {
            Promotions();
            levelRank++;
            if (levelRank >= 0 && levelRank < 3) levelScore = inventoryScore[levelRank];
        }
        UpdateText();
    }

    public int GetIScore()
    {
        return iScore;
    }

    public void SetIScore(int newIScore)
    {
        iScore = newIScore;
        Debug.Log(iScore);
    }

    // public void SetInventoryScore()
    // {
    //     in[0] = findingItem.quantity[0];
    //     quantity[1] = findingItem.quantity[1];
    //     quantity[2] = findingItem.quantity[2];   
    // }

    // public Texture getTexture()
    // {
    //     return findingItem.itemIcon.texture;
    // }

    public override void UpdateText()
    {
        //uiText.text = "scored your inventory "  + "<color=" + color + "> ( " + iScore + " / " + levelScore + " )</color>";
    }
}
