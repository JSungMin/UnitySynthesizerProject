using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAbleObject : MonoBehaviour {
	public ClickableType clickableType;
	public ClickableObjectType objectType;

	public delegate void ClickAction (ClickAbleObject nowObj);
	public delegate void DragAction (ClickAbleObject nowObj, Vector3 targetPos);

	public ClickAction clickAction;
	public DragAction dragAction;

	public void DragObjectToPoint (Vector3 point)
	{
		transform.position = point;
	}
}
