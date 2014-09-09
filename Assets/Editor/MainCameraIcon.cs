using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
class MainCameraIcon
{
	static Texture2D texture;
	static int[] id;

	static MainCameraIcon ()
	{
		// Init
		texture = AssetDatabase.LoadAssetAtPath ("Assets/Resources/cam.png", typeof(Texture2D)) as Texture2D;
		EditorApplication.update += onUpdate;
		EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
	}
	
	static void onUpdate ()
	{

		Camera[] cam = Object.FindObjectsOfType(typeof(Camera)) as Camera[];
		if(cam != null)
		{
			id = new int[cam.Length];
			for(int i = 0; i<cam.Length; i++)
			{
				id[i] = cam[i].gameObject.GetInstanceID();
			}
		}

	}

	static void hierarchWindowOnGUI (int instanceID, Rect selectionRect)
	{
		// place the icon to the right
		Rect r = new Rect (selectionRect); 
		r.x = r.width - 10;
		r.width = 18;
		if(id.Length >0 && id != null )
		{
			foreach(int i in id)
			{
				if (i == instanceID) 
				{
					// Draw the texture if it's a cam (e.g.)
					GUI.Label (r,texture); 
					Object o = EditorUtility.InstanceIDToObject(instanceID);
					// create a style with same padding as normal labels. 
					GUIStyle style = new GUIStyle(((GUIStyle)"Hi Label")); 
					style.padding.left = EditorStyles.label.padding.left;
					
					// choose new color 
					style.normal.textColor = Color.red;
					
					// draw the new colored label over the old one 
					GUI.Label(selectionRect, o.name, style); 
				}
			}
		}

	}
}