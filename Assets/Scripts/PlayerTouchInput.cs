using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchInput : MonoBehaviour {
	
	public static PlayerTouchInput instance;

	public Transform uiNotePool;
	public NowSelectedPanel nowPanel;

	public GameObject uiKeyNoteObj;

	public ClickAbleObject nowClickedObj;
	public ClickAbleObject prevClickedObj;

	void Start ()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update () {
		GetTouchInput ();
	}

	public void ChangeClickObject (ClickAbleObject clickedObj)
	{
		if (clickedObj == nowClickedObj)
		{
			return;
		}
		prevClickedObj = nowClickedObj;
		nowClickedObj = clickedObj;
	}

	RaycastHit hit;

	void GetTouchInput ()
	{
		for (int i = 0; i < Input.touchCount; i++) {
			var touch = Input.GetTouch (i);
			if (nowPanel == NowSelectedPanel.PianoInputPanel)
				InputInPianoInputPanel (touch);
			if (nowPanel == NowSelectedPanel.PianoRollPanel)
				InputInPianoRollPanel (touch);
			if (nowPanel == NowSelectedPanel.ADSRPanel)
				InputInADSRPanel (touch);
		}
	}

	public void InputInPianoInputPanel (Touch touch)
	{
		var touchPosition = touch.position;
		switch (touch.phase) {
		case TouchPhase.Began:
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
				var obj = hit.collider.GetComponent<ClickAbleObject> ();

			}
			break;
		case TouchPhase.Moved:
			if (null != nowClickedObj && nowClickedObj.clickableType == ClickableType.ClickAndDrag) {
				nowClickedObj.dragAction.Invoke (nowClickedObj, touchPosition);
			} 
			break;
		case TouchPhase.Stationary:
			break;
		case TouchPhase.Ended:
			ChangeClickObject (null);
			break;
		}
	}

	public PianoRollToolType rollToolType = PianoRollToolType.Pencil;

	public void InputInPianoRollPanel (Touch touch)
	{
		Debug.Log ("Enter IN Roll");
		var touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
		switch (touch.phase) {
		case TouchPhase.Began:
			Debug.Log ("Began IN Roll");
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
				Debug.Log ("Casted");
				var obj = hit.collider.GetComponent<ClickAbleObject> ();
				if (null != obj) {
					ChangeClickObject (obj);
				} 
			}
			else {
				if (rollToolType == PianoRollToolType.Pencil) {
					var newObj = UIPianoNote.CreateUIPianoNote (touchPosition);
					newObj.GetComponent<UIPianoNote> ().NoteOn ();
					ChangeClickObject (newObj);
				}
			}
			break;
		case TouchPhase.Moved:
			Debug.Log ("Moved");
			if (rollToolType == PianoRollToolType.Pencil) {
				if (null != nowClickedObj && nowClickedObj.objectType == ClickableObjectType.Note) {
					nowClickedObj.GetComponent<UIPianoNote>().ScaleUpUINote (nowClickedObj, touchPosition);
				} 
			}
			else if (rollToolType == PianoRollToolType.Erase)
			{
				if (null == nowClickedObj) {
					if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
						Debug.Log ("Casted");
						var obj = hit.collider.GetComponent<ClickAbleObject> ();
						if (null != obj && obj.objectType == ClickableObjectType.Note) {
							obj.GetComponent<UIPianoNote> ().RemoveUiNote (obj);
						} 
					}
				} else {
					if (null != nowClickedObj && nowClickedObj.objectType == ClickableObjectType.Note) {
						nowClickedObj.GetComponent<UIPianoNote> ().RemoveUiNote (nowClickedObj);
					} 
				}
			}
			else if (rollToolType == PianoRollToolType.Move)
			{
				if (null == nowClickedObj) {
					if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
						Debug.Log ("Casted");
						var obj = hit.collider.GetComponent<ClickAbleObject> ();
						if (null != obj && obj.objectType == ClickableObjectType.Note) {
							obj.GetComponent<UIPianoNote> ().MoveUiNote (obj, touchPosition);
							obj.GetComponent<UIPianoNote> ().NoteOn ();
						} 
					}
				} else {
					if (null != nowClickedObj && nowClickedObj.objectType == ClickableObjectType.Note) {
						nowClickedObj.GetComponent<UIPianoNote> ().MoveUiNote (nowClickedObj, touchPosition);
						nowClickedObj.GetComponent<UIPianoNote> ().NoteOn ();
					} 
				}
			}
			break;
		case TouchPhase.Stationary:
			break;
		case TouchPhase.Canceled:
			if (null != nowClickedObj && nowClickedObj.objectType == ClickableObjectType.Note)
			{
				nowClickedObj.GetComponent<UIPianoNote> ().NoteOff ();
			}
			break;
		case TouchPhase.Ended:
			if (null != nowClickedObj && nowClickedObj.objectType == ClickableObjectType.Note)
			{
				nowClickedObj.GetComponent<UIPianoNote> ().NoteOff ();
			}
			ChangeClickObject (null);
			break;
		}
	}

	public void InputInADSRPanel (Touch touch)
	{
		var touchPosition = touch.position;
		switch (touch.phase) {
		case TouchPhase.Began:
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
				var obj = hit.collider.GetComponent<ClickAbleObject> ();
			}
			break;
		case TouchPhase.Moved:
			if (null != nowClickedObj && nowClickedObj.clickableType == ClickableType.ClickAndDrag) {
				nowClickedObj.dragAction.Invoke (nowClickedObj, touchPosition);
			} 
			break;
		case TouchPhase.Stationary:
			break;
		case TouchPhase.Ended:
			ChangeClickObject (null);
			break;
		}
	}
}
