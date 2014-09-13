using UnityEngine;
using System.Collections;

public class TouchObjectControl : MonoBehaviour {
	
	public TouchAction touchAction;
	
	public enum World
	{
		World2D, World3D
	}
	public World worldMode = World.World3D;
	/** want to make collider bigger */
	public bool safeClick = true;
	public float safeClickMultiplier = 1.2f;
}
[System.Serializable]
public class TouchAction
{
	/** Drag Axis */
	public enum DragAxis
	{
		Both, x, y
	}
	/** Drag Axis */
	public enum RotateAxis
	{
		Both, x, y
	}
	/** Scale Axis */
	public enum ScaleAxis
	{
		Both, x, y, z
	}
	[Header("Drag options")]
	public bool drag = true;
	public bool smoothDrag = true;
	public float smoothFactor = 10f;
	public DragAxis dragAxis = DragAxis.Both;
	/** want scale anim on drag*/
	public Vector3 onDragScale = new Vector3(1f,1f,1f);
	[Space(10f)]
	public bool useWallHit = false;
	public float wallHitTreshold = .1f;
	public LayerMask layer;

	[Header("Rotate options")]
	public bool rotate = false;
	public float rotateSpeed = 1f;
	public float rotateTreshold = 10f;
	public bool rotateOnTouch = false;
	public float rotateTouchEndTime = 0f;
	public RotateAxis rotateAxis;
	
	[Header("Scale options")]
	public bool scale = false;
	public ScaleAxis scaleAxis = ScaleAxis.Both;
	public Vector2 scaleBetween = new Vector2(.5f,2f);
	public float scaleTreshold = 10f;
	public float scaleMultiplier = .5f;
	public bool touchOnScale = false;
	
	bool dragScale;
	public bool ScaleOnDrag {
		get {
			return (onDragScale.x > 1f || onDragScale.y > 1f || onDragScale.z > 1f) ? true : false;
		}
	}
}
