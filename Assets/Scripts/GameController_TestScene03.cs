using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameController_TestScene03 : MonoBehaviour {

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
	public GameObject gameArea;

	public GameObject gameStartSign;
	public GameObject pauseMenu;
	public GameObject endCanvas;
	public GameObject newHighScore;

	public GameAreaController gameAreaController;

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
		sceneController.FadeAndLoadScene ("3_Hard");
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
		experienceAdded += 15;

		if (experienceAdded < 0) {
			experienceAdded = 0;
		}


		if (perfectPerformance) {
			resultLevelText.text = "Perfect Performance";
			totalScore = fullScore;
			experienceAdded += 5000;
			totalScoreText.text = totalScore.ToString ();
			resultLevelText.font = perfectPerformanceFont;
			resultLevelText.fontSize = 265;
			sceneController.achievePerfectPerformance ();
		} else if (allCombo) {
			resultLevelText.text = "Amazing Performance";
			experienceAdded += 400;
			resultLevelText.font = amazingPerformanceFont;
			resultLevelText.fontSize = 190;
			sceneController.achieveAmazingPerformance ();
		} else if (totalScore >= (fullScore * 0.9f)) {
			resultLevelText.text = "Great Performance";
			experienceAdded += 130;
			resultLevelText.font = greatPerformanceFont;
		} else if (totalScore >= (fullScore * 0.7f)) {
			resultLevelText.text = "Good Performance";
			experienceAdded += 70;
			resultLevelText.font = goodPerformanceFont;
		} else if (totalScore >= (fullScore * 0.6f)) {
			resultLevelText.text = "Mediocre Performance";
			experienceAdded += 30;
			resultLevelText.font = mediocrePerformanceFont;
		} else if (totalScore != 0) {
			resultLevelText.text = "Bad Performance";
			experienceAdded += 20;
			resultLevelText.font = badPerformanceFont;
		} else {
			resultLevelText.text = "No Performance?";
			experienceAdded += 1;
			sceneController.achieveNoPerformance ();
		}


		experienceAddedText.text = "Experience +" + experienceAdded;

		PlayerPrefs.SetInt ("experience_3", PlayerPrefs.GetInt ("experience_3") + experienceAdded);
		PlayerPrefs.SetInt ("totalExperience", PlayerPrefs.GetInt ("experience_1") + PlayerPrefs.GetInt ("experience_2") + PlayerPrefs.GetInt ("experience_3"));

		if (Social.localUser.authenticated) {  
			Social.ReportScore(PlayerPrefs.GetInt ("totalExperience"), "Total_Experience_Leaderboard", HandleScoreReported);  
		}  


		if (totalScore > PlayerPrefs.GetInt ("highScore_3_hard")) {
			PlayerPrefs.SetInt ("highScore_3_hard", totalScore);
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

		points.Add (new Point(4f, 2.3f));
		points.Add (new Point(6f, 3.69f));
		points.Add (new Point(8f, 5.1f));
		points.Add (new Point(7f, 6.45f));
		points.Add (new Point(4f, 7.84f));
		points.Add (new Point(6f, 9.28f));
		points.Add (new Point(8f, 10.62f));
		centerPoints.Add (new CenterPoint (90f, 11.94f));
		points.Add (new Point(8f, 13.33f));
		centerPoints.Add (new CenterPoint (90f, 14.71f));
		points.Add (new Point (-4f, 16.1f));
		centerPoints.Add (new CenterPoint(90f, 17.45f));
		points.Add (new Point(8f, 18.83f));
		centerPoints.Add (new CenterPoint(90f, 20.21f));
		points.Add (new Point(-4f, 21.55f));

		points.Add (new Point(7f, 23.08f));
		points.Add (new Point(4f, 24.44f));
		points.Add (new Point(6f, 25.85f));
		points.Add (new Point(8f, 27.17f));
		points.Add (new Point(7f, 28.53f));
		points.Add (new Point(4f, 29.94f));
		points.Add (new Point(6f, 31.36f));
		points.Add (new Point(8f, 32.755f));

		points.Add (new Point(8f, 34.05f));
		points.Add (new Point(-5f, 34.05f));
		slidePoints.Add (new SlidePoint(7.6f,34.716f));
		slidePoints.Add (new SlidePoint(7f,34.878f));
		slidePoints.Add (new SlidePoint(6.4f,35.065f));
		centerPoints.Add (new CenterPoint(45f, 35.4f));
		points.Add (new Point(8f, 36.75f));
		points.Add (new Point(-5f, 36.75f));
		slidePoints.Add (new SlidePoint(7f,37.477f));
		slidePoints.Add (new SlidePoint(6.4f,37.644f));
		slidePoints.Add (new SlidePoint(5.8f,37.802f));
		centerPoints.Add (new CenterPoint(90f, 38.18f));
		slidePoints.Add (new SlidePoint(9f,38.505f));
		slidePoints.Add (new SlidePoint(9.2f,38.848f));
		points.Add (new Point(8f, 39.56f));
		points.Add (new Point(- 7f, 39.56f));
		centerPoints.Add (new CenterPoint(45f, 40.9f));
		points.Add (new Point(8f, 42.27f));
		points.Add (new Point(- 8f, 42.27f));
		centerPoints.Add (new CenterPoint(-45f, 43.69f));

		centerPoints.Add (new CenterPoint(0f, 45.06f));
		points.Add (new Point(8f, 46.44f));
		centerPoints.Add (new CenterPoint(0f, 47.82f));
		points.Add (new Point(8f, 49.22f));
		centerPoints.Add (new CenterPoint(0f, 50.67f));
		points.Add (new Point(8f, 52.01f));
		centerPoints.Add (new CenterPoint(0f, 53.31f));
		points.Add (new Point(8f, 54.75f));

		points.Add (new Point(11f, 56.1f));
		points.Add (new Point(11f, 193f, 57.447f, 1.3f));
		points.Add (new Point(11f, 58.83f));
		points.Add (new Point(11f, 60.25f));
		points.Add (new Point(11f, 61.6f));
		points.Add (new Point(11f, 62.95f));
		points.Add (new Point(11f, 64.32f));
		points.Add (new Point(11f, 65.74f));
		points.Add (new Point(11f, 67.12f));
		points.Add (new Point(11f, 68.51f));
		points.Add (new Point(11f, 69.85f));
		points.Add (new Point(11f, 71.25f));
		points.Add (new Point(11f, 72.67f));
		points.Add (new Point(11f, 74.02f));
		points.Add (new Point(11f, 75.33f));
		points.Add (new Point(11f, 76.76f));

		centerPoints.Add (new CenterPoint(90f, 78.17f));
		points.Add (new Point(5f, 79.56f));
		points.Add (new Point(5f, 80.9f));
		centerPoints.Add (new CenterPoint(0f, 82.29f));
		centerPoints.Add (new CenterPoint(45f, 83.69f));
		points.Add (new Point(-9f, 85.1f));
		points.Add (new Point(8f, 86.43f));
		points.Add (new Point(-6f, 86.43f));
		centerPoints.Add (new CenterPoint(0f, 87.8f));

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

//			if (Mathf.Abs (timeSinceSongStarted - 0.5f) < 0.05f) {
//				Debug.Log ("time: " + timeSinceSongStarted + "   plate angle: " + PlateController.rotationZ);
//			}

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
							if (Mathf.Abs (dynamicCenterPoints [i].getClickTime () - timeSinceSongStarted) < 1.2f) {
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
					if (dynamicPoints[j].IsFixed () && pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > (dynamicPoints [j].getClickTime () - 0.8f) && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x, pointTouches [i - 1].getTouchPosition ().y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
						//						Debug.Log ("in if");
						pendingJudgements.Add (new Judgement (pointTouches [i - 1].getTouchTime (), dynamicPoints [j].getClickTime (), dynamicPoints [j]));
						pointTouches.RemoveAt (i - 1);
						dynamicPoints.RemoveAt (j);
						j--;
						break;
					} else if (pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > (dynamicPoints [j].getClickTime () - 0.8f) && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x - gameArea.transform.position.x, pointTouches [i - 1].getTouchPosition ().y - gameArea.transform.position.y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
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
				experienceAdded += 3;
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
				experienceAdded += 1;
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
				experienceAdded -= 1;
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
				experienceAdded -= 3;
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
