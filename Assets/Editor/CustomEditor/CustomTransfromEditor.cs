using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(Transform))]
public class CustomTransfromEditor : Editor {
	Transform _transform;
	EditorBehaviorMode editorMode;
	Vector3 position,eulerAngles,scale;

	// 2d options
	float z;
	Vector2 s ;

	void OnEnable()
	{
		_transform = target as Transform;
		editorMode = EditorSettings.defaultBehaviorMode;
	}

	public override void OnInspectorGUI ()
	{
		// Check editor mode
		if(editorMode.Equals(EditorBehaviorMode.Mode3D))
		{
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localPosition = Vector3.zero;
			}
			position = EditorGUILayout.Vector3Field("Position", _transform.localPosition);
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				position = Selection.activeTransform.localPosition;
			}
			GUILayout.EndHorizontal();
			//ROTATE 
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localRotation = new Quaternion(0f,0f,0f,0f);
			}
			eulerAngles = EditorGUILayout.Vector3Field("Rotation", _transform.localEulerAngles);
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				eulerAngles = Selection.activeTransform.eulerAngles;
			}
			GUILayout.EndHorizontal();
			
			// SCALE
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localScale = Vector3.one;
			}
			scale = EditorGUILayout.Vector3Field("Scale", _transform.localScale);
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				scale = Selection.activeTransform.localScale;
			}
			GUILayout.EndHorizontal();
		}
		else 
		{
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localPosition = Vector3.zero;
			}
			Vector2 p = EditorGUILayout.Vector2Field("Position", new Vector2(_transform.localPosition.x,_transform.localPosition.y));
			position = new Vector3(p.x,p.y,0f);
			GUILayout.EndHorizontal();
			//ROTATE 
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localRotation = new Quaternion(0f,0f,0f,0f);
			}
			z = EditorGUILayout.FloatField("Rotation",z);
			eulerAngles = new Vector3(eulerAngles.x,eulerAngles.y,z);
			GUILayout.EndHorizontal();
			
			// SCALE
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localScale = Vector3.one;
			}
			s = EditorGUILayout.Vector2Field("Scale", new Vector2(_transform.localScale.x,_transform.localScale.y));
			scale = new Vector3(s.x,s.y,1f);
			GUILayout.EndHorizontal();
		}

		// Normal editor changes
		if (GUI.changed)
		{
			regUndo();
			_transform.localPosition = FixIfNaN(position);
			_transform.localEulerAngles = FixIfNaN(eulerAngles);
			_transform.localScale = FixIfNaN(scale);
		}

		// Copy button
		if(GUILayout.Button("Copy Transfrom From Selection"))
		{
			regUndo();
			Transform t= Selection.activeTransform;
			_transform.localPosition = t.localPosition;
			_transform.localRotation = t.localRotation;
			_transform.localScale = t.localScale;
		}
		EditorGUI.indentLevel = 0;
	}
	
	private Vector3 FixIfNaN(Vector3 v)
	{
		if (float.IsNaN(v.x))
		{
			v.x = 0;
		}
		if (float.IsNaN(v.y))
		{
			v.y = 0;
		}
		if (float.IsNaN(v.z))
		{
			v.z = 0;
		}
		return v;
	}

	void regUndo()
	{
		Undo.RecordObject(_transform, "Transform Change");
	}
}
