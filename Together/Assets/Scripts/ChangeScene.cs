using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour {

	public Text t; 
	public Text n;

	private bool wait;
	private bool name;
	private bool transparent;

	void Start() {
		Cursor.visible = false;
		wait = true; 
		name = false;
		transparent = true;
		StartCoroutine (Wait ());
	}

	void Update() {
		if (Input.GetKey ("escape")) {
			Application.Quit ();
		}
		if (name) {
			n.color = Color.Lerp (n.color, new Color (1, 1, 1, 1), 1.2f * Time.deltaTime);
			if (n.color.a > 0.95) {
				name = false;
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

	public void change() {
		Application.LoadLevel ("Main");
	}

	private IEnumerator Wait() {
		yield return new WaitForSeconds (2.0f);
		name = true;
		yield return new WaitForSeconds (3.0f);
		wait = false;
	}
}
