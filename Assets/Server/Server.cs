using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SimpleJSON;

public class Server : Singleton<Server> {
	protected Server() {}
	private byte[] bytesFromString(string s) {
		byte[] data = new byte[s.Length * sizeof(char)];
		System.Buffer.BlockCopy (s.ToCharArray (), 0, data, 0, data.Length);
	public void UpdateObject(GameObject obj) {
		string request = "{action:1, 
	}		return data;
	}
	private string stringFromBytes(byte[] data) {
		char[] chars = new char[data.Length / sizeof(char)];
		System.Buffer.BlockCopy (data, 0, chars, 0, data.Length);
		return new string (chars);	
	}
	
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
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
