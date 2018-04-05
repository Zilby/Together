using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerBeat : Beat
{
	protected override void Start() 
	{
		base.Start();
		StartCoroutine(GrowAndStress());
	}


	private IEnumerator GrowAndStress() 
	{
		for (;;)
		{
			yield return new WaitForSecondsRealtime(60f / 80f);
			love += 1.2f;
			frequency -= 1;
		}
	}

	public override void ScaleLove()
	{
		GetComponent<Transform>().localScale = (GetComponent<Transform>().localScale / 2.2f) * ((love / 25f) + 1);
	}
}

