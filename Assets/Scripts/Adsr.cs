using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adsr : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    public Transform start;
    public Transform Attack;
    public BeizerSpline atb;
    public Transform Decay;
    public BeizerSpline dcb;
    public Transform Sustain;
    public BeizerSpline rlb;
    public Transform release;

    // Update is called once per frame
    void Update () {
		
	}
}
