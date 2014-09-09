using UnityEngine;
using System.Collections;

public class DecorativeAttributes : MonoBehaviour {
	[Range(0f,10f)]
	public float floatValue;

	[Range(0,10)]
	public int intValue;

	[Space(10f)]
	public string spaceUpThere;

	[Header("Usefull information Header!")]
	public string dontForget;

	[TextArea()]
	public string textArea;
	
	[Multiline(5)]
	public string multiLineString;

	[HideInInspector]
	public string notShownInEditor;
	
}
