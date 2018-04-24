using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player heart and its beating. 
/// </summary>
public class Beat : MonoBehaviour
{

	/// <summary>
	/// The frequency of the heartbeat, and the amount
	/// of stress the player has currently.
	/// </summary>
	[SerializeField]
	protected float frequency;

	/// <summary>
	/// The frequency of the heartbeat, and the amount
	/// of stress the player has currently.
	/// </summary>
	public float Frequency
	{
		get { return frequency; }
		set { frequency = Mathf.Clamp(value, 0, 100); }
	}

	/// <summary>
	/// How frozen the player's heart is 
	/// (how little compassion they have). 
	/// </summary>
	[SerializeField]
	protected float frozen;

	/// <summary>
	/// How frozen the player's heart is 
	/// (how little compassion they have). 
	/// </summary>
	public float Frozen
	{
		get { return frozen; }
		set { frozen = Mathf.Clamp(value, 0, 100); }
	}

	/// <summary>
	/// How much love the player has. 
	/// </summary>
	[SerializeField]
	protected float love;

	/// <summary>
	/// How much love the player has. 
	/// </summary>
	public float Love
	{
		get { return love; }
		set { love = Mathf.Clamp(value, 0, 100); }
	}

	/// <summary>
	/// The current number of cracks in the player's heart. 
	/// </summary>
	public int cracks;

	/// <summary>
	/// The list of crack gameobjects. 
	/// </summary>
	public List<GameObject> cracklist;

	/// <summary>
	/// The blood particle system.
	/// </summary>
	public ParticleSystem blood;

	/// <summary>
	/// Whether or not the player has reached the end of the game. 
	/// </summary>
	public bool end;

	/// <summary>
	/// The timer for the heart beat. 
	/// </summary>
	protected float timer;

	/// <summary>
	/// Whether or not the heart is currently large (ie: beating). 
	/// </summary>
	protected bool beating;

	/// <summary>
	/// Whether or not the heart is currently large (ie: beating). 
	/// </summary>
	public bool Beating
	{
		get { return beating; }
	}

	/// <summary>
	/// Whether or not the heart is currently dying (for 
	/// bleeding death sequence). 
	/// </summary>
	protected bool dying;

	/// <summary>
	/// Whether or not the heart is currently dying (for 
	/// bleeding death sequence). 
	/// </summary>
	public bool Dying
	{
		get { return dying; }
	}

	/// <summary>
	/// The death timer for the death cracking sequence. 
	/// </summary>
	protected float deathTimer;

	/// <summary>
	/// The current sequence count for the death cracking sequence. 
	/// </summary>
	protected int deathSequenceCount;

	/// <summary>
	/// The beat sound effect.
	/// </summary>
	protected AudioSource beat;

	/// <summary>
	/// The sprite renderer for the heart. 
	/// </summary>
	protected SpriteRenderer rend;

	/// <summary>
	/// Causes player to crack if they're at too high a stress for too long.
	/// </summary>
	protected int stressTimer;

	/// <summary>
	/// The frequency used visually (frequency of 0 would be too high).
	/// </summary>
	protected float VisualFrequency
	{
		get { return Mathf.Clamp(frequency, 10, 100); }
	}

	protected virtual void Start()
	{
		beat = GetComponent<AudioSource>();
		rend = GetComponent<SpriteRenderer>();
		// sets the pitch higher as the heartbeats get faster and vice versa
		timer = 0;
		stressTimer = 0;
		beating = false;
		dying = false;
		deathTimer = -1f;
		deathSequenceCount = 0;
		StartCoroutine(Stressed());
		SetPitch();
		SetColor();
		end = false;
	}

	protected void FixedUpdate()
	{
		if (deathTimer != -1f)
		{
			DeathSequence();
		}
		if (dying)
		{
			StartCoroutine(DieSequence());
			Color c = rend.material.color;
			// fades color over time
			c.a -= 0.02f;
			rend.material.color = c;
		}
		else if (Frozen == 100 && deathTimer == -1f)
		{
			StartDeathSequence();
		}
		else
		{
			float x = Input.GetAxis("Horizontal");
			if (x != 0.0f && !(x > 0.0f && transform.position.x > 7.0f) && !(x < 0.0f && transform.position.x < -7.0f))
			{
				transform.position = new Vector3(transform.position.x + x / 3.0f, transform.position.y, transform.position.z);
			}
			SetPitch();
			SetColor();
			if (timer == 0)
			{
				beat.Stop();
				beat.Play();
				timer = Mathf.Max(VisualFrequency, 10);
			}
			else
			{
				timer--;
			}
			if (timer == (int)VisualFrequency * 9 / 10)
			{
				transform.localScale = new Vector3(1f, 1f, 0);
				ScaleLove();
				beating = true;
			}
			if (timer == (int)VisualFrequency * 8 / 10)
			{
				transform.localScale = new Vector3(0.8f, 0.8f, 0);
				ScaleLove();
				beating = false;
			}
			if (timer == (int)VisualFrequency * 7 / 10)
			{
				transform.localScale = new Vector3(1f, 1f, 0);
				ScaleLove();
				beating = true;
			}
			if (timer == (int)VisualFrequency * 5 / 10)
			{
				transform.localScale = new Vector3(0.8f, 0.8f, 0);
				ScaleLove();
				beating = false;
			}
		}
	}

	/// <summary>
	/// Scales the transform based on the current love.
	/// </summary>
	public virtual void ScaleLove()
	{
		transform.localScale = (transform.localScale / 2.2f) * ((Love / 50f) + 1);
	}

	/// <summary>
	/// Sets the color of the heart based on the frozen value. 
	/// </summary>
	public void SetColor()
	{
		Color temp = rend.material.color;
		temp.b = Frozen / 20f + 1f;
		temp.r = (255f - (Frozen * (255f / 100f))) / 255f;
		temp.g = (255f - Frozen) / 255f;
		rend.material.color = temp;
	}

	/// <summary>
	/// Sets the pitch of the beat. 
	/// </summary>
	public void SetPitch()
	{
		beat.pitch = 1 + (50 - VisualFrequency) / 150;
	}

	/// <summary>
	/// Creates a crack in the player heart. 
	/// </summary>
	public void Crack()
	{
		if (cracks < 4)
		{
			cracklist[cracks].SetActive(true);
			cracks++;
			if (cracks == 4)
			{
				dying = true;
			}
			iTween.ShakePosition(gameObject, new Vector3(1f, 1f, 0f), 0.4f);
		}
	}

	/// <summary>
	/// Sets up the particle blood system. 
	/// </summary>
	public void Die()
	{
		blood.gameObject.SetActive(true);
		Color c = Color.black;
		c.b = Frozen / 100;
		c.r = 0.5f - Frozen / 100;
		ParticleSystem.MainModule m = blood.main;
		m.startColor = c;
	}

	/// <summary>
	/// Starts the dying sequence. 
	/// </summary>
	private IEnumerator DieSequence()
	{
		yield return new WaitForSeconds(0.5f);
		Die();
		yield return new WaitForSeconds(1.5f);
		dying = false;
		Destroy(gameObject);
	}

	/// <summary>
	/// Increments the stress timer and cracks the heart 
	/// if the player is too stressed. Disabled if the player
	/// has reached the end of the game. 
	/// </summary>
	protected IEnumerator Stressed()
	{
		while (!end)
		{
			if (Frequency <= 20)
			{
				stressTimer += 20 / Mathf.Max((int)Frequency, 1);
				if (stressTimer >= 20)
				{
					Crack();
					stressTimer = 0;
				}
			}
			else
			{
				stressTimer = 0;
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	/// <summary>
	/// Creates a sequence of cracks resulting in death. 
	/// Caused by losing all compassion (100 frozen). 
	/// </summary>
	protected void DeathSequence()
	{
		if ((Time.time - deathTimer >= 1f && deathSequenceCount == 0) ||
			(Time.time - deathTimer >= 0.8f && deathSequenceCount == 1) ||
			(Time.time - deathTimer >= 0.65f && deathSequenceCount == 2) ||
			(Time.time - deathTimer >= 0.5f && deathSequenceCount == 3))
		{
			Crack();
			deathSequenceCount++;
			deathTimer = Time.time;
		}
	}

	/// <summary>
	/// Starts the death sequence.
	/// </summary>
	protected void StartDeathSequence()
	{
		deathTimer = Time.time;
	}
}
