using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 15f;
	public Transform target;
	public float damage = 1f;
	public float radius = 0;

	float volRangeLow = .1f;
	float volRangeHigh = .3f;

	public GameObject explosionPrefab;

	public AudioClip bulletHitSound;
	public AudioClip rocketHitSound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null) {
			// Our enemy went away!
			Destroy(gameObject);
			return;
		}


		Vector3 dir = target.position - this.transform.localPosition;

		float distThisFrame = speed * Time.deltaTime;

		if(dir.magnitude <= distThisFrame) {
			// We reached the node
			DoBulletHit();
		}
		else {
			// TODO: Consider ways to smooth this motion.

			// Move towards node
			transform.Translate( dir.normalized * distThisFrame, Space.World );
			Quaternion targetRotation = Quaternion.LookRotation( dir );
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime*5);
		}

	}

	void DoBulletHit() {
		// TODO:  What if it's an exploding bullet with an area of effect?

		float vol = Random.Range (volRangeLow, volRangeHigh);


		if(radius == 0) {
			target.GetComponent<Enemy>().TakeDamage(damage);

			source.PlayOneShot (bulletHitSound, vol);

			GetComponent<Bullet> ().enabled = false;
			GetComponentInChildren<MeshRenderer> ().enabled = false;

			Destroy(gameObject, bulletHitSound.length);
		}
		else {
			Collider[] cols = Physics.OverlapSphere(transform.position, radius); // returns list of colliders we've hit

			foreach(Collider c in cols) {
				Enemy e = c.GetComponent<Enemy>();
				if(e != null) {
					e.GetComponent<Enemy>().TakeDamage(damage);
				}
			}

			Instantiate (explosionPrefab, this.transform.position, this.transform.rotation); // create explosion

			source.PlayOneShot (rocketHitSound, vol);

			GetComponent<Bullet> ().enabled = false;
			GetComponentInChildren<MeshRenderer> ().enabled = false;

			Destroy(gameObject, rocketHitSound.length);
		}


	}
}
