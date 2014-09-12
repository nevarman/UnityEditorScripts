using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[InitializeOnLoad]
public class HierarchyIconManager : EditorWindow {
	// Help string
	string helpString = string.Empty;
	// Gui fields
	GameObject gameObjectSlot;
	Texture2D iconSlot;
	Texture2D iconSlotEdit;
	Color colorSlot;
	Color colorSlotEdit;
	bool addBoxBorder;
	bool addBoxBorderEdit;
	// edit options holder
	int databaseIndex;
	bool showAll;
	bool selectionChanged = false;
	// Database options
	static IconDatabase nData = null;
	static string databasePath = "Assets/Resources/IconDatabase.asset";
	static bool isDatabaseSet = false;

	/// <summary>
	/// Initializes the <see cref="HierarchyIconManager"/> class.
	/// </summary>
	static HierarchyIconManager()
	{
		initDatabase();
		EditorApplication.projectWindowChanged += onProjectWindowChanged;
	}

	[MenuItem("Custom/Hierarch Icon Manager")]
	static void Init()
	{
		HierarchyIconManager iconManager  = (HierarchyIconManager)EditorWindow.GetWindow (typeof (HierarchyIconManager));
		iconManager.Show();
		initDatabase();

	}
	/// <summary>
	/// Inits the database on start to show the right gui
	/// </summary>
	static void initDatabase ()
	{
		if((IconDatabase) AssetDatabase.LoadAssetAtPath(databasePath,typeof(IconDatabase)) != null)
		{
			nData = (IconDatabase) AssetDatabase.LoadAssetAtPath(databasePath,typeof(IconDatabase));
			isDatabaseSet = true;
		}
		else
		{
			isDatabaseSet = false;
		}
	}
	/// <summary>
	/// In case we delete our scriptable object re-init our database on project window changes
	/// </summary>
	static void onProjectWindowChanged ()
	{
		initDatabase();
	}
	/// <summary>
	/// Creates the database of scriptable object
	/// </summary>
	void createDatabase()
	{
		nData = (IconDatabase)ScriptableObject.CreateInstance(typeof(IconDatabase));
		if(!System.IO.Directory.Exists("Assets/Resources"))
		{
			AssetDatabase.CreateFolder("Assets","Resources");
		}
		AssetDatabase.CreateAsset(nData, databasePath);
		AssetDatabase.SaveAssets();
		isDatabaseSet = true;
	}

	void OnGUI()
	{
		if(!isDatabaseSet)
		{
			GUI.color = Color.green;
			if(GUILayout.Button("Create icon database"))
			{
				createDatabase();
			}
			GUI.color = Color.white;
			return;
		}
		GUI.color = Color.red;
		if(GUILayout.Button("Delete Icon Database"))
		{
			AssetDatabase.DeleteAsset(databasePath);
			IconData[] i = FindObjectsOfType<IconData>();
			foreach(IconData s in i)
			{
				DestroyImmediate(s);
			}
		}
		GUI.color = Color.white;
		GUILayout.Space(10f);

		EditorGUILayout.HelpBox(helpString,MessageType.Info);
		// we want to assign icon to selected object from Hierarchy window
		gameObjectSlot = Selection.activeGameObject;
		if(gameObjectSlot == null)
		{
			helpString = "Select a gameobject from Hierarch view to add icon or edit";
		}
		else 
		{
			// check database
			int id = gameObjectSlot.GetComponent<IconData>()!=null ? gameObjectSlot.GetComponent<IconData>().id : -1;
			databaseIndex = getObjectDataBaseIndex(id);
			// show edit ops
			if(databaseIndex != -1)
			{
				helpString = "Edit icon options.";

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();

				colorSlotEdit = EditorGUILayout.ColorField("Update Color",colorSlotEdit);
				addBoxBorderEdit = EditorGUILayout.Toggle("Box Border",addBoxBorderEdit);

				EditorGUILayout.EndVertical();
				iconSlotEdit = (Texture2D)EditorGUILayout.ObjectField("Update Icon Texture",iconSlotEdit,typeof(Texture2D),false);
				if(selectionChanged)
				{
					colorSlotEdit = nData.hierarchyIcon[databaseIndex].color;
					addBoxBorderEdit = nData.hierarchyIcon[databaseIndex].addBox;
					iconSlotEdit = nData.hierarchyIcon[databaseIndex].icon;
					selectionChanged = false;
				}
				EditorGUILayout.BeginVertical();
				if(GUILayout.Button("Update icon"))
				{
					updateIconObject(databaseIndex,iconSlotEdit,colorSlotEdit,addBoxBorderEdit);
				}
				GUI.color = Color.red;
				if(GUILayout.Button("Remove"))
				{
					removeIconObject(databaseIndex);
				}
				GUI.color = Color.white;
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
			else 
			{
				helpString = "Add a texture and place color.";
				// show add ops
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
				colorSlot = EditorGUILayout.ColorField("Color",colorSlot);
				addBoxBorder = EditorGUILayout.Toggle("Add Box Border",addBoxBorder);
				EditorGUILayout.EndVertical();
				iconSlot =(Texture2D)EditorGUILayout.ObjectField("Icon Texture",iconSlot,typeof(Texture2D),false);
				EditorGUILayout.BeginVertical();
				
				if(GUILayout.Button("Add icon"))
				{
					if(gameObjectSlot != null )
					{
						addIconObject(gameObjectSlot,iconSlot,colorSlot,addBoxBorder);
					}
					else 
					{
						EditorApplication.Beep();
					}
				}
				if(GUILayout.Button("Clear"))
				{
					gameObjectSlot =null;
					iconSlot = null;
					colorSlot = new Color(.8f,.8f,.8f,0f);
				}
				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();
			}
		}
	}
	void OnInspectorUpdate() {
		Repaint();
	}
	void OnSelectionChange() {
		selectionChanged = true;
	}

	void addIconObject (GameObject gameObjectSlot, Texture2D iconSlot,Color color,bool addBox)
	{
		if(nData != null)
		{
			Undo.RecordObject(nData,"database");
			Undo.RegisterCompleteObjectUndo(nData,"databaseC");
			// Must call before making changes on scriptableObject
			EditorUtility.SetDirty(nData);
			// Add dummy icon data script!!!
			gameObjectSlot.AddComponent<IconData>().id = gameObjectSlot.GetInstanceID();
			HierarchyIcon h = new HierarchyIcon(gameObjectSlot.GetInstanceID(),color,iconSlot,addBox);
			if(isObjectInsideDatabase(h.objectId))
			{
				gameObjectSlot = null;
				EditorApplication.Beep();
				return;
			}
			nData.hierarchyIcon.Add(h);	
			// repaint our hierarcy view to see the icon immediatly
			EditorApplication.RepaintHierarchyWindow();
		}
		else 
		{
			isDatabaseSet = false;
		}
	}

	void updateIconObject (int databaseIndex, Texture2D iconSlotEdit, Color colorSlotEdit,bool addBox)
	{
		EditorUtility.SetDirty(nData);
		if(iconSlotEdit == null)
		{
			int i = EditorUtility.DisplayDialogComplex("Empty icon slot","Do you want to remove your icon?","Yes","No","Cancel");
			if(i == 0)
			{
				nData.hierarchyIcon[databaseIndex].icon = iconSlotEdit;
			}
			else if(i == 2)
			{
				return;
			}
		}
		else 
		{
			nData.hierarchyIcon[databaseIndex].icon = iconSlotEdit;
		}
		nData.hierarchyIcon[databaseIndex].color = colorSlotEdit;
		nData.hierarchyIcon[databaseIndex].addBox = addBox;
		EditorApplication.RepaintHierarchyWindow();
	}

	void removeIconObject (int databaseIndex)
	{
		EditorUtility.SetDirty(nData);
		nData.hierarchyIcon.RemoveAt(databaseIndex);
		DestroyImmediate(gameObjectSlot.GetComponent<IconData>());
		EditorApplication.RepaintHierarchyWindow();
	}

	bool isObjectInsideDatabase(int id)
	{
		return getObjectDataBaseIndex(id) == -1 ? false  : true;
	}
	int getObjectDataBaseIndex(int id)
	{
		for(int i = 0; i< nData.hierarchyIcon.Count; i++)
		{
			if(id == nData.hierarchyIcon[i].objectId)
			{
				return i;
			}
		}
		return -1;
	}
}
