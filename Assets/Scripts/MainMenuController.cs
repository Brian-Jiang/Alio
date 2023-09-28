using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

#if UNITY_IOS
using UnityEngine.SocialPlatforms.GameCenter;
#endif

public class Difficulty {
	private bool hasVeryEasy;
	private bool hasEasy;
	private bool hasNormal;
	private bool hasHard;
	private bool hasVeryHard;
	private bool hasSpecial;

	public Difficulty(bool veryEasy,bool easy,bool normal,bool hard,bool veryHard,bool special){
		hasVeryEasy = veryEasy;
		hasEasy = easy;
		hasNormal = normal;
		hasHard = hard;
		hasVeryHard = veryHard;
		hasSpecial = special;
	}

	public bool songHasVeryEasy(){
		return hasVeryEasy;
	}

	public bool songHasEasy(){
		return hasEasy;
	}

	public bool songHasNormal(){
		return hasNormal;
	}

	public bool songHasHard(){
		return hasHard;
	}

	public bool songHasVeryHard(){
		return hasVeryHard;
	}

	public bool songHasSpecial(){
		return hasSpecial;
	}
}


public class Song {
	private int code;
	private string title;
	private string subtitle;
	private string sceneName;
	private AudioClip preview;
	private Sprite backgroundImage;
	private Difficulty difficultyOfSong;

	public Song(int songCode, string songTitle, string songSubtitle, AudioClip prev, Sprite background, Difficulty difficulty){
		code = songCode;
		title = songTitle;
		subtitle = songSubtitle;
		preview = prev;
		backgroundImage = background;
		difficultyOfSong = difficulty;
	}

	public string getSongTitle(){
		return title;
	}

	public string getSongSubtitle(){
		return subtitle;
	}

	public int getSongCode(){
		return code;
	}

	public AudioClip getPreview(){
		return preview;
	}

	public Sprite getBackgroundImage(){
		return backgroundImage;
	}

	public Difficulty getDifficulty(){
		return difficultyOfSong;
	}
}



[RequireComponent(typeof(AudioSource))]
public class MainMenuController : MonoBehaviour {

	public CanvasGroup mainMenuCanvasGroup;
	public CanvasGroup difficultyChooseCanvasGroup;
	public CanvasGroup aboutCanvasGroup;
	public CanvasGroup statisticsCanvasGroup;
	public CanvasGroup advanceCanvasGroup;
	public GameObject difficultyChoosePanel;
	public GameObject mainMenuPanel;
	public GameObject aboutPanel;
	public GameObject statisticsPanel;
	public GameObject advancePanel;
	public Text lastSongText;
	public Text nextSongText;
	public Text currentSongTitleText;
	public Text currentSongSubtitleText;
	public Text experience;

	public Text highScoreVeryEasy;
	public Text highScoreEasy;
	public Text highScoreNormal;
	public Text highScoreHard;
	public Text highScoreVeryHard;
	public Text highScoreSpecial;

	public Text totalExperience;

	public Text versionInformation;

	public GameObject veryEasyChooser;
	public GameObject easyChooser;
	public GameObject normalChooser;
	public GameObject hardChooser;
	public GameObject veryHardChooser;
	public GameObject specialChooser;

	public AudioSource audioSource;
	public AudioClip preview_1;
	public AudioClip preview_2;
	public AudioClip preview_3;
	public AudioClip preview_4;

	public Image backgroundImage;
	public Sprite background_1;
	public Sprite background_2;
	public Sprite background_3; 
	public Sprite background_4; 

	public Slider volumeController;
	public Slider calibrator;
	public Text calibrationText;

	private bool difficultyChooseReacting = false;

	private int currentSong;
	private int nextSong;
	private int lastSong;
	private string currentSongTitle;
	private string currentSongSubtitle;
	private string lastSongTitle;
	private string nextSongTitle;
	private List<Song> songsList = new List<Song>();

	private SceneController sceneController;

	// private bool menuIsOpen = false;
	private bool difficultyChooseOpen = false;

	void OnEnable(){
		sceneController = (SceneController) FindObjectOfType (typeof(SceneController));


		if (PlayerPrefs.GetInt ("songUnlock_1")==1) {
			songsList.Add (new Song (1, "Piano Sonata No.1 in G major Hob.XVI, 8 ： I Allegro", "by Franz Joseph Haydn", preview_1, background_1, new Difficulty(false,true,false,false,false,true)));
			Debug.Log ("add 1");
		}
		if (PlayerPrefs.GetInt ("songUnlock_2")==1) {
			songsList.Add (new Song (2, "Notebook for Anna Magdalena Bach: Minuet", "by Johann Sebastian Bach", preview_2, background_2, new Difficulty(true,false,true,false,false,false)));
			Debug.Log ("add 2");
		}
		if (PlayerPrefs.GetInt ("songUnlock_3")==1) {
			songsList.Add (new Song (3, "Un Sospiro", "by Franz Liszt", preview_3, background_3, new Difficulty(true,false,false,false,false,false)));
		}
		if (PlayerPrefs.GetInt ("songUnlock_4")==1) {
			songsList.Add (new Song (4, "Well-Tempered Clavier, Book 1： Prelude and Fugue No. 1 in C major", "by Johann Sebastian Bach", preview_4, background_4, new Difficulty(false,false,false,true,false,false)));
		}

		difficultyChoosePanel.SetActive (false);
		mainMenuPanel.SetActive (false);


		totalExperience.text = PlayerPrefs.GetInt ("totalExperience").ToString ();
		AudioListener.volume = PlayerPrefs.GetFloat ("volume");
		volumeController.value = PlayerPrefs.GetFloat ("volume");
		calibrator.value = PlayerPrefs.GetFloat ("timeAdjust");
		calibrationText.text = PlayerPrefs.GetFloat ("timeAdjust").ToString ();

		versionInformation.text = "v " + Application.version;
	}

	void Start(){
		currentSong = PlayerPrefs.GetInt ("activeSongNumber",1);
		lastSong = currentSong - 1;
		if (lastSong <= 0) {
			lastSong = songsList.Count;
		}

		nextSong = currentSong + 1;
		if (nextSong > songsList.Count) {
			nextSong = 1;
		}

		updateInterfaceInformation ();
	}

	public void onDonateSucceed(){
		sceneController.donateMessage (true);
	}

	public void onDonateFailed(){
		sceneController.donateMessage (false);
	}


	public void changeVolume(){
		AudioListener.volume = volumeController.value;
		PlayerPrefs.SetFloat ("volume", volumeController.value);
	}

	public void calibrate(){
		PlayerPrefs.SetFloat ("timeAdjust", calibrator.value);
		calibrationText.text = PlayerPrefs.GetFloat ("timeAdjust").ToString ();
	}

	public void calibrationResetOnClick(){
		PlayerPrefs.SetFloat ("timeAdjust", 0f);
		calibrator.value = 0f;
		calibrationText.text = PlayerPrefs.GetFloat ("timeAdjust").ToString ();
	}
		
	public void showDifficultyChooseAreaOnClick(){
//		Debug.Log ("reacting " + difficultyChooseReacting);
		if (!difficultyChooseReacting) {
			if (!difficultyChooseOpen) {
				difficultyChooseReacting = true;
//				Debug.Log ("opening panel" + difficultyChooseReacting);
				StartCoroutine (openDifficultyChoosePanel ());
//				Debug.Log ("after open");


			} else {
				difficultyChooseReacting = true;
				StartCoroutine (closeDifficultyChoosePanel ());


			}
		}
	}

	public void reviewButtonOnClick(){
		Application.OpenURL ("itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=1263456188");
	}

	public void facebookButtonOnClick(){
		Application.OpenURL ("https://twitter.com/XiaoJiang_Brian");
	}

	public void menuButtonOnclick(){
		
		mainMenuPanel.SetActive (true);
		StartCoroutine (Fade (mainMenuCanvasGroup, 0.1f, 1f));
	}

	public void backButtonOnClick(){
		StartCoroutine (closeMainMenu ());
	}

	public void backAboutButtonOnClick(){
		StartCoroutine (closeAbout ());
	}

	public void backStatisticsButtonOnClick(){
		StartCoroutine (closeStatistics ());
	}

	public void backAdvanceButtonOnClick(){
		StartCoroutine (closeAdvance ());
	}

	public void backToIntroButtonOnClick(){
		sceneController.FadeAndLoadScene ("Intro");
	}

	private IEnumerator closeMainMenu(){
		yield return StartCoroutine (Fade (mainMenuCanvasGroup, 0.1f, 0f));
		yield return new WaitForSeconds (0.3f);
		mainMenuPanel.SetActive (false);
	}

	private IEnumerator closeAbout(){
		yield return StartCoroutine (Fade (aboutCanvasGroup, 0.1f, 0f));
		yield return new WaitForSeconds (0.3f);
		aboutPanel.SetActive (false);
	}

	private IEnumerator closeStatistics(){
		yield return StartCoroutine (Fade (statisticsCanvasGroup, 0.1f, 0f));
		yield return new WaitForSeconds (0.3f);
		statisticsPanel.SetActive (false);
	}

	private IEnumerator closeAdvance(){
		yield return StartCoroutine (Fade (advanceCanvasGroup, 0.1f, 0f));
		yield return new WaitForSeconds (0.3f);
		advancePanel.SetActive (false);
	}

	private IEnumerator closeDifficultyChoosePanel(){
		yield return StartCoroutine (Fade (difficultyChooseCanvasGroup, 0.1f, 0f));
		yield return new WaitForSeconds (.3f);
		difficultyChoosePanel.SetActive (false);
		difficultyChooseOpen = false;
		difficultyChooseReacting = false;
	}

	private IEnumerator openDifficultyChoosePanel(){
		difficultyChoosePanel.SetActive (true);
		yield return StartCoroutine (Fade (difficultyChooseCanvasGroup, 0.1f, 1f));
		difficultyChooseOpen = true;
//		Debug.Log ("after fade");
		yield return new WaitForSeconds (.3f);
//		Debug.Log ("after wait");
		difficultyChooseReacting = false;
	}

	public void aboutButtonOnClick(){
		aboutPanel.SetActive (true);
		StartCoroutine (Fade (aboutCanvasGroup, 0.1f, 1f));
	}

	public void statisticsButtonOnClick(){
		statisticsPanel.SetActive (true);
		StartCoroutine (Fade (statisticsCanvasGroup, 0.1f, 1f));
	}

	public void advanceButtonOnClick(){
		advancePanel.SetActive (true);
		StartCoroutine (Fade (advanceCanvasGroup, 0.1f, 1f));
	}

	public void leaderboardButtonOnClick(){
		if (Social.localUser.authenticated) {
#if UNITY_IOS
			GameCenterPlatform.ShowLeaderboardUI ("Total_Experience_Leaderboard",TimeScope.AllTime);
#endif
			
		}  
	}

	public void veryEasyChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_VeryEasy");
	}

	public void easyChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_Easy");
	}

	public void normalChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_Normal");
	}

	public void hardChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_Hard");
	}

	public void veryHardChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_VeryHard");
	}

	public void specialChooserOnClick(){
		sceneController.FadeAndLoadScene (currentSong + "_Special");
	}

	public void lastSongButtonOnClick(){
		currentSong--;
		if (currentSong <= 0) {
			currentSong = songsList.Count;
		}

		lastSong = currentSong - 1;
		if (lastSong <= 0) {
			lastSong = songsList.Count;
		}

		nextSong = currentSong + 1;
		if (nextSong > songsList.Count) {
			nextSong = 1;
		}
//		Debug.Log ("lastSongButtonOnClick" + lastSong);
		updateInterfaceInformation ();
	}

	public void nextSongButtonOnClick(){
		currentSong++;
		if (currentSong > songsList.Count) {
			currentSong = 1;
		}

		lastSong = currentSong - 1;
		if (lastSong <= 0) {
			lastSong = songsList.Count;
		}

		nextSong = currentSong + 1;
		if (nextSong > songsList.Count) {
			nextSong = 1;
		}
//		Debug.Log ("nextSongButtonOnClick" + currentSong);
		updateInterfaceInformation ();
	}

	public void tutorialButtonOnClick(){
		sceneController.FadeAndLoadScene ("Tutorial");
	}

	//for debug only
	public void resetButtonOnClick(){
		Debug.Log ("RESET ALL DATA");
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetInt ("firstTimeStart", 1);
		Application.Quit ();
	}

	public void openAchievementButtonOnClick(){
		if (Social.localUser.authenticated) {  
			Social.ShowAchievementsUI();  
		} 
	}


	private void updateInterfaceInformation(){
		currentSongTitle = songsList [currentSong - 1].getSongTitle ();
		currentSongSubtitle = songsList [currentSong - 1].getSongSubtitle ();
		lastSongTitle = songsList [lastSong - 1].getSongTitle ();
		nextSongTitle = songsList [nextSong - 1].getSongTitle ();

		lastSongText.text = lastSongTitle;
		nextSongText.text = nextSongTitle;
		currentSongTitleText.text = currentSongTitle;
		currentSongSubtitleText.text = currentSongSubtitle;

		backgroundImage.sprite = songsList [currentSong - 1].getBackgroundImage ();

		if (songsList[currentSong-1].getDifficulty ().songHasVeryEasy ()) {
			veryEasyChooser.SetActive (true);
		} else {
			veryEasyChooser.SetActive (false);
		}

		if (songsList[currentSong-1].getDifficulty ().songHasEasy ()) {
			easyChooser.SetActive (true);
		} else {
			easyChooser.SetActive (false);
		}

		if (songsList[currentSong-1].getDifficulty ().songHasNormal ()) {
			normalChooser.SetActive (true);
		} else {
			normalChooser.SetActive (false);
		}

		if (songsList[currentSong-1].getDifficulty ().songHasHard ()) {
			hardChooser.SetActive (true);
		} else {
			hardChooser.SetActive (false);
		}

		if (songsList[currentSong-1].getDifficulty ().songHasVeryHard ()) {
			veryHardChooser.SetActive (true);
		} else {
			veryHardChooser.SetActive (false);
		}

		if (songsList[currentSong-1].getDifficulty ().songHasSpecial()) {
			specialChooser.SetActive (true);
		} else {
			specialChooser.SetActive (false);
		}

		PlayerPrefs.SetInt ("activeSongNumber",currentSong);

		Debug.Log (songsList [currentSong - 1].getPreview ());
		audioSource.clip = songsList [currentSong - 1].getPreview ();
		Debug.Log ("get preview");
		audioSource.Play ();
			
		experience.text = "Experience: " + PlayerPrefs.GetInt ("experience_" + currentSong);

		highScoreVeryEasy.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_veryEasy").ToString ();
		highScoreEasy.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_easy").ToString ();
		highScoreNormal.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_normal").ToString ();
		highScoreHard.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_hard").ToString ();
		highScoreVeryHard.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_veryHard").ToString ();
		highScoreSpecial.text = PlayerPrefs.GetInt ("highScore_" + currentSong + "_special").ToString ();
	}

	private IEnumerator Fade (CanvasGroup canvasGroup, float fadeDuration, float finalAlpha)
	{
		float fadeSpeed = Mathf.Abs (canvasGroup.alpha - finalAlpha) / fadeDuration;

		while (!Mathf.Approximately (canvasGroup.alpha, finalAlpha))
		{
			canvasGroup.alpha = Mathf.MoveTowards (canvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

			yield return null;
		}
	}
}