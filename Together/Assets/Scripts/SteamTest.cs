﻿using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Facepunch.Steamworks;

public class SteamTest : MonoBehaviour
{
	void Start()
	{
		// Don't destroy this when loading new scenes
		DontDestroyOnLoad(gameObject);

		// Configure for Unity
		// This is VERY important - call this before doing anything
		Facepunch.Steamworks.Config.ForUnity(Application.platform.ToString());

		// Create the steam client using the test AppID (or your own AppID eventually)
		new Facepunch.Steamworks.Client(480);

		// Make sure we started up okay
		if (Client.Instance == null)
		{
			Debug.LogError("Error starting Steam!");
			return;
		}

		// Print out some basic information
		Debug.Log("My Steam ID: " + Client.Instance.SteamId);
		Debug.Log("My Steam Username: " + Client.Instance.Username);
		Debug.Log("My Friend Count: " + Client.Instance.Friends.AllFriends.Count());
	}

	private void OnDestroy()
	{
		if (Client.Instance != null)
		{
			// Properly get rid of the client if this object is destroyed
			Client.Instance.Dispose();
		}

	}


	void Update()
	{
		if (Client.Instance != null)
		{
			// This needs to be called in Update for the library to properly function
			Client.Instance.Update();
		}

	}
}