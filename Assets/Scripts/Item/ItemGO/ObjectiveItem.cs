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
        Debug.Log("Interact Laew Na : " + objectiveType);
        
        SceneManager.LoadScene (sceneName:"SummaryScene");
    }
}
