using UnityEngine;
using System.Collections;
using System.Reflection;

public class MethodChooserExample : MethodChooserExampleBase {

	public string strParameter;
	public int intParameter;

	void Start () {
		callAPublicMethod();
	}

	public void someMethod()
	{
		Debug.Log("someMethod called");
	}
	public void someOtherMethod(string value)
	{
		Debug.Log("someOtherMethod called with param "+value);
	}
	public void someOtherMethodWithIntParam(int value)
	{
		Debug.Log("someOtherMethod called with int param "+value);
	}
	void callAPublicMethod()
	{
		if(methods[methodToCall].GetParameters().Length<1)
			methods[methodToCall].Invoke(this,null);
		else
		{
			object p = variables[paramToCall].GetValue(this);
			Debug.Log("param "+p);
			methods[methodToCall].Invoke(this,new object[]{p });
		}
	}
}
