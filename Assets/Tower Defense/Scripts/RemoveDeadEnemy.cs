using UnityEngine;
using System.Collections;

public class RemoveDeadEnemy : MonoBehaviour {

	public float fadeTime;
	public float timeBeforeFade;

	// Use this for initialization
	void Start () {
        StartCoroutine(FadeMeshRenderer(timeBeforeFade, fadeTime));
	}
	
	// Update is called once per frame
	void Update () {
	}

	public IEnumerator FadeMeshRenderer(float _timeBeforeFade, float _fadeTime)
	{

        yield return new WaitForSeconds(_timeBeforeFade); // How long should we wait before we start fading the object

        //fades the objects material to 0a over a given amount of time
		for (float f = _fadeTime; f >= 0; f -= Time.deltaTime) {
            Color c = GetComponentInChildren<MeshRenderer>().material.color;
			c.a = f;
            GetComponentInChildren<MeshRenderer>().material.color = c;
			yield return null;
		}
		Destroy (gameObject);
	}
}
