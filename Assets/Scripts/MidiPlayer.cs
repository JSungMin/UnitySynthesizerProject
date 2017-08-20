using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidiPlayer : MonoBehaviour
{
    public AudioClip[] audioClips = new AudioClip[128];
    public List<GameObject> sList = new List<GameObject>();
    public string[] scale = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
    public GameObject parent;
    public GameObject soundObject;

    public bool isPlaying = false;
    public bool isSorted = false;

    public float currentTimer = 0f;
    public int quarternote;
    public int tempo;

    public int currEventCount;

    public float debugTimer = 0f;
    
    public int currNoteNumber;

    public float ticksPerSecond;
    public List<MidiTrack> tracks;
	[SerializeField]
    public List<MidiEvent>[] events;
    public int[] tmpEventIndex = new int[32];

    private const float totalTempo = 60000000;
    private int currentTrackIndex;
    public int totalpulses;

	public void PlayPlayer ()
	{
		SetSort ();
		isPlaying = true;
	}

	public void SetSort() {
		List<MidiEvent> mevents = new List<MidiEvent>();
		Debug.Log (EditableMidiData.instance.uiNoteList.Count);
		foreach (var un in EditableMidiData.instance.uiNoteList)
		{
			mevents.Add(un.noteOnEvent);
			mevents.Add(un.noteOffEvent);
		}

		SortDataByStartTime(events[0], mevents);
	}

	public void StopPlayer ()
	{
		currentTimer = 0;
		currentTrackIndex = 0;
		for (int i = 0; i < 32; i++) {
			tmpEventIndex [i] = 0;
		}
		currEventCount = 0;
		var lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.SetPosition(0,new Vector3(0 / EditableMidiData.instance.defaultQuarterPerTicks * 0.64f,Camera.main.ViewportToWorldPoint (Vector3.zero).y,0f));
		lineRenderer.SetPosition(1,new Vector3(0 / EditableMidiData.instance.defaultQuarterPerTicks * 0.64f,Camera.main.ViewportToWorldPoint (Vector3.up).y,0f));
		isPlaying = false;
	}

    // Use this for initialization
    void Start()
    {
        ticksPerSecond = (totalTempo / tempo * quarternote / 60);

        for (int i = 0; i < 90; i++)
        {
            audioClips[i] = Resources.Load<AudioClip>("Piano/Piano_" + scale[(i + 3) % 12] + (i / 12));

            var newSoundObject = Instantiate(soundObject, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            newSoundObject.GetComponent<AudioSource>().clip = audioClips[i];
            newSoundObject.GetComponent<AudioSource>().volume = 0.3f;
            sList.Add(newSoundObject);
        }

        Debug.Log("Ticks per quarter notes: " + quarternote);
        Debug.Log("Tempo : " + tempo);
        Debug.Log("ticks Per Frame : " + ticksPerSecond);
		GetComponent<LineRenderer> ().sortingOrder = 5;
    }

    private void FixedUpdate()
    {
        if (isPlaying)
            currentTimer += ticksPerSecond * Time.fixedDeltaTime;
        else
            currentTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (isPlaying)
        {

			GetComponent<LineRenderer>().SetPosition(0,new Vector3(currentTimer / EditableMidiData.instance.defaultQuarterPerTicks * 0.64f,Camera.main.ViewportToWorldPoint (Vector3.zero).y,0f));
			GetComponent<LineRenderer>().SetPosition(1,new Vector3(currentTimer / EditableMidiData.instance.defaultQuarterPerTicks * 0.64f,Camera.main.ViewportToWorldPoint (Vector3.up).y,0f));
            debugTimer += Time.deltaTime;

            if (debugTimer > 1f)
			{
                Debug.Log(events.Length);
                debugTimer = 0f;
            }

            currentTrackIndex = 0;

            for (int i = 0; i < events.Length && currentTimer <= totalpulses; i++)
            {
                List<MidiEvent> mevent = events[i];
				Debug.Log ("mevent : " + mevent.Count);
                currEventCount = tmpEventIndex[i];
                while (currEventCount < mevent.Count && currentTimer >= mevent[currEventCount].StartTime)
                {
                    if (mevent[currEventCount].EventFlag == MidiFile.EventNoteOn && mevent[currEventCount].Velocity > 0 &&
                        currentTimer >= mevent[currEventCount].StartTime)
                    {
                        currNoteNumber = mevent[currEventCount].Notenumber;
						sList[currNoteNumber].GetComponent<AudioSource>().Play();
						Debug.Log ("asdfasdf");
                    }
                    else if (mevent[currEventCount].EventFlag == MidiFile.EventNoteOn && mevent[currEventCount].Velocity == 0 && currentTimer >= mevent[currEventCount].StartTime)
                    {
                        //  sList[currNoteNumber - 24].GetComponent<AudioSource>().Stop();
                    }
                    else if (mevent[currEventCount].EventFlag == MidiFile.EventNoteOff && currentTimer >= mevent[currEventCount].StartTime)
                    {
                        //   sList[currNoteNumber - 24].GetComponent<AudioSource>().Stop();
                    }
                    else if (mevent[currEventCount].EventFlag == MidiFile.MetaEvent && mevent[currEventCount].Tempo != 0)
                    {
                        tempo = mevent[currEventCount].Tempo;
                        ticksPerSecond = (totalTempo / tempo * quarternote / 60);
                        Debug.Log("tempo : " + tempo);
                    }
                    currEventCount++;
                }
                tmpEventIndex[i] = currEventCount;
            }
        }
    }

    public void SortDataByStartTime(List<MidiEvent> source, List<MidiEvent> events2)
    {
		foreach (var mevent in events2) {
			source.Add (mevent);
		}

		source.Sort(((x, y) => x.StartTime - y.StartTime));
		Debug.Log ("Source: " + source.Count);
		Debug.Log ("events2: " + events2.Count);
    }
}