using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {
    public GameObject elite;
    public GameObject normal;
    private int level = 0;
    private int baseEnemyCount = 10;

    private class Stage
    {
        private int stage;
        private int max = 5;
        public Stage()
        {
            stage = 0;
        }

        public void next()
        {
            stage = (stage + 1) % max;
        }

        public int current()
        {
            return stage+1;
        }
    }
    private Stage stageManager = new Stage();
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
        normals.num = levelEnemies - stageManager.current();
        wave.Add(normals);

        WaveComponent elites = new WaveComponent();
        elites.enemyPrefab = elite;
        elites.num = stageManager.current();
        wave.Add(elites);

        stageManager.next();

        return wave;
    }
}
