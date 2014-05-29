using UnityEngine;
using System.Collections.Generic;
using gsFramework;

public class Server : Singleon<Server> {
	protected Server() {}
	
	public int numRegions = 10;	// The number of SolReg in the sector.
	public List<SolReg> regions = new List<SolReg>();
	
	public List<SolReg> regions;
	private enum Actions
		{
		ACTION_SEND_CHAT,
		ACTION_GET_CHAT,
		ACTION_UPDATE_GAMESTATE,
		ACTION_PLAYER_EVENT
		}
	private void ParseGameState(byte[] data) {
		char[] chars = new char[data.Length / sizeof(char)];
		System.Buffer.BlockCopy (data, 0, chars, 0, data.Length);
		string data_str = new string (chars);
		//Deserialize the json array into gamedata.
		JSONArray s = JSON.Parse (data_str).AsArray;
		/* Naiive game-state systems read in the entire
		 * game state each frame or tick. This needs to be
		 * implemented using a much more efficient algorithm
		 * (only send players what they need/make client-side
		 * predictions.
		 */
		foreach (JSONNode node in s[1].AsArray) {
			SolReg r = new SolReg();
			r.sector = node[0].AsInt;
			r.id = node[1].AsInt;
			r.x = node[2].AsFloat;
			r.z = node[3].AsFloat;
			r.scale = node[4].AsFloat;
			r.texture = "texture" + node[5].AsInt.ToString ();
			r.owner = node[6].AsInt;
			r.income = node[7].AsInt;
			r.slots = node[8].AsInt;
		}
	}
	
	private string server_url = "http://deco3800-14.uqcloud.net/game.php";
	private Hashtable header = new Hashtable ();
	void Startup() {
		header.Add ("Content-Type", "text/json");
	}

	private IEnumerator UpdateGame() {
		string request = "{action:" + Actions.ACTION_UPDATE_GAMESTATE + "}";
		byte[] data = new byte[request.Length * sizeof(char)];
		System.Buffer.BlockCopy (request.ToCharArray (), 0, data, 0, data.Length);
		header["Content-Length"] = data.Length;
		WWW www = new WWW (server_url, data, header);
		yield return www;
		ParseGameState (www.bytes);
	}
	
	private int tick = 0; 
	private int tick_step = 100; //How often to request the game state.
	void Update() {
		/* Apply the current game state to the locally running game. */
		if (tick % tick_step == 0) {
			StartCoroutine (UpdateGame ());
			tick = 0;
		}
		tick++;
	}
}
