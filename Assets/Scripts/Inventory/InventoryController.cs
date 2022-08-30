using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    // Start is called before the first frame update
    public ItemGrid selectedItemGrid;

    private void Update()
    {
        if (selectedItemGrid == null) { return; }

        //Debug.Log(selectedItemGrid.GetTileGridPosition(Input.mousePosition));
    }
}
