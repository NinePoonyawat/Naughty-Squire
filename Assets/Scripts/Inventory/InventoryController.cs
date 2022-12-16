using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Unique ItemGrid")]
    public ItemGrid selectedItemGrid;
    [SerializeField] public ItemGrid[] itemGrids;
    [SerializeField] public ItemGrid[] handItemGrid;

    [SerializeField] InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [Header("Custom")]
    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject inventoryItemPrefab;
    [SerializeField] Transform canvasTransform;

    [Header("Drop Item")]
    [SerializeField] GameObject pickableItemPrefab;
    [SerializeField] private Transform dropPosition;
    [SerializeField] private Transform pickableParent;

    [Header("Quick Use")]
    public InventoryItem[] quickUseItems;

    public bool isInventoryOpen = true;
    InventoryHighlight inventoryHighlight;
    InventoryDescription inventoryDescription;


    [SerializeField] private GunSystem gunSystem;
    private int bulletLeft = -1;

    [SerializeField] InventoryInitialize inventoryInitialize;

    public event OnPlacingItemEvent OnPlacingItem;
    public delegate void OnPlacingItemEvent(ItemData itemData);

    public event OnPickUpItemEvent OnPickUpItem;
    public delegate void OnPickUpItemEvent(ItemData itemData);
    public event OnScoreChangeEvent OnScoreChange;
    public delegate void OnScoreChangeEvent();

    bool isToolOn = false;

    private int iScore = 0;

    private void Start()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        inventoryDescription = GetComponent<InventoryDescription>();
        //itemGrids = FindObjectsOfType<ItemGrid>();
        SetOpen(false);

        pickableParent = GameObject.Find("PickableItem").transform;

        foreach(ItemGrid itemGrid in handItemGrid)
        {
            itemGrid.onPickupWeaponEvent += GetBulletLeftInMagazine;
            itemGrid.weaponChangeEvent += WeaponChange;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuickUseItem(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuickUseItem(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QuickUseItem(2);
        }

        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            isToolOn = !isToolOn;
        }

        if (!isInventoryOpen) return;

        ItemIconDrag();

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            inventoryDescription.Show(false);
        }
        else
        {
            HandleHighlight();
            HandleDescription();
        }

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RightMouseButtonPress();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RotateItem();
        }

        if (Input.GetKeyDown(KeyCode.Q) && isToolOn)
        {
            CreateItem(-1);
        }
        
        if (Input.GetKeyDown(KeyCode.Z) && isToolOn)
        {
            DeleteItem();
        }

        if (Input.GetKeyDown(KeyCode.G) && isToolOn)
        {
            InsertItem(-1);
        }
    }

    private void DeleteItem()
    {
        if (selectedItem == null) { return; }
        
        Destroy(selectedItem.gameObject);
    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }

        selectedItem.Rotate();
    }

    private void CreateItem(int selectedItemID)
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }

        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        
        if (selectedItemID == -1) selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void CreateItem(ItemData selectItemData)
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }

        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        
        inventoryItem.Set(selectItemData);
    }

    private void InsertItem(int selectedItemID)
    {
        if (selectedItemGrid == null) { return; }

        CreateItem(selectedItemID);
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;      

        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        
        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
        
    }

    public bool InsertItem(ItemData selectedItem, ItemGrid selectedItemGrid,BulletRecognize bulletRecognize)
    {
        InventoryItem itemToInsert = Instantiate(inventoryItemPrefab).GetComponent<InventoryItem>();
        
        itemToInsert.Set(selectedItem);

        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);
        if (posOnGrid == null)
        {
            Destroy(itemToInsert.gameObject);
            return false;
        }
        iScore += selectedItem.value;
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y,bulletRecognize);
        try
        {
            OnPlacingItem?.Invoke(selectedItem);
        }
        catch
        {
            
        }
        return true;
    }

    public bool FillItem(ItemData selectedItem)
    {
        foreach (ItemGrid itemGrid in itemGrids)
        {
            if (InsertItem(selectedItem, itemGrid, null)) return true;
        }
        return false;
    }

    public bool FillItem(ItemData selectedItem, BulletRecognize bulletRecognize)
    {
        foreach (ItemGrid itemGrid in itemGrids)
        {
            if (InsertItem(selectedItem, itemGrid, bulletRecognize)) return true;
        }
        return false;
    }

    public int CountItem(ItemData selectedItem)
    {
        int count = 0;
        int size = selectedItem.width * selectedItem.height;
        foreach (ItemGrid itemGrid in itemGrids)
        {
            count += itemGrid.CountItemInGrid(selectedItem);
        }
        return count/size;
    }

    public void DropItem(InventoryItem selectedItem)
    {
        GameObject pickableGO = Instantiate(pickableItemPrefab, dropPosition.position, dropPosition.rotation, pickableParent) as GameObject;
        PickableItem pickableItem = pickableGO.GetComponent<PickableItem>();
        BulletRecognize bulletRecognize = pickableItem.GetComponent<BulletRecognize>();
        if (bulletRecognize != null)
        {
            bulletRecognize.SetMagazine(selectedItem.GetMagazine());
        }
        
        pickableItem.itemData = selectedItem.itemData;
        pickableItem.SetMesh();

        Destroy(selectedItem.gameObject);
        iScore -= selectedItem.itemData.value;
        try {OnPickUpItem.Invoke(selectedItem.itemData);}
        catch {}
        selectedItem = null;
    }

    public void QuickUseItem(int num)
    {
        if (selectedItem != null)
        {
            quickUseItems[num] = selectedItem;
            FindObjectOfType<AudioManager>().Play("InventoryInteract");
            return;
        }
        if (quickUseItems[num] == null) { return; }

        foreach (ItemGrid itemGrid in itemGrids)
        {
            Vector2Int? tileGridPosition = itemGrid.FindItemInGrid(quickUseItems[num]);
            if (tileGridPosition != null)
            {
                bool complete = itemGrid.InteractItem(tileGridPosition.Value.x, tileGridPosition.Value.y);
                if (complete)
                {
                    iScore -= quickUseItems[num].itemData.value;
                    quickUseItems[num] = null;
                }
                return;
            }
        }
    }

    private void LeftMouseButtonPress()
    {
        if (selectedItemGrid == null) { return; }

        Vector2Int tileGridPosition = GetTileGridPosition();
        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void RightMouseButtonPress()
    {

        if (selectedItemGrid == null && selectedItem != null)
        {
            DropItem(selectedItem);
            return;
        }
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            InteractItem(tileGridPosition);
        }
        else
        {
            ContactItem(selectedItem, tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            //if (!selectedItem.GetRotate()) RotateItem();
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {     
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            FindObjectOfType<AudioManager>().Play("InventoryInteract");

            WeaponData weaponData = selectedItem.itemData as WeaponData;
            if (weaponData != null && bulletLeft != -1)
            {
                gunSystem.SetBulletLeftInMagazine(bulletLeft);
            }
            iScore += selectedItem.itemData.value;

            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }

        }
    }

    private void InteractItem(Vector2Int tileGridPosition)
    {
        ItemData returnData = selectedItemGrid.InteractItem(tileGridPosition.x, tileGridPosition.y);
        if (returnData != null)
        {
            CreateItem(returnData);
        }
    }

    private void ContactItem(InventoryItem contactItem, Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.ContactItem(contactItem, tileGridPosition.x, tileGridPosition.y);
    }
    
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

    InventoryItem itemToHighlight;

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (selectedItem == null)
        {
            if (positionOnGrid.x < 0 || positionOnGrid.y < 0
                || positionOnGrid.x >= selectedItemGrid.gridSizeWidth
                || positionOnGrid.y >= selectedItemGrid.gridSizeHeight)
            {
                return;
            }

            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {        
                inventoryHighlight.Show(true);        
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetParent(selectedItemGrid);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT
                ));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetParent(selectedItemGrid);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void HandleDescription()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if (selectedItem == null)
        {
            if (positionOnGrid.x < 0 || positionOnGrid.y < 0
                || positionOnGrid.x >= selectedItemGrid.gridSizeWidth
                || positionOnGrid.y >= selectedItemGrid.gridSizeHeight)
            {
                return;
            }

            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {        
                inventoryDescription.Show(true);
                inventoryDescription.SetText(itemToHighlight);
                inventoryDescription.SetParent(selectedItemGrid);
                inventoryDescription.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryDescription.Show(false);
            }
        }
        else
        {
            inventoryDescription.Show(false);
        }
    }

    public void SetOpen(bool isOpen)
    {
        if (isOpen) { isInventoryOpen = true; }
        else        { isInventoryOpen = false; }

        foreach (ItemGrid itemGrid in itemGrids)
        {
            itemGrid.gameObject.SetActive(isInventoryOpen);
        }
    }

    public void GetBulletLeftInMagazine()
    {
        bulletLeft = gunSystem.GetBulletLeftInMagazine();
        Debug.Log(bulletLeft);
    }

    public void WeaponChange(InventoryItem inventoryItem)
    {
        if(bulletLeft != -1)
        {
            gunSystem.SetBulletLeftInMagazine(bulletLeft);
            gunSystem.isSetBullet = true;
        }
    }

    public int GetScore()
    {
        int score = 0;
        foreach(var grid in itemGrids)
        {
            score += grid.GetGridScore();
        }
        return score;
    }

    public int GetIScore()
    {
        return iScore;
    }

    public void UpdateScore(int toUpdate)
    {
        iScore += toUpdate;
        try {OnScoreChange?.Invoke();}
        catch {}
    }
}
