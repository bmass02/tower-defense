using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void SwitchScene(string scene)
	{
		SceneManager.LoadScene (scene);

	}

	public void Close()
	{
		Application.Quit ();
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
