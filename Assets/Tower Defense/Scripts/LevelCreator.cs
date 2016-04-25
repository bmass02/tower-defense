using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class LevelCreator : MonoBehaviour {
    public GameObject deadEnemy;
    public GameObject emptyEnemy;
    public GameObject explosionPrefab;
    public GameObject pathParent;
    private int level = 0;
    private int baseEnemyCount = 10;
    private int stage = 0;
    private int maxStages = 5;
    private bool started = false;
    private List<EnemyParams> enemyParams = new List<EnemyParams>();
    
	// Use this for initialization
	void Start () {
        if (!started)
        {
            EnemyParams firstEnemy = new EnemyParams();
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

    public void createPath()
    {

    }

    public List<WaveComponent> createSpawnSeq()
    {
        if (!started) { Start(); }
        int lastIndex = enemyParams.Count - 1;
        List<WaveComponent> wave = new List<WaveComponent>();
        int levelEnemies = baseEnemyCount + level;
        WaveComponent normals = new WaveComponent();
        normals.eParams = enemyParams[lastIndex - 1];
        normals.num = levelEnemies - stage;
        wave.Add(normals);

        WaveComponent elites = new WaveComponent();
        elites.eParams = enemyParams[lastIndex];
        elites.num = stage;
        wave.Add(elites);

        nextStage();

        return wave;
    }

    private void nextStage()
    {
        stage++;
        createNewEnemy();
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

    public int currentLevel()
    {
        return level+1;
    }

    private void createNewEnemy()
    {
        int numEnemies = enemyParams.Count;
        EnemyParams prevEnemy = enemyParams[numEnemies - 1];
        EnemyParams newEnemy = new EnemyParams();
        newEnemy.health = prevEnemy.health + 5;
        newEnemy.lifeValue = prevEnemy.lifeValue + 1;
        newEnemy.moneyValue = prevEnemy.moneyValue + 5;
        newEnemy.speed += level;
        newEnemy.actuallyDie = false;
        newEnemy.nextParams = prevEnemy;
        newEnemy.emptyEnemy = emptyEnemy;
        newEnemy.deadEnemy = deadEnemy;
        newEnemy.explosion = explosionPrefab;
        enemyParams.Add(newEnemy);
    }
}
