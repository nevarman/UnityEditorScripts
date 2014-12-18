using UnityEngine;
using System.Collections;
using System.Reflection;
public class MethodChooserExampleBase : MonoBehaviour {

	public MethodInfo[] methods ;
	public FieldInfo[] variables;
	[HideInInspector]
	public int methodToCall,paramToCall;
}
