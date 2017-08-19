using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NowSelectedPanel 
{
	PianoInputPanel,
	PianoRollPanel,
	ADSRPanel
}

public enum PianoRollToolType
{
	Pencil,
	Erase,
	Move
}

public enum ClickableType
{
	ClickOnly,
	ClickAndDrag
}

public enum ClickableObjectType
{
	Note,
	ADSRLine,
	PianoButton
}

public class EnumPool : MonoBehaviour {
}
