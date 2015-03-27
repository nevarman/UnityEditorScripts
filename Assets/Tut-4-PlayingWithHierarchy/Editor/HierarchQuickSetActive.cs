using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
public class HierarchQuickSetActive {
	private static bool isEnabled = false;
	/// <summary>
	/// Initializer <see cref="HierarchQuickSetActive"/> class.
	/// </summary>
	static HierarchQuickSetActive ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
		isEnabled = EditorPrefs.GetBool("quick_setactive",true);
	}
	[MenuItem("Tools/QuickSetActive/Toggle")]
	static void ToggleEnable(){
		isEnabled = !isEnabled;
	}
	[MenuItem("Tools/QuickSetActive/Go to tutorial")]
	static void OpenTutorial(){
		Application.OpenURL("http://nevzatarman.com/2014/12/19/unity-editor-scripting-quick-setactive/");
	}
	/// <summary>
	/// Editor delegate callback
	/// </summary>
	/// <param name="instanceID">Instance id.</param>
	/// <param name="selectionRect">Selection rect.</param>
	static void hierarchWindowOnGUI (int instanceID, Rect selectionRect)
	{
		if(!isEnabled )return;
		// make rectangle
		Rect r = new Rect (selectionRect); 
		r.x = r.width - 10;
		r.width = 18;
		// get objects
		Object o = EditorUtility.InstanceIDToObject(instanceID);
		GameObject g = (GameObject)o as GameObject;
		// drag toggle gui
		g.SetActive(GUI.Toggle(r,g.activeSelf,string.Empty));

	}
}
