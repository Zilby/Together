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
			Love += 1.2f;
			Frequency -= 1;
		}
	}

	public override void ScaleLove()
	{
		transform.localScale = (transform.localScale / 2.2f) * ((Love / 25f) + 1);
	}
}

