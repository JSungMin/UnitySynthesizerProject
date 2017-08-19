using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewScript : MonoBehaviour {
	public string strUrl = "http://www.naver.com";
	private WebViewObject webViewObject;

	// Use this for initialization
	void Start () {
		StartWebView ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android)
		{
			if (Input.GetKeyDown (KeyCode.Escape))
			{
				Destroy (webViewObject);
				return;
			}
		}
	}

	public void StartWebView ()
	{
		webViewObject = (new GameObject ("WebViewObject")).AddComponent<WebViewObject>();
		webViewObject.Init((msg) => {
				Debug.Log (string.Format("CallFromJS[{0}]", msg));
		});

		webViewObject.LoadURL (strUrl);
		webViewObject.SetVisibility (true);
		webViewObject.SetMargins (50, 50, 50, 50);
	}
}
