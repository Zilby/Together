using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

    public GameObject heart;
    private Material background;
    private bool blacked;

	// Use this for initialization 
	void Start () {
        background = GetComponent<MeshRenderer>().material;
        background.color = Color.black;
        blacked = true;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(heart.GetComponent<Beat>().GetDying())
        {
            Destroy(gameObject);
        }
        if (heart.GetComponent<Beat>().GetBeating())
        {
            // sets transparency to none for full duration of beat
            background.color = Color.white;
            setToHeartColor();
            if (blacked)
            {
                // rotates beat randomly if first appearance
                Vector3 temp = GetComponent<Transform>().localEulerAngles;
				temp.z = (temp.z + 45 + Random.Range(0, 270)) % 360;
                GetComponent<Transform>().localEulerAngles = temp;
                blacked = false;
            }
        } 
        else if (background.color != Color.black)
        {
            setToHeartColor();
            Color c = background.color;
            // fades color over time, faster depending on the frequency of the beat
            c.a -= 0.02f + (((100 / heart.GetComponent<Beat>().frequency) - 1) / 100);
            background.color = c;
            blacked = true;
        }
	}

	public void Update() {
		transform.position = heart.transform.position;
	}

    private void setToHeartColor()
    {
        Color c = background.color;
        // sets the color to that of the heart
        c.b = heart.GetComponent<SpriteRenderer>().material.color.b;
        c.r = heart.GetComponent<SpriteRenderer>().material.color.r;
        background.color = c;
    }
}

