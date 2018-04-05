using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {

    public float frequency; //stress
    public float frozen; //compassion
	public float love;
    public int cracks;
    public List<GameObject> cracklist;
    public GameObject blood;
    public bool end;
    protected float timer; // timer for the heart beating
    protected bool beating; // ie: if the heart is large
    protected bool dying; // for bleeding death sequence
	protected float deathTime; // for death-crack sequence
	protected int deathSequenceCount; 
    protected AudioSource beat;
    protected int stressTimer; // cracks you if you're at too high a stress for too long
	protected bool moveRight;
	protected bool moveLeft;
	protected float actualFrequency;

	protected virtual void Start()
    {
        beat = GetComponent<AudioSource>();
        // sets the pitch higher as the heartbeats get faster and vice versa
        timer = 0;
        stressTimer = 0;
        beating = false;
        dying = false;
		deathTime = -1f;
		deathSequenceCount = 0;
        InvokeRepeating("Stressed", 2.0f, 1.0f);
        SetPitch();
        SetColor();
        end = false;
		//startDeathSequence (); 
    }

    protected void FixedUpdate () {
		if (love < 0) {
			love = 0;
		}
		if (frozen < 0) {
			frozen = 0;
		}
		if (frequency < 0) {
			frequency = 0;
		}
		if (love > 100) {
			love = 100;
		}
		if (frozen > 100) {
			frozen = 100;
		}
		if (frequency > 100) {
			frequency = 100;
		}
		if (deathTime != -1f) {
			DeathSequence ();
		}
		actualFrequency = frequency; 
		if (actualFrequency < 10) {
			actualFrequency = 10;
		}
		if (dying)
        {
            StartCoroutine(HandleIt());
            Color c = GetComponent<SpriteRenderer>().material.color;
            // fades color over time
            c.a -= 0.02f;
            GetComponent<SpriteRenderer>().material.color = c;
        } else if (frozen == 100 && deathTime == -1f)
        {
            startDeathSequence();
        }
        else
        {
			float x = Input.GetAxis ("Horizontal");
			if (x != 0.0f && !(x > 0.0f && transform.position.x > 7.0f) && !(x < 0.0f && transform.position.x < -7.0f)) {
				transform.position = new Vector3 (transform.position.x + x / 3.0f, transform.position.y, transform.position.z);
			}
            SetPitch();
            SetColor();
            if (timer == 0)
            {
                beat.Stop();
                beat.Play();
				timer = Mathf.Max(actualFrequency, 10);
            }
            else
            {
                timer--;
            }
			if (timer == (int)actualFrequency * 9 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
				ScaleLove ();
                beating = true;
            }
			if (timer == (int)actualFrequency * 8 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0);
				ScaleLove ();
                beating = false;
            }
			if (timer == (int)actualFrequency * 7 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
				ScaleLove ();
                beating = true;
            }
			if (timer == (int)actualFrequency * 5 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0);
				ScaleLove ();
                beating = false;
            }

        }
    }

	public void Update() {
		
	}

	public virtual void ScaleLove() {
		GetComponent<Transform> ().localScale = (GetComponent<Transform>().localScale/2.2f)*((love/50f) + 1);
	}

    public bool GetBeating()
    {
        return beating;
    }

    public bool GetDying()
    {
        return dying;
    }

    public void SetColor()
    {
		Color temp = GetComponent<SpriteRenderer>().color;
		temp.b = frozen / 20f + 1f;
		temp.r = (255f - (frozen * (255f / 100f))) / 255f;
		temp.g = (255f - frozen) / 255f;
		GetComponent<SpriteRenderer>().material.color = temp;
    }

    public void SetPitch()
    {
		beat.pitch = 1 + (50 - actualFrequency) / 150;
    }

    public void Crack(int i)
    {
        if (i > 0 && i < 5)
        {
            cracklist[i - 1].SetActive(true);
        }
        cracks++; 
        if(cracks == 4)
        {
            dying = true;
        }
        iTween.ShakePosition(gameObject, new Vector3(1f, 1f, 0f), 0.4f);
    }

    public void Die()
    {
        blood.SetActive(true);
        Color c = Color.black;
        c.b = frozen/100;
        c.r = 0.5f - frozen/100;
        blood.GetComponent<ParticleSystem>().startColor = c;
    }

    private IEnumerator HandleIt()
    {
        yield return new WaitForSeconds(0.5f);
        Die();
        yield return new WaitForSeconds(1.5f);
        dying = false;
        Destroy(gameObject);
    }

	protected void Stressed()
    {
        if (!end)
        {
            if (frequency <= 20)
            {
				stressTimer += 20 / Mathf.Max((int)frequency, 1);
            }
            else
            {
                stressTimer = 0;
            }
            if (stressTimer >= 20)
            {
                Crack((int)cracks + 1);
                stressTimer = 0;
            }
        }
    }


	protected void DeathSequence() {
		if (Time.time - deathTime >= 1f && deathSequenceCount == 0)
		{
            Crack(cracks + 1);
			deathSequenceCount++;
			deathTime = Time.time;
		} else if (Time.time - deathTime >= 0.8f && deathSequenceCount == 1)
		{
            Crack(cracks + 1);
			deathSequenceCount++;
			deathTime = Time.time;
		} else if (Time.time - deathTime >= 0.65f && deathSequenceCount == 2)
		{
            Crack(cracks + 1);
			deathSequenceCount++;
			deathTime = Time.time;
		} else if (Time.time - deathTime >= 0.5f && deathSequenceCount == 3)
		{
            Crack(cracks + 1);
			deathSequenceCount++;
			deathTime = Time.time;
		}
	}


	protected void startDeathSequence() {
		deathTime = Time.time; 
	}
}
