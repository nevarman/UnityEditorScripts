using UnityEngine;
using System.Collections;
/// <summary>
/// Icon data.Used to store a unique id for database. Actually is a stupid temporary method since instance id of obejcts on scene changes on scene loads,
/// and we can't get LocalIdentifierInFile from UnityEditor.Unsupported(returns 0 )
/// </summary>
public class IconData : MonoBehaviour {
	[HideInInspector]
	public int id;

	void OnDestroy()
	{
		Debug.Log("destroyed");
	}
}
