using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_TestScene02 : MonoBehaviour {

	public LineController lineController;
	public GameObject visualPoint;
	public GameObject visualSlidePoint;
	public GameObject visualCenterPoint;
	public List<Point> points=new List<Point>();
	public List<SlidePoint> slidePoints=new List<SlidePoint>();
	public List<CenterPoint> centerPoints = new List<CenterPoint> ();
	public float startDelay = 3f;
	public static float timeSinceSongStarted = 0f;
	public static float timeOfSongStart = 5f;
	public float timeAdjust = 0f;
	public float greatRange=0.1f;
	public float goodRange=0.4f;
	public float badRange=1.2f;

	public bool gameStarted = false;

	public float gameStartSignTime = 2.5f;
	public float gameStartSignDisappearTime = 0.5f;
	public float gameStartSignAfterDisappearTime = 1f;

	public CanvasGroup gameStartSignCanvasGroup;

	public Animator lineAnimator;
	public Animator cameraAnimator;
	public Animator gameAreaAnimator;


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

	public Camera cam;


	private AudioSource song;

	private IEnumerator Fade (float finalAlpha)
	{
		float fadeSpeed = Mathf.Abs (gameStartSignCanvasGroup.alpha - finalAlpha) / gameStartSignDisappearTime;

		while (!Mathf.Approximately (gameStartSignCanvasGroup.alpha, finalAlpha))
		{
			gameStartSignCanvasGroup.alpha = Mathf.MoveTowards (gameStartSignCanvasGroup.alpha, finalAlpha,
				fadeSpeed * Time.deltaTime);

			yield return null;
		}
	}

	private IEnumerator Start(){
//		Debug.Log ("first " + Time.realtimeSinceStartup);
		yield return new WaitForSeconds (gameStartSignTime);
//		Debug.Log ("after game start sign wait time" + Time.realtimeSinceStartup);
		StartCoroutine(Fade (0f));
//		Debug.Log ("after fade(0)" + Time.realtimeSinceStartup);
		yield return new WaitForSeconds (gameStartSignAfterDisappearTime);
//		Debug.Log ("after game Start Sign After Disappear wait Time" + Time.realtimeSinceStartup);
		StartPlaying ();
	}




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
//		Debug.Log ("spawn center point");
		cpoint.refCenterPoint = Instantiate (thePoint, worldPosition, cpoint.getRotation (),gameArea.transform);
	}


	void OnEnable () {
//		Debug.Log (lineController.transform.rotation.z);
//		Debug.Log("enable" + Time.realtimeSinceStartup);


//		cam = Camera.main;

		//test points
//		points.Add (new Point(7f,5f));
//		points.Add (new Point(7f,6f));
//		points.Add (new Point(7f,7f));
//		points.Add (new Point(7f,8f));
//		points.Add (new Point(7f,9f));
//		points.Add (new Point(7f,10f));
//		points.Add (new Point(7f,11f));


		points.Add (new Point(7f,53.02f));
//		points.Add (new Point(-7f,53.73f));
		points.Add (new Point(7f,54.52f));
//		points.Add (new Point(-7f,55.07f));
		points.Add (new Point(7f,55.83f));
//		points.Add (new Point(-7f,56.44f));
		points.Add (new Point(7f,57.2f));
//		points.Add (new Point(-7f,57.9f));
		points.Add (new Point(7f,58.5f));
//		points.Add (new Point(-7f,59.16f));
		points.Add (new Point(7f,59.87f));
//		points.Add (new Point(-7f,60.44f));
		points.Add (new Point(7f,61.2f));
//		points.Add (new Point(-7f,61.78f));
		points.Add (new Point(7f,62.52f));
//		points.Add (new Point(-7f,63.2f));

		points.Add (new Point(-7f,63.99f));
		points.Add (new Point(-7f,64.58f));
		points.Add (new Point(-7f,65.24f));
		points.Add (new Point(7f,65.86f));
		points.Add (new Point(7f,66.53f));
		points.Add (new Point(7f,67.18f));
		points.Add (new Point(7f,67.9f));
		points.Add (new Point(7f,68.51f));
		points.Add (new Point(7f,69.25f));
		points.Add (new Point(7f,69.9f));
		points.Add (new Point(7f,70.56f));
		points.Add (new Point(7f,71.23f));
		points.Add (new Point(-7f,71.92f));
		points.Add (new Point(-7f,72.59f));
		points.Add (new Point(-7f,73.28f));
		points.Add (new Point(-7f,73.93f));
		points.Add (new Point(-7f,74.63f));
		points.Add (new Point(-7f,75.25f));

		centerPoints.Add (new CenterPoint(0f,2.5f));
		centerPoints.Add (new CenterPoint(90f,5.304f));
		centerPoints.Add (new CenterPoint(180f,7.998f));
		centerPoints.Add (new CenterPoint(270f,10.65f));
		centerPoints.Add (new CenterPoint(0f,13.3f));
		centerPoints.Add (new CenterPoint(90f,15.7f));
		centerPoints.Add (new CenterPoint(180f,18.55f));
		centerPoints.Add (new CenterPoint(270f,21.16f));
		centerPoints.Add (new CenterPoint(0f,23.85f));
		centerPoints.Add (new CenterPoint(0f,26.76f));
		centerPoints.Add (new CenterPoint(0f,29.25f));
		centerPoints.Add (new CenterPoint(0f,32.09f));
		centerPoints.Add (new CenterPoint(0f,34.67f));
		centerPoints.Add (new CenterPoint(0f,37.41f));
//		centerPoints.Add (new CenterPoint(0f,39.85f));
//		centerPoints.Add (new CenterPoint(0f,40.54f));
		centerPoints.Add (new CenterPoint(0f,41.19f));
//		centerPoints.Add (new CenterPoint(0f,41.85f));
		centerPoints.Add (new CenterPoint(0f,42.5f));
//		centerPoints.Add (new CenterPoint(0f,43.18f));
		centerPoints.Add (new CenterPoint(0f,43.84f));
//		centerPoints.Add (new CenterPoint(0f,44.52f));
		centerPoints.Add (new CenterPoint(0f,45.18f));
//		centerPoints.Add (new CenterPoint(0f,45.85f));
		centerPoints.Add (new CenterPoint(0f,46.52f));
//		centerPoints.Add (new CenterPoint(0f,47.18f));
		centerPoints.Add (new CenterPoint(0f,47.84f));
//		centerPoints.Add (new CenterPoint(0f,49.18f));
		centerPoints.Add (new CenterPoint(0f,50.485f));

		centerPoints.Add (new CenterPoint(0f,63.99f));
		centerPoints.Add (new CenterPoint(0f,64.58f));
		centerPoints.Add (new CenterPoint(0f,65.24f));
		centerPoints.Add (new CenterPoint(0f,65.86f));
		centerPoints.Add (new CenterPoint(0f,66.53f));
		centerPoints.Add (new CenterPoint(0f,67.18f));
		centerPoints.Add (new CenterPoint(0f,67.9f));
		centerPoints.Add (new CenterPoint(0f,68.51f));
		centerPoints.Add (new CenterPoint(0f,69.25f));
		centerPoints.Add (new CenterPoint(0f,69.9f));
		centerPoints.Add (new CenterPoint(0f,70.56f));
		centerPoints.Add (new CenterPoint(0f,71.23f));
		centerPoints.Add (new CenterPoint(0f,71.92f));
		centerPoints.Add (new CenterPoint(0f,72.59f));
		centerPoints.Add (new CenterPoint(0f,73.28f));
		centerPoints.Add (new CenterPoint(0f,73.93f));
		centerPoints.Add (new CenterPoint(0f,74.63f));
		centerPoints.Add (new CenterPoint(0f,75.25f));

		song = GetComponent<AudioSource> ();

		for (int i = 0; i < points.Count; i++) {
			dynamicPoints.Add (points [i]);
		}

		for (int i = 0; i < slidePoints.Count; i++) {
			dynamicSlidePoints.Add (slidePoints [i]);
		}

		for (int i = 0; i < centerPoints.Count; i++) {
			dynamicCenterPoints.Add (centerPoints [i]);
		}
//		timeOfSongStart += Time.realtimeSinceStartup;

	}

	void Update () {
		if (gameStarted) {

//			Debug.Log ("time since song started " + timeSinceSongStarted);
//			Debug.Log (lineController.transform.rotation.z);
//			Debug.Log ("timeOfSongStart" + timeOfSongStart);
			timeSinceSongStarted = Time.time - timeOfSongStart - timeAdjust;

			//		Debug.Log (timeSinceSongStarted);

			for (int i = 0; i < points.Count; i++) {
				if (points [i].getAppearTime () <= timeSinceSongStarted && points [i].getStatus () == false) {
					SprawnPoint (points [i], visualPoint);
					points [i].setStatus (true);
				}
			}

			for (int i = 0; i < slidePoints.Count; i++) {
				if (slidePoints [i].getAppearTime () <= timeSinceSongStarted && slidePoints [i].getStatus () == false) {
					SprawnSlidePoint (slidePoints [i], visualSlidePoint);
					slidePoints [i].setStatus (true);
				}
			}

			for (int i = 0; i < centerPoints.Count; i++) {
				if (centerPoints [i].getAppearTime () <= timeSinceSongStarted && centerPoints [i].getStatus () == false) {
					SprawnCenterPoint (centerPoints [i], visualCenterPoint);
					centerPoints [i].setStatus (true);
//					Debug.Log ("center point " + i + " spawned, time: " + timeSinceSongStarted);
				}
			}

			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					Vector3 touchPosition = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
					if (Vector2.Distance (touchPosition, gameArea.transform.position) < 1.8f) {
//						Debug.Log ("center touch");
						for (int i = 0; i < dynamicCenterPoints.Count; i++) {
							if (Mathf.Abs (dynamicCenterPoints [i].getClickTime () - timeSinceSongStarted) < 1.2f) {
								pendingJudgements.Add (new Judgement (timeSinceSongStarted, dynamicCenterPoints [i].getClickTime (), dynamicCenterPoints [i]));
								dynamicCenterPoints.RemoveAt (i);
								i--;
								break;
							}
						}

					} else {
						pointTouches.Add (new PointTouch (touchPosition, timeSinceSongStarted));
//						Debug.Log ("add point touch " + timeSinceSongStarted);
//						Debug.Log ("position: " + touchPosition);
					}
				}
			}

			//		Debug.Log (pointTouches.Count);

			for (int i = pointTouches.Count; i > 0; i--) {
				//			Debug.Log (i);
				for (int j = 0; j < dynamicPoints.Count; j++) {
					//				Debug.Log ("point times: " + j);
					if (pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > dynamicPoints [j].getAppearTime () && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x - gameArea.transform.position.x, pointTouches [i - 1].getTouchPosition ().y - gameArea.transform.position.y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
						Debug.Log ("in if");
						pendingJudgements.Add (new Judgement (pointTouches [i - 1].getTouchTime (), dynamicPoints [j].getClickTime (), dynamicPoints [j]));
						pointTouches.RemoveAt (i - 1);
						dynamicPoints.RemoveAt (j);
						j--;
						break;
					}
				}
			}

			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began) {
					for (int i = 0; i < dynamicSlidePoints.Count; i++) {
						if (Mathf.Abs (timeSinceSongStarted - dynamicSlidePoints [i].getClickTime ()) < 0.1f) {
							//						Debug.Log ("inside 1");
							Vector3 worldPositionv3 = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
							//						Debug.Log ("outside");
							//						Debug.Log ("i = " + i);
							//						Debug.Log ("touch position " + (Vector2) worldPositionv3);
							//						Debug.Log ("point position " + new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ()));
							//						Debug.Log ("adjust touch position " + new Vector2 (worldPositionv3.x - gameArea.transform.position.x, worldPositionv3.y - gameArea.transform.position.y));
							if (Vector2.Distance (new Vector2 (worldPositionv3.x - gameArea.transform.position.x, worldPositionv3.y - gameArea.transform.position.y), new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ())) <= dynamicSlidePoints [i].getJudgeRange ()) {
								//							Debug.Log ("inside");
								//							Debug.Log ("i = " + i);
								pendingJudgements.Add (new Judgement (0f, 0f, dynamicSlidePoints [i]));
								dynamicSlidePoints.RemoveAt (i);
								i--;
							}
						}
					}
				}
			}


			for (int i = 0; i < dynamicPoints.Count; i++) {
				if (dynamicPoints [i].getDisappearTime () <= timeSinceSongStarted) {
					pendingJudgements.Add (new Judgement (-5f, 0f, dynamicPoints [i]));
					dynamicPoints.RemoveAt (i);
					i--;
				}
			}

			for (int i = 0; i < dynamicSlidePoints.Count; i++) {
				if (dynamicSlidePoints [i].getDisappearTime () <= timeSinceSongStarted) {
					pendingJudgements.Add (new Judgement (-5f, 0f, dynamicSlidePoints [i]));
					dynamicSlidePoints.RemoveAt (i);
					i--;
				}
			}

			for (int i = 0; i < dynamicCenterPoints.Count; i++) {
				if (dynamicCenterPoints [i].getDisappearTime () <= timeSinceSongStarted) {
					pendingJudgements.Add (new Judgement (-5f, 0f, dynamicCenterPoints [i]));
					dynamicCenterPoints.RemoveAt (i);
					i--;
				}
			}
			//		Debug.Log (pendingJudgements.Count);

			Judge ();
		}
	}

	void StartPlaying()
	{
		

		timeOfSongStart = Time.time;
		lineAnimator.SetTrigger ("StartPlaying");
		cameraAnimator.SetTrigger ("StartPlaying");
		gameAreaAnimator.SetTrigger ("StartPlaying");
		gameStarted = true;

		song.Play ();
		Debug.Log ("start play song " + timeSinceSongStarted);
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
