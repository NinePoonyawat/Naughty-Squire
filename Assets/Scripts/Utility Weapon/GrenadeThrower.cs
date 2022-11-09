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
    private bool isArmed =false;

    
    
    void Start()
    {   
        ItemGrid LhandItemGrid = GameObject.Find("UI/UIInventory/Grid-L-Hand").GetComponent<ItemGrid>();
        ItemGrid RhandItemGrid = GameObject.Find("UI/UIInventory/Grid-R-Hand").GetComponent<ItemGrid>();
            //GameObject.Find("UI/UIInventory").SetActive(false);

        LhandItemGrid.grenadeChangeEvent += setNewData;
        LhandItemGrid.onPickupWeaponEvent += disarm;

        RhandItemGrid.grenadeChangeEvent += setNewData;
        RhandItemGrid.onPickupWeaponEvent += disarm;

        inventoryManager.OnInventoryOpen += openInventory;
        inventoryManager.OnInventoryClose += closeInventory;

        if (currentData == null) isArmed = false;
    }

    
    

    public void setNewData(GrenadeData grenadeData)
        {
            isArmed = true;
            currentData = grenadeData;
            damage = grenadeData.damage;
            ExplodeTime = grenadeData.ExplodeTime;
            lifeTime = grenadeData.lifeTime;
            explodeRadius = grenadeData.explodeRadius;
            bombType = grenadeData.bombtype;
        }
    public void disarm()
        {
            Debug.Log("enter");
            isArmed = false;
            currentData = null;
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
    public bool isThrowable()
    {
        return isArmed;
    }

    public List<float> getfloatdata() {
        List<float> ld = new List<float>();
        ld.Add(damage); ld.Add(lifeTime); ld.Add(explodeRadius);
        return ld;
    }
    public GrenadeData.BombType getbombtype() {
        return bombType;
    }
}