using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditableMidiData : MonoBehaviour {
	public static EditableMidiData instance;

	public int defaultQuarterPerTicks = 128;
	public byte channel; // 현재 One Channel
	public byte instrument;
	public List <UIPianoNote> uiNoteList = new List<UIPianoNote>();

	// Use this for initialization
	void Awake () {
		instance = this;
	}
}
