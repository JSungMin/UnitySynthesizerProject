using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPianoNote : ClickAbleObject {
	public MidiEvent noteOnEvent;
	public MidiEvent noteOffEvent;

	public int tick;//행*ticksperquaternote
	public int key;//열
	public int duration;//기간

	// Use this for initialization
	void Start () {
		
		objectType = ClickableObjectType.Note;
		clickableType = ClickableType.ClickAndDrag;

		dragAction += ScaleUpUINote;
	}

	public int GetNoteStartTime ()
	{
		return tick;
	}

	public int GetNoteEndTime ()
	{
		return tick + duration;
	}

	public byte GetNoteNumber ()
	{
		
		return (byte)key;
	}

	public void NoteOn ()
	{
		noteOnEvent = new MidiEvent ();
		noteOnEvent.EventFlag = EventNoteOn;
		noteOnEvent.Channel = EditableMidiData.instance.channel;
		noteOnEvent.StartTime = GetNoteStartTime ();
		noteOnEvent.Notenumber = GetNoteNumber ();
		noteOnEvent.Velocity = 127;
		GameObject.FindObjectOfType<MidiPlayer> ().sList [noteOnEvent.Notenumber].GetComponent<AudioSource> ().Play ();
	}

	public void NoteOff ()
	{
		noteOffEvent = new MidiEvent ();
		noteOffEvent.EventFlag = EventNoteOff;
		noteOffEvent.StartTime = GetNoteEndTime ();
		noteOffEvent.Notenumber = GetNoteNumber ();
		GameObject.FindObjectOfType<MidiPlayer> ().sList [noteOnEvent.Notenumber].GetComponent<AudioSource> ().Stop ();
	}

	public static ClickAbleObject CreateUIPianoNote (Vector3 touchPos)
	{
		int diviX;
		int diviY;

		var table = GameObject.FindObjectOfType <CreateNodeTable> ();

		var multipler = 1;

		if (table.resolution == CreateNodeTable.timeResolution.Eighth) {
			multipler = 2;
		}
		
		diviX = (int)(touchPos.x / (0.64f / multipler));
		diviY = (int)(touchPos.y / 0.54f);
		var firstX = diviX * (0.64f / multipler);
		var firstY = diviY * 0.54f;

		var newObj = Instantiate (PlayerTouchInput.instance.uiKeyNoteObj, Vector3.zero, Quaternion.identity);
		newObj.transform.parent = PlayerTouchInput.instance.uiNotePool;
		newObj.transform.position = new Vector3 (firstX, firstY, 0);
		newObj.GetComponent<UIPianoNote> ().tick = diviX * EditableMidiData.instance.defaultQuarterPerTicks/multipler;
		newObj.GetComponent<UIPianoNote> ().key = diviY;
		Debug.Log (newObj);
	
		EditableMidiData.instance.uiNoteList.Add (newObj.GetComponent<UIPianoNote> ());
		return newObj.GetComponent<ClickAbleObject> ();
	}
	public static ClickAbleObject CreateUIPianoNote (int ticks,int key)
	{
		int diviX;
		int diviY;

		var table = GameObject.FindObjectOfType <CreateNodeTable> ();

		var multipler = 1;
		Debug.Log ("!");
		if (table.resolution == CreateNodeTable.timeResolution.Eighth) {
			multipler = 2;
		}

		diviX = ticks/EditableMidiData.instance.defaultQuarterPerTicks;
		var firstX = diviX * (0.64f / multipler);
		var firstY = key * 0.54f;

		var newObj = Instantiate (PlayerTouchInput.instance.uiKeyNoteObj, Vector3.zero, Quaternion.identity);
		newObj.transform.parent = PlayerTouchInput.instance.uiNotePool;
		newObj.transform.position = new Vector3 (firstX, firstY, 0);
		newObj.GetComponent<SpriteRenderer>().size = new Vector2 (0.64f / multipler, 0.52f);
		newObj.GetComponent<BoxCollider> ().size = new Vector3 (0.64f / multipler, 0.52f, 0.1f);
		newObj.GetComponent<BoxCollider> ().center = new Vector3 (0.32f / multipler, 0.26f / multipler, 0.2f);
		newObj.GetComponent<UIPianoNote> ().tick = ticks;
		newObj.GetComponent<UIPianoNote> ().key = key;
		Debug.Log (newObj);

		EditableMidiData.instance.uiNoteList.Add (newObj.GetComponent<UIPianoNote> ());
		return newObj.GetComponent<ClickAbleObject> ();
	}

	public void ScaleUpUINote (ClickAbleObject obj, Vector3 pos)
	{
		int diviX;
		int diviY;

		Vector3 originPosition = obj.transform.position;

		var table = GameObject.FindObjectOfType <CreateNodeTable> ();

		var multipler = 1;

		Debug.Log (table.resolution);

		if (table.resolution == CreateNodeTable.timeResolution.Eighth) {
			multipler = 2;
			Debug.Log ("Eight : " + multipler);
		}

		diviX = (int)(Mathf.Max (pos.x - originPosition.x, 0) / (0.64f / multipler)) + 1;
		duration = diviX * EditableMidiData.instance.defaultQuarterPerTicks;
		obj.GetComponent<SpriteRenderer>().size = new Vector2 (diviX * (0.64f / multipler), 0.52f);
		obj.GetComponent<BoxCollider> ().size = obj.GetComponent<SpriteRenderer> ().size;
		var center = obj.GetComponent<BoxCollider> ().center;
		obj.GetComponent<BoxCollider> ().center = new Vector3 (obj.GetComponent<BoxCollider> ().size.x * 0.5f, center.y, center.z);
	}

	public void RemoveUiNote (ClickAbleObject obj)
	{

		var midiP = GameObject.FindObjectOfType <MidiPlayer> ();
		midiP.StopPlayer ();
		var midiF = GameObject.FindObjectOfType <MidiFile> ();
		if (null == obj.GetComponent<UIPianoNote> ())
			return;

		for (int i = 0; i< midiP.events.Length; i++)
		{
			
			midiP.events [i].Remove (obj.GetComponent<UIPianoNote> ().noteOnEvent);
			midiP.events [i].Remove (obj.GetComponent<UIPianoNote> ().noteOffEvent);
		}
		for (int i = 0; i < midiF.events.Length; i++)
		{
			midiF.events [i].Remove (obj.GetComponent<UIPianoNote> ().noteOnEvent);
			midiF.events [i].Remove (obj.GetComponent<UIPianoNote> ().noteOffEvent);
		}

		EditableMidiData.instance.uiNoteList.Remove (obj.GetComponent<UIPianoNote>());

		DestroyObject (obj.gameObject);
	}

	public void MoveUiNote (ClickAbleObject obj, Vector3 pos)
	{
		int diviX;
		int diviY;

		var table = GameObject.FindObjectOfType <CreateNodeTable> ();

		var multipler = 1;

		if (table.resolution == CreateNodeTable.timeResolution.Eighth)
			multipler = 2;

		diviX = (int)(pos.x / (0.64f / multipler));
		diviY = (int)(pos.y / 0.54f);
		var firstX = diviX * (0.64f / multipler);
		var firstY = diviY * 0.54f;

		obj.transform.position = new Vector3 (firstX, firstY, 0);
		var midiPlayer = GameObject.FindObjectOfType <MidiPlayer> ();
		var onIndex = midiPlayer.events [0].IndexOf (obj.GetComponent<UIPianoNote> ().noteOnEvent);
		var offIndex = midiPlayer.events [0].IndexOf (obj.GetComponent<UIPianoNote> ().noteOffEvent);

		var midiFile = GameObject.FindObjectOfType <MidiFile> ();

		obj.GetComponent<UIPianoNote> ().NoteOn ();
		obj.GetComponent<UIPianoNote> ().NoteOff ();

		var index = EditableMidiData.instance.uiNoteList.IndexOf (obj.GetComponent<UIPianoNote> ());
		midiPlayer.events [0] [onIndex] = obj.GetComponent<UIPianoNote> ().noteOnEvent;
		midiPlayer.events [0] [offIndex] = obj.GetComponent<UIPianoNote> ().noteOffEvent;

		midiFile.events [0] [onIndex] = obj.GetComponent<UIPianoNote> ().noteOnEvent;
		midiFile.events [0] [offIndex] = obj.GetComponent<UIPianoNote> ().noteOffEvent;

		EditableMidiData.instance.uiNoteList [index] = obj.GetComponent<UIPianoNote>();
	}

	/* The list of Midi Events */
	public const int EventNoteOff = 0x80;
	public const int EventNoteOn = 0x90;
	public const int EventKeyPressure = 0xA0;
	public const int EventControlChange = 0xB0;
	public const int EventProgramChange = 0xC0;
	public const int EventChannelPressure = 0xD0;
	public const int EventPitchBend = 0xE0;
	public const int SysexEvent1 = 0xF0;
	public const int SysexEvent2 = 0xF7;
	public const int MetaEvent = 0xFF;

	/* The list of Meta Events */
	public const int MetaEventSequence = 0x0;
	public const int MetaEventText = 0x1;
	public const int MetaEventCopyright = 0x2;
	public const int MetaEventSequenceName = 0x3;
	public const int MetaEventInstrument = 0x4;
	public const int MetaEventLyric = 0x5;
	public const int MetaEventMarker = 0x6;
	public const int MetaEventEndOfTrack = 0x2F;
	public const int MetaEventTempo = 0x51;
	public const int MetaEventSMPTEOffset = 0x54;
	public const int MetaEventTimeSignature = 0x58;
	public const int MetaEventKeySignature = 0x59;
}
