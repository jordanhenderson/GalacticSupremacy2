/* Galactic Supremacy Framework. Here are shared struct defs and functions.
 */
using System.Collections.Generic;
namespace gsFramework
{
	/*
	 * Planets have a sector ID, belonging to the sector that contains
	 * them, an individual ID, their x and z coordinates in the 
	 * sector-space, and a texture.
	 */
	public class Planet : ServerObject {
		public int sector;				// ID of sector that contains the Planet
		public float x, z;				// coordinates of the Planet in the sector-space.
		public float scale;				// indicator of the planets size
		public int texture;			// the planets texture for the map
		public int owner;				// owners playerID
		public int income;				// Income provided by Planet
		public int slots;				// Construction slots on this Planet
		public int emptySlots;			// Empty construction slots on this Planet
		public List<int> adjacent;			// ID's of adjacent Planet
		public List<int> buildings;	// List of buildings on this planet. Keep track of IDs here.
		public Planet(int id) : base(id, ObjectType.OBJECT_PLANET)
		{
			adjacent = new List<int>();
			buildings = new List<int>();
		}
	}

	/* Buildings are the options a player has to develop their SolarRegions.
	 */
	public class Building : ServerObject {
		public string name;				// The name of this building.
		public int cost;				// Construction Cost
		public int income;			// Income this building adds
		public float constructionTime;	// Time it takes to construct this building
		public string imageURL;			// Icon associated with this building
		public Building(int id) : base(id, ObjectType.OBJECT_BULIDING)
		{

		}
	}
}
