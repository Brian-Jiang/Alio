using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;

public class IntroController : MonoBehaviour {

	public CanvasGroup faderCanvasGroup;
	public float fadeDuration = 1f;
	public float introSceneWait = 2f;

	private bool canGoToNextScene = false;
	private SceneController sceneController;

//	Gyroscope gyroscope;

	private IEnumerator Start(){

//		gyroscope = Input.gyro;
//		gyroscope.enabled = true;

		sceneController = (SceneController) FindObjectOfType (typeof(SceneController));
//		Debug.Log (sceneController);
		yield return new WaitForSeconds (introSceneWait);
		StartCoroutine (Fade (1f));
		canGoToNextScene = true;

		yield return new WaitForSeconds (10f);
		StartCoroutine (startSignDisappear ());
	}

	private IEnumerator Fade (float finalAlpha)
	{
		faderCanvasGroup.blocksRaycasts = true;

		float fadeSpeed = (float)(Mathf.Abs (faderCanvasGroup.alpha - finalAlpha)) / fadeDuration;

//		Debug.Log (fadeSpeed);

		while (!Mathf.Approximately (faderCanvasGroup.alpha, finalAlpha))
		{
			faderCanvasGroup.alpha = Mathf.MoveTowards (faderCanvasGroup.alpha, finalAlpha,
				fadeSpeed * Time.deltaTime);

			yield return null;
		}

		faderCanvasGroup.blocksRaycasts = false;
	}

	private IEnumerator startSignDisappear(){
		yield return StartCoroutine (Fade (0f));
	}

//	public void goToNextScene(){
//		Debug.Log ("press");
//		if (canGoToNextScene) {
//			Debug.Log ("go to next scene");
//			sceneController.FadeAndLoadScene ("Main");
//		}
//	}

	void Update(){
//		Debug.Log (canGoToNextScene);
//		testGyroscope ();
		if (canGoToNextScene) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
//					Debug.Log ("touch");
//					if (PlayerPrefs.GetInt ("firstTimeStart") == 1) {
//						sceneController.FadeAndLoadScene ("PlotWindow_1");
//					}

					sceneController.FadeAndLoadScene ("Main");
				}
			}
		}
	}

//	public void testGyroscope (){
//		Debug.Log (GyroToUnity (gyroscope.attitude).eulerAngles);
//	}
//
//	private static Quaternion GyroToUnity(Quaternion q)
//	{
//		return new Quaternion(q.x, q.y, -q.z, -q.w);
//	}

}
