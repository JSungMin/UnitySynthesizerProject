using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcase : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetComponent<LineRenderer>().positionCount = 100;
        for (int i = 0; i < 100; i++)
        {
            GetComponent<LineRenderer>().SetPosition(i, GetComponent<BeizerSpline>().GetPoint(0.01f * i));

        }
    }
    int boold = 0;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && boold == 0)
        {
            boold = 1;
            GetComponent<BeizerSpline>().AddCurve();
            Vector3 td=Camera.main.ScreenToWorldPoint(Input.mousePosition);
            td.z = 0;
            GetComponent<BeizerSpline>().points[GetComponent<BeizerSpline>().points.Count - 2].position = td;
            Vector3[] tm = new Vector3[GetComponent<BeizerSpline>().points.Count];
            for (int i = 0; i < 100; i++)
            {
                GetComponent<LineRenderer>().SetPosition(i, GetComponent<BeizerSpline>().GetPoint(0.01f * i));
            }
        }
        else if (!Input.GetMouseButton(0)&&boold != 0) boold = 0;

    }
}
