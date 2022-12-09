using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

public class GrenadeThrower : MonoBehaviour {


    private float lifeTime;
    private float ExplodeTime;
    [SerializeField] private float explodeRadius;
    private GrenadeData.BombType bombType;

    //[SerializeField] private GameObject explodeEffect;

    [SerializeField] private float damage;
    [SerializeField] private float explodeForce = 100f;
    [SerializeField] private InventoryManager inventoryManager;
    private bool isInventoryOpen = false;

    private GrenadeData currentData = null;
    private bool leftArmed =false;
    private bool rightArmed =false;
    private ItemGrid grenadeItemClone;
    private int posX;
    private int posY;
    private ItemGrid LhandItemGrid;
    private ItemGrid RhandItemGrid;

    
    
    void Start()
    {   
        LhandItemGrid = GameObject.Find("UI/UIInventory/Grid-L-Hand").GetComponent<ItemGrid>();
        RhandItemGrid = GameObject.Find("UI/UIInventory/Grid-R-Hand").GetComponent<ItemGrid>();
            //GameObject.Find("UI/UIInventory").SetActive(false);

        LhandItemGrid.weaponChangeEvent += setNewData;
        LhandItemGrid.onPickupWeaponEvent += disarm;

        RhandItemGrid.weaponChangeEvent += setNewData;
        RhandItemGrid.onPickupWeaponEvent += disarm;

        inventoryManager.OnInventoryOpen += openInventory;
        inventoryManager.OnInventoryClose += closeInventory;

        if (currentData == null) leftArmed = false; rightArmed = false;
    }

    
    

    public void setNewData(InventoryItem grenadeItem)
        {
            //grenadeItemClone = grenadeItem;
            posX = grenadeItem.onGridPositionX ; posY = grenadeItem.onGridPositionY;
            InventoryItem Li = LhandItemGrid.getInventory(posX,posY);
            InventoryItem Ri = RhandItemGrid.getInventory(posX,posY);
            if (Li != null && Li.itemData as GrenadeData != null) {
                Debug.Log("left");
                leftArmed = true;
            } else leftArmed = false;
            if (Ri != null && Ri.itemData as GrenadeData != null) {
                Debug.Log("right");
                rightArmed = true;
            } else rightArmed = false;
            GrenadeData grenadeData = grenadeItem.itemData as GrenadeData;
            if (grenadeData == null) return;
            currentData = grenadeData;
            damage = grenadeData.damage;
            ExplodeTime = grenadeData.ExplodeTime;
            lifeTime = grenadeData.lifeTime;
            explodeRadius = grenadeData.explodeRadius;
            bombType = grenadeData.bombtype;
        }
    public void disarm()
        {
            currentData = null;
            InventoryItem Li = LhandItemGrid.getInventory(posX,posY);
            InventoryItem Ri = RhandItemGrid.getInventory(posX,posY);
            if (Li != null && Li.itemData as GrenadeData != null) {
                Debug.Log("left");
                leftArmed = false;
                LhandItemGrid.DiscardItem(posX,posY); return;
            }
            if (Ri != null && Ri.itemData as GrenadeData != null) {
                Debug.Log("right");
                rightArmed = false;
                RhandItemGrid.DiscardItem(posX,posY); return;
            }
        }
    
    // IEnumerator ExplodeDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Explode();
    // }
    
    public void openInventory(object o,EventArgs e)
        {
            isInventoryOpen = true;
        }

    public void closeInventory(object o,EventArgs e)
    {
        isInventoryOpen = false;
    }
    public bool isLeftThrowable()
    {
        return leftArmed;
    }
    public bool isRightThrowable()
    {
        return rightArmed;
    }

    public List<float> getfloatdata() {
        List<float> ld = new List<float>();
        ld.Add(damage); ld.Add(ExplodeTime); ld.Add(explodeRadius);
        return ld;
    }
    public float getlifetime() {
        return lifeTime;
    }
    public GrenadeData.BombType getbombtype() {
        //Debug.Log(bombType);
        return bombType;
    }
}