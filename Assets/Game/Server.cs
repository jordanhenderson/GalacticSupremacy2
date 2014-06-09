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
	public void Submit() {
		//Push the object to the server.
	}
	//Update() is called with the byte array returned from the Refresh request.
	//Update should be implemented for each ServerObject instance.
	public abstract void UpdateData(byte[] data);
}

public class Server : MonoBehaviour {
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
	protected Server() {}
	private startup s;
	private WWW www;
	
	private Hashtable header = new Hashtable ();
	void Startup() {
		header.Add ("Content-Type", "text/json");
	}

	void Start() {
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
	
	}
	
	private IEnumerator RefreshGame() {
		yield return www;
		print(ServerUtility.stringFromBytes(www.bytes));
	}

	private IEnumerator DoUpdate() {
		yield return new WaitForSeconds(0.5f);
		//Request a new game state.
		byte[] data = ServerUtility.bytesFromString("{}");
		www = new WWW(server_url, data, header);
		yield return StartCoroutine(RefreshGame());
	}

	void Update() {
		StartCoroutine (DoUpdate ());
	}
}
