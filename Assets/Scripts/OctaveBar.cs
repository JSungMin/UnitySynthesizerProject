using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OctaveBar : MonoBehaviour {
    public static OctaveBar instance;
    Scrollbar scroll;
    public Text octaveText;
	// Use this for initialization
	void Start () {
        instance = this;
        scroll = GetComponent<Scrollbar>();
        scroll.onValueChanged.AddListener(delegate { scrollValueChangeHandler(scroll); });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Destroy()
    {
        scroll.onValueChanged.RemoveAllListeners();
    }
    private void scrollValueChangeHandler(Scrollbar target)
    {
        int tmp = (int)(target.value * 10);
        octaveText.text =  tmp.ToString();
    }
}
