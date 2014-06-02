/* Galactic Supremacy Framework. Here are shared struct defs and functions.
 */
namespace gsFramework
{
	/*
	 * SolRegs represent the Solar Regions of the Gameworld. Solar
	 * Regions have a sector ID, belonging to the sector that contains
	 * them, an individual ID, their x and z coordinates in the 
	 * sector-space, and a texture.
	 */
	public struct SolReg {
		public int sector;				// ID of sector that contains the SolReg
		public int id;					// ID of this SolReg
		public float x, z;				// coordinates of the SolReg in the sector-space.
		public float scale;				// indicator of the planets size
		public string texture;			// the planets texture for the map
		public int[] adjacent;			// ID's of adjacent SolReg
		public int owner;				// owners playerID
		public int income;				// Income provided by SolReg
		public int slots;				// Construction slots on this SolReg
		public int emptySlots;			// Empty construction slots on this SolReg
		public Building[] buildings;	// List of buildings on this planet
		public SolReg(JSONNode node) {
			sector = node[0].AsInt;
			id = node[1].AsInt;
			x = node[2].AsFloat;
			z = node[3].AsFloat;
			scale = node[4].AsFloat;
			texture = "texture" + node[5].AsInt.ToString ();
			owner = node[6].AsInt;
			income = node[7].AsInt;
			slots = node[8].AsInt;
			emptySlots = node[9].AsInt;
		}
	}

	/* Buildings are the options a player has to develop their SolarRegions.
	 */
	public struct Building {
		public int id;					// Unique ID of the building type
		public string name;				// The name of this building.
		public int cost;				// Construction Cost
		public int income;			// Income this building adds
		public float constructionTime;	// Time it takes to construct this building
		public string imageURL;			// Icon associated with this building
		public Building(int i, string n, int c, int inc, float ct, string u) {
			this.id = i;
			this.name = n;
			this.cost = c;
			this.income = inc;
			this.constructionTime = ct;
			this.imageURL = u;
		}
	}
}
