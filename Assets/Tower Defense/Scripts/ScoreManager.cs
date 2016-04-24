using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int lives = 20;
	public int money = 100;

	public Text moneyText;
	public Text livesText;
	public Text moneyUpdateText;

	GameObject playerCrystal;

	public GameObject hugeExplosionPrefab;

	void Start()
	{
		playerCrystal = GameObject.FindGameObjectWithTag ("Player");
	}

	public void LoseLife(int l) {
		lives -= l;
		if(lives <= 0) {
			GameOver();
		}
	}

	public void GameOver() {
		Debug.Log("Game Over");

		Instantiate (hugeExplosionPrefab, playerCrystal.transform.position, playerCrystal.transform.rotation);

		Destroy (playerCrystal);

		GameObject.Find ("EnemySpawner").GetComponent<EnemySpawner> ().enabled = false;


		// TODO: send player to game over screen
		//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	void Update() {
		moneyText.text = "Money: $" + money.ToString();
		livesText.text = "Lives: "  + lives.ToString();
	}

	//show any message for a period of time coroutine
	public IEnumerator ShowMessage(Text updateMessageText, string message, float delay)
	{
		updateMessageText.text = message;
		updateMessageText.enabled = true;
		yield return new WaitForSeconds (delay);

		for (float f = 1f; f >= 0; f -= Time.deltaTime) {
			Color c = updateMessageText.GetComponent<CanvasRenderer> ().GetColor();
			c.a = f;
			updateMessageText.GetComponent<CanvasRenderer>().SetColor(c);
			yield return null;
		}

		updateMessageText.enabled = false;
	}
}
