using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class JsonParser : MonoBehaviour
{
	[SerializeField]
	string m_filename;
	[HideInInspector]
	Root m_data;
	List<GameObject> m_cellPoints = new List<GameObject>();

	public void Parse()
	{
		print(m_filename);
		m_data = JsonUtility.FromJson<Root>(File.ReadAllText(m_filename));
		List <Vector2> points = new List<Vector2>();
		DisplayMap();
	}

	private void DisplayMap() {
        Texture2D tx = new Texture2D(2000,2000);
		
        foreach (Vertex v in m_data.vertices)
		{
			Vector2 p0 = new Vector2(v.p[0], v.p[1]);
			print("X: " + p0.x + " Y: " + p0.y);
			foreach(int id in v.v)
			{
				if (id != -1)
				{
					Vector2 p1 = new Vector2(m_data.vertices[id].p[0], m_data.vertices[id].p[1]);
					DrawLine(p0, p1, tx, Color.black);
				}
			}
        }

		foreach(Cell cell in m_data.cells.cells)
		{
			switch(cell.biome)
			{
				case 0:
					tx.SetPixel((int)cell.p[0],(int)cell.p[1], Color.blue);
					break;
				default:
					break;
			}
		}

        tx.Apply();
 
        GetComponent<MeshRenderer>().material.mainTexture = tx;
    }
 
    // Bresenham line algorithm
    private void DrawLine(Vector2 p0, Vector2 p1, Texture2D tx, Color c, int offset = 0) {
        int x0 = (int)p0.x;
        int y0 = (int)p0.y;
        int x1 = (int)p1.x;
        int y1 = (int)p1.y;
       
        int dx = Mathf.Abs(x1-x0);
        int dy = Mathf.Abs(y1-y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx-dy;
       
        while (true) {
            tx.SetPixel(x0+offset,y0+offset,c);
           
            if (x0 == x1 && y0 == y1) break;
            int e2 = 2*err;
            if (e2 > -dy) {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx) {
                err += dx;
                y0 += sy;
            }
        }
    }
}


[Serializable]
public struct Root
{
	public Info info;
	public Settings settings;
	public Coords coords;
	public Mapcells cells;
	public List<Vertex> vertices;
	public Biomes biomes;
	public List<Note> notes;
	public List<NameBasis> nameBases;
}

[Serializable]
public struct Info
{
	public string version;
	public string description;
	public DateTime exportedAt;
	public string mapName;
	public string seed;
	public long mapId;
}

[Serializable]
public struct Settings
{
	public string distanceUnit;
	public string distanceScale;
	public string areaUnit;
	public string heightUnit;
	public string heightExponent;
	public string temperatureScale;
	public string barSize;
	public string barLabel;
	public string barBackOpacity;
	public string barBackColor;
	public string barPosX;
	public string barPosY;
	public int populationRate;
	public int urbanization;
	public string mapSize;
	public string latitudeO;
	public string temperatureEquator;
	public string temperaturePole;
	public string prec;
	public Options options;
	public string mapName;
	public bool hideLabels;
	public string stylePreset;
	public bool rescaleLabels;
	public int urbanDensity;
}

[Serializable]
public struct Mapcells
{
	public List<Cell> cells;
	public List<object> features;
	public List<Culture> cultures;
	public List<Burg> burgs;
	public List<State> states;
	public List<object> provinces;
	public List<Religion> religions;
	public List<River> rivers;
	public List<Marker> markers;
}

[Serializable]
public struct Biomes
{
	public List<int> i;
	public List<string> name;
	public List<string> color;
	public List<BiomesMartix> biomesMartix;
	public List<int> habitability;
	public List<int> iconsDensity;
	public List<List<string>> icons;
	public List<int> cost;
}

[Serializable]
public struct BiomesMartix
{
	public int _0;
	public int _1;
	public int _2;
	public int _3;
	public int _4;
	public int _5;
	public int _6;
	public int _7;
	public int _8;
	public int _9;
	public int _10;
	public int _11;
	public int _12;
	public int _13;
	public int _14;
	public int _15;
	public int _16;
	public int _17;
	public int _18;
	public int _19;
	public int _20;
	public int _21;
	public int _22;
	public int _23;
	public int _24;
	public int _25;
}

[Serializable]
public struct Burg
{
	public int? cell;
	public double? x;
	public double? y;
	public int? state;
	public int? i;
	public int? culture;
	public string name;
	public int? feature;
	public int? capital;
	public int? port;
	public double? population;
	public string type;
	public Coa coa;
	public int? citadel;
	public int? plaza;
	public int? walls;
	public int? shanty;
	public int? temple;
}

[Serializable]
public struct Campaign
{
	public string name;
	public int start;
	public int end;
}

[Serializable]
public struct Cell
{
	public int i;
	public List<int> v;
	public List<int> c;
	public List<double> p;
	public int g;
	public int h;
	public int area;
	public int f;
	public int t;
	public int haven;
	public int harbor;
	public int fl;
	public int r;
	public int conf;
	public int biome;
	public int s;
	public int pop;
	public int culture;
	public int burg;
	public int road;
	public int crossroad;
	public int state;
	public int religion;
	public int province;
}

[Serializable]
public struct Charge
{
	public string charge;
	public string t;
	public string p;
	public double size;
	public string divided;
	public int sinister;
}

[Serializable]
public struct Coa
{
	public string t1;
	public List<Charge> charges;
	public string shield;
	public Division division;
	public List<Ordinary> ordinaries;
}

[Serializable]
public struct Coords
{
	public int latT;
	public double latN;
	public double latS;
	public int lonT;
	public int lonW;
	public int lonE;
}

[Serializable]
public struct Culture
{
	public string name;
	public int i;
	public int @base;
	public int? origin;
	public string shield;
	public int? center;
	public string color;
	public string type;
	public double? expansionism;
	public string code;
}

[Serializable]
public struct Division
{
	public string division;
	public string t;
	public string line;
}

[Serializable]
public struct Marker
{
	public string icon;
	public string type;
	public int dx;
	public int px;
	public double x;
	public double y;
	public int cell;
	public int i;
	public int? dy;
}

[Serializable]
public struct Military
{
	public string icon;
	public string name;
	public double rural;
	public double urban;
	public int crew;
	public int power;
	public string type;
	public int separate;
	public int i;
	public int a;
	public int cell;
	public double x;
	public double y;
	public double bx;
	public double by;
	public U u;
	public int n;
	public int state;
}

[Serializable]
public struct NameBasis
{
	public string name;
	public int i;
	public int min;
	public int max;
	public string d;
	public double m;
	public string b;
}

[Serializable]
public struct Note
{
	public string id;
	public string name;
	public string legend;
}

[Serializable]
public struct Options
{
	public bool pinNotes;
	public bool showMFCGMap;
	public List<int> winds;
	public string stateLabelsMode;
	public int year;
	public string era;
	public string eraShort;
	public List<Military> military;
}

[Serializable]
public struct Ordinary
{
	public string ordinary;
	public string t;
	public string line;
}

[Serializable]
public struct Religion
{
	public int i;
	public string name;
	public string color;
	public int? culture;
	public string type;
	public string form;
	public string deity;
	public int? center;
	public int? origin;
	public string code;
	public string expansion;
	public int? expansionism;
}

[Serializable]
public struct River
{
	public int i;
	public int source;
	public int mouth;
	public int discharge;
	public double length;
	public double width;
	public double widthFactor;
	public int sourceWidth;
	public int parent;
	public List<int> cells;
	public int basin;
	public string name;
	public string type;
}



[Serializable]
public struct State
{
	public int i;
	public string name;
	public double urban;
	public double rural;
	public int burgs;
	public int area;
	public int cells;
	public List<int> neighbors;
	public List<string> diplomacy;
	public List<int> provinces;
	public string color;
	public double? expansionism;
	public int? capital;
	public string type;
	public int? center;
	public int? culture;
	public Coa coa;
	public List<Campaign> campaigns;
	public string form;
	public string formName;
	public string fullName;
	public List<double> pole;
	public double? alert;
	public List<Military> military;
}

[Serializable]
public struct U
{
	public int infantry;
	public int archers;
	public int cavalry;
	public int artillery;
	public int? fleet;
}

[Serializable]
public struct Vertex
{
	public List<int> p;
	public List<int> v;
	public List<int> c;
}



