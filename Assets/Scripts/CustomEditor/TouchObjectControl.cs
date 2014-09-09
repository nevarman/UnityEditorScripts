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
	
	/** transform component */
	private Transform _transform;
	/** scale transform component for 2D*/
	private Transform _scaleTransfrom2D;
	/** is clicked **/
	private bool dragFlagOn = false;
	/** touch id */
	private int touchId = -1;
	/** is clicked **/
	private bool rotateFlagOn = false;
	//** rotate counter */
	private float _rotateCounter;
	/** screen position holder */
	private Vector3 screenPos ;
	/** Screen position first holder */
	private Vector3 _firstScreenPos;
	/** Check if we disabled drag */
	private bool _isDragDisabled = false;
	/** if we are hitting wall */
	private bool _isWallHit = false;
	/** Click anywhere to drag the object */
	private Vector3 _clickDistance;
	float _startTouchDistanceForScale = .1f;
	Vector2 _firstFingerPosForScale = Vector2.zero ;
	SpriteRenderer _spRenderer;
	/// Events 
	/** On drag start callback */
	public delegate void OnDragStart(string objectName, Transform transform);
	public static event OnDragStart onDragStarted;
	/** On drag ended callback */
	public delegate void OnDragFinish(string objectName, Transform transform);
	public static event OnDragFinish onDragFinished;

	public bool IsDragging {
		get {
			return dragFlagOn;
		}
	}
	
	#region UNITY_FUNCS


	/// <summary>
	/// /*Added actions for game state changes*/
	/// </summary>
	public virtual void OnEnable()
	{

	}
	
	public virtual void OnDisable()
	{

	}
	void Awake()
	{
		if(worldMode == World.World2D)
		{
			if(GetComponent<Rigidbody2D>() == null ) gameObject.AddComponent<Rigidbody2D>();
			rigidbody2D.gravityScale = 0f;
			if(GetComponent<BoxCollider2D>() == null) gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
			if(safeClick) GetComponent<BoxCollider2D>().size *= safeClickMultiplier;
			_spRenderer = GetComponent<SpriteRenderer>();
		}
		else 
		{
			if(GetComponent<Rigidbody>() == null ) gameObject.AddComponent<Rigidbody>();
			rigidbody.useGravity = false;
			if(GetComponent<BoxCollider>() == null) gameObject.AddComponent<BoxCollider>().isTrigger = true;
			if(safeClick) GetComponent<BoxCollider>().size *= safeClickMultiplier;
		}
	}
	void Start () 
	{
		_transform = this.transform;
		if(worldMode == World.World2D)
		{
			if(touchAction.ScaleOnDrag)
			{
				try 
				{
					_scaleTransfrom2D = _transform.GetChild (0);
				} 
				catch (System.Exception ex) 
				{
					Debug.LogWarning("no child to scale add one to scale action");
				}
			}
			if(touchAction.scale)
			{
				Debug.LogError("Do not scale collider2D may cause unwanted action on Collision, will disable scale");
				touchAction.scale = false;
			}
		}
	}
	void Update () 
	{
		if(touchAction.drag)
			dragObject();
		if(touchAction.rotate)
			rotateObject();
		if(touchAction.scale)
			scaleObject();
	}
	#endregion

	void pauseGame ()
	{
		if(touchAction.drag) disableDrag();
		if(touchAction.scale) disableScale();
		if(touchAction.rotate) disableRotate();
	}

	void resumeGame ()
	{
		if(touchAction.drag) enableDrag();
		if(touchAction.scale) enableScale();
		if(touchAction.rotate) enableRotate();
	}

	void onLevelFailed ()
	{
		if(touchAction.drag) disableDrag();
		if(touchAction.scale) disableScale();
		if(touchAction.rotate) disableRotate();
	}
	
	#region PUBLIC_METHODS
	/** Disables drag after wanted action */
	public void disableDrag()
	{
		dragFlagOn = false; 
		_isDragDisabled = true;
		if(worldMode == World.World3D)
		{
			if(_transform != null & touchAction.ScaleOnDrag && !touchAction.scale)
				iTween.ScaleTo(_transform.gameObject, Vector3.one, .5f);
		}
		else 
		{
			if(_scaleTransfrom2D != null & touchAction.ScaleOnDrag && !touchAction.scale)
				iTween.ScaleTo(_scaleTransfrom2D.gameObject, Vector3.one, .5f);
		}
		
	}
	/** Enables drag */
	public void enableDrag()
	{
		//		dragFlagOn = true; 
		_isDragDisabled = false;
	}
	public void disableScale()
	{
		touchAction.drag = false;
	}
	public void enableScale()
	{
		touchAction.drag = true;
	}
	public void disableRotate()
	{
		touchAction.rotate = false;
	}
	public void enableRotate()
	{
		touchAction.rotate = true;
	}
	#endregion
	/** Drag method */
	void dragObject()
	{	
		if(touchAction.useWallHit && Input.GetButton("Fire1"))
		{
			if(worldMode == World.World2D)
			{
				RaycastHit2D hit ;
				switch(touchAction.dragAxis)
				{
				case TouchAction.DragAxis.Both:
					RaycastHit2D hit2;
					Vector2 rayDirb = Input.mousePosition.x > _firstScreenPos.x
						? Vector2.right *(_spRenderer.sprite.bounds.size.x/2f+touchAction.wallHitTreshold) 
							: -Vector2.right*( _spRenderer.sprite.bounds.size.x/2f+touchAction.wallHitTreshold);
					hit = Physics2D.Linecast(_transform.position,new Vector2(_transform.position.x,_transform.position.y) + rayDirb,touchAction.layer.value);
					Vector2 rayDi2 = Input.mousePosition.y > _firstScreenPos.y
						? Vector2.up *(_spRenderer.sprite.bounds.size.y/2f+touchAction.wallHitTreshold) 
							: -Vector2.up*( _spRenderer.sprite.bounds.size.y/2f+touchAction.wallHitTreshold);
					hit2 = Physics2D.Linecast(_transform.position,new Vector2(_transform.position.x,_transform.position.y) + rayDi2,touchAction.layer.value);

					_isWallHit = hit.collider != null || hit2.collider != null ? true : false;
					break;
				case TouchAction.DragAxis.x:
					Vector2 rayDir = Input.mousePosition.x > _firstScreenPos.x
						? Vector2.right *(_spRenderer.sprite.bounds.size.x/2f+touchAction.wallHitTreshold) 
							: -Vector2.right*( _spRenderer.sprite.bounds.size.x/2f+touchAction.wallHitTreshold);
					hit = Physics2D.Linecast(_transform.position,new Vector2(_transform.position.x,_transform.position.y) + rayDir,touchAction.layer.value);

					_isWallHit = hit.collider != null ? true : false;
					break;
				case TouchAction.DragAxis.y:
					Vector2 rayDi = Input.mousePosition.y > _firstScreenPos.y
						? Vector2.up *(_spRenderer.sprite.bounds.size.y/2f+touchAction.wallHitTreshold) 
							: -Vector2.up*( _spRenderer.sprite.bounds.size.y/2f+touchAction.wallHitTreshold);
					hit = Physics2D.Linecast(_transform.position,new Vector2(_transform.position.x,_transform.position.y) + rayDi,touchAction.layer.value);
					_isWallHit = hit.collider != null ? true : false;
					break;
				default:break;
				}
			}
			else
			{
				Debug.LogWarning("Wall hit not implemented yet for 3d world");
			}
		}
		#if UNITY_EDITOR
		if(dragFlagOn)
		{
			Vector3 mousePos = Input.mousePosition;
			Vector3 pos ;
			if(worldMode == World.World2D)
			{
				screenPos = Camera.main.ScreenToWorldPoint(mousePos);
				pos = new Vector3(screenPos.x,screenPos.y,transform.localPosition.z);
			}
			else
			{
				mousePos = new Vector3(mousePos.x, mousePos.y, 8f);
				var ray = Camera.main.ScreenToWorldPoint (mousePos);
				pos = new Vector3(ray.x,ray.y,transform.localPosition.z);
			}
			// Apply drag 
			if((touchAction.useWallHit && !_isWallHit) || !touchAction.useWallHit)
			{
				switch(touchAction.dragAxis)
				{
				case TouchAction.DragAxis.Both:
					if(!touchAction.smoothDrag)
					{
						_transform.localPosition = pos + new Vector3(_clickDistance.x, _clickDistance.y, _transform.localPosition.z);
						
					}
					else
					{
						_transform.localPosition = Vector3.Lerp(_transform.localPosition,
						                                        pos + new Vector3(_clickDistance.x, _clickDistance.y,0f)
						                                        ,Time.deltaTime*touchAction.smoothFactor);
					}
					break;
				case TouchAction.DragAxis.x:
					if(!touchAction.smoothDrag)
					{
						_transform.setXLocal(pos.x + _clickDistance.x);
					}
					else
					{
						_transform.localPosition = Vector3.Lerp(_transform.localPosition
						                                        ,new Vector3(pos.x+_clickDistance.x, _transform.localPosition.y,_transform.localPosition.z)
						                                        ,Time.deltaTime*touchAction.smoothFactor);
					}
					break;
				case TouchAction.DragAxis.y:
					if(!touchAction.smoothDrag)
					{
						_transform.setYLocal(pos.y + _clickDistance.y);
					}
					else
					{
						_transform.localPosition = Vector3.Lerp(_transform.localPosition
						                                        ,new Vector3(_transform.localPosition.x,pos.y + _clickDistance.y,_transform.localPosition.z)
						                                        ,Time.deltaTime*touchAction.smoothFactor);
					}
					break;
				}
			}

		}
		if(Input.GetButtonDown("Fire1"))
		{
			_firstScreenPos = Input.mousePosition;
			Vector3 _mousePos = _firstScreenPos;
			Vector3 _screenPos;
			if(worldMode == World.World2D)
			{
				_screenPos = Camera.main.ScreenToWorldPoint(_mousePos);
			}
			else
			{
				Vector3 _mousePos2 = new Vector3(_mousePos.x, _mousePos.y, 8f);
				_screenPos = Camera.main.ScreenToWorldPoint(_mousePos2);
			}
			//Vector3 _screenPos = Camera.main.ScreenToWorldPoint(_mousePos);
			//Debug.Log("M "+ _mousePos + " s " + _screenPos);
			if(worldMode == World.World2D)
			{

				RaycastHit2D hit = Physics2D.Raycast(_screenPos, Vector2.zero);
				if(hit.collider != null && hit.collider.transform.Equals(_transform) && !_isDragDisabled)
				{
					// move from where we click
					_clickDistance = _transform.localPosition - _screenPos;
					dragFlagOn = true;
					if(_scaleTransfrom2D != null & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_scaleTransfrom2D.gameObject, touchAction.onDragScale, .5f);
					// fire event
					if(onDragStarted != null) onDragStarted(this.name,_transform);
				}
			}
			else
			{
				Ray ray = Camera.main.ScreenPointToRay (_mousePos);
				RaycastHit hit;
				if (!Physics.Raycast (ray,out hit, 10000))
					return;
				if(hit.collider != null && hit.collider.transform.Equals(_transform) && !_isDragDisabled)
				{
					// move from where we click
					_clickDistance = _transform.localPosition - _screenPos ;
					dragFlagOn = true;
					//scale
					if(_transform != null & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_transform.gameObject, touchAction.onDragScale, .5f);
					// fire event
					if(onDragStarted != null) onDragStarted(this.name,_transform);
				}
			}
		}		
		else if(Input.GetButtonUp("Fire1"))
		{
			if(worldMode == World.World3D)
			{
				if(_transform != null && dragFlagOn & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_transform.gameObject, Vector3.one, .5f);
			}
			else 
			{
				if(_scaleTransfrom2D != null && dragFlagOn & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_scaleTransfrom2D.gameObject, Vector3.one, .5f);
			}
			
			if(dragFlagOn )
			{
				if(onDragFinished != null ) onDragFinished(this.name,_transform);
			}
			dragFlagOn = false;
		}
		#else
		
		for(int i = 0; i< Input.touchCount; i++)
		{
			Touch t = Input.GetTouch(i);
//			Debug.Log(string.Format("Name: {0} id: {1} ",gameObject.name, t.fingerId));
			if(dragFlagOn && t.fingerId == touchId)
			{
				Vector3 mousePos = new Vector3(t.position.x, t.position.y,0f);
				Vector3 pos ;
				if(worldMode == World.World2D)
				{
					screenPos = Camera.main.ScreenToWorldPoint(mousePos);
					pos = new Vector3(screenPos.x,screenPos.y,_transform.localPosition.z);
				}
				else
				{
					mousePos = new Vector3(mousePos.x, mousePos.y, 8f);
					var ray = Camera.main.ScreenToWorldPoint (mousePos);
					pos = new Vector3(ray.x,ray.y,_transform.localPosition.z);
				}
				// Apply drag 
				if(touchAction.useWallHit && !_isWallHit || !touchAction.useWallHit)
				{
					switch(touchAction.dragAxis)
					{
					case TouchAction.DragAxis.Both:
						if(!touchAction.smoothDrag)
						{
							_transform.localPosition = pos + new Vector3(_clickDistance.x, _clickDistance.y,_transform.localPosition.z);
						}
						else
						{
							_transform.localPosition = Vector3.Lerp(_transform.localPosition,
							                                        pos + new Vector3(_clickDistance.x, _clickDistance.y,0f)
							                                        ,Time.deltaTime*touchAction.smoothFactor);
						}
						break;
					case TouchAction.DragAxis.x:
						if(!touchAction.smoothDrag)
						{
							_transform.setXLocal(pos.x + _clickDistance.x);
						}
						else
						{
							_transform.localPosition = Vector3.Lerp(_transform.localPosition
							                                        ,new Vector3(pos.x+_clickDistance.x, _transform.localPosition.y,_transform.localPosition.z)
							                                        ,Time.deltaTime*touchAction.smoothFactor);
						}
						break;
					case TouchAction.DragAxis.y:
						if(!touchAction.smoothDrag)
						{
							_transform.setYLocal(pos.y + _clickDistance.y);
						}
						else
						{
							_transform.localPosition = Vector3.Lerp(_transform.localPosition
							                                        ,new Vector3(_transform.localPosition.x,pos.y + _clickDistance.y,_transform.localPosition.z)
							                                        ,Time.deltaTime*touchAction.smoothFactor);
						}
						break;
					}
				}
			}
			if(t.phase == TouchPhase.Began)
			{
				_firstScreenPos = new Vector3(t.position.x, t.position.y,0f);

				Vector3 _mousePos = _firstScreenPos;
				Vector3 _screenPos;
				if(worldMode == World.World2D)
				{
					_screenPos = Camera.main.ScreenToWorldPoint(_mousePos);
				}
				else
				{
					Vector3 _mousePos2 = new Vector3(_mousePos.x, _mousePos.y, 8f);
					_screenPos = Camera.main.ScreenToWorldPoint(_mousePos2);
				}
				//Vector3 _screenPos = Camera.main.ScreenToWorldPoint(_mousePos);
				//Debug.Log("M "+ _mousePos + " s " + _screenPos);
				if(worldMode == World.World2D)
				{
					RaycastHit2D hit = Physics2D.Raycast(_screenPos, Vector2.zero);
					if(hit.collider != null && hit.collider.transform.Equals(_transform) && !_isDragDisabled)
					{
						// move from where we click
						_clickDistance = _transform.localPosition - _screenPos;
						dragFlagOn = true;
						if(_scaleTransfrom2D != null & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_scaleTransfrom2D.gameObject, touchAction.onDragScale, .5f);
						// fire event
						if(onDragStarted != null) onDragStarted(this.name,_transform);
						touchId = t.fingerId;
					}
				}
				else
				{
					Ray ray = Camera.main.ScreenPointToRay (_mousePos);
					RaycastHit hit;
					if (!Physics.Raycast (ray,out hit, 10000))
						return;
					if(hit.collider != null && hit.collider.transform.Equals(_transform) && !_isDragDisabled)
					{
						// move from where we click
						_clickDistance = _transform.localPosition - _screenPos ;
						dragFlagOn = true;
						//scale
						if(_transform != null & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_transform.gameObject, touchAction.onDragScale, .5f);
						// fire event
						if(onDragStarted != null) onDragStarted(this.name,_transform);
						touchId = t.fingerId;
					}
				}
			}
			else if(t.phase == TouchPhase.Ended)
			{
				if(worldMode == World.World3D)
				{
					if(_transform != null && dragFlagOn & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_transform.gameObject, Vector3.one, .5f);
				}
				else 
				{
					if(_scaleTransfrom2D != null && dragFlagOn & touchAction.ScaleOnDrag && !touchAction.scale) iTween.ScaleTo(_scaleTransfrom2D.gameObject, Vector3.one, .5f);
				}
				
				if(dragFlagOn )
				{
					if(onDragFinished != null ) onDragFinished(this.name,_transform);
				}
				dragFlagOn = false;
				touchId = -1;
			}
		}
		#endif
	}
	
	void rotateObject ()
	{
		// TODO rotate world3d not finished
		if(worldMode == World.World3D)
		{
			if(!dragFlagOn && !touchAction.rotateOnTouch)
			{
				// if we are not dragging
				if(Input.GetButtonDown("Fire1"))
				{
					_firstScreenPos = Input.mousePosition;
				}
				else if(Input.GetButton("Fire1"))
				{
					//count rotate
					if(touchAction.rotateTouchEndTime> 0f)
					{
						_rotateCounter += Time.deltaTime;
						if(_rotateCounter >= touchAction.rotateTouchEndTime)
						{
							rotateFlagOn = false;
							return;
						}
					}
					Vector3 pos = Input.mousePosition;
					float x = pos.y -_firstScreenPos.y;
					float y = _firstScreenPos.x - pos.x;
					if(touchAction.rotateAxis == TouchAction.RotateAxis.Both && 
					   (Mathf.Abs(x) > touchAction.rotateTreshold || Mathf.Abs(y) > touchAction.rotateTreshold))
					{
						_transform.Rotate(new Vector3(x ,y ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
					else if(touchAction.rotateAxis == TouchAction.RotateAxis.x && Mathf.Abs(x) > touchAction.rotateTreshold)
					{
						_transform.Rotate(new Vector3(x ,0f ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
					else if(touchAction.rotateAxis == TouchAction.RotateAxis.y && Mathf.Abs(y) > touchAction.rotateTreshold)
					{
						_transform.Rotate(new Vector3(0f ,y ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
				}
			}
			else if(!touchAction.drag && touchAction.rotateOnTouch)
			{
				// if we are not dragging
				if(Input.GetButtonDown("Fire1"))
				{
					_firstScreenPos = Input.mousePosition;
					Vector3 _mousePos2 = new Vector3(_firstScreenPos.x, _firstScreenPos.y, 8f);
					Vector3 _screenPos = Camera.main.ScreenToWorldPoint(_mousePos2);

					Ray ray = Camera.main.ScreenPointToRay (_firstScreenPos);
					RaycastHit hit;
					if (!Physics.Raycast (ray,out hit, 10000))
						return;
					if(hit.collider != null && hit.collider.transform.Equals(_transform))
						rotateFlagOn = true;
				}
				else if(Input.GetButton("Fire1"))
				{
					//count rotate
					if(touchAction.rotateTouchEndTime> 0f)
					{
						_rotateCounter += Time.deltaTime;
						if(_rotateCounter >= touchAction.rotateTouchEndTime)
						{
							rotateFlagOn = false;
							return;
						}
					}
					Vector3 pos = Input.mousePosition;
					float x = pos.y -_firstScreenPos.y;
					float y = _firstScreenPos.x - pos.x;
					if(touchAction.rotateAxis == TouchAction.RotateAxis.Both && 
					   (Mathf.Abs(x) > touchAction.rotateTreshold || Mathf.Abs(y) > touchAction.rotateTreshold) && rotateFlagOn)
					{
						_transform.Rotate(new Vector3(x ,y ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
					else if(touchAction.rotateAxis == TouchAction.RotateAxis.x && Mathf.Abs(x) > touchAction.rotateTreshold)
					{
						_transform.Rotate(new Vector3(x ,0f ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
					else if(touchAction.rotateAxis == TouchAction.RotateAxis.y && Mathf.Abs(y) > touchAction.rotateTreshold)
					{
						_transform.Rotate(new Vector3(0f ,y ,0f) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
				}
				else if(Input.GetButtonUp("Fire1"))
				{
					rotateFlagOn = false;
					_rotateCounter = 0f;
				}
			}
		}
		else
		{
			if(!touchAction.drag && touchAction.rotateOnTouch)
			{
				// if we are not dragging
				if(Input.GetButtonDown("Fire1"))
				{
					_firstScreenPos = Input.mousePosition;
					Vector3 _screenPos = Camera.main.ScreenToWorldPoint(_firstScreenPos);
					RaycastHit2D hit = Physics2D.Raycast(_screenPos, Vector2.zero);
					if(hit.collider != null && hit.collider.transform.Equals(_transform))
						rotateFlagOn = true;
				}		
				else if(Input.GetButton("Fire1"))
				{
					Vector3 pos = Input.mousePosition;
					float x = pos.x -_firstScreenPos.x;
					float y = _firstScreenPos.y - pos.y;
					if((Mathf.Abs(x) > touchAction.rotateTreshold || Mathf.Abs(y) > touchAction.rotateTreshold) && rotateFlagOn)
					{
						//count rotate
						if(touchAction.rotateTouchEndTime> 0f)
						{
							_rotateCounter += Time.deltaTime;
							if(_rotateCounter >= touchAction.rotateTouchEndTime)
							{
								rotateFlagOn = false;
								return;
							}
						}
						Vector3 _screenPos =  Camera.main.ScreenToWorldPoint(_firstScreenPos);
						//calculate where we touch
						if(_screenPos.x < _transform.position.x) y = -y;
						if(_screenPos.y > _transform.position.y) x = -x;
						//calculate spin
						float spin = Mathf.Abs(x) > Mathf.Abs(y) ? x : -y;
						if(touchAction.rotateAxis == TouchAction.RotateAxis.x && spin > 0f)// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
						else if(touchAction.rotateAxis == TouchAction.RotateAxis.y && spin < 0f)// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
						else if(touchAction.rotateAxis == TouchAction.RotateAxis.Both )// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
				}
				else if(Input.GetButtonUp("Fire1"))
				{
					rotateFlagOn = false;
					_rotateCounter = 0f;
				}
			}
			else if(!dragFlagOn && !touchAction.rotateOnTouch)
			{
				// if we are not dragging
				if(Input.GetButtonDown("Fire1"))
				{
					_firstScreenPos = Input.mousePosition;
				}
				else if(Input.GetButton("Fire1"))
				{
					if(touchAction.rotateTouchEndTime> 0f)
					{
						_rotateCounter += Time.deltaTime;
						if(_rotateCounter >= touchAction.rotateTouchEndTime)
						{
							rotateFlagOn = false;
							return;
						}
					}
					Vector3 pos = Input.mousePosition;
					float x = pos.y -_firstScreenPos.y;
					float y = _firstScreenPos.x - pos.x;
					if(Mathf.Abs(x) > touchAction.rotateTreshold || Mathf.Abs(y) > touchAction.rotateTreshold)
					{
						Vector3 _screenPos = Camera.main.ScreenToWorldPoint(_firstScreenPos);
						//calculate where we touch
						if(_screenPos.x < _transform.position.x) y = -y;
						if(_screenPos.y > _transform.position.y) x = -x;
						//calculate spin
						float spin = Mathf.Abs(x) > Mathf.Abs(y) ? x : -y;
						if(touchAction.rotateAxis == TouchAction.RotateAxis.x && spin > 0f)// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
						else if(touchAction.rotateAxis == TouchAction.RotateAxis.y && spin < 0f)// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
						else if(touchAction.rotateAxis == TouchAction.RotateAxis.Both )// right
							_transform.Rotate(new Vector3(0f ,0f , spin) * touchAction.rotateSpeed * Time.deltaTime,Space.World);
					}
				}
				else if(Input.GetButtonUp("Fire1"))
				{
					_rotateCounter = 0f;
				}
			}
		}

	}
	
	void scaleObject ()
	{
#if !UNITY_EDITOR
		if(Input.touchCount >= 1)
		{
			for(int i = 0; i< Input.touchCount; i++)
			{
				Touch firtsFinger = Input.GetTouch(0);
				if(firtsFinger.phase == TouchPhase.Began) 
				{
					_firstFingerPosForScale = firtsFinger.position;

				}
				if(i >= 1)
				{
					Touch secondFinger = Input.GetTouch(1);
					
					if(secondFinger.phase == TouchPhase.Began) _startTouchDistanceForScale = Vector2.Distance(_firstFingerPosForScale, secondFinger.position);
					
					if(firtsFinger.phase != TouchPhase.Ended && secondFinger.phase != TouchPhase.Ended && (firtsFinger.phase == TouchPhase.Moved || secondFinger.phase == TouchPhase.Moved))
					{
						float _dis = Vector2.Distance(firtsFinger.position, secondFinger.position);
						//						Debug.Log("Distance " + _dis + " start distance "+ _startTouchDistanceForScale);
						
						if(Mathf.Abs( _startTouchDistanceForScale - _dis) > touchAction.scaleTreshold)
						{
//							_transform.setScale(_dis * touchAction.scaleMultiplier * _transform.localScale / _startTouchDistanceForScale, touchAction.scaleBetween);
							float val = (_dis - _startTouchDistanceForScale)/100f;
							Vector3 scale;
							if(touchAction.scaleAxis == TouchAction.ScaleAxis.Both)
							{
								scale = new Vector3(val,val,val) * touchAction.scaleMultiplier;
							}
							else if(touchAction.scaleAxis == TouchAction.ScaleAxis.x)
							{
								scale = new Vector3(val,0f,0f) * touchAction.scaleMultiplier;
							}
							else if(touchAction.scaleAxis == TouchAction.ScaleAxis.z)
							{
								scale = new Vector3(0f,0f,val) * touchAction.scaleMultiplier;
							}
							else 
							{
								scale = new Vector3(0f,val,0f) * touchAction.scaleMultiplier;
							}
							_transform.addScale(scale,touchAction.scaleBetween);
						}						
					}
				}
			}
		}

#else
		if(Input.GetButtonDown("Fire1"))
		{
			_firstScreenPos = Input.mousePosition;
		}
		else if(Input.GetButton("Fire1"))
		{
			float _dis = Vector3.Distance(_firstScreenPos, Input.mousePosition);
//									Debug.Log("Distance " + _dis);
			
			if(Mathf.Abs( _dis) > touchAction.scaleTreshold)
			{
				float x = (Input.mousePosition.x - _firstScreenPos.x)/100f;
				float y = (Input.mousePosition.y - _firstScreenPos.y)/100f;
				float val = Mathf.Abs(x) > Mathf.Abs(y) ? x : y;
				Vector3 scale;
				if(touchAction.scaleAxis == TouchAction.ScaleAxis.Both)
				{
					scale = new Vector3(val,val,val) * touchAction.scaleMultiplier;
				}
				else if(touchAction.scaleAxis == TouchAction.ScaleAxis.x)
				{
					scale = new Vector3(x,0f,0f) * touchAction.scaleMultiplier;
				}
				else if(touchAction.scaleAxis == TouchAction.ScaleAxis.z)
				{
					scale = new Vector3(0f,0f,val) * touchAction.scaleMultiplier;
				}
				else 
				{
					scale = new Vector3(0f,y,0f) * touchAction.scaleMultiplier;
				}
				_transform.addScale(scale,touchAction.scaleBetween);
			}						
		}
		else if(Input.GetButtonUp("Fire1"))
		{
			rotateFlagOn = false;
			_rotateCounter = 0f;
		}
#endif
	}
//	void OnGUI()
//	{
//		touchAction.scaleMultiplier = GUI.HorizontalSlider(new Rect(300,200,500,50),touchAction.scaleMultiplier,0f,1f);
//		GUI.Label(new Rect(300,300,200,50),"scale "+touchAction.scaleMultiplier);
//	}
	
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
