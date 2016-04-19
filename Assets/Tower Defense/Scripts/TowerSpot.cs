using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerSpot : MonoBehaviour {

	private AudioSource source;

	void Start()
	{
		source = GetComponent<AudioSource> ();
	}

	void OnMouseUp() {
		//Debug.Log("Towerspot clicked");

		BuildingManager bm = GameObject.FindObjectOfType<BuildingManager>();
		if(bm.selectedTower != null) {
			ScoreManager sm = GameObject.FindObjectOfType<ScoreManager>();
			if(sm.money < bm.selectedTower.GetComponent<Tower>().cost) {
				source.PlayOneShot (bm.notEnoughMoneySound);
				sm.StartCoroutine(sm.ShowMessage(sm.moneyUpdateText, "Not Enough Money To Build " + bm.selectedTower.gameObject.name + "!", 2f));
				return;
			}

			sm.money -= bm.selectedTower.GetComponent<Tower>().cost;
			sm.StartCoroutine(sm.ShowMessage(sm.moneyUpdateText, "- $" + bm.selectedTower.GetComponent<Tower>().cost, 2f));
		}
			
		source.PlayOneShot (bm.buildTowerSound, 1);
		Instantiate(bm.selectedTower, transform.parent.position, transform.parent.rotation); // create tower
		Destroy(transform.parent.gameObject); // delete platform
	}
}
