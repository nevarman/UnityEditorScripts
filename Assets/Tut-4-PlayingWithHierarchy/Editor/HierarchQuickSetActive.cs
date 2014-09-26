using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
public class HierarchQuickSetActive {

	static HierarchQuickSetActive ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
	}

	static void hierarchWindowOnGUI (int instanceID, Rect selectionRect)
	{
		// make rectangle
		Rect r = new Rect (selectionRect); 
		r.x = r.width - 28;
		r.width = 18;
		// get objects
		Object o = EditorUtility.InstanceIDToObject(instanceID);
		GameObject g = (GameObject)o as GameObject;
		// drag toggle gui
		g.SetActive(GUI.Toggle(r,g.activeSelf,string.Empty));

//		if(g.GetComponent<MeshRenderer>()!=null)
//		{
//			Texture2D mIcon = AssetPreview.GetMiniTypeThumbnail(typeof(MeshRenderer));
//			Rect icon = new Rect (selectionRect); 
//			Rect rend = new Rect (selectionRect); 
//			icon.x = icon.width - 65;
//			icon.width = 18;
//			rend.x = rend.width - 48;
//			rend.width = 18;
//			GUI.Label(icon,mIcon);
//			g.renderer.enabled = GUI.Toggle(rend,g.renderer.enabled,string.Empty);
//			GUI.Label(rend,"---");
//		}


	}
}
