using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {
    public GameObject deadEnemy;
    public GameObject emptyEnemy;
    public GameObject explosionPrefab;
    public GameObject pathParent;
    public GameObject towerSpotPrefab;
    private int level = 1;
    private int baseEnemyCount = 10;
    private int stage = 0;
    private int maxStages = 5;
    private bool started = false;
    private List<EnemyParams> enemyParams = new List<EnemyParams>();
    private float RED = 1.0f;
    private float GREEN = 1.0f;
    private float BLUE = 1.0f;
    private int whichColor = 0;
    private float towerSpotHeight = 3.17f;
    
	// Use this for initialization
	void Start () {
        if (!started)
        {
            placeTowers(PlayerPrefs.GetString("Mode"));
            EnemyParams firstEnemy = new EnemyParams();
            firstEnemy.objColor = newColor();
            firstEnemy.emptyEnemy = emptyEnemy;
            firstEnemy.deadEnemy = deadEnemy;
            firstEnemy.explosion = explosionPrefab;
            enemyParams.Add(firstEnemy);
            createNewEnemy();
            started = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void placeTowers(string levelName)
    {
        List<GameObject> towerSpots = new List<GameObject>(GameObject.FindGameObjectsWithTag("Story"));
        if(levelName == "Infinite")
        {
            towerSpots.AddRange(GameObject.FindGameObjectsWithTag("Infinite"));
        }
        foreach(GameObject towerSpot in towerSpots)
        {
            Transform towerTransform = towerSpot.transform;
            Instantiate(towerSpotPrefab, new Vector3(towerTransform.position.x, towerSpotHeight, towerTransform.position.z), towerTransform.rotation);
        }
    }

    public List<WaveComponent> createSpawnSeq()
    {
        if (!started) { Start(); }
        List<WaveComponent> wave = new List<WaveComponent>();
        int levelEnemies = baseEnemyCount + level;
        WaveComponent normals = new WaveComponent();
        normals.eParams = enemyParams[level - 1];
        normals.num = levelEnemies - stage;
        wave.Add(normals);

        WaveComponent elites = new WaveComponent();
        elites.eParams = enemyParams[level];
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
            while(enemyParams.Count <= level)
            {
                createNewEnemy();
            }
        }
    }
    private void nextLevel()
    {
        level++;
    }

    public int currentLevel()
    {
        return level;
    }

    private void createNewEnemy()
    {
        int numEnemies = enemyParams.Count;
        EnemyParams prevEnemy = enemyParams[numEnemies - 1];
        EnemyParams newEnemy = new EnemyParams();
        newEnemy.health = prevEnemy.health + 50;
        newEnemy.lifeValue = prevEnemy.lifeValue + 1;
        newEnemy.moneyValue = prevEnemy.moneyValue + 5;
        newEnemy.speed += (level-1)*2;
        newEnemy.actuallyDie = false;
        newEnemy.objColor = newColor();
        newEnemy.nextParams = prevEnemy;
        newEnemy.emptyEnemy = emptyEnemy;
        newEnemy.deadEnemy = deadEnemy;
        newEnemy.explosion = explosionPrefab;
        enemyParams.Add(newEnemy);
    }

    private Color newColor()
    {
        if(BLUE > 0.0f && whichColor == 0)
        {
            BLUE -= 0.87f;
            if (BLUE < 0.0f) { BLUE += 1.0f; }
        } else if (GREEN > 0.0f && whichColor == 1)
        {
            GREEN -= 0.43f;
            if (GREEN < 0.0f) { GREEN += 1.0f; }
        } else if (RED > 0.0f && whichColor == 2)
        {
            RED -= 0.23f;
            if(RED < 0.0f) { RED += 1.0f; }
        }
        whichColor += 2;
        whichColor = whichColor % 3;
        return new Color(RED, GREEN, BLUE);
    } 
}
