﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitter : MonoBehaviour
{
	[SerializeField] private KeyCode keyToQuit = KeyCode.Escape;

	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(keyToQuit))
		{
			Application.Quit(0);
		}
    }
}
