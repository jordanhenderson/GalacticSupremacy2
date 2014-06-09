using UnityEngine;
using System.Collections;

public class LoginCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (1.0f * Time.deltaTime, 1.0f * Time.deltaTime, 1.0f * Time.deltaTime);
	}
}
