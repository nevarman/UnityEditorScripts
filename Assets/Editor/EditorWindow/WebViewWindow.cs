using UnityEngine;
using System.Collections;
using UnityEditor;

public class WebViewWindow : EditorWindow {
	static WebView w;
	static WebViewWindow wv;

	[MenuItem("Custom/Web view")]
	static void Init()
	{
		wv  = (WebViewWindow)EditorWindow.GetWindow (typeof (WebViewWindow));
		wv.Show();
		w =(WebView)ScriptableObject.CreateInstance(typeof(WebView)) as WebView;
		w.InitWebView((int)wv.position.width,(int)wv.position.height,true);
			//new WebView((int)wv.position.width,(int)wv.position.height,true);
			//(WebView)ScriptableObject.CreateInstance(typeof(WebView)) as WebView;
		wv.wantsMouseMove = true;
		w.LoadURL("http://unity3d.com/unity");
	}

	void OnGUI()
	{
		if(w != null )
		{
			w.DoGUI(new Rect(0f,0f,wv.position.width,wv.position.height));
			w.Focus();
			Repaint();
		}
	}
	void OnInspectorUpdate() {
		if(w != null )
			w.Focus();
	}

}
