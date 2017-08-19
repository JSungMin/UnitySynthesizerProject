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
    public List<MidiEvent>[] events;
    public int[] tmpEventIndex = new int[32];

    private const float totalTempo = 60000000;
    private int currentTrackIndex;
    private int currentNoteCount;
    public int totalpulses;

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
        if (isSorted)
        {
            isSorted = false;

            List<MidiEvent> mevents = new List<MidiEvent>();
            foreach (var un in EditableMidiData.instance.uiNoteList)
            {
                mevents.Add(un.noteOnEvent);
                mevents.Add(un.noteOffEvent);
            }

            SortDataByStartTime(events[0], mevents);
        }

        if (isPlaying)
        {

            debugTimer += Time.deltaTime;

            if (debugTimer > 1f)
            {
                Debug.Log("Current Ticks : " + currentTimer);
                Debug.Log(events.Length);
                debugTimer = 0f;
            }

            currentTrackIndex = 0;

            for (int i = 0; i < events.Length && currentTimer <= totalpulses; i++)
            {
                List<MidiEvent> mevent = events[i];

                currEventCount = tmpEventIndex[i];
                while (currEventCount < mevent.Count && currentTimer >= mevent[currEventCount].StartTime)
                {
                    if (mevent[currEventCount].EventFlag == MidiFile.EventNoteOn && mevent[currEventCount].Velocity > 0 &&
                        currentTimer >= mevent[currEventCount].StartTime)
                    {
                        currNoteNumber = mevent[currEventCount].Notenumber;
                        sList[currNoteNumber - 24].GetComponent<AudioSource>().Play();
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

    public void SortDataByStartTime(List<MidiEvent> source, List<MidiEvent> events)
    {
        foreach (MidiEvent mevent in events)
        {
            for (int i = source.Count - 1; i >= 0; i--)
            {
                if (i >= 0 && mevent.StartTime >= source[i].StartTime)
                {
                    if (i == source.Count - 1)
                    {
                        source.Add(mevent);
                    }
                    else
                    {
                        source.Insert(i + 1, mevent);
                    }
                }
            }
        }
    }
}