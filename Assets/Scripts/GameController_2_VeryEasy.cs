﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController_2_VeryEasy : MonoBehaviour {

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

	public int fullScore=100000;

	public bool gameStarted = false;

	public float gameStartSignTime = 2.5f;
	public float gameStartSignDisappearTime = 0.5f;
	public float gameStartSignAfterDisappearTime = 1f;
	public float resumeSignWaitTime = 1f;

	public CanvasGroup gameStartSignCanvasGroup;
	public CanvasGroup endCanvasGroup;

	public Animator lineAnimator;
	public Animator cameraAnimator;
	public Animator gameAreaAnimator;
	public Animator plateAnimator;

	public GameObject plate;
	public GameObject line;
	public GameObject gameArea;

	public GameObject gameStartSign;
	public GameObject pauseMenu;
	public GameObject endCanvas;
	public GameObject newHighScore;

	public GameAreaController_2_VeryEasy gameAreaController;

	public Text resultLevelText;
	public Text fullScoreText;
	public Text totalScoreText;
	public Text liveScoreText;
	public Text experienceAddedText;

	public Object greatHit;
	public Object goodHit;
	public Object badHit;

	public Font perfectPerformanceFont;
	public Font amazingPerformanceFont;
	public Font greatPerformanceFont;
	public Font goodPerformanceFont;
	public Font mediocrePerformanceFont;
	public Font badPerformanceFont;

	public static List<Judgement> judgements=new List<Judgement>();

	private List<Judgement> pendingJudgements = new List<Judgement> ();
	private List<Point> dynamicPoints = new List<Point> ();
	private List<SlidePoint> dynamicSlidePoints = new List<SlidePoint> ();
	private List<CenterPoint> dynamicCenterPoints = new List<CenterPoint> ();
	private List<PointTouch> pointTouches = new List<PointTouch> ();

	private bool canPauseGame = false;

	private SceneController sceneController;
//	private PlayerDataController dataController;

	public Camera cam;
	private AudioSource song;

	private int fullScoreOfOneKey;
	private int totalScore=0;
	private int experienceAdded = 0;
	private bool perfectPerformance=true;
	private bool allCombo=true;


	private IEnumerator Fade (CanvasGroup canvasGroup, float fadeDuration, float finalAlpha)
	{
		float fadeSpeed = Mathf.Abs (canvasGroup.alpha - finalAlpha) / fadeDuration;

		while (!Mathf.Approximately (canvasGroup.alpha, finalAlpha))
		{
			canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			yield return null;
		}
	}

	private IEnumerator Start(){
		yield return new WaitForSeconds (gameStartSignTime);
		StartCoroutine (Fade (gameStartSignCanvasGroup, gameStartSignDisappearTime, 0f));
		yield return new WaitForSeconds (gameStartSignAfterDisappearTime);
		gameStartSign.SetActive (false);
		canPauseGame = true;
		StartPlaying ();
	}


	public void pauseButtonOnClick(){
		if (canPauseGame) {
			song.Pause ();
			gameStarted = false;
			Time.timeScale = 0f;
			pauseMenu.SetActive (true);
		}

	}

	public void continueButtonOnClick(){
		pauseMenu.SetActive (false);
		gameStarted = true;
		song.UnPause ();
		Time.timeScale = 1f;
	}

	public void nextButtonOnClick(){
		sceneController.FadeAndLoadScene ("Main");
	}

	public void retryButtonOnClick(){
		Time.timeScale = 1f;
		sceneController.FadeAndLoadScene ("2_VeryEasy");
	}

	public void quitButtonOnClick(){
		Debug.Log ("quitButtonOnClick");
		Time.timeScale = 1f;
		sceneController.FadeAndLoadScene ("Main");

	}

	void OnApplicationPause(){
		pauseButtonOnClick ();
	}


	private void songEnd(){
		fullScoreText.text = fullScore.ToString ();
		totalScoreText.text = totalScore.ToString ();
		experienceAdded += 5;

		if (experienceAdded < 0) {
			experienceAdded = 0;
		}


		if (perfectPerformance) {
			resultLevelText.text = "Perfect Performance";
			totalScore = fullScore;
			experienceAdded += 700;
			totalScoreText.text = totalScore.ToString ();
			resultLevelText.font = perfectPerformanceFont;
			resultLevelText.fontSize = 265;
			sceneController.achievePerfectPerformance ();
		} else if (allCombo) {
			resultLevelText.text = "Amazing Performance";
			experienceAdded += 50;
			resultLevelText.font = amazingPerformanceFont;
			resultLevelText.fontSize = 190;
			sceneController.achieveAmazingPerformance ();
		} else if (totalScore >= (fullScore * 0.9f)) {
			resultLevelText.text = "Great Performance";
			experienceAdded += 20;
			resultLevelText.font = greatPerformanceFont;
		} else if (totalScore >= (fullScore * 0.7f)) {
			resultLevelText.text = "Good Performance";
			experienceAdded += 10;
			resultLevelText.font = goodPerformanceFont;
		} else if (totalScore >= (fullScore * 0.6f)) {
			resultLevelText.text = "Mediocre Performance";
			experienceAdded += 5;
			resultLevelText.font = mediocrePerformanceFont;
		} else if (totalScore != 0) {
			resultLevelText.text = "Bad Performance";
			experienceAdded += 0;
			resultLevelText.font = badPerformanceFont;
		} else {
			resultLevelText.text = "No Performance?";
			experienceAdded += 1;
			sceneController.achieveNoPerformance ();
		}


		experienceAddedText.text = "Experience +" + experienceAdded;

		PlayerPrefs.SetInt ("experience_2", PlayerPrefs.GetInt ("experience_2") + experienceAdded);
		PlayerPrefs.SetInt ("totalExperience", PlayerPrefs.GetInt ("experience_1") + PlayerPrefs.GetInt ("experience_2") + PlayerPrefs.GetInt ("experience_3") + PlayerPrefs.GetInt ("experience_4"));

		if (PlayerPrefs.GetInt ("experience_2") >= 400) {
			sceneController.unlockNewSongMessage ("Well-Tempered Clavier, Book 1： Prelude and Fugue No. 1 in C major",4);
		}

		if (Social.localUser.authenticated) {  
			Social.ReportScore(PlayerPrefs.GetInt ("totalExperience"), "Total_Experience_Leaderboard", HandleScoreReported);  
		}  


		if (totalScore > PlayerPrefs.GetInt ("highScore_2_veryEasy")) {
			PlayerPrefs.SetInt ("highScore_2_veryEasy", totalScore);
			newHighScore.SetActive (true);
		}

		endCanvas.SetActive (true);
		StartCoroutine (Fade (endCanvasGroup, 0.5f, 1f));

	}

	public void HandleScoreReported(bool success)  
	{  
		Debug.Log("*** HandleScoreReported: success = " + success);  
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
		cpoint.refCenterPoint = Instantiate (thePoint, worldPosition, cpoint.getRotation (),gameArea.transform);
	}


	void OnEnable () {
		sceneController = (SceneController) FindObjectOfType (typeof(SceneController));
//		dataController = (PlayerDataController) FindObjectOfType (typeof(PlayerDataController));


		points.Add (new Point(-7f, 1.895f));
		points.Add (new Point(-7f, 3.228f));
		points.Add (new Point(-8f, 4.635f));
		points.Add (new Point(-8f, 6.002f));
		points.Add (new Point(-7.5f, 7.406f));
		points.Add (new Point(-8.5f, 8.735f));
		points.Add (new Point(-7f, 10.073f));
		points.Add (new Point(-8f, 11.4f));

		points.Add (new Point (-7f, 12.827f));
		slidePoints.Add (new SlidePoint(-7.3f, 13.319f));
		slidePoints.Add (new SlidePoint(-7.6f, 13.524f));
		slidePoints.Add (new SlidePoint(-7.9f, 13.742f));
		slidePoints.Add (new SlidePoint(-8.4f, 13.945f));
		points.Add (new Point (-7f, 14.226f));
		slidePoints.Add (new SlidePoint(-6f, 14.65f));
		slidePoints.Add (new SlidePoint(-6f, 15.089f));

		points.Add (new Point (-7f, 15.55f));
		centerPoints.Add (new CenterPoint (-60f, 16.845f));

		points.Add (new Point (-9.5f, 18.176f));
		points.Add (new Point (-8f, 19.537f));
		points.Add (new Point (-7f, 20.858f));
		points.Add (new Point (-5.5f, 22.222f));

		centerPoints.Add (new CenterPoint (-60f, 23.955f));
		points.Add (new Point (8f, 25.343f));
		points.Add (new Point (9f, 26.727f));
		points.Add (new Point (9f, 28.047f));
		points.Add (new Point (8f, 29.483f));
		points.Add (new Point (7.7f, 30.826f));
		points.Add (new Point (7.2f, 32.134f));
		points.Add (new Point (8f, 33.5f));

		centerPoints.Add (new CenterPoint (60f, 34.828f));
		points.Add (new Point (8.5f, 36.157f));
		points.Add (new Point (7.9f, 37.528f));
		points.Add (new Point (7.8f, 38.809f));
		points.Add (new Point (6.5f, 40.146f));
		points.Add (new Point (6.1f, 41.468f));
		points.Add (new Point (5.7f, 42.748f));
		points.Add (new Point (5.3f, 44.141f));

		points.Add (new Point (-4f, 45.712f));
		points.Add (new Point (6f, 47.037f));
		points.Add (new Point (-3.4f, 48.367f));
		points.Add (new Point (5.4f, 49.715f));

		slidePoints.Add (new SlidePoint(6.4f, 51.057f));
		slidePoints.Add (new SlidePoint(6.5f, 51.295f));
		slidePoints.Add (new SlidePoint(6.6f, 51.508f));
		slidePoints.Add (new SlidePoint(6.7f, 51.7f));
		slidePoints.Add (new SlidePoint(6.8f, 51.904f));
		slidePoints.Add (new SlidePoint(6.9f, 52.1f));
		slidePoints.Add (new SlidePoint(7f, 52.375f));
		centerPoints.Add (new CenterPoint (-150f, 53.722f));
		points.Add (new Point (6.1f, 55.123f));

		points.Add (new Point (6.1f, 56.53f));
		points.Add (new Point (7.1f, 57.866f));
		points.Add (new Point (6.1f, 59.227f));
		points.Add (new Point (6f, 60.536f));
		slidePoints.Add (new SlidePoint(6f,60.785f));
		slidePoints.Add (new SlidePoint(7f,60.972f));
		slidePoints.Add (new SlidePoint(8f,61.194f));
		slidePoints.Add (new SlidePoint(9f,61.396f));
		points.Add (new Point (6f, 63.147f));
		points.Add (new Point (11f, 63.147f));

		points.Add (new Point (-10f, 67.244f));
		points.Add (new Point (-9.7f, 68.575f));
		points.Add (new Point (-9.2f, 69.9f));
		points.Add (new Point (-8f, 71.263f));
		points.Add (new Point (-8.4f, 73.84f));
		points.Add (new Point (-8f, 75.202f));
		points.Add (new Point (-8.7f, 76.606f));

		points.Add (new Point (-8f, 77.973f));
		points.Add (new Point (-9f, 79.322f));
		points.Add (new Point (-7.8f, 80.65f));
		points.Add (new Point (-6.7f, 82.021f));
		points.Add (new Point (4.7f, 83.356f));
		points.Add (new Point (5.9f, 84.621f));
		points.Add (new Point (-5.4f, 86.001f));
		points.Add (new Point (-8.7f, 87.539f));



		fullScoreOfOneKey = fullScore / (points.Count + centerPoints.Count + slidePoints.Count);
		Debug.Log (fullScoreOfOneKey);

		timeAdjust = PlayerPrefs.GetFloat ("timeAdjust");

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
	}

	void Update () {
		if (gameStarted) {
			timeSinceSongStarted = Time.time - timeOfSongStart - timeAdjust;

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
				}
			}

			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					Vector3 touchPosition = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
					if (Vector2.Distance (touchPosition, gameArea.transform.position) < 1.8f) {
						for (int i = 0; i < dynamicCenterPoints.Count; i++) {
							if (Mathf.Abs (dynamicCenterPoints [i].getClickTime () - timeSinceSongStarted) < 0.8f) {
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

			for (int i = pointTouches.Count; i > 0; i--) {
				for (int j = 0; j < dynamicPoints.Count; j++) {
					//					Debug.Log ("distance="+ Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x - gameArea.transform.position.x, pointTouches [i - 1].getTouchPosition ().y - gameArea.transform.position.y), new Vector2 (dynamicPoints [j].refPoint.transform.position.x, dynamicPoints [j].refPoint.transform.position.y)));
					if (dynamicPoints[j].IsFixed () && pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > (dynamicPoints [j].getClickTime () - 0.4f) && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x, pointTouches [i - 1].getTouchPosition ().y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
						//						Debug.Log ("in if");
						pendingJudgements.Add (new Judgement (pointTouches [i - 1].getTouchTime (), dynamicPoints [j].getClickTime (), dynamicPoints [j]));
						pointTouches.RemoveAt (i - 1);
						dynamicPoints.RemoveAt (j);
						j--;
						break;
					} else if (pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > (dynamicPoints [j].getClickTime () - 0.4f) && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x - gameArea.transform.position.x, pointTouches [i - 1].getTouchPosition ().y - gameArea.transform.position.y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
						//						Debug.Log ("in if");
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
							Vector3 worldPositionv3 = cam.ScreenToWorldPoint (new Vector3 (touch.position.x, touch.position.y, 0f));
							if (Vector2.Distance (new Vector2 (worldPositionv3.x - gameArea.transform.position.x, worldPositionv3.y - gameArea.transform.position.y), new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ())) <= dynamicSlidePoints [i].getJudgeRange ()) {
								pendingJudgements.Add (new Judgement (0f, 0f, dynamicSlidePoints [i]));
								dynamicSlidePoints.RemoveAt (i);
								i--;
								break;
							} else if (dynamicSlidePoints[i].IsFixed () && Vector2.Distance (new Vector2 (worldPositionv3.x, worldPositionv3.y), new Vector2 (dynamicSlidePoints [i].getX (), dynamicSlidePoints [i].getY ())) <= dynamicSlidePoints [i].getJudgeRange ()) {
								pendingJudgements.Add (new Judgement (0f, 0f, dynamicSlidePoints [i]));
								dynamicSlidePoints.RemoveAt (i);
								i--;
								break;
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
			Judge ();
		}
	}

	void StartPlaying()
	{
		timeOfSongStart = Time.time;
		lineAnimator.SetTrigger ("StartPlaying");
		cameraAnimator.SetTrigger ("StartPlaying");
		gameAreaAnimator.SetTrigger ("StartPlaying");
		plateAnimator.SetTrigger("StartPlaying");
		gameStarted = true;

		gameAreaController.animateGameArea ();

		song.Play ();
		lineController.startSpin = true;
		StartCoroutine (waitForPlayToEnd ());
	}

	private IEnumerator waitForPlayToEnd(){
		yield return new WaitForSeconds (song.clip.length + 2);
		songEnd ();
	}

	void Judge()
	{
		while (pendingJudgements.Count>0) {
			if (pendingJudgements [0].getDifferTime () <= greatRange) {
				Debug.Log ("great");
				Instantiate (greatHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				totalScore += fullScoreOfOneKey;
				experienceAdded += 5;
				liveScoreText.text = totalScore.ToString ();
				pendingJudgements.RemoveAt (0);
				Debug.Log ("score = " + totalScore);
				Debug.Log ("exprience = " + experienceAdded);
				Debug.Log ("perfect performance = " + perfectPerformance);
				Debug.Log ("all combo = " + allCombo);
			} else if (pendingJudgements [0].getDifferTime () <= goodRange) {
				Debug.Log ("good");
				Instantiate (goodHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				totalScore += (int)((float)fullScoreOfOneKey * 0.7f);
				liveScoreText.text = totalScore.ToString ();
				experienceAdded += 3;
				pendingJudgements.RemoveAt (0);
				perfectPerformance = false;
				Debug.Log ("score = " + totalScore);
				Debug.Log ("exprience = " + experienceAdded);
				Debug.Log ("perfect performance = " + perfectPerformance);
				Debug.Log ("all combo = " + allCombo);
			} else if (pendingJudgements [0].getDifferTime () <= badRange) {
				Debug.Log ("bad");
				Instantiate (badHit,pendingJudgements[0].getReferencePoint ().transform.position,Quaternion.identity); 
				Destroy (pendingJudgements [0].getReferencePoint ());
				totalScore += (int)((float)fullScoreOfOneKey * 0.4f);
				liveScoreText.text = totalScore.ToString ();
				experienceAdded += 1;
				pendingJudgements.RemoveAt (0);
				perfectPerformance = false;
				allCombo = false;
				Debug.Log ("score = " + totalScore);
				Debug.Log ("exprience = " + experienceAdded);
				Debug.Log ("perfect performance = " + perfectPerformance);
				Debug.Log ("all combo = " + allCombo);
			} else {
				Debug.Log ("miss");
				Destroy (pendingJudgements [0].getReferencePoint ());
				pendingJudgements.RemoveAt (0);
//				experienceAdded -= 0;
				perfectPerformance = false;
				allCombo = false;
				Debug.Log ("score = " + totalScore);
				Debug.Log ("exprience = " + experienceAdded);
				Debug.Log ("perfect performance = " + perfectPerformance);
				Debug.Log ("all combo = " + allCombo);
			}
		}
	}

}
