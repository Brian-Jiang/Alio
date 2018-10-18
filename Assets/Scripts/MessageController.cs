using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour {
	public CanvasGroup messageFaderCanvasGroup;
	public CanvasGroup newSongNotificationPanelCanvasGroup;
	public CanvasGroup donateNotificationPanelCanvasGroup;

	public Text newSongName;
	public Text donateResultText;

	public GameObject newSongNotificationPanel;
	public GameObject donateNotificationPanel;
	public GameObject messageCanvas;

	private bool canCloseNotification = false;

//	public RectTransform newSongNotificationPanelRectTransform;


	public void openNewSongNotificationPanel(){
		messageCanvas.SetActive (true);
		StartCoroutine (Fade (messageFaderCanvasGroup,0.3f,1f));
		StartCoroutine (Fade (newSongNotificationPanelCanvasGroup,0.3f,1f));
	}

	public IEnumerator closeNewSongNotificationPanel(){
//		Debug.Log ("in close");
		StartCoroutine (Fade (messageFaderCanvasGroup,0.3f,0f));
		StartCoroutine (Fade (newSongNotificationPanelCanvasGroup,0.3f,0f));
		yield return new WaitForSeconds (0.4f);
		messageCanvas.SetActive (false);
	}

	public void openDonateNotificationPanel(){
		messageCanvas.SetActive (true);
		StartCoroutine (Fade (messageFaderCanvasGroup,0.3f,1f));
		StartCoroutine (Fade (donateNotificationPanelCanvasGroup,0.3f,1f));
	}

	public IEnumerator closeDonateNotificationPanel(){
		StartCoroutine (Fade (messageFaderCanvasGroup,0.3f,0f));
		StartCoroutine (Fade (donateNotificationPanelCanvasGroup,0.3f,0f));
		yield return new WaitForSeconds (0.4f);
		messageCanvas.SetActive (false);
	}

	public void faderAreaOnClick(){
		if (canCloseNotification) {
//			Debug.Log ("on click");
			canCloseNotification = false;
			StartCoroutine (closeNewSongNotificationPanel ());
			StartCoroutine (closeDonateNotificationPanel ());
		}
	}

	public IEnumerator unlockNewSong(string songName){
//		Debug.Log ("unlocknewsong");
		newSongName.text = songName;
		openNewSongNotificationPanel ();
		yield return new WaitForSeconds (1f);
		canCloseNotification = true;
	}

	public IEnumerator donateComplete(bool succeed){
		if (succeed) {
			donateResultText.text = "Thank you!";
		} else {
			donateResultText.text = "Failed";
		}
		openDonateNotificationPanel ();
		yield return new WaitForSeconds (1f);
		canCloseNotification = true;
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

//	private IEnumerator MoveY (RectTransform rectTransform, float moveDuration, Vector2 finalPosition){
//		float moveSpeed = Mathf.Abs (rectTransform.anchoredPosition.y - finalPosition.y) / moveDuration;
//
//		while (!Mathf.Approximately (rectTransform.anchoredPosition.y, finalPosition.y))
//		{
//			rectTransform.anchoredPosition.y = Mathf.MoveTowards (rectTransform.anchoredPosition.y, finalPosition.y, moveSpeed * Time.deltaTime);
//
//			yield return null;
//		}
//	}


}
