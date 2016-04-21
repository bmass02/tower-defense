using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {
    public GameObject elite;
    public GameObject normal;
    public GameObject pathParent;
    private int level = 0;
    private int baseEnemyCount = 10;
    private int stage = 0;
    private int maxStages = 5;
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void createPath()
    {

    }

    public List<WaveComponent> createSpawnSeq()
    {
        List<WaveComponent> wave = new List<WaveComponent>();
        int levelEnemies = baseEnemyCount + level;
        WaveComponent normals = new WaveComponent();
        normals.enemyPrefab = normal;
        normals.num = levelEnemies - stage;
        wave.Add(normals);

        WaveComponent elites = new WaveComponent();
        elites.enemyPrefab = elite;
        elites.num = stage;
        wave.Add(elites);

        nextStage();

        return wave;
    }

    private void nextStage()
    {
        stage++;
        if(stage == maxStages)
        {
            stage = 0;
            nextLevel();
        }
    }
    private void nextLevel()
    {
        level++;
    }
}
