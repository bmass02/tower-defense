using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {

	public GameObject selectedTower;

	public GameObject basicTower;
	public GameObject rocketTower;

	GameObject basicTowerButton;
	GameObject rocketTowerButton;

	GameObject[] existingBasicTowers;
	GameObject[] existingRocketTowers;

	public AudioClip buildTowerSound;
	public AudioClip upgradeTowerSound;
	public AudioClip notEnoughMoneySound;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		basicTowerButton = GameObject.Find ("BasicTowerButton");
		rocketTowerButton = GameObject.Find ("RocketTowerButton");
	}

	// Update is called once per frame
	void Update () {
		//Change color of last selected button & play SFX
		if (selectedTower.gameObject.name == "Basic Tower") {
			basicTowerButton.GetComponent<Image> ().color = Color.green;
			rocketTowerButton.GetComponent<Image> ().color = Color.white;
		} else if (selectedTower.gameObject.name == "Rocket Tower") {
			rocketTowerButton.GetComponent<Image> ().color = Color.green;
			basicTowerButton.GetComponent<Image> ().color = Color.white;
		}
	}

	public void SelectTowerType(GameObject prefab) {
		selectedTower = prefab;
	}

	//when upgrade button is selected
	public void UpgradeSelectedTowerType(GameObject prefab)
	{
		prefab = selectedTower;
		if (prefab != null) {
			ScoreManager sm = GameObject.FindObjectOfType<ScoreManager> ();
			if (sm.money < selectedTower.GetComponent<Tower> ().upgradeCost) {
				source.PlayOneShot (notEnoughMoneySound);
				sm.StartCoroutine (sm.ShowMessage (sm.moneyUpdateText, "Not Enough Money To Upgrade " + selectedTower.gameObject.name + "!", 2f));
				return;
			}

			source.PlayOneShot (upgradeTowerSound, 1f);

			sm.money -= prefab.GetComponent<Tower> ().upgradeCost;
			sm.StartCoroutine(sm.ShowMessage(sm.moneyUpdateText, "- $" + selectedTower.GetComponent<Tower>().upgradeCost, 2f));


			if (selectedTower.gameObject.name == "Basic Tower") {
				//Upgrade existing towers
				existingBasicTowers = GameObject.FindGameObjectsWithTag ("Basic Tower");
				foreach (GameObject t in existingBasicTowers) {
					t.GetComponent<Tower> ().damage = t.GetComponent<Tower> ().damage + 20f;
					t.GetComponent<Tower> ().range = t.GetComponent<Tower> ().range + 3f;
					//t.GetComponent<Tower> ().fireCooldown = t.GetComponent<Tower> ().fireCooldown - .1f;
				}
			} else if (selectedTower.gameObject.name == "Rocket Tower") {
				existingRocketTowers = GameObject.FindGameObjectsWithTag ("Rocket Tower");
				foreach (GameObject t in existingRocketTowers) {
					t.GetComponent<Tower> ().damage = t.GetComponent<Tower> ().damage + 20f;
					t.GetComponent<Tower> ().range = t.GetComponent<Tower> ().range + 2f;
					//t.GetComponent<Tower> ().fireCooldown = t.GetComponent<Tower> ().fireCooldown - .1f;
					t.GetComponent<Tower> ().radius = t.GetComponent<Tower> ().radius + 1f;
				}
			}	

			//Upgrade Prefabs
			selectedTower.GetComponent<Tower> ().damage = selectedTower.GetComponent<Tower> ().damage + 20f;
			selectedTower.GetComponent<Tower> ().range = selectedTower.GetComponent<Tower> ().range + 2f;
			//selectedTower.GetComponent<Tower> ().fireCooldown = selectedTower.GetComponent<Tower> ().fireCooldown - .1f;

			//basic tower doesnt have splash damage
			if (selectedTower.GetComponent<Tower> ().gameObject.name != "Basic Tower") {
				selectedTower.GetComponent<Tower> ().radius = selectedTower.GetComponent<Tower> ().radius + 1f;
			}
		}
	}

	//Update stats GUI
	public GUIStyle statsGUI;
	void OnGUI()
	{
		GUI.Box (new Rect (Screen.width - 410, Screen.height - 240, 190, 150), selectedTower.GetComponent<Tower> ().gameObject.name + " Stats", statsGUI);
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 210, 180, 280), "Cost: $" + selectedTower.GetComponent<Tower> ().cost, statsGUI);
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 175, 180, 280), "Damage: " + selectedTower.GetComponent<Tower> ().damage, statsGUI);
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 140, 180, 280), "Range: " + selectedTower.GetComponent<Tower> ().range + " units", statsGUI);
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 105, 180, 280), "Blast Radius: " + selectedTower.GetComponent<Tower> ().radius + " units", statsGUI);
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 70, 180, 280), "Rate of Fire: " + selectedTower.GetComponent<Tower> ().fireCooldown + " seconds", statsGUI); // Cool down time
		GUI.Label (new Rect (Screen.width - 390, Screen.height - 35, 180, 280), "Upgrade Cost: $" + selectedTower.GetComponent<Tower> ().upgradeCost, statsGUI);
	}

	//janky way to make sure towers are reset to base stats when the application quits
	void OnApplicationQuit()
	{
		basicTower.GetComponent<Tower> ().range = 13;
		basicTower.GetComponent<Tower> ().damage = 35;
		basicTower.GetComponent<Tower> ().radius = 0;
		basicTower.GetComponent<Tower> ().fireCooldown = .75f;

		rocketTower.GetComponent<Tower> ().range = 10;
		rocketTower.GetComponent<Tower> ().damage = 70;
		rocketTower.GetComponent<Tower> ().radius = 3;
		rocketTower.GetComponent<Tower> ().fireCooldown = 1.25f;
	}
		
}
