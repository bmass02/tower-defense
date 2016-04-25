using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	GameObject pathGO;

	Transform targetPathNode;
	int pathNodeIndex = 0;

    public EnemyParams myParams;

	//public AudioClip reachedGoalSound;
	//public AudioClip deathSound; | instead the death sound is attached to the deadenemy prefab and played on awake
	//private AudioSource source;

	// Use this for initialization
	void Start () {
        pathGO = GameObject.Find("Path");
		//source = GetComponent<AudioSource> ();
	}

	void GetNextPathNode() {
		if(pathNodeIndex < pathGO.transform.childCount) {
			targetPathNode = pathGO.transform.GetChild(pathNodeIndex);
			pathNodeIndex++;
		}
		else {
			targetPathNode = null;
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

		float distThisFrame = myParams.speed * Time.deltaTime;

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

		GameObject.FindObjectOfType<ScoreManager>().LoseLife(myParams.lifeValue);
		Destroy(gameObject);
		Camera.main.GetComponent<RandomShake> ().PlayShake ();
	}

	public void TakeDamage(float damage) {
        Debug.Log("damage: " + damage.ToString());
        Debug.Log("Health: " + myParams.health.ToString());
		myParams.health -= damage;
		if(myParams.health <= 0) {
			
			Die();
		}
	}

	public void Die() {

		Instantiate (myParams.explosion, this.transform.position, this.transform.rotation);

		
        if (myParams.nextParams == null)
        {
            GameObject dead = Instantiate(myParams.deadEnemy, this.transform.position, this.transform.rotation) as GameObject;
            dead.GetComponent<Rigidbody>().AddForce(transform.forward * 250);
        } else
        {
            //Instantiate returns an Object not GameObject, needs a cast for AddForce
            GameObject g = Instantiate(myParams.emptyEnemy, this.transform.position, this.transform.rotation) as GameObject;
            Enemy gEnemy = g.AddComponent<Enemy>();
            gEnemy.myParams = myParams.nextParams;
            gEnemy.pathGO = pathGO;
            gEnemy.targetPathNode = targetPathNode;
            gEnemy.pathNodeIndex = pathNodeIndex;
        }

		GameObject.FindObjectOfType<ScoreManager>().money += myParams.moneyValue;
		Destroy(gameObject);
	}
}
