using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piano : MonoBehaviour {
    public int octav;
    // Use this for initialization
    void Start () {
        keyList1.Capacity = 12;
	}
    List<int> keyList1=new List<int>();
    List<int> note;
    public void setOctav(int o)
    {
        octav=(o + 5) * 12;
    }
    public void key_on(int key)
    {
        if (octav + key < 128)
        {
            keyList1[key] = Global.currentTime;
        }
    }
    public void key_off(int key)
    {
        if(octav+key<128)
        Global.maintrack.AddNote( new MidiNote(Global.currentTime, Global.channel, key + octav, Global.currentTime - keyList1[key]));
        //addnote(keyList1[key],Global.currentTime,Global.currentTime-keyList1[key],key+octav);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
