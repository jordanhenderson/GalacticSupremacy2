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
	OBJECT_PLANET
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
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
	protected Server() {}
	private startup s;
	private int state = 0;
	private WWW www;
	private List<Building> buildings;
	private Hashtable header = new Hashtable ();
	public Building GetBuilding(int id) {
		return buildings[id];
	}
	
	public int GetIncome(int id) {
		
	}

	void Start() {
		buildings = new List<Building>();
		
		header.Add ("Content-Type", "text/json");
		header.Add ("Cookie", "PHPSESSID=sbvcr7sgdll0n8756psrjr7hp3");
		
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
			s.AttachPlanet(p);
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
	}
	
	private IEnumerator DoUpdate() {
		if (state == 0 || state == 2) {
			state = 1;
			yield return new WaitForSeconds (1);
			//Request a new game state.
			byte[] data = ServerUtility.bytesFromString ("{}");
			www = new WWW (server_url, data, header);
			yield return www;
			if(www.error == null) state = 2;
			else print(www.error);
			print (ServerUtility.stringFromBytes (www.bytes));
		}
	}

	void Update() {
		StartCoroutine (DoUpdate ());
	}
}
