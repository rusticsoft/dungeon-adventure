using UnityEngine;
using System.Collections;
using System;

public class ClockAnimator : MonoBehaviour {
	public Transform hours, minutes, seconds;
	
	private const float
		hoursToDegrees = 360f / 12f,
		minutesToDegrees = 360f / 60f,
		secondsToDegrees = 360f / 60f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		DateTime time = DateTime.Now;
		seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
		minutes.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees / 2.0f);
		hours.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees / 6.0f);
	}
	
	
}
