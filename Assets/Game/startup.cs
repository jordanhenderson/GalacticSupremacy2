using UnityEngine;
using System.Collections;
using gsFramework;


/* startup is the main class for the client. It holds information
 * about the player(ID) and queries the server for map data. It also
 * holds information about the players GUI interaction (selection).
 */ 
public class startup : MonoBehaviour {
	public GameObject game;
	public planetScript selectedPlanet;
	public int selected = 0;
	GameObject selector;
	GameObject guiController;

	/* Initialization. Queries the server for the data needed to build and
	 * render the map. This includes the number regions, and data
	 */
	void Start () {
		selector = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		// selector is invisible until a selection is made
		selector.renderer.enabled = false;
		
		game = new GameObject();
		game.AddComponent<Server>();
	}

	void Update() {

	}
	
	public Planet GetPlanet() {
		//Get the underlying planet data of the currently selected planet.
		if(selectedPlanet != null) return selectedPlanet.GetPlanet();
		else return null;
	}

	public void SelectPlanet(planetScript ps) {
		// If a previous selection exists, deselect. 
		selectedPlanet.Deselect();
		// Update current selection in this script.
		selectedPlanet = ps;
		Planet p = selectedPlanet.GetPlanet();
		selector.renderer.enabled = true;
		selector.transform.localPosition = new Vector3(p.x, 0.1f, p.z);
		selector.transform.localScale = new Vector3(1.8f, 0, 1.8f);
		selector.renderer.material.color = Color.white;
	}

	//This function attaches a planet object to the game.
	public void AttachPlanet(Planet p) {
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
		
		// give the GO a meaningful name
		sphere.name = "Planet "+ p.id;
	}
}
