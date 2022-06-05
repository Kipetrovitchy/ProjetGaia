using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(JsonParser))]
public class JsonParserEditor : Editor
{
    public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Parse JSON"))
		{
			(target as JsonParser).Parse();
		}
		base.OnInspectorGUI();
	}
}
