using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a pulse effect behind the heart for visuals. 
/// </summary>
public class Pulse : MonoBehaviour {

	/// <summary>
	/// The player heart. 
	/// </summary>
    public Beat heart;

	/// <summary>
	/// The background material.
	/// </summary>
	protected Material background;

	/// <summary>
	/// If the background was just black. 
	/// </summary>
	protected bool blacked;

	/// <summary>
	/// The sprite renderer of the heart. 
	/// </summary>
	protected SpriteRenderer heartRender;


	// Use this for initialization 
	void Start () {
        background = GetComponent<MeshRenderer>().material;
        background.color = Color.black;
        blacked = true;
		heartRender = heart.GetComponent<SpriteRenderer>();
    }


	// Update is called once per frame
	void FixedUpdate () {
		if(heart.Dying)
        {
            Destroy(gameObject);
        }
		if (heart.Beating)
        {
            // sets transparency to none for full duration of beat
            background.color = Color.white;
            SetToHeartColor();
            if (blacked)
            {
                // rotates beat randomly if first appearance
                Vector3 temp = GetComponent<Transform>().localEulerAngles;
				temp.z = (temp.z + 45 + Random.Range(0, 270)) % 360;
				transform.localEulerAngles = temp;
                blacked = false;
            }
        } 
        else if (background.color != Color.black)
        {
			FadeBeat();
        }
	}


	public void Update() {
		transform.position = heart.transform.position;
	}


	/// <summary>
	/// Sets the r and b color components of the pulsing to that of the heart.
	/// </summary>
	private void SetToHeartColor()
    {
        Color c = background.color;
		c.b = heartRender.material.color.b;
        c.r = heartRender.material.color.r;
        background.color = c;
    }


	/// <summary>
	/// Fades the beat if it's visible. 
	/// </summary>
	private void FadeBeat() 
	{
		SetToHeartColor();
		Color c = background.color;
		// fades color over time, faster depending on the frequency of the beat
		c.a -= 0.02f + (((100 / heart.Frequency) - 1) / 100);
		background.color = c;
		blacked = true;
	}
}

