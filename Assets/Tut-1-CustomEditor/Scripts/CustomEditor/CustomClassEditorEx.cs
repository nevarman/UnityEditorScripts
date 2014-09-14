using UnityEngine;
using System.Collections;

public class CustomClassEditorEx : MonoBehaviour {
	public CustomClass customClass;
	public CustomClass[] customClassList;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

[System.Serializable]
public class CustomClass
{
	public string name;
	public Color color;
	public int state;
}
