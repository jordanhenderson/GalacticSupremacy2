using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using gsFramework;
using SimpleJSON;
public enum ServerAction {
	ACTION_SEND_CHAT,
	ACTION_GET_CHAT,
	ACTION_GET_INITIALSTATE,
	ACTION_GET_REGION,
	ACTION_UPDATE_REGION,
};

public class Server : Singleton<Server> {
	protected Server() {}
	private byte[] bytesFromString(string s) {
		byte[] data = new byte[s.Length * sizeof(char)];
		System.Buffer.BlockCopy (s.ToCharArray (), 0, data, 0, data.Length);
		return data;
	}
	private string stringFromBytes(byte[] data) {
		char[] chars = new char[data.Length / sizeof(char)];
		System.Buffer.BlockCopy (data, 0, chars, 0, data.Length);
		return new string (chars);	
	}
	
	public bool loaded = false;
	public List<SolReg> regions = new List<SolReg>();
	private void ParseInitialState(byte[] data) {
		string data_str = stringFromBytes(data);
		//Deserialize the json array into gamedata.
		JSONArray s = JSON.Parse (data_str).AsArray;
		/* Naiive game-state systems read in the entire
		 * game state each frame or tick. This needs to be
		 * implemented using a much more efficient algorithm
		 * (only send players what they need/make client-side
		 * predictions.
		 */
		foreach (JSONNode node in s[1].AsArray) {
			SolReg r = new SolReg(node);
		}
		loaded = true;
	}
	
	private void ParseGameState(byte[] data) {
		JSONArray s = JSON.Parse(stringFromBytes(data)).AsArray;
		foreach(JSONNode node in s) {
			
		} 
	}

	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
	private Hashtable header = new Hashtable ();
	void Startup() {
		header.Add ("Content-Type", "text/json");
	}

	private IEnumerator StartGame() {
		string request = "{action:" + ServerAction.ACTION_GET_INITIALSTATE + "}";
		byte[] data = bytesFromString (request);
		header["Content-Length"] = data.Length;
		WWW www = new WWW (server_url, data, header);
		yield return www;
		ParseInitialState(www.bytes);
	}
	

	void Update() {
		
	}
}
