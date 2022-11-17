using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveController : MonoBehaviour
{
    private GameObject main;
    private TimingObjective timingObjective;
    private ShootingObjective shootingObjective;
    private FindingObjective findingObjective1;
    private FindingObjective findingObjective2;
    private FindingObjective findingObjective3;

    [SerializeField] private GameObject UIObjective;
    private int totalScore;

    [SerializeField] private List<ItemData> itemData;

    // Start is called before the first frame update
    void Start()
    {
        main = this.gameObject;
        timingObjective =  main.AddComponent<TimingObjective>();
        shootingObjective = main.AddComponent<ShootingObjective>();
        findingObjective1 = main.AddComponent<FindingObjective>();
        findingObjective2 = main.AddComponent<FindingObjective>();
        findingObjective3 = main.AddComponent<FindingObjective>();
        totalScore = 0;
        SetFindingItem();

        timingObjective.text = UIObjective.transform.Find("TimingObjective").GetComponent<TMP_Text>();
        shootingObjective.text = UIObjective.transform.Find("ShootingObjective").GetComponent<TMP_Text>();
        findingObjective1.text = UIObjective.transform.Find("FindingObjective1").GetComponent<TMP_Text>();
        findingObjective2.text = UIObjective.transform.Find("FindingObjective2").GetComponent<TMP_Text>();
        findingObjective3.text = UIObjective.transform.Find("FindingObjective3").GetComponent<TMP_Text>();
    }

    public void SetFindingItem()
    {
        int a = Random.Range(0,itemData.Count);
        int b = Random.Range(0,itemData.Count);
        int c = Random.Range(0,itemData.Count);
        while (a == b)
        {
            b = Random.Range(0,itemData.Count);
        }
        while (a == c || b == c)
        {
            c = Random.Range(0,itemData.Count);
        }
        findingObjective1.SetItem(itemData[a]);
        findingObjective2.SetItem(itemData[b]);
        findingObjective3.SetItem(itemData[c]);
    }
}