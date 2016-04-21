﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

	float spawnCD = 0.5f; //time between each enemy
	float spawnCDremaining = 5; // time between each wave
	public int waveCountdown;

	private List<WaveComponent> waveComps;
    public GameObject levelCreatorObj;
    private LevelCreator levelCreator; 

	// Use this for initialization
	void Start () {
		spawnCDremaining = 5;
		waveCountdown = (int)spawnCDremaining;
        levelCreator = levelCreatorObj.GetComponent<LevelCreator>();
        waveComps = levelCreator.createSpawnSeq();
	}
		
	// Update is called once per frame
	void Update () {
		spawnCDremaining -= Time.deltaTime;
		waveCountdown = (int)spawnCDremaining;

		var e = GameObject.FindGameObjectsWithTag ("Enemy"); //get array of enemies currently in scene

		if(spawnCDremaining < 0) {
			spawnCDremaining = spawnCD;

			bool didSpawn = false;


			// Go through the wave comps until we find something to spawn;
			foreach(WaveComponent wc in waveComps) {
				if(wc.spawned < wc.num) {
					// Spawn it!
					wc.spawned++;
					Instantiate(wc.enemyPrefab, this.transform.position, this.transform.rotation);

					didSpawn = true;
					break;
				}
			}


			if(didSpawn == false && e.Length == 0) { // if there are no more enemies, move to next spawner and destroy current spawner
				// wave has finished spawning
				if(transform.parent.childCount > 1) {
					transform.parent.GetChild(1).gameObject.SetActive(true);
				}
				else {
					// Last wave
					// what should we do?
				}
				Destroy (gameObject);
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
		if (waveCountdown != 0) {
			GUI.Label (new Rect (Screen.width / 2 - 150, 50, 300, 100), "Wave in: " + waveCountdown.ToString (), countdownStyle);
		} else {
			GUI.Label (new Rect (Screen.width / 2 - 150, 50, 300, 100), "Begin!", countdownStyle);
		}
	}
}
