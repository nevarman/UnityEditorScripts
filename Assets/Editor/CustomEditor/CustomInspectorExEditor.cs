using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CustomEditorEx))]
public class CustomInspectorExEditor : Editor {

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector();

	}
}
