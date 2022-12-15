using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class ShootingObjective : Objective
{
    private ThirdPersonShooterController thirdPersonShooterController;

    [SerializeField] private int[] shoot = new int[3];
    private int shootCount;
    private int LevelShoot;
    private int LevelRank;

    // Start is called before the first frame update
    void Awake()
    {
        thirdPersonShooterController = GameObject.Find("Player").GetComponent<ThirdPersonShooterController>();
        thirdPersonShooterController.OnShoot += onShoot;
        level = CompleteLevel.Third;
        score[0] = 2;
        score[1] = 3;
        score[2] = 4;
    }

    void Start()
    {
        color = "#00FF00";
        shootCount = 0;

        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 2;
            score[1] = 3;
            score[2] = 4;
        }
        if (shoot[0] == 0 && shoot[1] == 0 && shoot[2] == 0)
        {
            shoot[0] = 120;
            shoot[1] = 160;
            shoot[2] = 200;
        }
        LevelShoot = shoot[0];
        LevelRank = 0;

        UpdateText();
    }

    // Update is called once per frame

    public void onShoot(float recoil)
    {
        shootCount++;
        if (shootCount == LevelShoot)
        {
            Relagations();
            LevelRank++;
            if (!(LevelRank == 3)) LevelShoot = shoot[LevelRank];
        }
        UpdateText();
    }

    public int GetShoot()
    {
        return shootCount;
    }

    public override void UpdateText()
    {
        uiText.text = "don't spend  more than " + LevelShoot + " bullets <color=" + color + "> (" + shootCount + " / " + LevelShoot + " )</color>";
    }
}
