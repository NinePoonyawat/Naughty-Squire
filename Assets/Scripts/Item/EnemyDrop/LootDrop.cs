using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField] private DropData dropPool;
    [SerializeField] private GameObject destroyPrefab;
    private Transform pickableParent;

    public void Drop()
    {
        Instantiate(destroyPrefab, transform.position, transform.rotation);
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
