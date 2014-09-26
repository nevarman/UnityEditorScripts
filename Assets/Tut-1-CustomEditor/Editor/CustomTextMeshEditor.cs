using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(TextMesh))]
public class CustomTextMeshEditor : Editor {
	TextMesh textMesh;
	void OnEnable ()
	{
		textMesh = target as TextMesh;
	}
	public override void OnInspectorGUI ()
	{
		checkMouse();
		textMesh.text = EditorGUILayout.TextArea(textMesh.text);
		DrawDefaultInspector();
	}
	void checkMouse() {
		Event e = Event.current;
		if (e.type.Equals(EventType.KeyDown)) {
			Undo.RecordObject(textMesh,"textMeshCustom");
		}
	}
}
