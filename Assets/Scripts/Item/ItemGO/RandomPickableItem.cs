using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPickableItem : MonoBehaviour
{
    private Transform pickableParent;
    public DropData dropPool;

    void Start()
    {
        pickableParent = GameObject.Find("PickableItem").transform;
        
        float sum = 0;
        float random = Random.Range(0f, dropPool.poolRate);
        
        foreach (ItemDropRate item in dropPool.items)
        {
            sum += item.ratio;
            if (random <= sum)
            {
                Instantiate(item.droppedItem, transform.position, transform.rotation, pickableParent);
                return;
            }
        }
    }
}
