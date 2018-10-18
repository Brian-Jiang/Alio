using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameController_1_Special : MonoBehaviour {

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
	public CanvasGroup alioCanvaseGroup;

	public CanvasGroup pauseButtonCanvasGroup;
	public CanvasGroup scoreBackgroundCanvasGroup;
	public CanvasGroup scoreTextCanvasGroup;

	public Animator lineAnimator;
	public Animator cameraAnimator;
	public Animator gameAreaAnimator;
	public Animator plateAnimator;

	public GameObject plate;
	public GameObject gameArea;

	public GameObject endRetryButton;
	public GameObject pauseRetryButton;
	public GameObject quitButton;

	public GameObject gameStartSign;
	public GameObject pauseMenu;
	public GameObject endCanvas;
	public GameObject newHighScore;
	public GameObject alioCanvase;

	public GameAreaController_1_Special gameAreaController;

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
		sceneController.FadeAndLoadScene ("1_Special");
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
		experienceAdded += 10;

		if (experienceAdded < 0) {
			experienceAdded = 0;
		}


		if (perfectPerformance) {
			resultLevelText.text = "Perfect Performance";
			totalScore = fullScore;
			experienceAdded += 900;
			totalScoreText.text = totalScore.ToString ();
			resultLevelText.font = perfectPerformanceFont;
			resultLevelText.fontSize = 265;
			sceneController.achievePerfectPerformance ();
		} else if (allCombo) {
			resultLevelText.text = "Amazing Performance";
			experienceAdded += 75;
			resultLevelText.font = amazingPerformanceFont;
			resultLevelText.fontSize = 190;
			sceneController.achieveAmazingPerformance ();
		} else if (totalScore >= (fullScore * 0.9f)) {
			resultLevelText.text = "Great Performance";
			experienceAdded += 50;
			resultLevelText.font = greatPerformanceFont;
		} else if (totalScore >= (fullScore * 0.7f)) {
			resultLevelText.text = "Good Performance";
			experienceAdded += 25;
			resultLevelText.font = goodPerformanceFont;
		} else if (totalScore >= (fullScore * 0.6f)) {
			resultLevelText.text = "Mediocre Performance";
			experienceAdded += 10;
			resultLevelText.font = mediocrePerformanceFont;
		} else if (totalScore != 0) {
			resultLevelText.text = "Bad Performance";
			experienceAdded += 3;
			resultLevelText.font = badPerformanceFont;
		} else {
			resultLevelText.text = "No Performance?";
			experienceAdded += 1;
			sceneController.achieveNoPerformance ();
		}


		experienceAddedText.text = "Experience +" + experienceAdded;

		PlayerPrefs.SetInt ("experience_1", PlayerPrefs.GetInt ("experience_1") + experienceAdded);
		PlayerPrefs.SetInt ("totalExperience", PlayerPrefs.GetInt ("experience_1") + PlayerPrefs.GetInt ("experience_2") + PlayerPrefs.GetInt ("experience_3") + PlayerPrefs.GetInt ("experience_4"));

		if (Social.localUser.authenticated) {  
			Social.ReportScore(PlayerPrefs.GetInt ("totalExperience"), "Total_Experience_Leaderboard", HandleScoreReported);  
		}  

		if (totalScore > PlayerPrefs.GetInt ("highScore_1_special")) {
			PlayerPrefs.SetInt ("highScore_1_special", totalScore);
			newHighScore.SetActive (true);
		}
		PlayerPrefs.SetInt ("firstTimeStart", 0);

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


		points.Add (new Point(7f,2.53f));
		points.Add (new Point(7f,4.36f));
		points.Add (new Point(7f,5.84f));
		points.Add (new Point(-7f,7.33f));
		points.Add (new Point(-7f,8.4f));
		points.Add (new Point(-7f,9.84f));
		points.Add (new Point(-7f,11.4f));
		centerPoints.Add (new CenterPoint(0f, 13.64f));
		centerPoints.Add (new CenterPoint(0f, 15.13f));
		centerPoints.Add (new CenterPoint(0f, 18.16f));
		centerPoints.Add (new CenterPoint(0f, 19.66f));
		points.Add (new Point(-7f,23.032f));

		centerPoints.Add (new CenterPoint(-90f, 26.55f));
		centerPoints.Add (new CenterPoint(-90f, 28.09f));
		points.Add (new Point(7f,29.7f));
		points.Add (new Point(7f,31.3f));
		centerPoints.Add (new CenterPoint(-90f, 32.83f));
		centerPoints.Add (new CenterPoint(-90f, 33.59f));
		points.Add (new Point(-7f,35.09f));
		slidePoints.Add (new SlidePoint(-7.3f,35.891f));
		slidePoints.Add (new SlidePoint(-7.6f,36.057f));
		slidePoints.Add (new SlidePoint(-7.3f,36.24f));
		slidePoints.Add (new SlidePoint(-6.9f,36.432f));
		centerPoints.Add (new CenterPoint(-90f, 38.19f));
		centerPoints.Add (new CenterPoint(-90f, 39.73f));
		points.Add (new Point(7f,41.22f));
		centerPoints.Add (new CenterPoint(-90f, 42.73f));
		centerPoints.Add (new CenterPoint(-90f, 44.22f));

		centerPoints.Add (new CenterPoint(180f, 48.34f));
		points.Add (new Point(-7f,50.89f));
		points.Add (new Point(-7f,53.24f));
		centerPoints.Add (new CenterPoint(180f, 55.54f));
		points.Add (new Point(7f,57.48f));
		centerPoints.Add (new CenterPoint(180f, 58.66f));
		points.Add (new Point(-7f,60.56f));
		points.Add (new Point(-7f,62.49f));
		points.Add (new Point(-7f,63.27f));

		centerPoints.Add (new CenterPoint(90f, 66.89f));
		points.Add (new Point(7f,68.81f));
		points.Add (new Point(7f,70.39f));
		points.Add (new Point(7f,71.98f));
		slidePoints.Add (new SlidePoint(7.3f,73.14f));
		slidePoints.Add (new SlidePoint(7f,73.36f));
		slidePoints.Add (new SlidePoint(7.3f,73.52f));
		slidePoints.Add (new SlidePoint(7.6f,73.71f));
		slidePoints.Add (new SlidePoint(7.9f,73.89f));
		slidePoints.Add (new SlidePoint(7.6f,74.075f));
		slidePoints.Add (new SlidePoint(7.3f,74.25f));
		slidePoints.Add (new SlidePoint(7f,74.44f));
		centerPoints.Add (new CenterPoint(90f, 76.22f));
		points.Add (new Point(-7f,78.58f));
		points.Add (new Point(-7f,80.07f));
		centerPoints.Add (new CenterPoint(90f, 81.57f));
		centerPoints.Add (new CenterPoint(90f, 83.06f));
		centerPoints.Add (new CenterPoint(90f, 84.57f));
		points.Add (new Point(7f,86.024f));

		centerPoints.Add (new CenterPoint(90f, 87.28f));
//		centerPoints.Add (new CenterPoint(90f, 87.64f));
//		centerPoints.Add (new CenterPoint(90f, 88.03f));
//		centerPoints.Add (new CenterPoint(90f, 88.44f));
		centerPoints.Add (new CenterPoint(90f, 88.84f));
//		centerPoints.Add (new CenterPoint(90f, 89.614f));
		centerPoints.Add (new CenterPoint(90f, 90.407f));
//		centerPoints.Add (new CenterPoint(90f, 90.788f));
//		centerPoints.Add (new CenterPoint(90f, 91.184f));
		centerPoints.Add (new CenterPoint(90f, 91.99f));
		centerPoints.Add (new CenterPoint(90f, 92.736f));



		fullScoreOfOneKey = fullScore / (points.Count + centerPoints.Count + slidePoints.Count);
		Debug.Log (fullScoreOfOneKey);

		timeAdjust = PlayerPrefs.GetFloat ("timeAdjust");

		song = GetComponent<AudioSource> ();

		if (PlayerPrefs.GetInt ("firstTimeStart") == 1) {
			endRetryButton.SetActive (false);
			pauseRetryButton.SetActive (false);
			quitButton.SetActive (false);
		}

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
					if (pointTouches.Count > 0 && dynamicPoints.Count > 0 && pointTouches [i - 1].getTouchTime () > (dynamicPoints [j].getClickTime () - 0.8f) && pointTouches [i - 1].getTouchTime () < dynamicPoints [j].getDisappearTime () && Vector2.Distance (new Vector2 (pointTouches [i - 1].getTouchPosition ().x - gameArea.transform.position.x, pointTouches [i - 1].getTouchPosition ().y - gameArea.transform.position.y), new Vector2 (dynamicPoints [j].getX (), dynamicPoints [j].getY ())) <= dynamicPoints [j].getJudgeRange ()) {
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
		yield return new WaitForSeconds (95f);
		canPauseGame = false;
		StartCoroutine (Fade (pauseButtonCanvasGroup,0.5f,0f));
		StartCoroutine (Fade (scoreTextCanvasGroup,0.5f,0f));
		StartCoroutine (Fade (scoreBackgroundCanvasGroup,0.5f,0f));
		yield return new WaitForSeconds (3f);
		alioCanvase.SetActive (true);
		StartCoroutine (Fade (alioCanvaseGroup,1f,1f)); 
		yield return new WaitForSeconds (7f);
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
				experienceAdded += 2;
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
