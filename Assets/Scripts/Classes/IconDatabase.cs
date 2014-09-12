using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IconDatabase : ScriptableObject
{
	public List<HierarchyIcon> hierarchyIcon= new List<HierarchyIcon>();
	void OnEnable() {
		if(hierarchyIcon.Count ==0)
		{
			hierarchyIcon= new List<HierarchyIcon>();
		}
	}
}

[System.Serializable]
public class HierarchyIcon
{
	public int objectId;
	public Color color;
	public Texture2D icon;
	public bool addBox;
	public HierarchyIcon(int id,Color c, Texture2D t)
	{
		objectId = id;
		color = c;
		icon = t;
	}
	public HierarchyIcon(int id,Color c, Texture2D t,bool add)
	{
		objectId = id;
		color = c;
		icon = t;
		addBox = add;
	}

}