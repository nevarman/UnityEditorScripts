using UnityEngine;
using System.Collections;

public static class TransfomExtensions  {
	#region POSITION
	/** Sets the transforms x position */
	public static void setX(this Transform transform, float x)
	{
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
	}
	/** Sets the transforms y position */
	public static void setY(this Transform transform, float y)
	{
		transform.position = new Vector3(transform.position.x, y, transform.position.z);
	}
	/** Sets the transforms z position */
	public static void setZ(this Transform transform, float z)
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, z);
	}
	/** Sets the transforms local x position */
	public static void setXLocal(this Transform transform, float x)
	{
		transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
	}
	/** Sets the transforms local y position */
	public static void setYLocal(this Transform transform, float y)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
	}
	/** Sets the transforms local z position */
	public static void setZLocal(this Transform transform, float z)
	{
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
	}
	public static void setTransfrom(this Transform transfrom, Vector3 tr)
	{
		transfrom.position = new Vector3(tr.x, tr.y, tr.z);
	}
	public static void setPosition(this Transform transform, float x, float y, float z)
	{
		transform.position = new Vector3(x, y, z);
	}
	/** Adds to transform x position */
	public static void addX(this Transform transform, float x)
	{
		float xAdd = transform.position.x +x;
		Vector3 position = new Vector3(xAdd, transform.position.y, transform.position.z);
		transform.position = position;
	}
	/** Adds to transform y position */
	public static void addY(this Transform transform, float y)
	{
		float yAdd = transform.position.y + y;
		Vector3 position = new Vector3(transform.position.x, yAdd, transform.position.z);
		transform.position = position;
	}
	/** Adds to transform z position */
	public static void addZ(this Transform transform, float z)
	{
		float zAdd = transform.position.z +z;
		Vector3 position = new Vector3(transform.position.x, transform.position.y, zAdd );
		transform.position = position;
	}
	/** Adds to transform y position local  */
	public static void addXLocal(this Transform transform, float x)
	{
		float xAdd = transform.localPosition.x + x;
		Vector3 position = new Vector3(xAdd, transform.localPosition.y, transform.localPosition.z);
		transform.localPosition = position;
	}
	/** Adds to transform y position local  */
	public static void addYLocal(this Transform transform, float y)
	{
		float yAdd = transform.localPosition.y + y;
		Vector3 position = new Vector3(transform.localPosition.x, yAdd, transform.localPosition.z);
		transform.localPosition = position;
	}
	/** Adds to transform y position local  */
	public static void addZLocal(this Transform transform, float z)
	{
		float zAdd = transform.localPosition.z + z;
		Vector3 position = new Vector3(transform.localPosition.x, transform.localPosition.y, zAdd);
		transform.localPosition = position;
	}
	/** Adds to transform x position with constraints*/
	public static void addX(this Transform transform, float x , Vector2 constraints)
	{
		float xAdd = Mathf.Clamp( transform.position.x +x,constraints.x, constraints.y);
		Vector3 position = new Vector3(xAdd, transform.position.y, transform.position.z);
		transform.position = position;
	}
	/** Adds to transform y position with constraints*/
	public static void addY(this Transform transform, float y , Vector2 constraints)
	{
		float yAdd = Mathf.Clamp( transform.position.y + y,constraints.x, constraints.y);
		Vector3 position = new Vector3(transform.position.x, yAdd, transform.position.z);
		transform.position = position;
	}
	#endregion
	#region ROTATION
	/** Adds Vector to transforms rotation  */
	public static void rotateAdd(this Transform transform, Vector3 rotation)
	{
		Vector3 thisRot = transform.rotation.eulerAngles;
		float rotX = thisRot.x + rotation.x;
		float rotY = thisRot.y + rotation.y;
		float rotZ = thisRot.z + rotation.z;
		transform.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
	}
	#endregion
	#region SCALE
	public static void setXScale(this Transform transform, float scaleX)
	{
		transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
	}
	public static void setYScale(this Transform transform, float scaleY)
	{
		transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
	}
	public static void setZScale(this Transform transform, float scaleZ)
	{
		transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleZ );
	}
	public static void setScale(this Transform transform, Vector3 scale)
	{
		transform.localScale = scale;
	}
	public static void setScale(this Transform transform, Vector3 scale, Vector2 constraints)
	{
		float x = Mathf.Clamp(scale.x,constraints.x,constraints.y);
		float y = Mathf.Clamp(scale.y,constraints.x,constraints.y);
		float z = Mathf.Clamp(scale.z,constraints.x,constraints.y);
		transform.localScale = new Vector3(x , y, z);
	}
	public static void addScale(this Transform transform, Vector3 scale)
	{
		Vector3 scaleAdd = new Vector3(transform.localScale.x + scale.x, transform.localScale.y+ scale.y, transform.localScale.z + scale.z);
		transform.localScale = scaleAdd;
	}
	public static void addScale(this Transform transform, Vector3 scale, Vector2 constraints)
	{
		float x = Mathf.Clamp(transform.localScale.x +scale.x,constraints.x,constraints.y);
		float y = Mathf.Clamp(transform.localScale.y +scale.y,constraints.x,constraints.y);
		float z = Mathf.Clamp(transform.localScale.z +scale.z,constraints.x,constraints.y);
//		Vector3 scaleAdd = new Vector3(transform.localScale.x + scale.x, transform.localScale.y+ scale.y, transform.localScale.z + scale.z);
		transform.localScale =  new Vector3(x , y, z);
	}
	#endregion
}
