﻿using UnityEngine;
using System.Collections;
using gsFramework;

public class MainGUI : MonoBehaviour {
	//public GUISkin mySkin;
	private startup s;
	public GameObject Player;
	public bool showConMenu = false;
	private int buttonClicked;
	//private int startTime;
	//private int restSec;
	//private int displaySec;
	//private int displayMin;

	//int countDownSec;

	Rect resourcePanel = new Rect(0, 0, 370, 50);
	Rect regionInfoPanel = new Rect(0, Screen.height-200, Screen.width, 250);
	Rect constructionPanel = new Rect(Screen.width/3, Screen.height/5, 250, 250);
	Rect endTurn = new Rect(Screen.width-100, 0, 100, 50);
	
	void Awake() {
		//startTime = Time.time;
	}

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		s = GameObject.Find("startup").GetComponent<startup>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI () {
		//int guiTime = Time.time-startTime;

		//restSec = countDownSec - guiTime;

		Planet p = s.GetPlanet();
		resourcePanel = GUI.Window(0, resourcePanel, DrawResourcePanel, "Resources");

		if(p != null) regionInfoPanel = GUI.Window(1, regionInfoPanel, DrawRegionPanel, "Planet "+ p.id);

		if (showConMenu) {
			constructionPanel = GUI.Window(2, constructionPanel, DrawConPanel, "Construction Menu");
		}
		
		if (GUI.Button (endTurn, "End Turn")) {

		}
	}

	void DrawResourcePanel(int id) {
		int credits = Player.GetComponent<PlayerState>().credits;
		int income = Player.GetComponent<PlayerState>().income;
		GUI.Label(new Rect(20, 20, 120, 20 ), "Credits: $"+ credits);
		GUI.Label(new Rect(120, 20, 120, 20 ),"Income: $"+ income +"/sec");
		GUI.Label(new Rect(220, 20, 120, 20 ),"Fleet Upkeep: 5/20");
	}

	/*	This function draws the panel at the bottom of the screen which shows
	 *	information on the currently selected region, and interaction options
	 * 	available to the player.
	 */
	void DrawRegionPanel(int id) {
		Planet p = s.GetPlanet();
		if(p != null) {
			// SolReg Stats get displayed in the far left:
			GUI.Label(new Rect(20, 20, 130, 20 ), "Region "+ p.id +" debug data");
			GUI.Label(new Rect(20, 55, 130, 20 ), "Owned by Player "+ p.owner);
			GUI.Label(new Rect(20, 90, 130, 20 ), "Income: "+ p.income);
			GUI.Label(new Rect(20, 125, 130, 20 ),"Construction Slots: "+ p.slots);
			GUI.Label(new Rect(20, 160, 130, 20 ),"Empty Slots: "+ p.emptySlots);

		
			//Grid layout variables
			int boxXY = 70;
			int xStart = 200;
			int yStart = 120;
			/*
			for (int i = 0; i < selectedSR.slots; ++i) {
				
				if (selectedSR.buildings[i].cost == 0) {
					if (GUI.Button(new Rect(xStart+(i*70), yStart, boxXY, boxXY), "Empty\nSlot")) {
						showConMenu = true;
						buttonClicked = i;
					}
				} else {
					if (GUI.Button(new Rect(xStart+(i*70), yStart, boxXY, boxXY), selectedSR.buildings[i].name)) {
						//showConMenu = true;
						//buttonClicked = i;

					}
				}
			}
			*/
		}
	}

	void DrawConPanel(int id) {

		//for (int i = 0; i < Buildings.nBuildings; ++i) {
		/*
			if (GUI.Button(new Rect(20, 20+(i*25), 100, 20), "Build" +Buildings.buildings[i].name)) {

			}
		*/
		//}

		if (GUI.Button(new Rect(150, 200, 60, 20), "Close")) {
				showConMenu = false;
			}
	}
		
}
