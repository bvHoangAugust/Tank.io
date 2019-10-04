using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TankModel
{
    public int health;
    public int baseHP;

    public float moveSpeed;
    public float barrelRotateSpeed;

    public float speedBullet;
    public int dame;
    public float maxTimeDelayShoot;
    public float timeShoot;
}
