using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	GameObject pathGO;

	public GameObject explosionPrefab;

	Transform targetPathNode;
	int pathNodeIndex = 0;

	float speed = 5f;

	public float health = 1f;

	public int moneyValue = 1;

	public AudioClip deathSound;
	private AudioSource source;

	public GameObject enemyDead;

	// Use this for initialization
	void Start () {
		pathGO = GameObject.Find("Path");
		source = GetComponent<AudioSource> ();
	}

	void GetNextPathNode() {
		if(pathNodeIndex < pathGO.transform.childCount) {
			targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
			pathNodeIndex++;
		}
		else {
			targetPathNode = null;
			ReachedGoal();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(targetPathNode == null) {
			GetNextPathNode();
			if(targetPathNode == null) {
				// We've run out of path!
				ReachedGoal();
				return;
			}
		}

		Vector3 dir = targetPathNode.position - this.transform.localPosition;

		float distThisFrame = speed * Time.deltaTime;

		if(dir.magnitude <= distThisFrame) {
			// We reached the node
			targetPathNode = null;
		}
		else {
			// TODO: Consider ways to smooth this motion.

			// Move towards node
			transform.Translate( dir.normalized * distThisFrame, Space.World );
			Quaternion targetRotation = Quaternion.LookRotation( dir );
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime*5);
		}

	}

	void ReachedGoal() {
		GameObject.FindObjectOfType<ScoreManager>().LoseLife();
		Destroy(gameObject);
	}

	public void TakeDamage(float damage) {
		health -= damage;
		if(health <= 0) {
			
			Die();
		}
	}

	public void Die() {
		source.PlayOneShot (deathSound, 1);

		Instantiate (explosionPrefab, this.transform.position, this.transform.rotation);

		//Instantiate returns an Object not GameObject, needs a cast for AddForce
		GameObject g = Instantiate (enemyDead, this.transform.position, this.transform.rotation) as GameObject;
		g.GetComponent<Rigidbody> ().AddForce (transform.forward * 250);

		GameObject.FindObjectOfType<ScoreManager>().money += moneyValue;
		Destroy(gameObject);
	}
}
