using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public abstract void Glow(bool open);
    public abstract void Interacted();
}
