﻿using UnityEngine;
using System.Collections;

public class LoginGUI : MonoBehaviour {
	public GameObject player;	// linked to the playerobject


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	// creates a simple GUI with 2 buttons. button press will assign the player an id and start
	// the next scene.
	void OnGUI () {
		int width = 300;
		int height = 300;
		GUI.Box(new Rect((Screen.width - width) / 2,(Screen.height - height) / 2, width, height), "Galactic Supremacy");

		int button_x = (Screen.width - 80) / 2;
		int button_y = (Screen.height - 60) / 2;
	// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(button_x,button_y,80,20), "Start")) {
			//player.GetComponent<PlayerState>().id = 1;
			Application.LoadLevel("scene1");
		}

	}
}
