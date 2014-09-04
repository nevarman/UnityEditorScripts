using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class FilterHierarcyEditor : EditorWindow {

	FilterOptions filterOptions = FilterOptions.Tag;
	string selectedTag = "Untagged";
	int layer = 0;
	int objectCounter;
	List<int> objectIndex = new List<int>();

	[MenuItem("Custom/Filter Hierarcy")]
	static void Init()
	{
		FilterHierarcyEditor filter  = (FilterHierarcyEditor)EditorWindow.GetWindow (typeof (FilterHierarcyEditor));
		filter.Show();
	}
	void OnGUI () {
		EditorGUILayout.Space();
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

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Select All Objects"))
		{
			selectAll();
		}

		EditorGUILayout.EndHorizontal();

	}

	void selectAll ()
	{
		Object[] all = Object.FindObjectsOfType(typeof(GameObject)) as Object[];
		Selection.objects = all;
	}

	void filterSelected (FilterOptions ops)
	{
		Object[] selected = Selection.objects;
		// if nothing is selected select all by default
		if(selected.Length < 1) 
		{
			selectAll();
			selected = Selection.objects;
		}

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
