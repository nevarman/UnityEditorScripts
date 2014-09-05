using UnityEngine;
using System.Collections;
using UnityEditor;
public class EditorWin : EditorWindow {

	public Rect windowRect = new Rect(20, 20, 120, 50);

	[MenuItem ("Custom/NodeEditor")]
	static void Init()
	{
		EditorWindow window = GetWindow<EditorWin>();
		window.Show();
	}
	void OnGUI()
	{
		BeginWindows();
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "My Window");
		EndWindows();
	}
	void DoMyWindow(int windowID) {
		if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
			Debug.Log("Got a click");
		GUI.DragWindow();
	}
}
