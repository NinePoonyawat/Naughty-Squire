using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Unique ItemGrid")]
    public ItemGrid selectedItemGrid;
    public ItemGrid loadoutItemGrid;
    public ItemGrid pickupItemGrid;

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [Header("Custom")]
    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    InventoryHighlight inventoryHighlight;

    private void Awake() {
        inventoryHighlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateItem(-1);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RotateItem();
        }
        
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DeleteItem();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            InsertItem(-1);
        }

        if (selectedItemGrid == null)
        {
            inventoryHighlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
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

    private void CreateItem(int selectedItemID)
    {
        if (selectedItem != null)
        {
            Destroy(selectedItem.gameObject);
            selectedItem = null;
        }

        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;
        
        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        
        if (selectedItemID == -1) selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
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

    public bool InsertItem(ItemData selectedItem)
    {
        InventoryItem itemToInsert = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        
        itemToInsert.Set(selectedItem);

        Vector2Int? posOnGrid = pickupItemGrid.FindSpaceForObject(itemToInsert);
        
        if (posOnGrid == null) { return false; }

        pickupItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);

        return true;
    }

    private void LeftMouseButtonPress()
    {
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
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {     
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            //Debug.Log(selectedItem.itemData + " " + tileGridPosition);
            FindObjectOfType<AudioManager>().Play("InventoryInteract");

            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
            }
        }
    }
    
    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }

}
