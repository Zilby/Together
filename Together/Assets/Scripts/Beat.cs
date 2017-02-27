using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beat : MonoBehaviour {

    public float frequency; //stress
    public float frozen; //love
    public float cracks;
    public List<GameObject> cracklist;
    public GameObject blood;
    private float timer;
    private bool beating;
    private bool dying;
    private AudioSource beat; 

    private void Start()
    {
        beat = GetComponent<AudioSource>();
        // sets the pitch higher as the heartbeats get faster and vice versa
        timer = 0;
        beating = false;
        dying = false;
        SetPitch();
        SetColor();
        //for(int i = 1; i < 5; i++) {
        //    Crack(i);
        //}
    }

    void FixedUpdate () {
        if (dying)
        {
            StartCoroutine(HandleIt());
            Color c = GetComponent<SpriteRenderer>().material.color;
            // fades color over time
            c.a -= 0.02f;
            GetComponent<SpriteRenderer>().material.color = c;
        }
        else
        {
            SetPitch();
            SetColor();
            if (timer == 0)
            {
                beat.Stop();
                beat.Play();
                timer = frequency;
            }
            else
            {
                timer--;
            }
            if (timer == (int)frequency * 9 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
                beating = true;
            }
            if (timer == (int)frequency * 8 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0);
                beating = false;
            }
            if (timer == (int)frequency * 7 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(1f, 1f, 0);
                beating = true;
            }
            if (timer == (int)frequency * 5 / 10)
            {
                GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 0);
                beating = false;
            }
        }
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
        temp.b += frozen;
        temp.r -= frozen / 100;
        GetComponent<SpriteRenderer>().material.color = temp;
    }

    public void SetPitch()
    {
        beat.pitch = 1 + (50 - frequency) / 150;
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
        yield return new WaitForSeconds(1);
        Die();
        yield return new WaitForSeconds(1);
        dying = false;
        Destroy(gameObject);
    }
}
