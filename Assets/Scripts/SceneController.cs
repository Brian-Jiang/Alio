using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {


	public CanvasGroup faderCanvasGroup;
	public GameObject faderCanvas;
	public float fadeDuration = 2f;
	public string startingSceneName = "Intro";
	public string initialStartingPositionName = "DoorToMarket";

	public MessageController messageController;
	public PlayerDataController dataController;

	private bool isFading;


	void OnEnable(){
//		messageController = (MessageController)FindObjectOfType (typeof(MessageController));
	}

	private IEnumerator Start ()
	{
		faderCanvasGroup.alpha = 1f;

		if (PlayerPrefs.GetInt ("firstTimeStart") == 1) {
			yield return StartCoroutine (LoadSceneAndSetActive ("PlotWindow_1"));
		} else {
			yield return StartCoroutine (LoadSceneAndSetActive (startingSceneName));
		}

		yield return new WaitForSeconds (0.5f);

		StartCoroutine (Fade (0f));

		yield return new WaitForSeconds (1f);

		faderCanvas.SetActive (false);
	}

	public void unlockNewSongMessage(string name,int number){
		dataController.unlockSong (number);
		StartCoroutine (messageController.unlockNewSong (name));
	}

	public void donateMessage(bool succeed){
		StartCoroutine (messageController.donateComplete (succeed));
	}

	public void achievePerfectPerformance(){
		if (PlayerPrefs.GetInt ("perfectPerformances") == 0) {
			dataController.completeAchievement ("First_PerfectPerformance_Achievement");
		}
		PlayerPrefs.SetInt ("perfectPerformances",PlayerPrefs.GetInt ("perfectPerformances") + 1);
	}

	public void achieveAmazingPerformance(){
		if (PlayerPrefs.GetInt ("amazingPerformances") == 0) {
			dataController.completeAchievement ("First_AmazingPerformance_Achievement");
		}
		PlayerPrefs.SetInt ("amazingPerformances",PlayerPrefs.GetInt ("amazingPerformances") + 1);
	}

	public void achieveNoPerformance(){
		if (PlayerPrefs.GetInt ("noPerformances") == 0) {
			dataController.completeAchievement ("First_NoPerformance_Achievement");
		}
		PlayerPrefs.SetInt ("noPerformances",PlayerPrefs.GetInt ("noPerformances") + 1);
	}

	public void FadeAndLoadScene(string sceneName){
		StartCoroutine (FadeAndSwitchScenes (sceneName));
	}


	private IEnumerator FadeAndSwitchScenes (string sceneName)
	{
		if (!isFading) {
//		Debug.Log("fade and switch scene");
			faderCanvas.SetActive (true);

			yield return StartCoroutine (Fade (1f));

			yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);

			yield return StartCoroutine (LoadSceneAndSetActive (sceneName));

			yield return new WaitForSeconds (0.5f);

			yield return StartCoroutine (Fade (0f));

			faderCanvas.SetActive (false);
		}
	}


	private IEnumerator LoadSceneAndSetActive (string sceneName)
	{
		yield return SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);

		Scene newlyLoadedScene = SceneManager.GetSceneAt (SceneManager.sceneCount - 1);
		SceneManager.SetActiveScene (newlyLoadedScene);

	}


	private IEnumerator Fade (float finalAlpha)
	{
//		Debug.Log ("start fade" + Time.time);
		isFading = true;
//		faderCanvasGroup.blocksRaycasts = true;

		float fadeSpeed = Mathf.Abs (faderCanvasGroup.alpha - finalAlpha) / fadeDuration;
		float audioFadeSpeed = Mathf.Abs (AudioListener.volume - (PlayerPrefs.GetFloat ("volume") - finalAlpha)) / fadeDuration;

		while (!Mathf.Approximately (faderCanvasGroup.alpha, finalAlpha))
		{
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
			AudioListener.volume = Mathf.MoveTowards (AudioListener.volume, (PlayerPrefs.GetFloat ("volume") - finalAlpha), audioFadeSpeed * Time.deltaTime);
//			Debug.Log (faderCanvasGroup.alpha);

			yield return null;
		}
//		Debug.Log ("fade end" + Time.time);

		isFading = false;
//		faderCanvasGroup.blocksRaycasts = false;
	}
}
