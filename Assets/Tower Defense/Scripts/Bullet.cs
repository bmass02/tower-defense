using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float speed = 15f;
	public Transform target;
	public float damage = 1f;
	public float radius = 0;

	public float volRangeLow = .1f;
	public float volRangeHigh = .3f;

	public AudioClip bulletHitSound;
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
		source.PlayOneShot (bulletHitSound, vol);

		if(radius == 0) {
			target.GetComponent<Enemy>().TakeDamage(damage);
		}
		else {
			Collider[] cols = Physics.OverlapSphere(transform.position, radius); // returns list of colliders we've hit

			foreach(Collider c in cols) {
				Enemy e = c.GetComponent<Enemy>();
				if(e != null) {
					e.GetComponent<Enemy>().TakeDamage(damage);
				}
			}
		}

		GetComponent<Bullet> ().enabled = false;
		GetComponentInChildren<MeshRenderer> ().enabled = false;

		// Maybe spawn a cool "explosion" object here?
		Destroy(gameObject, bulletHitSound.length);
	}
}
