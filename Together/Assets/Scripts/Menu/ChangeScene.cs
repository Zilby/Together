using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Changes the scene from the menu. 
/// </summary>
public class ChangeScene : MonoBehaviour {

	/// <summary>
	/// Text that says press space.
	/// </summary>
	public Text t; 

	/// <summary>
	/// Developer name text. 
	/// </summary>
	public Text n;

	/// <summary>
	/// False before the press space text fades in and out. 
	/// </summary>
	private bool wait;

	/// <summary>
	/// False before the name text fades in. 
	/// </summary>
	private bool nameFadeIn;

	/// <summary>
	/// Whether or not the press space text is currently turning from transparent to opaque. 
	/// </summary>
	private bool transparent;

	void Start() {
		Cursor.visible = false;
		wait = true; 
		nameFadeIn = false;
		transparent = true;
		StartCoroutine (Wait ());
	}

	void Update() {
		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
		if (nameFadeIn) {
			n.color = Color.Lerp (n.color, new Color (1, 1, 1, 1), 1.2f * Time.deltaTime);
			if (n.color.a > 0.95) {
				nameFadeIn = false;
			}
		}
		if (!wait) {
			if (transparent) {
				t.color = Color.Lerp (t.color, new Color (1, 1, 1, 1), 1.2f * Time.deltaTime);
				if (t.color.a > 0.9) {
					transparent = false;
				}
			} else {
				t.color = Color.Lerp (t.color, new Color (1, 1, 1, 0), 1.5f * Time.deltaTime);
				if (t.color.a < 0.05) {
					transparent = true;
				}
			}
		}
	}

	/// <summary>
	/// Loads the main scene. 
	/// </summary>
	public void LoadScene() {
		SceneManager.LoadScene("Main");
	}

	/// <summary>
	/// Delays the text fading in. 
	/// </summary>
	private IEnumerator Wait() {
		yield return new WaitForSeconds (2.0f);
		nameFadeIn = true;
		yield return new WaitForSeconds (3.0f);
		wait = false;
	}
}
