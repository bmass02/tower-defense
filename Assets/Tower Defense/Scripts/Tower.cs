using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tower : MonoBehaviour {

	Transform turretTransform;

	public float range = 15;
	public GameObject bulletPrefab;
	public float damage = 1;
	public float radius = 0;

	public int cost = 10;
	public int upgradeCost = 30;


	public float fireCooldown = 0.75f;
	float fireCooldownLeft = 0;

	public AudioClip shootSound;
	private AudioSource source;

    public float volLow = .25f;
    public float volHigh = .75f;

	// Use this for initialization
	void Start () {
		turretTransform = transform.Find("Turret");
		source = GetComponent<AudioSource> ();

	}

	Enemy nearestEnemy = null;
	float dist = Mathf.Infinity;

	// Update is called once per frame
	void Update () {
		Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

		foreach(Enemy e in enemies) {
			float d = Vector3.Distance(this.transform.position, e.transform.position);
			if (nearestEnemy == null || d < dist) {
				nearestEnemy = e;
				dist = d;
			} 
		}

		if(nearestEnemy == null) {
			//Debug.Log("No enemies?");
			return;
		}

		Vector3 dir = nearestEnemy.transform.position - this.transform.position;

		Quaternion lookRot = Quaternion.LookRotation( dir );

		//Debug.Log(lookRot.eulerAngles.y);
		if (dir.magnitude <= range) {
			turretTransform.rotation = Quaternion.Euler( 0, lookRot.eulerAngles.y, 0 );
		}


		fireCooldownLeft -= Time.deltaTime;
		if(fireCooldownLeft <= 0 && dir.magnitude <= range) {
			fireCooldownLeft = fireCooldown;
			ShootAt(nearestEnemy);
		}

	}

	void ShootAt(Enemy e) {

        float vol = Random.Range(volLow,volHigh);

		source.PlayOneShot (shootSound, vol);
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, this.turretTransform.transform.position, this.turretTransform.transform.rotation);

		Bullet b = bulletGO.GetComponent<Bullet>();
		b.target = e.transform;
		b.damage = damage;
		b.radius = radius;
	}
}
