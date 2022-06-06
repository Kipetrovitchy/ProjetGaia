using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapViewer : MonoBehaviour
{
	static readonly float s_underwaterHeight = -10f;
	[SerializeField]
	public Mesh mesh;
	[SerializeField]
	JsonParser m_json;
	Vector2 m_min;
	Vector2 m_max;

	public void GenerateMesh()
	{
		if (mesh == null)
		{
			Debug.LogError("no mesh");
			return;
		}

		mesh.Clear();
		List<Vector3> vertices = new List<Vector3>();
		List<Vector2> uv = new List<Vector2>();
		List<int> indices = new List<int>();
		int offset = m_json.data.vertices.Count;

		ComputeBounds();

		foreach(Vertex v in m_json.data.vertices)
		{
			Vector3 point = new Vector3(v.p[0], s_underwaterHeight, m_max.y -v.p[1]);
			int landcells = 0;
			foreach(int id in v.c)
			{
				if (id < 0 || id > m_json.data.cells.cells.Count - 1 || m_json.data.cells.cells[id].biome == 0)
					continue;
				landcells++;
				if (point.y == s_underwaterHeight)
					point.y = 0f;

				point.y = (point.y + GetHeight(m_json.data.cells.cells[id]));
			}

			if (landcells > 0)
				point.y = (point.y / landcells);
			
			vertices.Add(point);
			uv.Add(GetUV(point));
		}

		foreach (Cell c in m_json.data.cells.cells)
		{
			Vector3 point = new Vector3((float)c.p[0], GetHeight(c), m_max.y - (float)c.p[1]);
			if (c.biome == 0)
				point.y = s_underwaterHeight;
			vertices.Add(point);
			uv.Add(GetUV(point));

			for(int i = 0; i < c.v.Count; i++)
			{
				if (i == c.v.Count - 1)
					indices.Add(c.v[0]);
				else
					indices.Add(c.v[i+1]);
				indices.Add(offset + c.i);
				indices.Add(c.v[i]);
			}
		}
		mesh.vertices = vertices.ToArray();
		mesh.triangles = indices.ToArray();
		mesh.uv = uv.ToArray();

		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}

	void ComputeBounds()
	{
		m_min = m_max = new Vector2(-1f, -1f);
		foreach (Vertex v in m_json.data.vertices)
		{
			if (v.p[0] < m_min.x || m_min.x < 0f)
				m_min.x = v.p[0];
			else if (v.p[0] > m_max.x || m_max.x < 0f)
				m_max.x = v.p[0];
			
			if (v.p[1] < m_min.y || m_min.y < 0f)
				m_min.y = v.p[1];
			else if (v.p[1] > m_max.y || m_max.y < 0f)
				m_max.y = v.p[1];
		}
	}

	Vector2 GetUV(Vector3 input)
	{

		return new Vector2(
			input.x / (m_max.x - m_min.x),
			input.z / (m_max.y - m_min.y)
		);
	}

	float GetHeight(Cell c)
	{
		return (c.h - 19f);
	}
}