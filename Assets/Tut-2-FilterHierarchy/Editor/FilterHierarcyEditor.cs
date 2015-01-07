using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

enum FilterOptions
{
	Tag,Layer,Both
}
public class FilterHierarcyEditor : EditorWindow {

	FilterOptions filterOptions = FilterOptions.Tag;
	string selectedTag = "Untagged";
	int layer = 0;
	List<int> objectIndex = new List<int>();
	bool isFilterModeOn = false;

	static Object[] newSelected;
	
	[MenuItem("Custom/Tut2-Filter Hierarcy")]
	static void Init()
	{
		FilterHierarcyEditor filter  = (FilterHierarcyEditor)EditorWindow.GetWindow (typeof (FilterHierarcyEditor));
		filter.Show();
	}

	void OnGUI () {
		/**You can delete this line **/
		EditorGUILayout.Space();
		GUI.color = Color.green;
		if(GUILayout.Button("Go to tutorial page"))
		{
			Application.OpenURL("http://wp.me/p3azza-4C");
		}
		GUI.color = Color.white;
		/** ------------------ **/

		EditorGUILayout.PrefixLabel("Filtering Options");
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
		else 
		{
			selectedTag = EditorGUILayout.TagField("Select Tag",selectedTag);
			layer = EditorGUILayout.LayerField("Select Layer",layer);
			if(GUILayout.Button("Filter by All"))
			{
				filterSelected(filterOptions);
			}
		}
		// if filter mode show clear button
		if(isFilterModeOn)
		{
			GUI.color = Color.red;
			if(GUILayout.Button("Clear"))
			{
				isFilterModeOn = false;
				Object[] allObjects = selectObjects();
				foreach(Object o in allObjects)
				{
					if(o.hideFlags != HideFlags.HideAndDontSave &&
					   o.hideFlags !=HideFlags.DontSave && o.hideFlags !=HideFlags.NotEditable)
						o.hideFlags = o.hideFlags & ~HideFlags.HideInHierarchy;
				}
			}
			GUI.color = Color.white;
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
	// Causes some problems so disabled!
//	Object[] selectAll ()
//	{
//		return Resources.FindObjectsOfTypeAll(typeof(GameObject)) as Object[];
//	}
	Object[] selectObjects ()
	{
		return Object.FindObjectsOfType(typeof(GameObject)) as Object[];
	}

	void filterSelected (FilterOptions ops)
	{
		Object[] selected  = selectObjects();

		objectIndex = new List<int>();
		for(int i = 0; i< selected.Length; i++)
		{
			GameObject g = (GameObject)selected[i] as GameObject;

			if(ops == FilterOptions.Tag && g.tag == selectedTag)
			{
				objectIndex.Add(i);
			}
			else if(ops == FilterOptions.Layer && g.layer == layer)
			{
				objectIndex.Add(i);
			}
			else if(ops == FilterOptions.Both && g.layer == layer && g.tag == selectedTag)
			{
				objectIndex.Add(i);
			}
		}
		newSelected = new Object[objectIndex.Count];
		for(int i = 0; i< objectIndex.Count; i++)
		{
			newSelected[i] = selected[objectIndex[i]];
		}
		Selection.objects = newSelected;
		// set filter mode on option to draw button
		isFilterModeOn = true;
		for(int i = 0; i<selected.Length; i++)
		{
			// change hide flags
			if(!objectIndex.Contains(i))
				selected[i].hideFlags = HideFlags.HideInHierarchy;
		}
	}

}

