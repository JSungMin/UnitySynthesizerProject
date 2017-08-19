using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundForker : MonoBehaviour {
	public List<AudioMixerGroup> mixerList = new List<AudioMixerGroup> ();

	public void SetAudioMixer (AudioSource[] aSources)
	{
		for (int i = 0 ; i < aSources.Length; i++)
		{
			aSources [i].outputAudioMixerGroup = mixerList [i];
		}
	}

}
