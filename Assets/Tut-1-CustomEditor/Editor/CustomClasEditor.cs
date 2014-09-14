using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(CustomClass))]
public class CustomClasEditor : PropertyDrawer {
	const int helpHeight = 20;
	const int textHeight = 16;

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight(property,label)  +helpHeight + textHeight;
	}

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		SerializedProperty name = property.FindPropertyRelative ("name");
		SerializedProperty color = property.FindPropertyRelative ("color");
		SerializedProperty state = property.FindPropertyRelative ("state");

		// Calculate rects
		Rect nameRect = new Rect (position.x, position.y , position.width, textHeight);
		Rect colorRect = new Rect (position.x, position.y+textHeight , position.width/2, helpHeight);
		Rect stateRect = new Rect (position.x+ position.width/2, position.y+ textHeight, position.width/2, textHeight);
		// Draw fields - passs GUIContent.none to each so they are drawn without labels
		if(name != null)
			EditorGUI.PropertyField (stateRect, name, new GUIContent("Text"));
		if(color != null)
			EditorGUI.PropertyField (colorRect,color, new GUIContent("Color"));
//		EditorGUI.PropertyField (stateRect, property.FindPropertyRelative ("state"), GUIContent.none);	
		if(state != null)
			EditorGUI.IntSlider(nameRect,state,0,25);

	}

	void DrawHelpBox (Rect position, SerializedProperty prop) {
		EditorGUI.BeginChangeCheck();
		EditorGUI.HelpBox (position, "regexAttribute.helpMessage", MessageType.Error);
		EditorGUI.EndChangeCheck();
	}
}
