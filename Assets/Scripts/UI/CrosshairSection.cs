using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairSection : MonoBehaviour
{
    private Transform crosshairTransform;
    private Image crosshairImage;
    private Color crosshairColor;

    [SerializeField] private Color defaultColor = new Color(0.3f,0.3f,0.3f);
    [SerializeField] private Color outOfAmmoColor = Color.red;

    private Vector3 defaultRange;

    private float speed = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        crosshairTransform = gameObject.transform;
        crosshairImage = gameObject.GetComponent<Image>();

        crosshairImage.color = defaultColor;

        defaultRange = crosshairTransform.localPosition;
    }

    void Update()
    {
        crosshairTransform.localPosition = Vector3.MoveTowards(crosshairTransform.localPosition, Vector3.Lerp(crosshairTransform.localPosition,defaultRange,1f),speed);
    }

    public void OutOfAmmo()
    {
        crosshairImage.color = outOfAmmoColor;
    }

    public void Reload()
    {
        crosshairImage.color = defaultColor;
    }

    public void Recoil(float recoil)
    {
        crosshairTransform.localPosition = defaultRange * recoil;
    }
}
