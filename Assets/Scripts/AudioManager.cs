using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public AudioClip[] audioClips = new AudioClip[128];
    public List<GameObject> sList = new List<GameObject>();
    public GameObject parent;
    public GameObject soundObject;
    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 60; i++)
        {
            var newSoundObject = Instantiate(soundObject, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            //Instantiate(soundObject, new Vector3(0, 0, 0), Quaternion.identity, parent.transform);
            newSoundObject.GetComponent<AudioSource>().clip = audioClips[i];
            sList.Add(newSoundObject);
        }
    }
	
	// Update is called once per frame
	void Update () {


	}
}
