using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;

public enum ObjectTypes {
	OBJECT_SOLREG,
	OBJECT_BULIDING,
	OBJECT_PLANET
}

public class ServerUtility {
	public byte[] bytesFromString(string s) {
		byte[] data = new byte[s.Length * sizeof(char)];
		System.Buffer.BlockCopy (s.ToCharArray (), 0, data, 0, data.Length);
		return data;
	}
	
	public string stringFromBytes(byte[] data) {
  		char[] chars = new char[data.Length / sizeof(char)];
  		System.Buffer.BlockCopy (data, 0, chars, 0, data.Length);
 		string data_str = new string (chars);
 		return new string (chars);	
 	}
}

public abstract class ServerObject : MonoBehavior {
	public int id; //ID of the server object
	public int type; //Type of the server object (used to inform server of data/operation type).
	public ServerObject(int id, int type) {
		id = id;
		type = type;
	}
	//This function is called after the object has been reloaded.
	abstract void Update(JSONNode node);
	
	//This function is called when the object should be committed, populating a JSONClass object.
	abstract JSONClass Serialize();
	
	public void Commit() {
		JSONClass c = Serialize();
		Server.CommitObject(c);
	}

	private void _Refresh() {
		WWW www = Server.RefreshObject(this);
		yield return www;
		Update(JSON.Parse(ServerUtility.stringFromBytes(www.bytes)));
	}
	
	public void Refresh() {
		StartCoroutine(_Refresh());
	}
}

public class Server : Singleton<Server> {
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
	protected Server() {}

	private Hashtable header = new Hashtable ();
	void Startup() {
		header.Add ("Content-Type", "text/json");
	}

	private IEnumerator CommitObject(JSONClass c) {
		string request = "{action:1, data: " + c.ToString() + "}";
		byte[] data = bytesFromString(request);
		header["Content-Length"] = data.Length;
		WWW www = new WWW(server_url, data, header);
		yield return www;
	}
	
	private WWW RefreshObject(ServerObject obj) {
		string request = "{action: 2, type: " + obj.type + ", id: " + obj.id + "}";
		byte[] data = bytesFromString(request);
		header["Content-Length"] = data.Length;
		return new WWW(server_url, data, header);
	}
}
