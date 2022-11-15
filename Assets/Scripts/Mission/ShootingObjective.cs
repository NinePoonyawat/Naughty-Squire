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
    }

    void Start()
    {
        shootCount = 0;
        if (score[0] == 0 && score[1] == 0 && score[2] == 0)
        {
            score[0] = 100;
            score[1] = 150;
            score[2] = 200;
        }
        if (shoot[0] == 0 && shoot[1] == 0 && shoot[2] == 0)
        {
            shoot[0] = 150;
            shoot[1] = 250;
            shoot[2] = 300;
        }
        LevelShoot = shoot[0];
        LevelRank = 0;
    }

    // Update is called once per frame

    public void onShoot(float recoil)
    {
        shootCount++;
        if (level != CompleteLevel.Fail) return;
        if (shootCount == LevelShoot)
        {
            Relagations();
            LevelRank++;
            LevelShoot = shoot[LevelRank];
        }
        Debug.Log(shootCount);
    }

    public int GetShoot()
    {
        return shootCount;
    }
}
