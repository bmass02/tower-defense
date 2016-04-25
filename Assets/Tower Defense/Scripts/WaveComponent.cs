using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaveComponent
{
    public EnemyParams eParams;
    public int num;
    [System.NonSerialized]
    public int spawned = 0;
}
