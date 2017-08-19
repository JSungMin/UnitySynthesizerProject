using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour {
    public int key;

	// Use this for initialization
	void Start () {
		
	}
	public void key_on()
    {
        transform.parent.GetComponent<Piano>().key_on(key);
    }
    public void key_off()
    {
        transform.parent.GetComponent<Piano>().key_off(key);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
