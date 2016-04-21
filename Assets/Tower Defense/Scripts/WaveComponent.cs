using UnityEngine;
using System.Collections;

[System.Serializable]
public class WaveComponent
{
    public GameObject enemyPrefab;
    public int num;
    [System.NonSerialized]
    public int spawned = 0;
}
