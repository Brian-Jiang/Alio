using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public class TemporaryLineProperty {
//	private float angle;
//	private float time;
//
//	public TemporaryLineProperty(float currentAngle, float currentTime)
//	{
//		angle = currentAngle;
//		time = currentTime;
//	}
//
//	public float getAngle(){
//		return angle;
//	}
//
//	public float getTime(){
//		return time;
//	}
//}




public class LineController : MonoBehaviour {


	public static float speed;
	public bool startSpin = false;
//	public static List<TemporaryLineProperty> temporary=new List<TemporaryLineProperty>();

	public float initSpeed=30f;

	public static float RotationZ;






	void Update() {
		speed = initSpeed;
		RotationZ = transform.rotation.eulerAngles.z;

//		temporary.Add (new TemporaryLineProperty (transform.rotation.eulerAngles.z,GameController.timeSinceSongStarted));


		if (startSpin) {
//			Debug.Log (RotationZ);

//			transform.localRotation = Quaternion.Euler (new Vector3 (0, 0, speed *  (Time.realtimeSinceStartup - GameController.timeOfSongStart)));
			transform.Rotate(0,0,speed*Time.deltaTime);
		}

//		Debug.Log (transform.rotation.eulerAngles.z);
		

	}
}
