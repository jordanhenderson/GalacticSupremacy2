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

	private int costEx = 10;
	private int costCol = 100;
	private int costConq = 500;

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
		int income = server.GetIncome(p.id);

		GUI.Label(new Rect(20, 20, 120, 20 ), "Credits: $"+ credits);
		GUI.Label(new Rect(120, 20, 120, 20 ),"Income: $"+ income);
		GUI.Label(new Rect(240, 20, 120, 20 ),"Turn " + server.GetTurn ());

	}

	/*	This function draws the panel at the bottom of the screen which shows
	 *	information on the currently selected region, and interaction options
	 * 	available to the player.
	 */
	void DrawRegionPanel(int id) {
		//Grid layout variables
		int boxXY = 100;
		int xStart = 200;
		int yStart = 80;

		PlayerState player = server.GetCurrentPlayer();
		Planet p = s.GetPlanet();
		if(p != null) {
			if (isExplored(p)) {
			// SolReg Stats get displayed in the far left:
			GUI.Label(new Rect(20, 20, 130, 20), "Region "+ p.id +" report:");
			//GUI.Label(new Rect(20, 55, 130, 20), "Owned by Player "+ p.owner);
			GUI.Label(new Rect(20, 90, 130, 20), "Income: "+ p.income);
			GUI.Label(new Rect(20, 125, 130, 20),"Construction Slots: "+ p.slots);
			}
			
			/*
			if (GUI.Button (closeButton, "Close")) {
				windowOpen = false;    
			}
			*/

			// Case 1: Planet is owned by player, display construction options
			if (p.owner == pid) {	
				for (int i = 0; i < p.slots; ++i) {
					if (server.GetBuilding(p.buildings[i]).id == 0) {
						//print ("is empty");
						if (GUI.Button(new Rect(xStart+(i*100), yStart, boxXY, boxXY), "Empty\nSlot")) {
							showConMenu = true;
							buttonClicked = i;
						}
					} else {
						if (GUI.Button(new Rect(xStart+(i*100), yStart, boxXY, boxXY), server.GetBuilding(p.buildings[i]).name)) {
							showConMenu = true;
							buttonClicked = i;
						}
					}	
				}

			// Case 2: Planet is not owned by player.
			} else if (isAdjacent(p)){ 
				// Case 2.1: Planet is owned by another player.
				if (p.owner != 0) {
					int turrets = countTurrets(p);
					int attackCost = costConq + turrets * 500;
					if (GUI.Button(new Rect(xStart+(100), yStart, boxXY*2, boxXY), "Conquer ($"+attackCost+")")) {
						// count turrets, add count*500 to costConq
						
						if (player.credits >= (attackCost)) {
							player.credits -= attackCost;
							// Set new Owner
							p.owner = pid;
							// Find related object
							GameObject go = GameObject.Find("Planet "+p.id);
							// Change selector color
							go.GetComponent<planetScript>().SetOwner();
							s.RedrawLines(go);
						}	
					}
				// Case 2.2: Planet is unoccupied, explored
				} else if (isExplored(p)) {
					if (GUI.Button(new Rect(xStart+(100), yStart, boxXY*2, boxXY), "Colonise ($"+costCol+")")) {
						if (player.credits >= costCol) {
							player.credits -= costCol;

							// Set new Owner
							p.owner = pid;
							// Find related object
							GameObject go = GameObject.Find("Planet "+p.id);
							// Change selector color
							go.GetComponent<planetScript>().SetOwner();
							s.RedrawLines(go);	

							// Compute new income

						} else {
							// Not enough resources, do something
						}
					}
				// Case 2.3: Planet is unoccupied, unexplored
				} else {
					if (GUI.Button(new Rect(xStart+(100), yStart, boxXY*2, boxXY), "Explore ($"+costEx+")")) {
						// Check if resources available
						if (player.credits >= costEx) {
							player.credits -= costEx; 	// Deduct Cost
							
							player.explored.Add(p.id);	// Add planet to the explored list.
						} else {
							// Not enough resources, do something
						}
					}
				}
			}
		}
	}

	int countTurrets(Planet p) {
		int count = 0;
		for (int i = 0; i < p.buildings.Count; i++) {
			if (p.buildings[i] == 3) count++;
		}

		return count;
	}

	bool isExplored(Planet p) {
		PlayerState player = server.GetCurrentPlayer();
		int count = player.explored.Count;
		for (int i = 0; i < count; i++) {
			if (player.explored[i] == p.id) return true;
		}
		return false;

	}

	bool isAdjacent(Planet p) {
		for (int i = 0; i < p.adjacent.Count; i++) {
			int id = p.adjacent[i];
			Planet p2 = server.GetPlanetByID(id);
			if (p2.owner == pid) return true;
		}
		return false;
	}

	void DrawConPanel(int id) {
		// retrieve number of possible buildings from server:
		int numBuildings = server.getNumBuildings();
		Planet p = s.GetPlanet();
		PlayerState player = server.GetCurrentPlayer();

		// draw a button for each construction option
		// start with 1, as 0 is the empty building
		for (int i = 1; i < numBuildings ; ++i) {
			string name = server.GetBuilding(i).name;
			int cost = server.GetBuilding(i).cost;
			string myString = name + ": $" + cost;

			if (GUI.Button(new Rect(20, 20+(i*25), 200, 20), myString)) {
				if (player.credits >= cost) {
					player.credits -= cost;
					server.AddBuilding(i, p.id, buttonClicked, pid);
					showConMenu = false;
				} else {

				}
			}
		}

		if (GUI.Button(new Rect(150, 200, 60, 20), "Close")) {
				showConMenu = false;
			}
	}

}
