using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNodeTable : MonoBehaviour {

    public enum timeResolution
    {
       Quarter,Eighth,Sixteenth
    }
    public timeResolution resolution = timeResolution.Quarter;
    //Xsize = 64px, Ysize = 54px
    public GameObject QuarterNote;
    public GameObject EighthNote;
    public GameObject SixteenthNote;


    GameObject cur_obj;
    public List<GameObject> resolObj;

	// Use this for initialization
	void Start () {

        resolObj.Add(Instantiate(QuarterNote) as GameObject);
        resolObj.Add(Instantiate(EighthNote) as GameObject);
        resolObj.Add(Instantiate(SixteenthNote) as GameObject);
        for(int i=0; i<3; i++)
        {
            resolObj[i].transform.parent = transform;
            resolObj[i].SetActive(false);
        }

        switch (resolution)
        {
            case timeResolution.Quarter:
                cur_obj = resolObj[0];
                resolObj[0].SetActive(true);
                break;
            case timeResolution.Eighth:
                cur_obj = resolObj[1];
                resolObj[1].SetActive(true);
                break;
            case timeResolution.Sixteenth:
                cur_obj = resolObj[2];
                resolObj[2].SetActive(true);
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        
		
	}
    public void changeResolution(string resol)
    {
        if(cur_obj.name == resol + "Note")
        {
            print("Same");
            return;
        }else
        {
            print(resol);
            cur_obj.SetActive(false);
            if (resol == "Quarter")
            {
                resolObj[0].SetActive(true);
                cur_obj = resolObj[0];
            }
            else if(resol == "Eighth")
            {
                resolObj[1].SetActive(true);
                cur_obj = resolObj[1];
            }
            else if(resol == "Sixteenth")
            {
                resolObj[2].SetActive(true);
                cur_obj = resolObj[2];
            }else
            {
                print("Error. No proper value to change");
            }
        }
    }
}
