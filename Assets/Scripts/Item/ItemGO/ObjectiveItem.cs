using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveItem : InteractableItem
{
    [Header("Settings")]
    [SerializeField] private bool isGlow = false;
    [SerializeField] private GameObject auraPrefab;
    [SerializeField] private GameObject aura;
    private enum ObjectiveType { EXIT }
    [SerializeField] private ObjectiveType objectiveType;
    [SerializeField] private ObjectiveData objectiveData;
    [SerializeField] private bool isTutorial = false;

    public override void Glow(bool open)
    {
        if (!isGlow && open) {
            aura = Instantiate(auraPrefab, transform.position, Quaternion.identity, transform);
            isGlow = true;
        }
        if (isGlow && !open)
        {
            Destroy(aura);
            isGlow = false;
        }
    }

    public override void Interacted()
    {
        InventoryController inventoryController = GameObject.Find("InventoryController").GetComponent<InventoryController>();
        objectiveData.SetInventoryScore(inventoryController.GetScore());
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!isTutorial) SceneManager.LoadScene (sceneName:"SummaryScene");
        else SceneManager.LoadScene (sceneName:"LobbyScene");
    }
}
