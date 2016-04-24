using UnityEngine;
using System.Collections;

public class PlayerCrystalRotation : MonoBehaviour {

	float angle = 360.0f;
	public float time = 1.0f;
	Vector3 axis = Vector3.up;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		this.GetComponent<Transform> ().RotateAround (this.transform.position, axis, angle * Time.deltaTime / time);
	}	
}
