using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
    int cas;
    Transform selects;
    void ray_on(Transform sel)
    {
        selects = sel;
        if (sel == GetComponent<Adsr>().Attack) cas = 0;
        if (sel == GetComponent<Adsr>().Decay) cas = 1;
        if (sel == GetComponent<Adsr>().Sustain) cas = 2;
        if (GetComponent<Adsr>().atb.points.Contains(sel)) cas = 3;
        if (GetComponent<Adsr>().dcb.points.Contains(sel)) cas = 4;
        if (GetComponent<Adsr>().rlb.points.Contains(sel)) cas = 5;
    }
    void ray_out()
    {
        selects = null;

    }
    void move(Vector3 position)
    {
        if (cas == 0)
        {
            if (position.x > GetComponent<Adsr>().start.position.x) position.x = GetComponent<Adsr>().start.position.x;
            if (position.x < GetComponent<Adsr>().Decay.position.x) position.x = GetComponent<Adsr>().Decay.position.x;
        }
        else if (cas == 1)
        {
            if (position.x > GetComponent<Adsr>().Attack.position.x) position.x = GetComponent<Adsr>().Attack.position.x;
            if (position.x < GetComponent<Adsr>().Sustain.position.x) position.x = GetComponent<Adsr>().Sustain.position.x;
        }
        else if (cas == 2)
        {
            if (position.x > GetComponent<Adsr>().Decay.position.x) position.x = GetComponent<Adsr>().Decay.position.x;
            if (position.x < GetComponent<Adsr>().release.position.x) position.x = GetComponent<Adsr>().release.position.x;
        }
        else if (cas == 3)
        {
            if (position.x > GetComponent<Adsr>().start.position.x) position.x = GetComponent<Adsr>().start.position.x;
            if (position.x < GetComponent<Adsr>().release.position.x) position.x = GetComponent<Adsr>().release.position.x;
        }
        else if (cas == 4)
        {
            if (position.x > GetComponent<Adsr>().start.position.x) position.x = GetComponent<Adsr>().start.position.x;
            if (position.x < GetComponent<Adsr>().release.position.x) position.x = GetComponent<Adsr>().release.position.x;
        }
        else if (cas == 5)
        {
            if (position.x > GetComponent<Adsr>().start.position.x) position.x = GetComponent<Adsr>().start.position.x;
            if (position.x < GetComponent<Adsr>().release.position.x) position.x = GetComponent<Adsr>().release.position.x;
        }

        position.z = selects.position.z;
        selects.position = position;
        if (cas == 1)
        {
            position.x = GetComponent<Adsr>().Sustain.position.x;
            GetComponent<Adsr>().Sustain.position = position;
        }
        else if (cas == 2)
        {
            position.x = GetComponent<Adsr>().Sustain.position.x;
            GetComponent<Adsr>().Sustain.position = position;
        }
    }
    // Update is called once per frame
    void Update () {
        if (selects == null) ;
        else
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ray_out();
            }
            else
            {
                move(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position));
            }
        }

    }
}
