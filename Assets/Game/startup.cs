﻿using UnityEngine;
using System.Collections;
using gsFramework;


/* startup is the main class for the client. It holds information
 * about the player(ID) and queries the server for map data. It also
 * holds information about the players GUI interaction (selection).
 */ 
public class startup : MonoBehaviour {
	public Server server;
	public planetScript selectedPlanet;
	public GameObject selector;
	public GameObject mainCamera;
	public GameObject mainGUI;

	/* Initialization. Queries the server for the data needed to build and
	 * render the map. This includes the number regions.
	 */
	void Start () {
		//Create server component first (handles main game logic)
		server = gameObject.AddComponent<Server>();
		
		//Create camera
		mainCamera = GameObject.Find ("MainCamera");
		mainCamera.AddComponent<cameraController> ().server = server;
	
		//Create GUI
		mainGUI = new GameObject();
		MainGUI gui = mainGUI.AddComponent<MainGUI> ();
		gui.server = server;
		gui.s = this;
		gui.name = "MainGUI";

		//Create planet selector.
		selector = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		selector.name = "selector";
		// selector is invisible until a selection is made
		selector.renderer.enabled = false;

	}

	public void DrawLine(GameObject go1, GameObject go2) {
		Planet p1 = go1.GetComponent<planetScript>().GetPlanet();
		Planet p2 = go2.GetComponent<planetScript>().GetPlanet();

		Color color1 = FindColor(p1);
		Color color2 = FindColor(p2);

		GameObject line = new GameObject();
		LineRenderer lr = line.AddComponent<LineRenderer>();

		lr.SetPosition(0, go1.transform.position);
		lr.SetPosition(1, go2.transform.position); 
		lr.SetWidth(0.5f,0.5f);
		lr.material = new Material(Shader.Find("Particles/Additive"));
		lr.SetColors(color1, color2);
		line.name = "Line "+p1.id+ "-"+p2.id;
	}

	public void RedrawLines(GameObject go) {
		// go is the Planet object which changed ownership
		Planet p = go.GetComponent<planetScript>().GetPlanet();
		
		foreach (int id in p.adjacent) {
			// determine if adjacent pid is greater than this pid
			int p1 = Mathf.Min(id, p.id);
			int p2 = Mathf.Max(id, p.id);
			
			
			// find corresponding LineRenderer
			GameObject line = GameObject.Find("Line "+p1+"-"+p2);
			LineRenderer lr = line.GetComponent<LineRenderer>();

			// recolor it.
			Color color1 = FindColor(server.GetPlanetByID(p1));
			Color color2 = FindColor(server.GetPlanetByID(p2));
			lr.SetColors(color1, color2);
		}

	}

	/*
	 *	This method returns a color based on a planets ownership status. 
	 */
	private Color FindColor(Planet p) {
		if (p.owner == 0) {
			return Color.gray;
		} else if (p.owner == 1) {
			return Color.blue;
		} else if (p.owner == 2) {
			return Color.red;
		} else {
			return Color.white;
		}
	}

	public Planet GetPlanet() {
		//Get the underlying planet data of the currently selected planet.
		if (selectedPlanet != null) { 
					return selectedPlanet.GetPlanet ();
			}
		else return null;
	}



	public void SelectPlanet(planetScript ps) {
		// If a previous selection exists, deselect. 
		if (selectedPlanet != null) selectedPlanet.Deselect();
		// Update current selection in this script.
		selectedPlanet = ps;
		Planet p = selectedPlanet.GetPlanet();
		selector.renderer.enabled = true;
		selector.transform.localPosition = new Vector3(p.x, -0.1f, p.z);
		selector.transform.localScale = new Vector3(1.8f, 0, 1.8f);
		selector.renderer.material.color = Color.white;
	}

	//This function attaches a planet object to the game.
	public planetScript AttachPlanet(Planet p) {
		GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		sphere.transform.localPosition = new Vector3(p.x, 0, p.z);
		sphere.transform.localRotation = Quaternion.Euler (90, 0, 0);

		//Set up textures for the planet
		string tex_str = "texture" + p.texture.ToString ();
		Material mat = Resources.Load (tex_str, typeof(Material)) as Material;
		sphere.renderer.material = mat;
		Texture2D tex = Resources.Load (tex_str, typeof(Texture2D)) as Texture2D;
		sphere.renderer.material.mainTexture = tex;

		//Create the planet script
		planetScript ps = sphere.AddComponent<planetScript>();

		ps.SetPlanet(p);
		return ps;

	}
}
