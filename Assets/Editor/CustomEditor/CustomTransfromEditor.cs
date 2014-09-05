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

	// store
	static Vector3 savedPos,savedScale,savedRotation;

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
			/// Copy button
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedPos = _transform.position;
			}
			/// Paste button
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				position = savedPos;
			}
			GUILayout.EndHorizontal();

			///ROTATE 
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localRotation = new Quaternion(0f,0f,0f,0f);
			}
			eulerAngles = EditorGUILayout.Vector3Field("Rotation", _transform.localEulerAngles);
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedRotation = _transform.localEulerAngles;
			}
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				eulerAngles = savedRotation;
			}
			GUILayout.EndHorizontal();
			
			/// SCALE
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localScale = Vector3.one;
			}
			scale = EditorGUILayout.Vector3Field("Scale", _transform.localScale);
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedScale = _transform.localScale;
			}
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				scale = savedScale;
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
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedPos = _transform.position;
			}
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				position = savedPos;
			}
			GUILayout.EndHorizontal();

			///ROTATE 
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localRotation = new Quaternion(0f,0f,0f,0f);
			}
			z = EditorGUILayout.FloatField("Rotation",z);
			eulerAngles = new Vector3(eulerAngles.x,eulerAngles.y,z);
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedRotation = _transform.localEulerAngles;
			}
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				eulerAngles = savedRotation;
			}
			GUILayout.EndHorizontal();
			
			/// SCALE
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("R",GUILayout.Width(20f)))
			{
				regUndo();
				_transform.localScale = Vector3.one;
			}
			s = EditorGUILayout.Vector2Field("Scale", new Vector2(_transform.localScale.x,_transform.localScale.y));
			scale = new Vector3(s.x,s.y,1f);
			if(GUILayout.Button("C",GUILayout.Width(20f)))
			{
				savedScale = _transform.localScale;
			}
			if(GUILayout.Button("P",GUILayout.Width(20f)))
			{
				regUndo();
				scale = savedScale;
			}
			GUILayout.EndHorizontal();
		}

//		 Normal editor changes
		// This part is taken from wiki: http://wiki.unity3d.com/index.php/TransformInspector
		if (GUI.changed)
		{
			regUndo();
			_transform.localPosition = FixIfNaN(position);
			_transform.eulerAngles = FixIfNaN(eulerAngles);
			_transform.localScale = FixIfNaN(scale);
		}
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
