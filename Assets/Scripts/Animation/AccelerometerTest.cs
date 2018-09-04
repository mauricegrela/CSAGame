using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerTest : MonoBehaviour {


    public float Inverterx;
    public float Invertery;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
        transform.Translate(Input.acceleration.x * Inverterx,-Input.acceleration.z * Invertery,0);//Input.acceleration.y * Invertery, 0);
	}
}
