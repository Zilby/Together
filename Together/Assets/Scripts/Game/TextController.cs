using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls an individual falling text object and its impact on individual stats. 
/// </summary>
public class TextController : MonoBehaviour
{

	/// <summary>
	/// The player heart.
	/// </summary>
	public Beat heart;

	/// <summary>
	/// How much love the player gains upon collecting this text. 
	/// </summary>
	public int love;

	/// <summary>
	/// How much more frozen the player's heart becomes upon collecting this text. 
	/// </summary>
	public int frozen;

	/// <summary>
	/// How much more stressed the player is upon collecting this text. 
	/// </summary>
	public int frequency;

	/// <summary>
	/// How much love the player gains if they don't collect this text. 
	/// </summary>
	public int love2;

	/// <summary>
	/// How much more frozen the player's heart becomes if they don't collect this text. 
	/// </summary>
	public int frozen2;

	/// <summary>
	/// How much more stressed the player is if they don't collect this text. 
	/// </summary>
	public int frequency2;

	// Use this for initialization
	void Start()
	{
		float x = Random.Range(-5f, 5f);
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (heart && !heart.Dying)
		{
			transform.position = Vector3.MoveTowards(transform.position,
				new Vector3(transform.position.x, -10f, transform.position.z),
				(2.0f * GameController.speed) * Time.deltaTime);
			if (transform.position.y < -6.5)
			{
				heart.Love += love2;
				heart.Frozen += frozen2;
				heart.Frequency += frequency2;
				Destroy(gameObject);
			}
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		heart.Love += love;
		heart.Frozen += frozen;
		heart.Frequency += frequency;
		Destroy(gameObject);
	}

}
