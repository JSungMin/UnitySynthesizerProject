using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownControl : MonoBehaviour {

    Dropdown drop;
    public GameObject table;
    CreateNodeTable tableChanger;
	// Use this for initialization
	void Start () {
        drop = GetComponent<Dropdown>();
        tableChanger = table.GetComponent<CreateNodeTable>();
        drop.onValueChanged.AddListener(delegate { dropdownValueChangeHandler(drop); });
	}
    void Destroy()
    {
        drop.onValueChanged.RemoveAllListeners();
    }

    private void dropdownValueChangeHandler(Dropdown target)
    {
        string tmp;
        if(target.value == 0)
        {
            tmp = "Quarter";
        }else if(target.value == 1)
        {
            tmp = "Eighth";
        }else if(target.value == 2)
        {
            tmp = "Sixteenth";
        }
        else
        {
            tmp = target.value.ToString();
        }
        tableChanger.changeResolution(tmp);
    }
}
