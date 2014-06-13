using UnityEngine;
using System.Collections;
using gsFramework;

public class cameraController : MonoBehaviour {
	int cameraVelocity = 1;
	public float camspeed = 20;
	public Server server;
	private Vector3 initPos;

	// Initialization
	void Start () {
		NextTurn ();
	}

	public void NextTurn() {
		int pid = server.GetCurrentPlayer ().id;
		Planet p = server.GetMainPlanet (pid); 
		print("CAMERA: pid: " + pid);
		// set starting view to players start planet
		int offset = 0;
		//if (pid == 0) offset *= -1;
		transform.localPosition = new Vector3(p.x + offset, 25, p.z + offset);
		transform.eulerAngles = new Vector3(90, 45, 0);
	}
	
	// Update is called once per frame
	void Update () {
		// Left
		if((Input.GetKey(KeyCode.LeftArrow)))
		{
			transform.Translate((Vector3.left* cameraVelocity) * Time.deltaTime* camspeed);
		}
		// Right
		if((Input.GetKey(KeyCode.RightArrow)))
		{
			transform.Translate((Vector3.right * cameraVelocity) * Time.deltaTime* camspeed);
		}
		// Up
		if((Input.GetKey(KeyCode.UpArrow)))
		{
			transform.Translate((Vector3.up * cameraVelocity) * Time.deltaTime* camspeed);
		}
		// Down
		if(Input.GetKey(KeyCode.DownArrow))
		{
			transform.Translate((Vector3.down * cameraVelocity) * Time.deltaTime* camspeed);
		}
	}
}
