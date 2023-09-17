using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController_4_Hard : MonoBehaviour {

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

	public GameAreaController_4_Hard gameAreaController;

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
	private MessageController messageController;

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
		sceneController.FadeAndLoadScene ("4_Hard");
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

		PlayerPrefs.SetInt ("experience_4", PlayerPrefs.GetInt ("experience_4") + experienceAdded);
		PlayerPrefs.SetInt ("totalExperience", PlayerPrefs.GetInt ("experience_1") + PlayerPrefs.GetInt ("experience_2") + PlayerPrefs.GetInt ("experience_3") + PlayerPrefs.GetInt ("experience_4"));

		if (Social.localUser.authenticated) {  
			Social.ReportScore(PlayerPrefs.GetInt ("totalExperience"), "Total_Experience_Leaderboard", HandleScoreReported);  
		}  


		if (totalScore > PlayerPrefs.GetInt ("highScore_4_hard")) {
			PlayerPrefs.SetInt ("highScore_4_hard", totalScore);
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
		messageController = (MessageController)FindObjectOfType (typeof(MessageController));

		points.Add (new Point(12f,7.73f));
		points.Add (new Point(12f,9.54f));
		points.Add (new Point(11f,11.37f));
		points.Add (new Point(10f,13.08f));
		points.Add (new Point(6f,14.9f));
		points.Add (new Point(10f,15.84f));
		points.Add (new Point(6.5f,16.78f));
		points.Add (new Point(10.5f,17.68f));
		points.Add (new Point(5.8f,18.62f));
		points.Add (new Point(10.8f,19.45f));
		points.Add (new Point(7f,20.37f));
		points.Add (new Point(11f,21.26f));

		points.Add (new Point(6.6f,22.1f));
		points.Add (new Point(7.3f,22.38f));
		points.Add (new Point(10.8f,23.92f));
		points.Add (new Point(11.4f,24.19f));

		slidePoints.Add (new SlidePoint(9.1f,25.72f));
		slidePoints.Add (new SlidePoint(9.3f,25.92f));
		slidePoints.Add (new SlidePoint(9.5f,26.15f));
		slidePoints.Add (new SlidePoint(9.7f,26.38f));
		points.Add (new Point(5f,26.62f));

		slidePoints.Add (new SlidePoint(9.1f,27.56f));
		slidePoints.Add (new SlidePoint(9.3f,27.56f + 0.2f));
		slidePoints.Add (new SlidePoint(9.5f,27.56f + 0.4f));
		slidePoints.Add (new SlidePoint(9.7f,27.56f + 0.6f));
		points.Add (new Point(5f,28.44f));

		slidePoints.Add (new SlidePoint(9.1f,29.31f));
		slidePoints.Add (new SlidePoint(9.3f,29.31f + 0.2f));
		slidePoints.Add (new SlidePoint(9.5f,29.31f + 0.4f));
		slidePoints.Add (new SlidePoint(9.7f,29.31f + 0.6f));
		points.Add (new Point(5f,30.24f));

		slidePoints.Add (new SlidePoint(9.1f,31.14f));
		slidePoints.Add (new SlidePoint(9.3f,31.14f + 0.2f));
		slidePoints.Add (new SlidePoint(9.5f,31.14f + 0.4f));
		slidePoints.Add (new SlidePoint(9.7f,31.14f + 0.6f));
		points.Add (new Point(5f,32.06f));

		points.Add (new Point(5.6f,33.11f));
		points.Add (new Point(11.6f,33.5f));
		points.Add (new Point(5.6f,33.9f));
		points.Add (new Point(11.6f,34.36f));
		points.Add (new Point(5.6f,34.82f));
		points.Add (new Point(11.6f,35.27f));
		points.Add (new Point(5.6f,35.74f));
		points.Add (new Point(11.6f,36.2f));

		points.Add (new Point(5.6f,36.69f));
		points.Add (new Point(11.6f,37.09f));
		points.Add (new Point(5.6f,37.57f));
		points.Add (new Point(11.6f,38.05f));
		points.Add (new Point(5.6f,38.53f));
		points.Add (new Point(11.6f,39f));
		points.Add (new Point(5.6f,39.48f));
		points.Add (new Point(11.6f,39.92f));

		points.Add (new Point(6.2f,40.35f));
		slidePoints.Add (new SlidePoint(6.4f,40.58f));
		slidePoints.Add (new SlidePoint(6.6f,40.79f));
		slidePoints.Add (new SlidePoint(6.8f,41.01f));
		points.Add (new Point(11.6f,41.25f));
		slidePoints.Add (new SlidePoint(11.4f,41.48f));
		slidePoints.Add (new SlidePoint(11.2f,41.71f));
		slidePoints.Add (new SlidePoint(11f,41.95f));

		points.Add (new Point(6.2f,42.14f));
		slidePoints.Add (new SlidePoint(6.4f,42.14f + 0.23f));
		slidePoints.Add (new SlidePoint(6.6f,42.14f + 0.46f));
		slidePoints.Add (new SlidePoint(6.8f,42.14f + 0.69f));
		points.Add (new Point(11.6f,42.14f + 0.92f));
		slidePoints.Add (new SlidePoint(11.4f,42.14f + 1.15f));
		slidePoints.Add (new SlidePoint(11.2f,42.14f + 1.38f));
		slidePoints.Add (new SlidePoint(11f,42.14f + 1.61f));

		points.Add (new Point(6.2f,43.97f));
		slidePoints.Add (new SlidePoint(6.4f,43.97f + 0.23f));
		slidePoints.Add (new SlidePoint(6.6f,43.97f + 0.46f));
		slidePoints.Add (new SlidePoint(6.8f,43.97f + 0.69f));
		points.Add (new Point(9.8f,43.97f + 0.92f));
//		slidePoints.Add (new SlidePoint(11.4f,43.97f + 1.15f));
//		slidePoints.Add (new SlidePoint(11.2f,43.97f + 1.38f));
//		slidePoints.Add (new SlidePoint(11f,43.97f + 1.61f));

		points.Add (new Point(6.2f,45.8f));
		slidePoints.Add (new SlidePoint(6.4f,45.8f + 0.23f));
		slidePoints.Add (new SlidePoint(6.6f,45.8f + 0.46f));
		slidePoints.Add (new SlidePoint(6.8f,45.8f + 0.69f));
		centerPoints.Add (new CenterPoint(90f,45.8f + 0.92f));
//		slidePoints.Add (new SlidePoint(11.4f,45.8f + 1.15f));
//		slidePoints.Add (new SlidePoint(11.2f,45.8f + 1.38f));
//		slidePoints.Add (new SlidePoint(11f,45.8f + 1.61f));

		points.Add (new Point(6.2f,47.6f));
		slidePoints.Add (new SlidePoint(6.4f,47.6f + 0.23f));
		slidePoints.Add (new SlidePoint(6.6f,47.6f + 0.46f));
		slidePoints.Add (new SlidePoint(6.8f,47.6f + 0.69f));
		centerPoints.Add (new CenterPoint(90f,47.6f + 0.92f));

		points.Add (new Point(5.8f,49.45f));
		slidePoints.Add (new SlidePoint(6f,49.45f + 0.23f));
		slidePoints.Add (new SlidePoint(6.2f,49.45f + 0.46f));
		slidePoints.Add (new SlidePoint(6.4f,49.45f + 0.69f));
		centerPoints.Add (new CenterPoint(70f,49.45f + 0.92f));

		centerPoints.Add (new CenterPoint(70f,51.25f));
		centerPoints.Add (new CenterPoint(60f,52.21f));
		centerPoints.Add (new CenterPoint(50f,53.14f));
		centerPoints.Add (new CenterPoint(40f,54.01f));
		points.Add (new Point(-4.8f,54.95f));
		centerPoints.Add (new CenterPoint(30f,55.84f));
		points.Add (new Point(-5f,56.79f));
		centerPoints.Add (new CenterPoint(25f,57.65f));
		points.Add (new Point(-5.4f,58.57f));
		centerPoints.Add (new CenterPoint(20f,59.54f));
		points.Add (new Point(-5.8f,60.54f));
		centerPoints.Add (new CenterPoint(15f,61.43f));
		points.Add (new Point(-6.5f,62.36f));
		centerPoints.Add (new CenterPoint(10f,63.26f));
		points.Add (new Point(-7.5f,64.2f));
		centerPoints.Add (new CenterPoint(5f,65.07f));

		points.Add (new Point(7.5f,66.13f));
		points.Add (new Point(7.8f,66.34f));
		points.Add (new Point(8.2f,66.53f));
		points.Add (new Point(8.5f,66.72f));
		points.Add (new Point(-7f,66.95f));

		points.Add (new Point(7f,67.93f));
		points.Add (new Point(7.3f,67.93f + 0.2f));
		points.Add (new Point(7.7f,67.93f + 0.4f));
		points.Add (new Point(8f,67.93f + 0.6f));
		points.Add (new Point(-6.5f,67.93f + 0.8f));

		points.Add (new Point(6.3f,69.76f));
		points.Add (new Point(6.6f,69.76f + 0.2f));
		points.Add (new Point(6.9f,69.76f + 0.4f));
		points.Add (new Point(7.2f,69.76f + 0.6f));
		points.Add (new Point(-5.5f,69.76f + 0.8f));

		points.Add (new Point(6.3f,71.63f));
		points.Add (new Point(6.5f,71.84f));
		points.Add (new Point(6.7f,72.07f));
		points.Add (new Point(6.9f,72.3f));
		points.Add (new Point(-6.3f,72.49f));
		points.Add (new Point(-6.5f,72.75f));
		points.Add (new Point(-6.7f,73f));
		points.Add (new Point(-6.9f,73.19f));

		points.Add (new Point(5.2f,73.38f));
		points.Add (new Point(5.4f,73.38f + 0.23f));
		points.Add (new Point(5.6f,73.38f + 0.46f));
		points.Add (new Point(5.8f,73.38f + 0.69f));
		points.Add (new Point(-5.2f,74.31f));
		points.Add (new Point(-5.4f,74.31f + 0.23f));
		points.Add (new Point(-5.6f,74.31f + 0.46f));
		points.Add (new Point(-5.8f,74.31f + 0.69f));

		points.Add (new Point(5.2f,75.28f));
		points.Add (new Point(5.4f,75.28f + 0.23f));
		points.Add (new Point(5.6f,75.28f + 0.46f));
		points.Add (new Point(5.8f,75.28f + 0.69f));
		points.Add (new Point(-5.2f,76.17f));
		points.Add (new Point(-5.4f,76.17f + 0.23f));
		points.Add (new Point(-5.6f,76.17f + 0.46f));
		points.Add (new Point(-5.8f,76.17f + 0.69f));

		points.Add (new Point(-6.2f,77.12f));
		points.Add (new Point(-6.6f,77.12f + 0.23f));
		points.Add (new Point(-7f,77.12f + 0.46f));
		points.Add (new Point(-7.4f,77.12f + 0.69f));
		points.Add (new Point(-5.2f,78.04f));
		points.Add (new Point(-4.8f,78.04f + 0.23f));
		points.Add (new Point(-4.4f,78.04f + 0.46f));
		points.Add (new Point(-4f,78.04f + 0.69f));

		points.Add (new Point(-6.2f,78.98f));
		points.Add (new Point(-6.6f,78.98f + 0.23f));
		points.Add (new Point(-7f,78.98f + 0.46f));
		points.Add (new Point(-7.4f,78.98f + 0.69f));
		points.Add (new Point(-5.2f,79.88f));
		points.Add (new Point(-4.8f,79.88f + 0.23f));
		points.Add (new Point(-4.4f,79.88f + 0.46f));
		points.Add (new Point(-4f,79.88f + 0.69f));

		points.Add (new Point(-6.2f,80.83f));
		points.Add (new Point(-10.2f,81.34f));
		points.Add (new Point(-7f,81.8f));
		points.Add (new Point(-6f,82.27f));
		centerPoints.Add (new CenterPoint(140f,82.74f));
		points.Add (new Point(-8.3f,83.2f));
		points.Add (new Point(-5.4f,83.68f));
		centerPoints.Add (new CenterPoint(145f,84.17f));

		points.Add (new Point(-7.9f,84.66f));
		points.Add (new Point(-11f,85.16f));
		centerPoints.Add (new CenterPoint(165f,85.67f));
		points.Add (new Point(-6.3f,86.12f));
		points.Add (new Point(-7.5f,86.6f));
		points.Add (new Point(-8.9f,87.06f));
		centerPoints.Add (new CenterPoint(170f,87.52f));
		points.Add (new Point(-10.3f,87.99f));

		points.Add (new Point(-10.3f,88.45f));
		centerPoints.Add (new CenterPoint(180f,88.91f));
		points.Add (new Point(-10.3f,89.35f));
		centerPoints.Add (new CenterPoint(170f,89.83f));
		points.Add (new Point(-10.3f,90.31f));
		points.Add (new Point(-10.3f,90.77f));
		points.Add (new Point(-10.3f,91.23f));
		centerPoints.Add (new CenterPoint(190f,91.69f));

		points.Add (new Point(-6.3f,92.16f));
		points.Add (new Point(-6.7f,92.36f));
		points.Add (new Point(-10.3f,93.06f));
		points.Add (new Point(-6.3f,94.02f));
		points.Add (new Point(-6.7f,94.26f));
		points.Add (new Point(-10.3f,94.94f));

		points.Add (new Point(-6.3f,95.92f));
		points.Add (new Point(-6.7f,96.16f));
		points.Add (new Point(-10.3f,96.81f));
		points.Add (new Point(-6.3f,97.79f));
		points.Add (new Point(-6.7f,98.02f));
		points.Add (new Point(-10.3f,98.68f));

		points.Add (new Point(-11.3f,99.62f));
		points.Add (new Point(-6.3f,99.62f));
		points.Add (new Point(-10.3f,100.59f));
		points.Add (new Point(-5.3f,100.59f));
		points.Add (new Point(-11.3f,101.57f));
		points.Add (new Point(-6.3f,101.57f));
		points.Add (new Point(-10.3f,102.52f));
		points.Add (new Point(-5.3f,102.52f));

		points.Add (new Point(-11.3f,103.4f));
		points.Add (new Point(-5.3f,103.4f));
		points.Add (new Point(-9.3f,104.32f));
		points.Add (new Point(-5.3f,104.32f));
		points.Add (new Point(-11.3f,105.23f));
		points.Add (new Point(-6.3f,105.23f));
		points.Add (new Point(-10.3f,106.18f));
		points.Add (new Point(-5.3f,106.18f));

		points.Add (new Point(-10.3f,107.12f));
		points.Add (new Point(-5.3f,107.12f));
		points.Add (new Point(-10.3f,108.07f));
		points.Add (new Point(-5.3f,108.07f));
		points.Add (new Point(-11.3f,109.03f));
		points.Add (new Point(-6.3f,109.03f));
		points.Add (new Point(-11.3f,110.01f));
		points.Add (new Point(-5.3f,110.01f));

		points.Add (new Point(-11.3f,110.95f));
		points.Add (new Point(-6.3f,110.95f));
		points.Add (new Point(-11.3f,111.88f));
		points.Add (new Point(-5.3f,111.88f));
		points.Add (new Point(-10.3f,112.74f));
		points.Add (new Point(-5.3f,112.74f));
		points.Add (new Point(-10.3f,113.77f));
		points.Add (new Point(-5.3f,113.77f));

		points.Add (new Point(-5.3f,114.72f));
		points.Add (new Point(-10.3f,115.66f));
		points.Add (new Point(-6.3f,116.66f));
		points.Add (new Point(-8.3f,117.59f));

		points.Add (new Point(-8.3f,118.52f));
//		points.Add (new Point(8.3f,118.52f));
		points.Add (new Point(-8.3f,122.33f));
		points.Add (new Point(4.3f,122.33f));
		points.Add (new Point(-8.3f,127.63f));
		points.Add (new Point(8.3f,127.63f));




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

//		sceneController.unlockNewSongMessage ("jweiuabfash fhebbvusgv");
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
