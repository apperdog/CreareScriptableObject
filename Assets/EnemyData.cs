using UnityEngine;
using System.Collections;
using System;

public class EnemyData : DataBase
{
    public int id;
    public int hp;
    public int hitCount;
    public int damage;
    public int agility;
    public int xp;
    public int gold;

    public override void SetData(string[] data)
    {
        if (data.Length == 7)
        {
            id = Convert.ToInt32(data[0]);
            hp = Convert.ToInt32(data[1]);
            hitCount = Convert.ToInt32(data[2]);
            damage = Convert.ToInt32(data[3]);
            agility = Convert.ToInt32(data[4]);
            xp = Convert.ToInt32(data[5]);
            gold = Convert.ToInt32(data[6]);
        }
    }
}