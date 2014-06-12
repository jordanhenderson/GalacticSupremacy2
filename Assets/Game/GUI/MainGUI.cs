using UnityEngine;
using System.Collections;
using gsFramework;

public class MainGUI : MonoBehaviour {
	public startup s;
	public Server server;
	public bool showConMenu = false;
	private int buttonClicked;
	public bool windowOpen = true;
	private int pid;

	Rect resourcePanel = new Rect(0, 0, 370, 50);
	Rect regionInfoPanel = new Rect(0, Screen.height-200, Screen.width, 250);
	Rect constructionPanel = new Rect(Screen.width/3, Screen.height/5, 250, 250);
	Rect endTurn = new Rect(Screen.width-100, 0, 100, 50);
	Rect closeButton = new Rect(Screen.width-50, 25,50, 30);
	Planet old_planet;

	void Awake() {
		//startTime = Time.time;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI () {
		Planet p = s.GetPlanet();
		pid = server.GetCurrentPlayer ().id + 1;

		resourcePanel = GUI.Window (0, resourcePanel, DrawResourcePanel, "Player " + pid + ": Resources");

		if (!windowOpen && p != old_planet) {
			windowOpen = true;
				}
		if (windowOpen && p!=null){
				regionInfoPanel = GUI.Window(1, regionInfoPanel, DrawRegionPanel, "Planet "+ p.id);
		}
		if (showConMenu) {
			constructionPanel = GUI.Window(2, constructionPanel, DrawConPanel, "Construction Menu");
		}
		
		if (GUI.Button (endTurn, "End Turn")) {
			server.NextTurn();
			s.mainCamera.GetComponent<cameraController>().NextTurn();
		}
	}

	void DrawResourcePanel(int id) {
		PlayerState p = server.GetCurrentPlayer ();
		int credits = p.credits;
		int income = p.income;
		GUI.Label(new Rect(20, 20, 120, 20 ), "Credits: $"+ credits);
		GUI.Label(new Rect(120, 20, 120, 20 ),"Income: $"+ income +"/turn");
		GUI.Label(new Rect(240, 20, 120, 20 ),"Turn " + server.GetTurn ());

	}

	/*	This function draws the panel at the bottom of the screen which shows
	 *	information on the currently selected region, and interaction options
	 * 	available to the player.
	 */
	void DrawRegionPanel(int id) {
		//Grid layout variables
		int boxXY = 70;
		int xStart = 200;
		int yStart = 120;


		Planet p = s.GetPlanet();
		if(p != null) {

			// SolReg Stats get displayed in the far left:
			GUI.Label(new Rect(20, 20, 130, 20), "Region "+ p.id +" debug data");
			GUI.Label(new Rect(20, 55, 130, 20), "Owned by Player "+ p.owner);
			GUI.Label(new Rect(20, 90, 130, 20), "Income: "+ p.income);
			GUI.Label(new Rect(20, 125, 130, 20),"Construction Slots: "+ p.slots);
			//GUI.Label(new Rect(20, 160, 130, 20),"Empty Slots: "+ p.emptySlots);

			if (GUI.Button (closeButton, "Close")) {
				windowOpen = false;    
			}
			old_planet = p;

			// Case 1: Planet is owned by player, display construction options
			if (p.owner == pid) {	
				for (int i = 0; i < p.slots; ++i) {
					if (server.GetBuilding(p.buildings[i]).id == 0) {
						//print ("is empty");
						if (GUI.Button(new Rect(xStart+(i*70), yStart, boxXY, boxXY), "Empty\nSlot")) {
							showConMenu = true;
							buttonClicked = i;
						}
					} else {
						if (GUI.Button(new Rect(xStart+(i*70), yStart, boxXY, boxXY), server.GetBuilding(p.buildings[i]).name)) {
							showConMenu = true;
							buttonClicked = i;
						}
					}	
				}
			// Case 2: Planet is not owned by player, but is adjacent.
			// Display expansion option:
				/*	Conditions:
				 *	- is not owned by any player
				 *	- is adjacent to an owned planet
				*/
			} else {
				if (GUI.Button(new Rect(xStart+(70), yStart, boxXY, boxXY), "Expand")) {
					// Set new Owner
					p.owner = pid;

					// Change selector color
				}
			}

			



		}
	}

	void DrawConPanel(int id) {
		// retrieve number of possible buildings from server:
		int numBuildings = server.getNumBuildings();
		Planet p = s.GetPlanet();

		// draw a button for each construction option
		for (int i = 1; i < numBuildings ; ++i) {
			if (GUI.Button(new Rect(20, 20+(i*25), 100, 20), server.GetBuilding(i).name)) {
				server.AddBuilding(i, p.id, buttonClicked, pid);
				showConMenu = false;
			}
		}

		if (GUI.Button(new Rect(150, 200, 60, 20), "Close")) {
				showConMenu = false;
			}
	}

}
