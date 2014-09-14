using UnityEngine;
using System.Collections;
using UnityEditor;

public class CreateGameGrid : EditorWindow {
	GameObject gridObject;
	Transform startPos;
	Vector2 cellSize;
	int xNum;
	int yNum;
	bool goRight = true;
	bool goDown = true;

	[MenuItem("Custom/Tut3-MakeGrid")]
	static void Init()
	{
		CreateGameGrid grid  = (CreateGameGrid)EditorWindow.GetWindow (typeof (CreateGameGrid));
		grid.Show();
	}
	void OnGUI () {
		GUILayout.Label ("Grid Settings", EditorStyles.boldLabel);

		gridObject = EditorGUILayout.ObjectField("Prefab",gridObject,typeof(GameObject),true) as GameObject;
		startPos = EditorGUILayout.ObjectField("Start Pos",startPos, typeof(Transform),true)as Transform;
		cellSize = EditorGUILayout.Vector2Field("Cell size",cellSize);
		xNum = EditorGUILayout.IntField("X tiles",xNum);
		yNum = EditorGUILayout.IntField("Y tiles",yNum);
		goRight = EditorGUILayout.Toggle("Go right", goRight);
		goDown = EditorGUILayout.Toggle("Go down", goDown);
		if(GUILayout.Button("Create"))
		{
			createGrid();
		}
	}

	void createGrid ()
	{
		for(int i = 0; i< xNum; i++)
		{
			for(int j= 0; j < yNum; j++)
			{
				float x = goRight ? i * cellSize.x + startPos.localPosition.x : startPos.localPosition.x -i * cellSize.x ;
				float y = goDown ? startPos.localPosition.y - j* cellSize.y : startPos.localPosition.y + j* cellSize.y;
				Vector3 pos = new Vector3(x,  y, 0f);
				Instantiate(gridObject,pos, gridObject.transform.rotation);
			}
		}
	}
}
