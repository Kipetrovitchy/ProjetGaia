using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapViewer))]
public class MapViewerEditor : Editor
{
	private const string ASSET_EXT = ".asset";
	private const string MESH_FOLDER_PATH = "Assets/Meshes/";
    public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Generate mesh"))
		{
			(target as MapViewer).GenerateMesh();
		}

		if (GUILayout.Button("Create mesh asset"))
		{
			(target as MapViewer).mesh = new Mesh();
			AssetDatabase.CreateAsset((target as MapViewer).mesh, MESH_FOLDER_PATH + target.name + ASSET_EXT);
		}
		base.OnInspectorGUI();
	}
}
