using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

	public int lives = 20;
	public int money = 100;
	private bool dead = false;

	public Text moneyText;
	public Text livesText;
	public Text moneyUpdateText;
    private bool fastForward = false;

	public Image fadePlane;
	public GameObject gameUI;
	public GameObject gameOverUI;

	GameObject playerCrystal;
	public AudioClip damagePlayerSound;
	private AudioSource source;

	public GameObject hugeExplosionPrefab;

	void Start()
	{
		playerCrystal = GameObject.FindGameObjectWithTag ("Player");
		source = GetComponent<AudioSource> ();
	}

	public void LoseLife(int l) {
		if (!dead) {
			lives -= l;
			source.PlayOneShot (damagePlayerSound);
			if (lives <= 0) {
				GameOver ();
			}
		}
	}

	public void GameOver() {
		Debug.Log("Game Over");

		Instantiate (hugeExplosionPrefab, playerCrystal.transform.position, playerCrystal.transform.rotation);
		Destroy (playerCrystal);

		GameObject.Find ("EnemySpawner").GetComponent<EnemySpawner> ().enabled = false;

		StartCoroutine (FadeToGameOverUI(Color.clear, Color.black, 1));
		gameOverUI.SetActive (true);
		gameUI.SetActive (false);

		dead = true;

		
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

	IEnumerator FadeToGameOverUI(Color from, Color to, float time)
	{
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp (from, to, percent);
			yield return null;
		}
	}

    public void FastForward()
    {
        if (fastForward)
        {
            Time.timeScale = 1.0f;
            fastForward = false;
        } else
        {
            Time.timeScale = 4.0f;
            fastForward = true;
        }
    }
}
