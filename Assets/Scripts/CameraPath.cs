using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPath : MonoBehaviour {

    public Transform cameraFocus;
    public float rotationSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TimeSpan time = DateTime.Now.TimeOfDay;

        cameraFocus.localRotation =
             Quaternion.Euler(0f, (float)time.TotalSeconds * rotationSpeed, 0f);
    }
}
