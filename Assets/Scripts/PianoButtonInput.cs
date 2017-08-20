using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoButtonInput : MonoBehaviour {
	public PlayerTouchInput touchInput;
	public CreateNodeTable nodeTable;

	public MidiPlayer midiPlayer;
	private CreateNodeTable table;

	public int buttonIndex;

	void Start ()
	{
		touchInput = GameObject.FindObjectOfType <PlayerTouchInput> ();
		nodeTable = GameObject.FindObjectOfType <CreateNodeTable> ();
		midiPlayer = GameObject.FindObjectOfType <MidiPlayer> ();
		table = GameObject.FindObjectOfType <CreateNodeTable> ();
	}

	public bool isPressed = false;

	public void PressUp ()
	{

	}

	public void CreateNode ()
	{
		var multipler = 1;

		if (table.resolution == CreateNodeTable.timeResolution.Eighth) {
			multipler = 2;
		}

		var currenttics = (int)(midiPlayer.currentTimer / (EditableMidiData.instance.defaultQuarterPerTicks / multipler)+0.5f) * EditableMidiData.instance.defaultQuarterPerTicks;

		var newObj = UIPianoNote.CreateUIPianoNote (currenttics, touchInput.nowPianoInputOctave * 12  + buttonIndex);
		newObj.GetComponent<UIPianoNote> ().NoteOn ();
		touchInput.ChangeClickObject (newObj);
	}
}
