using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoRuleNote : MonoBehaviour {

    public GameObject note;
    float firstX = -1;
    float firstY = -1;
    GameObject instObj = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            if (firstX == -1 && firstY == -1 && instObj == null)
            {
                int diviX;
                int diviY;

                print("들어와라");

                diviX = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).x / 0.64f);
                diviY = (int)(Camera.main.ScreenToWorldPoint(Input.mousePosition).y / 0.54f);
                firstX = diviX * 0.64f;
                firstY = diviY * 0.54f;

                instObj = Instantiate(note) as GameObject;
                instObj.transform.position = new Vector3(firstX, firstY, 0);
            }else
            {
                
                
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            firstX = -1;
            firstY = -1;
            instObj = null;
        }
        	
	}
}
