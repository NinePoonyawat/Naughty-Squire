using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapmine : MonoBehaviour
{
    void Update()
    {
        
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
