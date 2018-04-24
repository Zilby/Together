using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Automatically destroys the blood after a set period of time. 
/// </summary>
public class BloodAutoDestruct : MonoBehaviour {

	/// <summary>
	/// How much time before the blood is destroyed. 
	/// </summary>
    public float time;

    public void Start()
    {
        StartCoroutine(HandleIt());
    }

	/// <summary>
	/// Handles stopping the particle system and then destroying the gameobject. 
	/// </summary>
    private IEnumerator HandleIt()
    {
        yield return new WaitForSeconds(time);
        GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
