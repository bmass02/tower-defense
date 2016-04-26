using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

	float spawnCD = 0.5f; //time between each enemy
	float spawnCDremaining = 7.0f; // time between each wave
	public int waveCountdown;

	private List<WaveComponent> waveComps;
    public GameObject levelCreatorObj;
    public GameObject emptyEnemy;
    private LevelCreator levelCreator;
    private bool created = false;

	// Use this for initialization
	void Start () {
        Debug.Log("start");
		spawnCDremaining = 7.0f;
		waveCountdown = (int)spawnCDremaining;
        levelCreator = levelCreatorObj.GetComponent<LevelCreator>();
        waveComps = levelCreator.createSpawnSeq();
        created = true;
	}
		
	// Update is called once per frame
	void Update () {
		spawnCDremaining -= Time.deltaTime;
		waveCountdown = (int)spawnCDremaining;

		var e = GameObject.FindGameObjectsWithTag ("Enemy"); //get array of enemies currently in scene

		if(spawnCDremaining < 0 && created) {
			spawnCDremaining = spawnCD;

			bool didSpawn = false;


			// Go through the wave comps until we find something to spawn;
			foreach(WaveComponent wc in waveComps) {
				if(wc.spawned < wc.num)
                {
                    Debug.LogWarning("spawning");
                    // Spawn it!
                    wc.spawned++;
					GameObject newEnemy = Instantiate(wc.eParams.emptyEnemy, this.transform.position, this.transform.rotation) as GameObject;
                    MeshRenderer myRenderer = newEnemy.GetComponentInChildren<MeshRenderer>();
                    Material newMaterial = new Material(Shader.Find("Standard"));
                    newMaterial.color = wc.eParams.objColor;
                    myRenderer.material = newMaterial;
                    Enemy eScript = newEnemy.AddComponent<Enemy>();
                    eScript.myParams = wc.eParams.Clone();

					didSpawn = true;
					break;
				}
			}


			if(didSpawn == false && e.Length == 0) { // if there are no more enemies, move to next spawner and destroy current spawner
                created = false;
                spawnCDremaining = 7.0f;
                waveComps = levelCreator.createSpawnSeq();
                created = true;
                // wave has finished spawning
                //if(transform.parent.childCount > 1) {
                //	transform.parent.GetChild(1).gameObject.SetActive(true);
                //}
                //else {
                //	// Last wave
                //	// what should we do?
                //}
                //Destroy (gameObject);
            }
		}
	}

	//gui was buggy inside the score manager, so now it is attached to the spawner
	public GUIStyle countdownStyle;
	void OnGUI()
	{
		countdownStyle.fontSize = 30;
		countdownStyle.alignment = TextAnchor.MiddleCenter;
		countdownStyle.normal.textColor = Color.white;
        if (waveCountdown > 5.0f) {
            GUI.Label(new Rect(Screen.width / 2 - 150, 50, 300, 100), "Level " + levelCreator.currentLevel().ToString(), countdownStyle);
        } else if (waveCountdown != 0) {
			GUI.Label (new Rect (Screen.width / 2 - 150, 50, 300, 100), "Wave in: " + waveCountdown.ToString (), countdownStyle);
		} else {
			GUI.Label (new Rect (Screen.width / 2 - 150, 50, 300, 100), "Begin!", countdownStyle);
		}
	}
}
