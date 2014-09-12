using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
class HierarchyIconEditor {
	static IconDatabase nData = null;
	static string databasePath = "Assets/Resources/IconDatabase.asset";
	static bool isDatabaseSet = false;

	static HierarchyIconEditor ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
		EditorApplication.projectWindowChanged += onProjectWindowChanged;
		initDatabase();
	}
	static void initDatabase ()
	{
		if((IconDatabase) AssetDatabase.LoadAssetAtPath(databasePath,typeof(IconDatabase)) != null)
		{
			nData = (IconDatabase) AssetDatabase.LoadAssetAtPath(databasePath,typeof(IconDatabase));
			isDatabaseSet = true;
		}
		else
		{
			isDatabaseSet = false;
		}
	}

	static void hierarchWindowOnGUI (int instanceID, Rect selectionRect)
	{
		if(isDatabaseSet)
		{
			// place the icon to the right
			Rect r = new Rect (selectionRect); 
			r.x = r.width - 10;
			r.width = 18;
			Object o = EditorUtility.InstanceIDToObject(instanceID);
			GameObject g = (GameObject)o as GameObject;
			if(g != null && g.GetComponent<IconData>())
			{
				for(int i = 0; i <nData.hierarchyIcon.Count; i++)
				{
					if(g.GetComponent<IconData>().id == nData.hierarchyIcon[i].objectId)
					{
						GUI.Label (r,nData.hierarchyIcon[i].icon); 
						// create a style with same padding as normal labels. 
						GUIStyle style = new GUIStyle(((GUIStyle)"Hi Label")); 
						style.padding.left = EditorStyles.label.padding.left;
						// choose new color 
						style.normal.textColor = nData.hierarchyIcon[i].color;
						// draw the new colored label over the old one 
						GUI.Label(selectionRect, o.name, style); 
						if(nData.hierarchyIcon[i].addBox)
						{
							// draw box
							Rect bRect = new Rect(selectionRect);
							bRect.x = r.width - 10;
							bRect.height = bRect.height -1;
							GUI.Box(bRect,string.Empty);
						}
					}
				}
			}
		}
	}
	/// <summary>
	/// In case we delete our scriptable object re-init our database on project window changes
	/// </summary>
	static void onProjectWindowChanged ()
	{
		initDatabase();
	}
}
