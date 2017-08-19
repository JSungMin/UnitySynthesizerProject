using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeizerSpline : MonoBehaviour {

	[SerializeField]
	public List<Transform> points;
    public GameObject point;
    //public BeizerSpline(Transform start,Transform end)
    //{
    //    points.Add(start);
    //    points.Add(end);
    //}
    //Sta
    public Vector3 GetPoint (float t) {

        return GetPoint(0, points.Count, t);
	}
    Vector3 GetPoint(int s,int l,float t)
    {
        if (l == 2) return Vector3.Lerp(points[s].position, points[s + 1].position,t);
        return Vector3.Lerp(GetPoint(s, l - 1, t), GetPoint(s + 1, l - 1, t),t);
    }
	public void AddCurve(){
        var t=Instantiate(point);
        t.transform.position = Vector3.Lerp(points[points.Count - 2].position, points[points.Count - 1].position, 0.5f);
        t.SetActive( true);
        points.Insert(points.Count-1,t.transform);
	}
}