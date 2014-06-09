using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using gsFramework;

public static class ServerUtility {
	public static byte[] bytesFromString(string s) {
		byte[] data = new byte[s.Length * sizeof(char)];
		System.Buffer.BlockCopy (s.ToCharArray (), 0, data, 0, data.Length);
		return data;
	}
	
	public static string stringFromBytes(byte[] data) {
  		char[] chars = new char[data.Length / sizeof(char)];
  		System.Buffer.BlockCopy (data, 0, chars, 0, data.Length);
 		return new string (chars);	
 	}
}

public enum ObjectType {
	OBJECT_BULIDING,
	OBJECT_PLANET,
	OBJECT_PLAYERSTATE
}

public abstract class ServerObject {
	public int id; //ID of the server object
	public ObjectType type; //Type of the server object (used to inform server of data/operation type).
	public ServerObject(int _id, ObjectType _type) {
		id = _id;
		type = _type;
	}
}

public class Server : MonoBehaviour {
	private startup s;
	private int state = 0;
	private WWW www;
	private Hashtable header = new Hashtable ();
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";

	private List<Building> buildings;
	private List<PlayerState> players;
	private List<planetScript> planets;

	//Keep player id for prototype purposes (switch between players).
	private int pid = 0;
	private int turn = 0;
	
	public Building GetBuilding(int id) {
		return buildings[id];
	}

	//Get a specific player
	public PlayerState GetPlayer(int id) {
		return players [id];
	}


	//Get the current (prototype only - turn based) player
	public PlayerState GetCurrentPlayer() {
		if (players != null) return players [pid];
		else return null;
	}

	public Planet GetMainPlanet(int pid) {
		for (int i = 0; i < planets.Count; i++) {
			Planet p = planets[i].GetPlanet ();
			if(p.owner == pid) {
				return p;
			}
		}
		return null;
	}

	//Prototype-only method
	public void NextTurn() {
		if (pid == 1) {
			turn++;
			//Process the update (prototype only - move to startupdate later.
			byte[] empty = new byte[0];
			ProcessUpdate (empty);
		}
		pid = (pid + 1) % 2;
	}

	public int GetTurn() {
		return turn;
	}

	//mixed clientside/serverside
	void Start() {
		buildings = new List<Building>();
		players = new List<PlayerState> ();
		planets = new List<planetScript> ();
		
		header.Add ("Content-Type", "application/json");
		
		s = GameObject.Find("startup").GetComponent<startup>();
		
		//Create the initial planets. TODO: Read this from server.
		for(int i = 0; i < 10; i++) {
			Planet p = new Planet(i);
			p.sector = 0;
			p.x = 16 + (i * 10);
			p.z = 16 + (i * 10);
			p.scale = 1;
			p.texture = 1;
			p.owner = 0;

			if (i == 0) {
				p.owner = 1;
			} else if (i == 9) {
				p.owner = 2;
			}

			p.income = 100;
			p.slots = 3;
			p.emptySlots = 1;
			planets.Add (s.AttachPlanet(p));
		}
		
		for(int i = 0; i < 4; i++) {
			Building b = new Building(i);
			b.name = "Building " + i;
			b.cost = 100 + (i*100);
			b.income = 10 + (i* 10);
			b.constructionTime = 30.0f + (10 * i);
			b.imageURL = "building" + i + ".jpg";
			buildings.Add(b);
		}
		//Add Two players
		players.Add (new PlayerState (0));
		players.Add (new PlayerState (1));
		//Start the game state. (prototype only)
		byte[] empty = new byte[0];
		ProcessUpdate (empty);
	}

	//Serverside
	public int GetIncome(int pid) {
		//Provide data for the current player only (+security checks).
		int income = 0;
		for(int j = 0; j < planets.Count; j++) {
			Planet p = planets[j].GetPlanet ();
			//For each planet, add planet income and building income.
			if(p.owner == players[pid].id) {
				//Player owns the planet, add income.
				income += p.income;
				for(int k = 0; k < p.buildings.Count; k++) {
					income += GetBuilding(p.buildings[k]).income;
				}
			}
		}
		return income;
	}
	/*
	 * The logic in this function should be moved server-side.
	 * This function should be called on a timer (part of the game state).
	 */
	private void ProcessUpdate(byte[] data) {
		//Update the player credits.
		for (int i = 0; i < players.Count; i++) {
			//Calculate/Apply the players income server-side.
			//Apply the calculated credits.
			int income = GetIncome (i);
			players[i].credits = players[i].credits + income;
			players[i].income = income;
		}
	}

	private IEnumerator StartUpdate() {
		if (state == 0 || state == 2) {
			state = 1;
			yield return new WaitForSeconds (1);
			//Request a new game state.
			byte[] data = ServerUtility.bytesFromString ("{\"auth\":\"sbvcr7sgdll0n8756psrjr7hp3\"}");
			www = new WWW (server_url, data, header);
			yield return www;
			if(www.error == null) state = 2;
			else print(www.error);
			//ProcessUpdate (www.bytes);
		}
	}

	void Update() {
		StartCoroutine (StartUpdate ());
	}
}
