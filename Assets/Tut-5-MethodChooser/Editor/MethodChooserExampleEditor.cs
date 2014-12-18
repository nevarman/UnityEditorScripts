using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Reflection;
[CustomEditor(typeof(MethodChooserExampleBase),true)]
public class MethodChooserExampleEditor : Editor {

	MethodChooserExample methodChoose;

	string[] methodNames;
	string[] propertyNames;

	void OnEnable()
	{
		methodChoose = target as MethodChooserExample;
		System.Type type= methodChoose.GetType();
		methodChoose.methods = type.GetMethods(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
		methodChoose.variables = type.GetFields(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
		methodNames = new string[methodChoose.methods.Length];
		propertyNames = new string[methodChoose.variables.Length];
		for(int i=0; i< methodNames.Length; i++)
		{
			methodNames[i] = methodChoose.methods[i].Name;
		}
		for(int i=0; i< propertyNames.Length; i++)
		{
			propertyNames[i] = methodChoose.variables[i].ToString();
		}
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginHorizontal();
		methodChoose.methodToCall = EditorGUILayout.Popup("Method to call",methodChoose.methodToCall,methodNames);
//		methodChoose.addParameter = EditorGUILayout.Toggle("Add parameter",methodChoose.addParameter);
		EditorGUILayout.EndHorizontal();
		ParameterInfo[] p = methodChoose.methods[methodChoose.methodToCall].GetParameters();

		if(p.Length > 0)
		{
			methodChoose.paramToCall = EditorGUILayout.Popup("Param to add",methodChoose.paramToCall,propertyNames);
			if(methodChoose.variables[methodChoose.paramToCall].FieldType!= p[0].ParameterType)
				EditorGUILayout.HelpBox("Check parameter",MessageType.Warning);
		}

		EditorGUILayout.Space();
		DrawDefaultInspector();
	}
}
