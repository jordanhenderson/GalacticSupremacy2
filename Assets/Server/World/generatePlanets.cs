using UnityEngine;
using System.Collections;
using gsFramework;

public class generatePlanets : MonoBehaviour {
	private float dimensions = 20.0f;
	// "Server" startup; Initialisation
	void Start () {
		// draw map borders
		draw_borders();
		connect_regions();
	}
	
	private void connect_regions() {
		for (int i = 0; i < numRegions; ++i) {
			
		}
	}

	/* 
	 * calc_distance takes 2 vectors as inputs and calculates the
	 * straight-line distance using the pythagorean theorem.
	 */
	private float calc_distance(Vector3 point1, Vector3 point2) {
		float distance;
		//float distanceX = Mathf.Abs(point1.x - point2.x);
		//float distanceZ = Mathf.Abs(point1.z - point2.z);

		//distance = Mathf.Sqrt(Mathf.Pow(distanceX, 2.00) + Mathf.Pow(distanceZ, 2.00));
		distance = Vector3.Distance(point1, point2);

		return distance;
	}

	
	/*
	 * getRegions allows a client to retrieve a sectors regions and their attributes.
	 */
	public ref SolReg get_sol_reg (int index) {
		return Server.Instance.regions[index];
	}

	/*
	 * draw_borders simply places a cube on each corner of the map.
	 */
	private void draw_borders() {
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.localPosition = new Vector3(-dimensions, 0, -dimensions);
		GameObject cube2 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube2.transform.localPosition = new Vector3(dimensions, 0, dimensions);
		GameObject cube3 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube3.transform.localPosition = new Vector3(-dimensions, 0, dimensions);
		GameObject cube4 = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube4.transform.localPosition = new Vector3(dimensions, 0, -dimensions);
	}
	




	private Vector3 find_new_pos(int index) {
	 	float colDist = 2.0f;
	 	float minDist = colDist;
	 	float distance;
	 	Vector3 newPos= new Vector3(0, 0, 0);
	 	bool hasCollision = true;
	 	Vector3 otherPos;
	 	

	 	float randX, xDist;


	 	// while (something) {
	 		randX = Random.Range(-dimensions, dimensions);

	 		for (int i = 0; i < index; ++i) {
	 			xDist = Mathf.Abs(randX-regions[i].x);
	 		}
	 	//}


	 	//while (hasCollision == true) {
		 	//newPos = new Vector3(Random.Range(-dimensions, dimensions), 0, Random.Range(-dimensions, dimensions));
		 	
		 	
		 	
		 	/*
		 	for (int i = 0; i < index; ++i) {
		 		otherPos = new Vector3(regions[i].x, 0, regions[i].z);
		 		distance = Vector3.Distance(newPos, otherPos);
		 		
		 		minDist = Mathf.Min(distance, minDist);
		 		print((index+1) + " distance to " + (i+1) + ": " + distance + " | min: " + minDist);
		 	}
		 	if (minDist < colDist) {
		 		print((index+1) + " is in collision!");
		 		hasCollision = true;
		 	} else {
		 		hasCollision = false;
		 	} */
	 	//}
		

		return newPos;
	 }

}


