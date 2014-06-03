/* Galactic Supremacy Framework. Here are shared struct defs and functions.
 */
using System.Collections.Generic;
namespace gsFramework
{
	/*
	 * SolRegs represent the Solar Regions of the Gameworld. Solar
	 * Regions have a sector ID, belonging to the sector that contains
	 * them, an individual ID, their x and z coordinates in the 
	 * sector-space, and a texture.
	 */
	public struct SolReg : ServerObject {
		public int sector;				// ID of sector that contains the SolReg
		public float x, z;				// coordinates of the SolReg in the sector-space.
		public float scale;				// indicator of the planets size
		public int texture;			// the planets texture for the map
		public int owner;				// owners playerID
		public int income;				// Income provided by SolReg
		public int slots;				// Construction slots on this SolReg
		public int emptySlots;			// Empty construction slots on this SolReg
		public List<int> adjacent;			// ID's of adjacent SolReg
		public List<int> buildings;	// List of buildings on this planet. Keep track of IDs here.
		public SolReg(JSONNode node) : ServerObject(node[0].AsInt, ObjectTypes.OBJECT_SOLREG) {
			Update(node);
		}
		public override void Update(JSONNode node) {
			sector = node[1].AsInt;
			x = node[2].AsFloat;
			z = node[3].AsFloat;
			scale = node[4].AsFloat;
			texture = node[5].AsInt;
			owner = node[6].AsInt;
			income = node[7].AsInt;
			slots = node[8].AsInt;
			emptySlots = node[9].AsInt;
			//Load arrays.
			foreach(JSONNode n in node[10].AsArray) {
				adjacent.Add(n.AsInt);
			}
			foreach(JSONNode n in node[11].AsArray) {
				buildings.Add(n.AsInt);
			}
				
		}
		override JSONClass Serialize() {
			JSONClass c = new JSONClass;
			//Populate c.
		}

	}

	/* Buildings are the options a player has to develop their SolarRegions.
	 */
	public struct Building : ServerObject{
		public string name;				// The name of this building.
		public int cost;				// Construction Cost
		public int income;			// Income this building adds
		public float constructionTime;	// Time it takes to construct this building
		public string imageURL;			// Icon associated with this building
		public Building(JSONNode node) : ServerObject(node[0].AsInt, ObjectTypes.OBJECT_BUILDING) {
			name = node[1].AsString;
			cost = node[2].AsInt;
			income = node[3].AsInt;
			constructionTime = node[4].AsInt;
			imageURL = node[5].AsString;
		}
		public override void Commit() {
			
		}
		public override void Refresh() {
			
		}
	}
}
