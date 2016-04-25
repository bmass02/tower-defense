using UnityEngine;
using System.Collections;

[System.Serializable]
public class EnemyParams {
    public float speed = 5f;
    public float health = 50f;
    public int moneyValue = 5;
    public int lifeValue = 1;
    public bool actuallyDie = true;
    public EnemyParams nextParams = null;
    public GameObject emptyEnemy;
    public GameObject deadEnemy;
    public GameObject explosion;

    public EnemyParams Clone()
    {
        EnemyParams clone = new EnemyParams();
        clone.speed = speed;
        clone.health = health;
        clone.moneyValue = moneyValue;
        clone.lifeValue = lifeValue;
        clone.actuallyDie = actuallyDie;
        if(nextParams != null)
        {
            clone.nextParams = nextParams.Clone();
        }
        clone.emptyEnemy = emptyEnemy;
        clone.deadEnemy = deadEnemy;
        clone.explosion = explosion;
        return clone;
    }
}
