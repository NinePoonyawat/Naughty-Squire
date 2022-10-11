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

    [SerializeField] private bool isRecoil = true;

    private Vector3 defaultScale;

    private float speed = 0.005f;

    // Start is called before the first frame update
    void Awake()
    {
        crosshairTransform = gameObject.transform;
        crosshairImage = gameObject.GetComponent<Image>();

        crosshairImage.color = defaultColor;

        defaultScale = crosshairTransform.localScale;
    }

    void Update()
    {
        if (crosshairTransform.localScale == defaultScale) return;
        crosshairTransform.localScale -= Vector3.one * speed;
        if (crosshairTransform.localScale.x < defaultScale.x) crosshairTransform.localScale = defaultScale;
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
        if (!isRecoil) return;
        crosshairTransform.localScale = defaultScale * recoil;
    }
}
