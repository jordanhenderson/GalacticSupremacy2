using UnityEngine;
using System.Collections;
using gsFramework;

public class handleBuildings : MonoBehaviour {
	public void ConstructBuilding(int planet_id, int building_id) {
		Server.Instance.SendRequest (ServerAction.ACTION_CONSTRUCT_BUILDING);
	}
};