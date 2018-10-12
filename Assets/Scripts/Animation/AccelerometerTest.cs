using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AccelerometerTest : MonoBehaviour {


    public float Inverterx;
    public float Invertery;
    [SerializeField]
    private Text TextRef;
    private float PrevStep;
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

       // TextRef.text = transform.position.x.ToString();


            transform.Translate(Input.acceleration.x * Inverterx, -Input.acceleration.z * Invertery, 0); 


	}
}
