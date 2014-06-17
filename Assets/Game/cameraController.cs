using UnityEngine;
using System.Collections;
using gsFramework;

public class cameraController : MonoBehaviour {
	
	private Vector3 dragOrigin;
	int cameraVelocity = 1;
	public float camspeed = 20f;
	public float dragSpeed = 2f;
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
		transform.localPosition = new Vector3(p.x + offset, 30, p.z + offset);
		transform.eulerAngles = new Vector3(90, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		KeyBoardControls ();
		ClickToDrag ();

	}


	void ClickToDrag () {
		if (Input.GetMouseButtonDown(0)) {
			// record position of mouseclick
			dragOrigin = Input.mousePosition;
			return;
		}
		
		// continue only if mouse button is being held down.
		if (!Input.GetMouseButton(0)) return;
		
		// distance is the difference between drag origin and current mousePosition.
		Vector3 distance = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
		Vector3 move = new Vector3(distance.x * 2f, 0, distance.y * 2f);

		// move the camera to the new position
		transform.Translate(move, Space.World);	
	}

	void KeyBoardControls () {
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
