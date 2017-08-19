using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoButtonInput : MonoBehaviour {
	public PlayerTouchInput touchInput;
	public CreateNodeTable nodeTable;

	public int buttonIndex;

	void Start ()
	{
		touchInput = GameObject.FindObjectOfType <PlayerTouchInput> ();
		nodeTable = GameObject.FindObjectOfType <CreateNodeTable> ();
	}

	public void CreateNode ()
	{
		var width = 0.64f;
		var height = 0.52f;
		if (nodeTable.resolution == CreateNodeTable.timeResolution.Eighth) {
			width = 0.32f;
		} 


		var createPos = new Vector3 (0f,(touchInput.nowPianoInputOctave + buttonIndex) * height, 0f);

		var newObj = UIPianoNote.CreateUIPianoNote (createPos);
		newObj.GetComponent<UIPianoNote> ().NoteOn ();
		touchInput.ChangeClickObject (newObj);
	}
}
