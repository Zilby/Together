using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {

	public Beat heart; 
	public int love;
	public int frozen;
	public int frequency;

	public int love2;
	public int frozen2;
	public int frequency2;

	// Use this for initialization
	void Start () {
		float x = Random.Range (-6f, 6f);
		transform.position = new Vector3 (x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = Vector3.MoveTowards (transform.position, 
			new Vector3 (transform.position.x, -10f, transform.position.z), 
			2.0f * Time.deltaTime);
		if (transform.position.y < -6.5) {
			heart.love += love2;
			heart.frozen += frozen2;
			heart.frequency += frequency2;
			Destroy (gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D col) {
		heart.love += love;
		heart.frozen += frozen;
		heart.frequency += frequency;
		Destroy (gameObject);
	}

}
