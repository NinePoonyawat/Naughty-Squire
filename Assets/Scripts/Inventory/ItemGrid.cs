using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 100;
    public const float tileSizeHeight = 100;

    InventoryItem[,] inventoryItemSlot;

    [SerializeField] RectTransform rectTransform;

    enum InventoryType {LOADOUT, BAG, HAND};
    [SerializeField] private InventoryType inventoryType;
    public ItemGrid anotherHandGrid;

    [SerializeField] public int gridSizeWidth;
    [SerializeField] public int gridSizeHeight;

    int inventorySize;

    public event PlaceItemEvent placeItemEvent;
    public delegate void PlaceItemEvent(float damage,int bulletInMagazine);

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);
    }

    private void Init(int width, int height)
    {
        inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileSizeWidth, height * tileSizeHeight);
        rectTransform.sizeDelta = size;
    }

    //GET ITEM
    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x, y];
    }

    Vector2 positionOnGrid = new Vector2();
    Vector2Int tileGridPosition = new Vector2Int();

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnGrid.x / tileSizeWidth);
        tileGridPosition.y = (int)(positionOnGrid.y / tileSizeHeight);

        return tileGridPosition; 
    }

    //PICK UP AN ITEM
    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (inventoryType == InventoryType.HAND && toReturn.itemData.isTwoHanded == true)
        {
            anotherHandGrid.inventorySize = 0;
        }

        if (toReturn == null) { return null; }

        CleanGrid(toReturn);

        inventorySize -= 1;

        return toReturn;
    }

    private void CleanGrid(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
    }
    /// PLACE DOWN AN ITEM
    // LOADOUT - can place and remove it.
    // BAG - can place.
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        // v v v v
        if (inventoryType == InventoryType.HAND)
        {
            if (inventorySize != 0)
            {
                return false;
            }
            if (anotherHandGrid.inventorySize != 0 && inventoryItem.itemData.isTwoHanded == true)
            {
                return false;
            }
            if (inventorySize == 0 && inventoryItem.itemData.isTwoHanded == true)
            {
                anotherHandGrid.inventorySize = 1;
            }
        }
        // ^ ^ ^ ^

        if (BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGrid(overlapItem);
        }

        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        if (inventoryItem.itemData as WeaponData != null)
        {
            placeItemEvent?.Invoke(20f,20);
        }

        for (int ix = 0; ix < inventoryItem.WIDTH; ix++)
        {
            for (int iy = 0; iy < inventoryItem.HEIGHT; iy++)
            {
                inventoryItemSlot[posX + ix, posY + iy] = inventoryItem;
            }
        }
        
        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;

        inventorySize += 1;

        return true;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = - (posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        
        return position;
    }

    public bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0) 
        {
            return false;
        }
        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }
        return true;
    }

    public bool BoundryCheck(int posX, int posY, int width, int height)
    {
        if(PositionCheck(posX, posY) == false) { return false; }
        if(PositionCheck(posX + width - 1, posY + height - 1) == false) { return false; }

        return true;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                if (inventoryItemSlot[posX + ix, posY + iy] != null)
                {
                    if (overlapItem == null)
                    {
                        overlapItem = inventoryItemSlot[posX + ix, posY + iy];
                    }
                    else {
                        if(overlapItem != inventoryItemSlot[posX + ix, posY + iy])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }
}
