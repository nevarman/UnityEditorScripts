using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomClass))]
public class CustomClasEditor : PropertyDrawer {


	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property,label) *2f;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		Rect pos = new Rect(position.x,position.y ,position.width,position.height);

		EditorGUI.BeginProperty(pos, label, property);
		position = EditorGUI.PrefixLabel (pos, GUIUtility.GetControlID (FocusType.Native), label);
		// Don't make child fields be indented
		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
//
//
//		// Calculate rects
		Rect nameRect = new Rect (position.x, pos.y , position.width, position.height/2);
		Rect colorRect = new Rect (position.x, pos.y+ position.height/2 , 100, position.height/2);
		Rect stateRect = new Rect (position.x+105, pos.y+ position.height/2, 50, position.height/2);
//	
//		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		EditorGUI.PropertyField (nameRect, property.FindPropertyRelative ("name"), GUIContent.none);
		EditorGUI.PropertyField (colorRect, property.FindPropertyRelative ("color"), GUIContent.none);
		EditorGUI.PropertyField (stateRect, property.FindPropertyRelative ("state"), GUIContent.none);
//
//		colorRect = GUILayout.Window(0, colorRect, DoMyWindow, "My Window");
		// Set indent back to what it was
		EditorGUI.indentLevel = indent;


		EditorGUI.EndProperty();
	}
	void DoMyWindow(int windowID) {
		if (GUILayout.Button("Hello World"))
			Debug.Log("Got a click");
		
	}
}
