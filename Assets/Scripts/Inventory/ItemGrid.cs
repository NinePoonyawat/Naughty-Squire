using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GridType {ITEMSELECTED,LOADOUT,INGAME};
public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 100;
    public const float tileSizeHeight = 100;

    InventoryItem[,] inventoryItemSlot;

    [Header("Essential")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] public int gridSizeWidth;
    [SerializeField] public int gridSizeHeight;

    public enum InventoryType {LOADOUT, BAG, HAND};
    public InventoryType inventoryType;
    public GridType gridType = GridType.INGAME;

    [Header("HAND TYPE")]
    public ItemGrid anotherHandGrid;
    public int remainSize = 1000;

    public event WeaponChangeEvent weaponChangeEvent;
    public delegate void WeaponChangeEvent(InventoryItem changeWeapon);

    public event OnPickupWeaponEvent onPickupWeaponEvent;
    public delegate void OnPickupWeaponEvent();

    public event OnPickupGrenadeEvent onPickupGrenadeEvent;
    public delegate void OnPickupGrenadeEvent();

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Init(gridSizeWidth, gridSizeHeight);

        if (inventoryType == InventoryType.HAND) remainSize = 1;
    }

    private void Update()
    {
        if (remainSize == 0)
        {
            Image image = GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.3f);
        }
        else
        {
            Image image = GetComponent<Image>();
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
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

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        if (remainSize == 0) return null;

        int height = gridSizeHeight - itemToInsert.itemData.height + 1;
        int width = gridSizeWidth - itemToInsert.itemData.width + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert.itemData.width, itemToInsert.itemData.height) == true)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public Vector2Int? FindItemInGrid(InventoryItem itemToFind)
    {
        int height = gridSizeHeight;
        int width = gridSizeWidth;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (inventoryItemSlot[x, y] == itemToFind)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public int CountItemInGrid(ItemData itemToFind)
    {
        int height = gridSizeHeight;
        int width = gridSizeWidth;
        int count = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (inventoryItemSlot[x, y] == null) { continue; }
                if (inventoryItemSlot[x, y].itemData == itemToFind)
                {
                    count++;
                }
            }
        }
        return count;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }

        //check if this is HAND and item is twohanded
        if (inventoryType == InventoryType.HAND && toReturn.itemData.isTwoHanded == true)
        {
            anotherHandGrid.remainSize = 1;
        } 

        //check if this is LOADOUT, return copy of item, so grid's item wont lost
        if (inventoryType == InventoryType.LOADOUT)
        {
            InventoryItem copyReturn = Instantiate(toReturn);
            return copyReturn;
        }

        //send data to GunSystem
        WeaponData weaponData = toReturn.itemData as WeaponData;
        GrenadeData grenadeData = toReturn.itemData as GrenadeData;
        if (inventoryType == InventoryType.HAND && weaponData != null )
        {
            onPickupWeaponEvent.Invoke();
        }
        if (inventoryType == InventoryType.HAND && grenadeData != null) {
            onPickupGrenadeEvent.Invoke();
        }


        CleanGrid(toReturn);

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
        remainSize += 1;
    }
    
    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (inventoryType == InventoryType.HAND)
        {
            if (remainSize == 0)
            {
                return false;
            }
            if (anotherHandGrid.remainSize == 0 && inventoryItem.itemData.isTwoHanded == true)
            {
                return false;
            }
            if (remainSize > 0 && inventoryItem.itemData.isTwoHanded == true)
            {
                anotherHandGrid.remainSize = 0;
            }
        }

        if (BoundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        // check if this is Loadout
        if (inventoryType == InventoryType.LOADOUT && overlapItem != null)
        {
            Destroy(inventoryItem.gameObject);
            overlapItem = null;
            return true;
        }
        //////////////////////////////

        if (overlapItem != null)
        {
            CleanGrid(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

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

        WeaponData weaponData = inventoryItem.itemData as WeaponData;
        GrenadeData grenadeData = inventoryItem.itemData as GrenadeData;
        if (weaponData != null || grenadeData != null)
        {
            weaponChangeEvent?.Invoke(inventoryItem);
        }

        remainSize -= 1;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY,BulletRecognize bulletRecognize)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

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

        WeaponData weaponData = inventoryItem.itemData as WeaponData;
        GrenadeData grenadeData = inventoryItem.itemData as GrenadeData;
        if (weaponData != null || grenadeData != null)
        {
            weaponChangeEvent?.Invoke(inventoryItem);
        }

        remainSize -= 1;

        if (bulletRecognize != null && bulletRecognize != null)
        {
            ContactMagazine(bulletRecognize.GetMagazine(),posX,posY);
        }
    }

    public ItemData InteractItem(int posX, int posY)
    {
        InventoryItem interactedItem = inventoryItemSlot[posX, posY];

        if (interactedItem == null) return null;
        
        ConsumableData consumableItem = interactedItem.itemData as ConsumableData;
        if (consumableItem != null)
        {
            if (consumableItem.consumableType == ConsumableData.ConsumableType.CONSUME)
            {
                PlayerHitbox player = GameObject.FindObjectOfType<PlayerHitbox>();
                if (!player.Consume(consumableItem)) { return null; }
                interactedItem.durable -= 1;
                if (interactedItem.durable < interactedItem.itemData.durable) interactedItem.ChangeSprite(true);
                if (interactedItem.durable <= 0) DiscardItem(posX, posY);

                FindObjectOfType<AudioManager>().Play(consumableItem.consumeSoundName);
            }
            return null;
        }

        WeaponData weaponData = interactedItem.itemData as WeaponData;
        if (weaponData != null && interactedItem.HaveMagazine())
        {
            MagazineData toReturn = interactedItem.GetMagazine();
            weaponChangeEvent?.Invoke(interactedItem);
            return toReturn;
        }

        return null;
    }

    public InventoryItem ContactItem(InventoryItem contactItem, int posX, int posY)
    {
        InventoryItem interactedItem = inventoryItemSlot[posX, posY];

        if (interactedItem == null) return contactItem;

        WeaponData interactedWeapon = interactedItem.itemData as WeaponData;
        MagazineData contactMagazine = contactItem.itemData as MagazineData;

        if (contactMagazine != null)
        {
            if (interactedWeapon != null && contactMagazine.availableWeapon == interactedWeapon && !interactedItem.HaveMagazine())
            {
                interactedItem.EquipMagazine(contactItem);
                weaponChangeEvent?.Invoke(interactedItem);
                FindObjectOfType<AudioManager>().Play(interactedWeapon.reloadSoundName);
                Destroy(contactItem.gameObject);
                return null;
            }
            if (contactMagazine.refillTool == interactedItem.itemData)
            {
                contactItem.RefillAmmo();
                interactedItem.durable -= 1;
                if (interactedItem.durable <= 0) DiscardItem(posX, posY);
                FindObjectOfType<AudioManager>().Play(contactMagazine.refillSoundName);
                return contactItem;
            }
        }
        return contactItem;
    }

    public void ContactMagazine(MagazineData contactMagazine,int posX,int posY)
    {
        InventoryItem interactedItem = inventoryItemSlot[posX, posY];
        if (interactedItem == null) return;
        WeaponData interactedWeapon = interactedItem.itemData as WeaponData;
        if (contactMagazine != null)
        {
            if (interactedWeapon != null && contactMagazine.availableWeapon == interactedWeapon && !interactedItem.HaveMagazine())
            {
                interactedItem.EquipMagazine(contactMagazine);
                weaponChangeEvent?.Invoke(interactedItem);
                FindObjectOfType<AudioManager>().Play(interactedWeapon.reloadSoundName);
            }
            if (contactMagazine.refillTool == interactedItem.itemData)
            {
                interactedItem.durable -= 1;
                if (interactedItem.durable <= 0) DiscardItem(posX, posY);
                FindObjectOfType<AudioManager>().Play(contactMagazine.refillSoundName);
            }
        }
    }

    public void DiscardItem(int posX, int posY)
    {
        InventoryItem discardedItem = inventoryItemSlot[posX, posY];

        InventoryController inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        inventoryController.UpdateScore(- discardedItem.itemData.value);

        CleanGrid(discardedItem);
        Destroy(discardedItem.gameObject);
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

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int ix = 0; ix < width; ix++)
        {
            for (int iy = 0; iy < height; iy++)
            {
                if (inventoryItemSlot[posX + ix, posY + iy] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public InventoryItem getInventory(int x,int y) {
        return inventoryItemSlot[x, y];
    }

    public int GetGridScore()
    {
        int score = 0;
        InventoryItem it;
        Dictionary<ItemData,int> data = new Dictionary<ItemData, int>();
        for(int i = 0; i != gridSizeWidth; i++)
        {
            for(int j = 0; j != gridSizeHeight; j++)
            {
                if (inventoryItemSlot[i,j] == null) continue;
                it = inventoryItemSlot[i,j];
                if(data.ContainsKey(it.itemData)) data[it.itemData] += 1;
                else data[it.itemData] = 1;
            }
        }
        foreach(var item in data)
        {
            score += item.Value / item.Key.GetSize();
        }
        return score;
    }
}
