using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRecognize : MonoBehaviour
{
    public MagazineData magazineData;

    public void SetMagazine(MagazineData newMagazineData)
    {
        magazineData = newMagazineData;
    }

    public MagazineData GetMagazine()
    {
        return magazineData;
    }
}
