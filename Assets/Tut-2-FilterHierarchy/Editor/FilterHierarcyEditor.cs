using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class FilterHierarcyEditor : EditorWindow {

	FilterOptions filterOptions = FilterOptions.Tag;
	string selectedTag = "Untagged";
	int layer = 0;
	int objectCounter;
	bool filterInactive;
	List<int> objectIndex = new List<int>();
	
	[MenuItem("Custom/Tut2-Filter Hierarcy")]
	static void Init()
	{
		FilterHierarcyEditor filter  = (FilterHierarcyEditor)EditorWindow.GetWindow (typeof (FilterHierarcyEditor));
		filter.Show();
	}
	void OnGUI () {
		EditorGUILayout.Space();
		GUI.color = Color.green;
		if(GUILayout.Button("Go to tutorial page"))
		{
			Application.OpenURL("http://nevzatarman.com/");
		}
		GUI.color = Color.white;
		EditorGUILayout.PrefixLabel("Filtering Options");
		filterInactive = EditorGUILayout.Toggle("Filter Inactive",filterInactive);
		filterOptions =(FilterOptions) EditorGUILayout.EnumPopup("Filter By",filterOptions);
		if(filterOptions == FilterOptions.Tag)
		{
			selectedTag = EditorGUILayout.TagField("Select Tag",selectedTag);
			if(GUILayout.Button("Filter by Tag"))
			{
				filterSelected(filterOptions);
			}
		}
		else if(filterOptions == FilterOptions.Layer)
		{
			layer = EditorGUILayout.LayerField("Select Layer",layer);
			if(GUILayout.Button("Filter by Layer"))
			{
				filterSelected(filterOptions);
			}
		}
//		EditorGUILayout.PrefixLabel("Save and Load Options");
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if(Selection.objects.Length >= 1)
		{
			if(GUILayout.Button("Save Selection"))
			{
				int[] selectionIDs = Selection.instanceIDs;
				var saveStr = string.Empty;
				foreach( int i in selectionIDs)
					saveStr += i.ToString() + ";";
				saveStr = saveStr.TrimEnd(char.Parse(";"));
				EditorPrefs.SetString("SelectedIDs",saveStr);
			}
		}
		if(EditorPrefs.HasKey("SelectedIDs"))
		{
			if(GUILayout.Button("Load Selection"))
			{
			   	string[] strIDs= EditorPrefs.GetString("SelectedIDs").Split(char.Parse(";"));
				int[] ids = new int[strIDs.Length];
				for(var i = 0; i < strIDs.Length; i++)
					ids[i] = int.Parse(strIDs[i]);
				Selection.instanceIDs = ids;
			}
		}
		EditorGUILayout.EndHorizontal();
	}

	void OnInspectorUpdate() {
		Repaint();
	}

	Object[] selectAll ()
	{
		return Resources.FindObjectsOfTypeAll(typeof(GameObject)) as Object[];
	}
	Object[] selectActive ()
	{
		return Object.FindObjectsOfType(typeof(GameObject)) as Object[];
	}

	void filterSelected (FilterOptions ops)
	{

		Object[] selected  = filterInactive ? selectAll() : selectActive();
		Selection.objects = selected;

		objectCounter = 0;
		objectIndex = new List<int>();
		for(int i = 0; i< selected.Length; i++)
		{
			GameObject g = (GameObject)selected[i] as GameObject;

			if(ops == FilterOptions.Tag && g.tag == selectedTag)
			{
				objectCounter ++;
				objectIndex.Add(i);
			}
			else if(ops == FilterOptions.Layer && g.layer == layer)
			{
				objectCounter ++;
				objectIndex.Add(i);
			}
		}
		Object[] newSelected = new Object[objectCounter];
		for(int i = 0; i< objectCounter; i++)
		{
			newSelected[i] = selected[objectIndex[i]];
		}
		Selection.objects = newSelected;
	}

}
enum FilterOptions
{
	Tag,Layer
}
