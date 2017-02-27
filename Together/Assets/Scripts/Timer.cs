using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float time;

    public void Start()
    {
        StartCoroutine(HandleIt());
    }

    private IEnumerator HandleIt()
    {
        yield return new WaitForSeconds(time);
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
