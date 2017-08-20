using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerTouchInput : MonoBehaviour {
	
	public static PlayerTouchInput instance;

	public Transform uiNotePool;
	public NowSelectedPanel nowPanel;

	public GameObject uiKeyNoteObj;

	public ClickAbleObject nowClickedObj;
	public ClickAbleObject prevClickedObj;

	public int nowPianoInputOctave = 1;
	public Scrollbar octaveBar;
	public Text octaveLabel;

	void Start ()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update () {
		GetTouchInput ();
	}

	public void ChangeOctave ()
	{
		nowPianoInputOctave = (int)(octaveBar.value * 10);
		octaveLabel.text = nowPianoInputOctave.ToString ();
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
			if (Input.touchCount >= 2) {
				var firstTouch = Input.GetTouch (0);
				var secondTouch = Input.GetTouch (1);
				if (firstTouch.phase == TouchPhase.Moved && secondTouch.phase == TouchPhase.Moved) {
					//Move Piano Roll
					var firstDeltaPos = firstTouch.deltaPosition;

					Debug.Log (firstDeltaPos);
				
					if (nowPanel == NowSelectedPanel.PianoRollPanel) {
						if (Mathf.Abs (firstDeltaPos.x) > Mathf.Abs (firstDeltaPos.y)) {
							Camera.main.transform.Translate ((firstDeltaPos.x) * Vector3.right * Time.deltaTime * 0.5f);
						} else {
							Camera.main.transform.Translate ((firstDeltaPos.y) * Vector3.up * Time.deltaTime * 0.5f);
						}
					}
				}
				return;
			} 
			else {
				var touch = Input.GetTouch (i);
				if (nowPanel == NowSelectedPanel.PianoRollPanel)
					InputInPianoRollPanel (touch);
				if (nowPanel == NowSelectedPanel.ADSRPanel)
					InputInADSRPanel (touch);
			}
		}

	}

	public PianoRollToolType rollToolType = PianoRollToolType.Pencil;

	public void SetPianoRollTool (int type)
	{
		rollToolType = (PianoRollToolType)(type);
	}

	public LayerMask noteLayer;
	public void InputInPianoRollPanel (Touch touch)
	{
		Debug.Log ("Enter IN Roll");
		var touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
		switch (touch.phase) {
		case TouchPhase.Began:
			Debug.Log ("Began IN Roll");

			var pointerData = new PointerEventData (EventSystem.current);

			pointerData.position = Input.mousePosition;

			List<RaycastResult> results = new List<RaycastResult> ();
			EventSystem.current.RaycastAll (pointerData, results); 

			if (0 != results.Count)
				return;
			if (rollToolType == PianoRollToolType.Erase)
			{
				return;
			}
			if (Physics.Raycast (touchPosition, Vector3.forward, out hit)) {
				Debug.Log ("Casted");
				var obj = hit.collider.GetComponent<ClickAbleObject> ();
				if (null != obj && obj.clickableType != ClickableType.NoneClickable) {
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
