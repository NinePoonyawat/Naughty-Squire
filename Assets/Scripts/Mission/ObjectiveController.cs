using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveController : MonoBehaviour
{
    private GameObject main;
    private TimingObjective timingObjective;
    private ShootingObjective shootingObjective;
    private InventoryObjective inventoryObjective;
    private FindingObjective findingObjective1;
    private FindingObjective findingObjective2;
    private FindingObjective findingObjective3;

    [SerializeField] private GameObject UIObjective;
    private int totalScore;

    [SerializeField] private List<ItemData> itemData;

    [SerializeField] public ObjectiveData objectiveData;

    void Awake()
    {
        main = this.gameObject;
        timingObjective =  main.AddComponent<TimingObjective>();
        shootingObjective = main.AddComponent<ShootingObjective>();
        inventoryObjective = main.AddComponent<InventoryObjective>();
        findingObjective1 = main.AddComponent<FindingObjective>();
        findingObjective2 = main.AddComponent<FindingObjective>();
        findingObjective3 = main.AddComponent<FindingObjective>();
        totalScore = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        // main = this.gameObject;
        // timingObjective =  main.AddComponent<TimingObjective>();
        // shootingObjective = main.AddComponent<ShootingObjective>();
        // findingObjective1 = main.AddComponent<FindingObjective>();
        // findingObjective2 = main.AddComponent<FindingObjective>();
        // findingObjective3 = main.AddComponent<FindingObjective>();
        // totalScore = 0;

        timingObjective.uiText = UIObjective.transform.Find("TimingObjective").GetComponent<TMP_Text>();
        shootingObjective.uiText = UIObjective.transform.Find("ShootingObjective").GetComponent<TMP_Text>();
        findingObjective1.uiText = UIObjective.transform.Find("FindingObjective1").GetComponent<TMP_Text>();
        findingObjective2.uiText = UIObjective.transform.Find("FindingObjective2").GetComponent<TMP_Text>();
        findingObjective3.uiText = UIObjective.transform.Find("FindingObjective3").GetComponent<TMP_Text>();

        timingObjective.objectiveData = objectiveData;
        shootingObjective.objectiveData = objectiveData;
        findingObjective1.objectiveData = objectiveData;
        findingObjective2.objectiveData = objectiveData;
        findingObjective3.objectiveData = objectiveData;

        timingObjective.objectiveIdx = 0;
        shootingObjective.objectiveIdx = 1;
        findingObjective1.objectiveIdx = 2;
        findingObjective2.objectiveIdx = 3;
        findingObjective3.objectiveIdx = 4;

        SetFindingItem();

        UIObjective.transform.Find("FindingImage1").GetComponent<RawImage>().texture = findingObjective1.getTexture();
        UIObjective.transform.Find("FindingImage2").GetComponent<RawImage>().texture = findingObjective2.getTexture();
        UIObjective.transform.Find("FindingImage3").GetComponent<RawImage>().texture = findingObjective3.getTexture();

        findingObjective1.SetQuantity();
        findingObjective2.SetQuantity();
        findingObjective3.SetQuantity();

        timingObjective.UpdateText();
        shootingObjective.UpdateText();
        findingObjective1.UpdateText();
        findingObjective2.UpdateText();
        findingObjective3.UpdateText();

        timingObjective.UpdateObjectiveData();
        shootingObjective.UpdateObjectiveData();
        findingObjective1.UpdateObjectiveData();
        findingObjective2.UpdateObjectiveData();
        findingObjective3.UpdateObjectiveData();
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

    public bool SetIScore(int newIScore)
    {
        if(inventoryObjective != null)
        {
            inventoryObjective.SetIScore(newIScore);
            return true;
        }
        return false;
    }
}