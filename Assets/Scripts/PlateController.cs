using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour {

	public static float speed;
	public static float rotationZ;

	public float initSpeed=30f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		rotationZ = transform.rotation.eulerAngles.z;
		speed = initSpeed;
		transform.Rotate(0,0,speed*Time.deltaTime);
		
	}
}
