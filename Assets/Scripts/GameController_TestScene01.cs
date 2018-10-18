using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]


public class PointTouch {

	private Vector2 touchPosition;
	private float myTouchTime;

	public PointTouch(Vector2 touchPosi,float touchTime)
	{
		touchPosition = touchPosi;
		myTouchTime = touchTime;
	}

	public Vector2 getTouchPosition()
	{
		return touchPosition;
	}

	public float getTouchTime()
	{
		return myTouchTime;
	}

}




public class Judgement {

	private float touchTime;
	private float pointTime;
	private GameObject referencePoint;

	public Judgement(float tchTime,float pointT, Point refPoint)
	{
		touchTime = tchTime;
		pointTime = pointT;
		referencePoint = (GameObject) refPoint.refPoint;
	}

	public Judgement(float tchTime,float pointT, SlidePoint refPoint)
	{
		touchTime = tchTime;
		pointTime = pointT;
		referencePoint = (GameObject) refPoint.refPoint;
	}

	public Judgement(float tchTime,float pointT, CenterPoint refPoint)
	{
		touchTime = tchTime;
		pointTime = pointT;
		referencePoint = (GameObject) refPoint.refCenterPoint;
	}

	public float getTouchTime()
	{
		return touchTime;
	}

	public float getPointTime()
	{
		return pointTime;
	}

	public float getDifferTime()
	{
		return Mathf.Abs (touchTime - pointTime);
	}

	public GameObject getReferencePoint()
	{
		return referencePoint;
	}
}




public class Point
{
	private float xPos;
	private float yPos;
	private float radius;
	private float clickTime;
	private float appearTime;
	private float disappearTime;
	private float prepareTime;
	private float jRange=2.5f;
	private bool hasSpawned=false;
	private bool isFixed;
	private float angl;
	public GameObject refPoint;

	public Point(float r,float cliTime)
	{
		radius = r;
		clickTime = cliTime;
		prepareTime = 1.5f;
		appearTime = cliTime - prepareTime;
		isFixed = false;
	}

	public Point(float r, float cliTime, float judgeRange)
	{
		jRange = judgeRange;
		radius = r;
		clickTime = cliTime;
		prepareTime = 1.5f;
		appearTime = cliTime - prepareTime;
		isFixed = false;
	}

	public Point(float r, float cliTime, float judgeRange, float preTime)
	{
		jRange = judgeRange;
		prepareTime = preTime;
		appearTime = cliTime - prepareTime;
		radius = r;
		clickTime = cliTime;
		isFixed = false;
	}

	public Point(float r,float cliTime,float judgeRange, float preTime,float angle)
	{
		angl = angle;
		jRange = judgeRange;
		prepareTime = preTime;
		appearTime = cliTime - prepareTime;
		radius = r;
		clickTime = cliTime;
		isFixed = true;
	}
		

	public float getX()
	{
		if (!isFixed) {
			return radius * Mathf.Cos (LineController.RotationZ * Mathf.PI / 180);

		} else {
			return refPoint.transform.position.x;  //need to change
		}
	}

	public float getY()
	{
		if (!isFixed) {
			return radius * Mathf.Sin (LineController.RotationZ * Mathf.PI / 180);
		} else {
			return refPoint.transform.position.y;   //need to change
		}


	}


	public float getAppearX ()
	{
		if (!isFixed) {
			float appearAngleDegree = ((LineController.RotationZ + LineController.speed * prepareTime) -  PlateController.speed * prepareTime) * Mathf.PI / 180;
			return radius * Mathf.Cos (appearAngleDegree);
		} else {
			float appearAngleDegree = angl * Mathf.PI / 180;
			return radius * Mathf.Cos (appearAngleDegree);
		}
	}

	public float getAppearY ()
	{
		if (!isFixed) {
			float appearAngleDegree = ((LineController.RotationZ + LineController.speed * prepareTime) - PlateController.speed * prepareTime) * Mathf.PI / 180;
			return radius * Mathf.Sin (appearAngleDegree);
		} else {
			float appearAngleDegree = angl * Mathf.PI / 180;
			return radius * Mathf.Sin (appearAngleDegree);
		}
	}

	public Quaternion getRotation(){
		if (!isFixed) {
			float lineClickTimeAngle = LineController.RotationZ + LineController.speed * prepareTime;
			float plateAppearTimeAngle = lineClickTimeAngle - PlateController.speed * prepareTime;
			return Quaternion.Euler (new Vector3 (0f, 0f, plateAppearTimeAngle));
		} else {
			return Quaternion.Euler (new Vector3 (0f, 0f, angl));
		}
	}

	public bool IsFixed(){
		return isFixed;
	}


	public float getAppearTime()
	{
		return appearTime;
	}

	public float getDisappearTime()
	{
		return clickTime + 0.4f;
	}

	public float getClickTime()
	{
		return clickTime;
	}

	public bool getStatus()
	{
		return hasSpawned;
	}

	public void setStatus(bool status)
	{
		hasSpawned = status;
	}

	public float getJudgeRange(){
		return jRange;
	}
}
 



public class SlidePoint
{
	private float xPos;
	private float yPos;
	private float radius;
	private float clickTime;
	private float appearTime;
	private float disappearTime;
	private float prepareTime;
	private float jRange=2.5f;
	private bool hasSpawned=false;
	private float angl;
	private bool isFixed;

	public GameObject refPoint;

	public SlidePoint(float r,float cliTime)
	{
		radius = r;
		clickTime = cliTime;
		prepareTime = 1.5f;
		appearTime = cliTime - prepareTime;
		isFixed = false;
	}

	public SlidePoint(float r, float cliTime, float judgeRange)
	{
		jRange = judgeRange;
		radius = r;
		clickTime = cliTime;
		prepareTime = 1.5f;
		appearTime = cliTime - prepareTime;
		isFixed = false;
	}

	public SlidePoint(float r, float cliTime, float judgeRange, float preTime)
	{
		jRange = judgeRange;
		prepareTime = preTime;
		appearTime = cliTime - prepareTime;
		radius = r;
		clickTime = cliTime;
		isFixed = false;
	}

	public SlidePoint(float r,float cliTime,float judgeRange, float preTime,float angle)
	{
		angl = angle;
		jRange = judgeRange;
		prepareTime = preTime;
		appearTime = cliTime - prepareTime;
		radius = r;
		clickTime = cliTime;
		isFixed = true;
	}


	public float getX()
	{
		if (!isFixed) {
			return radius * Mathf.Cos (LineController.RotationZ * Mathf.PI / 180);

		} else {
			return refPoint.transform.position.x;  //need to change
		}
	}

	public float getY()
	{
		if (!isFixed) {
			return radius * Mathf.Sin (LineController.RotationZ * Mathf.PI / 180);
		} else {
			return refPoint.transform.position.y;   //need to change
		}
	}


	public float getAppearX ()
	{
		if (!isFixed) {
			float appearAngleDegree = ((LineController.RotationZ + LineController.speed * prepareTime) -  PlateController.speed * prepareTime) * Mathf.PI / 180;
			return radius * Mathf.Cos (appearAngleDegree);
		} else {
			float appearAngleDegree = angl * Mathf.PI / 180;
			return radius * Mathf.Cos (appearAngleDegree);
		}
	}

	public float getAppearY ()
	{
		if (!isFixed) {
			float appearAngleDegree = ((LineController.RotationZ + LineController.speed * prepareTime) - PlateController.speed * prepareTime) * Mathf.PI / 180;
			return radius * Mathf.Sin (appearAngleDegree);
		} else {
			float appearAngleDegree = angl * Mathf.PI / 180;
			return radius * Mathf.Sin (appearAngleDegree);
		}
	}

	public Quaternion getRotation(){
		if (!isFixed) {
			float lineClickTimeAngle = LineController.RotationZ + LineController.speed * prepareTime;
			float plateAppearTimeAngle = lineClickTimeAngle - PlateController.speed * prepareTime;
			return Quaternion.Euler (new Vector3 (0f, 0f, plateAppearTimeAngle));
		} else {
			return Quaternion.Euler (new Vector3 (0f, 0f, angl));
		}
	}

	public bool IsFixed(){
		return isFixed;
	}



	public float getAppearTime()
	{
		return appearTime;
	}

	public float getDisappearTime()
	{
		return clickTime + 0.2f;
	}

	public float getClickTime()
	{
		return clickTime;
	}

	public bool getStatus()
	{
		return hasSpawned;
	}

	public void setStatus(bool status)
	{
		hasSpawned = status;
	}

	public float getJudgeRange(){
		return jRange;
	}
}




public class CenterPoint {
	private float radius;
	private float cliTime;
	private float appearTime;
	private float disappearTime;
	private bool hasSpawned = false;

	public object refCenterPoint;

	public CenterPoint(float r,float clickTime){
		radius = r;
		cliTime = clickTime;
	}

	public Quaternion getRotation(){
		return Quaternion.Euler (0f, 0f, -radius);
	}

	public float getAppearTime(){
		return cliTime - 1.5f;
	}

	public float getDisappearTime(){
		return cliTime + 0.4f;
	}

	public float getClickTime()
	{
		return cliTime;
	}

	public bool getStatus()
	{
		return hasSpawned;
	}

	public void setStatus(bool status)
	{
		hasSpawned = status;
	}


}





public class GameController_TestScene01 : MonoBehaviour {

	public LineController lineController;
	public GameObject visualPoint;
	public GameObject visualSlidePoint;
	public GameObject visualCenterPoint;
	public List<Point> points=new List<Point>();
	public List<SlidePoint> slidePoints=new List<SlidePoint>();
	public List<CenterPoint> centerPoints = new List<CenterPoint> ();
	public float startDelay = 3f;
	public static float timeSinceSongStarted;
	public static float timeOfSongStart;
	public float greatRange=0.1f;
	public float goodRange=0.4f;
	public float badRange=1.2f;



	public GameObject plate;
	public GameObject gameArea;


	public Object greatHit;
	public Object goodHit;
	public Object badHit;

	public static List<Judgement> judgements=new List<Judgement>();

	private List<Judgement> pendingJudgements = new List<Judgement> ();
	private List<Point> dynamicPoints = new List<Point> ();
	private List<SlidePoint> dynamicSlidePoints = new List<SlidePoint> ();
	private List<CenterPoint> dynamicCenterPoints = new List<CenterPoint> ();
	private List<PointTouch> pointTouches = new List<PointTouch> ();

	Camera cam;

	private Point point0 = new Point (5f,4f);
	private Point point1 = new Point (6.6f,6f);
	private Point point2 = new Point (-7.2f,8f);
	private Point point3 = new Point (5f,10f);
	private Point point4 = new Point (-9-.2f,4.80f);
	private Point point5 = new Point (-7.5f,5.80f);
	private Point point6 = new Point (-6.1f,6.80f);
	private Point point7 = new Point (-6.8f,7.72f);
	private Point point8 = new Point (8.3f,8.72f);
	private Point point9 = new Point (5.3f,9.72f);
	private Point point10 = new Point (-6.89f,11.64f);
	private Point point11 = new Point (8.21f,12.68f);
	private Point point12 = new Point (7.2f,13.72f);
	private Point point13 = new Point (5.78f,14.84f);
	private Point point14 = new Point (6.79f,17.12f);
	private Point point15 = new Point (6.1f,18f);
	private Point point16 = new Point (5.0f,19f);
	private Point point17 = new Point (6.1f,20.1f);
	private Point point18 = new Point (8.76f,21.04f);
	private Point point19 = new Point (6.09f,22.02f);
	private Point point20 = new Point (6.38f,23f);
	private Point point21 = new Point (5.21f,24f);
	private Point point22 = new Point (8.33f,25.08f);
	private Point point23 = new Point (6.33f,26.12f);
	private Point point24 = new Point (5.45f,27.1f);
	private Point point25 = new Point (-6.3f,28.12f);
	private Point point26 = new Point (-5.4f,29.12f);
	private Point point27 = new Point (6.53f,30.08f);
	private Point point28 = new Point (-7.09f,31.1f);
	private Point point29 = new Point (5.9f,32.12f);
	private Point point30 = new Point (5.62f,33.14f);
	private Point point31 = new Point (-10.33f,25.08f);
	private Point point32 = new Point (-5.5f,4.80f);
	private SlidePoint spoint1 = new SlidePoint (6f, 7f);
	private SlidePoint spoint2 = new SlidePoint (6f, 7.2f);
	private SlidePoint spoint3 = new SlidePoint (6f, 7.4f);
	private SlidePoint spoint4 = new SlidePoint (6f, 7.6f);
	private SlidePoint spoint5 = new SlidePoint (6f, 7.8f);
	private SlidePoint spoint6 = new SlidePoint (6f, 8f);
	private SlidePoint spoint7 = new SlidePoint (6f, 8.2f);
	private SlidePoint spoint8 = new SlidePoint (6f, 8.4f);
	private SlidePoint spoint9 = new SlidePoint (6f, 8.6f);
	private SlidePoint spoint10 = new SlidePoint (6f, 8.8f);
	private SlidePoint spoint11 = new SlidePoint (6f, 9f);
	private SlidePoint spoint12 = new SlidePoint (6f, 9.2f);
	private SlidePoint spoint13 = new SlidePoint (6f, 9.4f);
	private SlidePoint spoint14 = new SlidePoint (6f, 9.6f);
	private CenterPoint cpoint1 = new CenterPoint (30f, 5f);
	private CenterPoint cpoint2 = new CenterPoint (60f, 8f);
	private CenterPoint cpoint3 = new CenterPoint (30f, 11f);
	private CenterPoint cpoint4 = new CenterPoint (30f, 16f);
	private CenterPoint cpoint5 = new CenterPoint (30f, 18f);
	private CenterPoint cpoint6 = new CenterPoint (30f, 20f);
//	private Point point33 = new Point (-8f,-3f,5f);
//	private Point point34 = new Point (3f,3f);
//	private Point point35 = new Point (3f,4f);
//	private Point point36 = new Point (3f,5f);
//	private Point point37 = new Point (3f,6f);
//	private Point point38 = new Point (3f,7f);
//	private Point point39 = new Point (3f,8f);
//	private Point point40 = new Point (3f,9f);
//	private Point point41 = new Point (3f,10f);
//	private Point point42 = new Point (3f,11f);
//	private Point point43 = new Point (3f,12f);
//	private Point point44 = new Point (3f,13f);
//	private Point point45 = new Point (3f,14f);
//	private Point point46 = new Point (3f,15f);
//	private Point point47 = new Point (3f,16f);

	private AudioSource song;

	public void SprawnPoint(Point point,GameObject thePoint)
	{
		Vector3 worldPosition;
		worldPosition.x = point.getAppearX () + gameArea.transform.position.x;
		worldPosition.y = point.getAppearY () + gameArea.transform.position.y;
		worldPosition.z = 0f;
		point.refPoint = Instantiate (thePoint, worldPosition, point.getRotation (),plate.transform);
	}

	public void SprawnSlidePoint(SlidePoint spoint,GameObject thePoint)
	{
		Vector3 worldPosition;
		worldPosition.x = spoint.getAppearX () + gameArea.transform.position.x;
		worldPosition.y = spoint.getAppearY () + gameArea.transform.position.y;
		worldPosition.z = 0f;
		spoint.refPoint = Instantiate (thePoint, worldPosition, spoint.getRotation (),plate.transform);
	}

	public void SprawnCenterPoint(CenterPoint cpoint,GameObject thePoint)
	{
		Vector3 worldPosition;
		worldPosition.x = gameArea.transform.position.x;
		worldPosition.y = gameArea.transform.position.y;;
		worldPosition.z = 0f;
		Debug.Log ("spawn center point");
		cpoint.refCenterPoint = Instantiate (thePoint, worldPosition, cpoint.getRotation (),gameArea.transform);
	}


	void Start () {




		cam = Camera.main;

		points.Add (point0);
		points.Add (point1);
		points.Add (point2);
		points.Add (point3);
//		points.Add (point4);
//		points.Add (point5);
//		points.Add (point6);
//		points.Add (point7);
//		points.Add (point8);
//		points.Add (point9);
//		points.Add (point10);
//		points.Add (point11);
//		points.Add (point12);
//		points.Add (point13);
//		points.Add (point14);
//		points.Add (point15);
//		points.Add (point16);
//		points.Add (point17);
//		points.Add (point18);
//		points.Add (point19);
//		points.Add (point20);
//		points.Add (point21);
//		points.Add (point22);
//		points.Add (point23);
//		points.Add (point24);
//		points.Add (point25);
//		points.Add (point26);
//		points.Add (point27);
//		points.Add (point28);
//		points.Add (point29);
//		points.Add (point30);
//		points.Add (point31);
//		points.Add (point32);
		slidePoints.Add (spoint1);
		slidePoints.Add (spoint2);
		slidePoints.Add (spoint3);
		slidePoints.Add (spoint4);
		slidePoints.Add (spoint5);
		slidePoints.Add (spoint6);
		slidePoints.Add (spoint7);
		slidePoints.Add (spoint8);
		slidePoints.Add (spoint9);
		slidePoints.Add (spoint10);
		slidePoints.Add (spoint11);
		slidePoints.Add (spoint12);
		slidePoints.Add (spoint13);
		slidePoints.Add (spoint14);
		centerPoints.Add (cpoint1);
		centerPoints.Add (cpoint2);
		centerPoints.Add (cpoint3);
		centerPoints.Add (cpoint4);
		centerPoints.Add (cpoint5);
		centerPoints.Add (cpoint6);
//		points.Add (point33);
//		points.Add (point34);
//		points.Add (point35);
//		points.Add (point36);
//		points.Add (point37);
//		points.Add (point38);
//		points.Add (point39);
//		points.Add (point40);
//		points.Add (point41);
//		points.Add (point42);
//		points.Add (point43);
//		points.Add (point44);
//		points.Add (point45);
//		points.Add (point46);
//		points.Add (point47);


		song = GetComponent<AudioSource> ();

		StartPlaying ();

	}

	void Update () {

		timeSinceSongStarted = Time.realtimeSinceStartup - timeOfSongStart;
//		Debug.Log ("timeOfSongStart" + timeOfSongStart);
//		Debug.Log (timeSinceSongStarted);

		for (int i=0;i<points.Count;i++) {
			if (Mathf.Abs(points[i].getAppearTime()-timeSinceSongStarted)<0.1f && points[i].getStatus() == false) {
				SprawnPoint (points[i],visualPoint);
				points [i].setStatus (true);
			}
		}

		for (int i=0;i<slidePoints.Count;i++) {
			if (Mathf.Abs(slidePoints[i].getAppearTime()-timeSinceSongStarted)<0.1f && slidePoints[i].getStatus() == false) {
				SprawnSlidePoint (slidePoints[i],visualSlidePoint);
				slidePoints [i].setStatus (true);
			}
		}

		for (int i=0;i<centerPoints.Count;i++) {
			if (Mathf.Abs(centerPoints[i].getAppearTime()-timeSinceSongStarted)<0.1f && centerPoints[i].getStatus() == false) {
				SprawnCenterPoint (centerPoints[i],visualCenterPoint);
				centerPoints [i].setStatus (true);
			}
		}

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				Vector3 touchPosition = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
				if (Vector2.Distance (touchPosition, gameArea.transform.position) < 1.8f) {
					Debug.Log ("center touch");
					for (int i = 0; i < dynamicCenterPoints.Count; i++) {
						if (Mathf.Abs (dynamicCenterPoints[i].getClickTime ()-timeSinceSongStarted) < 1.2f) {
							pendingJudgements.Add (new Judgement (timeSinceSongStarted, dynamicCenterPoints [i].getClickTime (), dynamicCenterPoints [i]));
							dynamicCenterPoints.RemoveAt (i);
							i--;
							break;
						}
					}

				} else {
					pointTouches.Add (new PointTouch (touchPosition, timeSinceSongStarted));
				}
			}
		}

//		Debug.Log (pointTouches.Count);

		for (int i = pointTouches.Count; i > 0; i--) {
//			Debug.Log (i);
			for (int j = 0; j < dynamicPoints.Count; j++) {
//				Debug.Log ("point times: " + j);
				if (pointTouches.Count>0 && dynamicPoints.Count>0 && pointTouches[i-1].getTouchTime() > dynamicPoints[j].getAppearTime () && pointTouches[i-1].getTouchTime() < dynamicPoints[j].getDisappearTime() && Vector2.Distance(new Vector2(pointTouches[i-1].getTouchPosition().x-gameArea.transform.position.x,pointTouches[i-1].getTouchPosition().y-gameArea.transform.position.y),new Vector2(dynamicPoints[j].getX(),dynamicPoints[j].getY()))<=dynamicPoints[j].getJudgeRange ()) {
//					Debug.Log ("in if");
					pendingJudgements.Add (new Judgement (pointTouches [i-1].getTouchTime (), dynamicPoints [j].getClickTime (),dynamicPoints[j]));
					pointTouches.RemoveAt (i-1);
					dynamicPoints.RemoveAt (j);
					j--;
					break;
				}
			}
		}

		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began){
				 for (int i = 0; i < dynamicSlidePoints.Count; i++) {
					if (Mathf.Abs (timeSinceSongStarted - dynamicSlidePoints[i].getClickTime ()) < 0.1f){
//						Debug.Log ("inside 1");
						Vector3 worldPositionv3 = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
//						Debug.Log ("outside");
//						Debug.Log ("i = " + i);
//						Debug.Log ("touch position " + (Vector2) worldPositionv3);
//						Debug.Log ("point position " + new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ()));
//						Debug.Log ("adjust touch position " + new Vector2 (worldPositionv3.x - gameArea.transform.position.x, worldPositionv3.y - gameArea.transform.position.y));
						if (Vector2.Distance(new Vector2(worldPositionv3.x-gameArea.transform.position.x,worldPositionv3.y-gameArea.transform.position.y),new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ()))<=dynamicSlidePoints[i].getJudgeRange ()) {
//							Debug.Log ("inside");
//							Debug.Log ("i = " + i);
							pendingJudgements.Add(new Judgement(0f,0f,dynamicSlidePoints[i]));
							dynamicSlidePoints.RemoveAt (i);
							i--;
						}
					}
				}
			}
		}


		for (int i = 0; i < dynamicPoints.Count; i++) {
			if (dynamicPoints[i].getDisappearTime() <= timeSinceSongStarted) {
				pendingJudgements.Add (new Judgement (-5f, 0f, dynamicPoints [i]));
				dynamicPoints.RemoveAt (i);
				i--;
			}
		}

		for (int i = 0; i < dynamicSlidePoints.Count; i++) {
			if (dynamicSlidePoints[i].getDisappearTime() <= timeSinceSongStarted) {
				pendingJudgements.Add (new Judgement (-5f, 0f, dynamicSlidePoints [i]));
				dynamicSlidePoints.RemoveAt (i);
				i--;
			}
		}

		for (int i = 0; i < dynamicCenterPoints.Count; i++) {
			if (dynamicCenterPoints[i].getDisappearTime() <= timeSinceSongStarted) {
				pendingJudgements.Add (new Judgement (-5f, 0f, dynamicCenterPoints [i]));
				dynamicCenterPoints.RemoveAt (i);
				i--;
			}
		}
//		Debug.Log (pendingJudgements.Count);

		Judge ();
	}
		
	void StartPlaying()
	{
		for (int i = 0; i < points.Count; i++) {
			dynamicPoints.Add (points [i]);
		}

		for (int i = 0; i < slidePoints.Count; i++) {
			dynamicSlidePoints.Add (slidePoints [i]);
		}

		for (int i = 0; i < centerPoints.Count; i++) {
			dynamicCenterPoints.Add (centerPoints [i]);
		}

		timeOfSongStart = Time.realtimeSinceStartup;

		song.Play ();
		lineController.startSpin = true;
	}

	void Judge()
	{
		while (pendingJudgements.Count>0) {
//			Debug.Log ("time since song started = " + timeSinceSongStarted);
			if (pendingJudgements [0].getDifferTime () <= greatRange) {
				Debug.Log ("great");
//				Debug.Log ("touch time: " + pendingJudgements [0].getTouchTime ());
//				Debug.Log ("click time: " + pendingJudgements [0].getPointTime ());
				Instantiate (greatHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				pendingJudgements.RemoveAt (0);
			} else if (pendingJudgements [0].getDifferTime () <= goodRange) {
				Debug.Log ("good");
//				Debug.Log ("touch time: " + pendingJudgements [0].getTouchTime ());
//				Debug.Log ("click time: " + pendingJudgements [0].getPointTime ());
				Instantiate (goodHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				pendingJudgements.RemoveAt (0);
			} else if (pendingJudgements [0].getDifferTime () <= badRange) {
				Debug.Log ("bad");
//				Debug.Log ("touch time: " + pendingJudgements [0].getTouchTime ());
//				Debug.Log ("click time: " + pendingJudgements [0].getPointTime ());
				Instantiate (badHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				pendingJudgements.RemoveAt (0);
			} else {
				Debug.Log ("miss");
//				Debug.Log ("touch time: " + pendingJudgements [0].getTouchTime ());
//				Debug.Log ("click time: " + pendingJudgements [0].getPointTime ());
				Destroy (pendingJudgements [0].getReferencePoint ());
				pendingJudgements.RemoveAt (0);
			}
		}
	}
}
