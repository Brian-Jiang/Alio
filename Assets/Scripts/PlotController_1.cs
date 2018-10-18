using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Analytics;


//public abstract class Plot {
//
//	public abstract int returnPlotType ();
//	//0=line
//	//1=choose
//}
//
//public class DialogueChoose : Plot {
//	private string firstChoice;
//	private string secondChoice;
//	private string thirdChoice;
//
//	public DialogueChoose (string first, string second, string third){
//		firstChoice = first;
//		secondChoice = second;
//		thirdChoice = third;
//	}
//
//	public override int returnPlotType ()
//	{
//		return 1;
//	}
//
//
//}
//
//public class DialogueLine : Plot{
//	private string dialogueLineTextChoice1;
//	private string dialogueLineTextChoice2;
//	private string dialogueLineTextChoice3;
//
//	public DialogueLine(string text1, string text2, string text3){
//		dialogueLineTextChoice1 = text1;
//		dialogueLineTextChoice2 = text2;
//		dialogueLineTextChoice3 = text3;
//	}
//
//	public override int returnPlotType ()
//	{
//		return 0;
//	}
//
//	public string getText1(){
//		return dialogueLineTextChoice1;
//	}
//
//	public string getText2(){
//		return dialogueLineTextChoice2;
//	}
//
//	public string getText3(){
//		return dialogueLineTextChoice3;
//	}
//}


public class PlotController_1 : MonoBehaviour {
	private SceneController sceneController;

	public GameObject faderPanel;
	public GameObject choosePanel;

	public Text plotText;

	public string line1;
	public string line2;
	public string line3;

//	public int choiceNumber;

	private List<string> lines = new List<string>();

	void OnEnable(){
		sceneController = (SceneController) FindObjectOfType (typeof(SceneController));
	}

	void Start(){
		lines.Add (line1);
		lines.Add (line2);
		lines.Add (line3);
		lines.Add ("*choice");
	}

	public void showNextPlot(){
		if (lines [0].CompareTo ("*choice") == 0) {
			Debug.Log ("choice");
			faderPanel.SetActive (true);
			choosePanel.SetActive (true);
		} else if (lines [0].CompareTo ("*end") == 0) {
			PlayerPrefs.SetInt ("plotProgress", 1);
//			if (Social.localUser.authenticated) {  
//				Social.ReportProgress ("70490135", 100.0, HandleProgressReported);
//			}
			sceneController.FadeAndLoadScene ("Tutorial");
		} else {
			plotText.text = lines [0];
			lines.RemoveAt (0);
		}
	}

	private void HandleProgressReported(bool success)  
	{  
		Debug.Log("*** HandleProgressReported: success = " + success);  
	}  

	public void choiceOneOnClick(){
		faderPanel.SetActive (false);
		choosePanel.SetActive (false);
//		choiceNumber = 1;
		Analytics.CustomEvent ("plot 1",new Dictionary<string,object> {
			{"choice",1}
		});
		PlayerPrefs.SetInt ("plotChoice0_1", 1);
		Debug.Log ("one");
		lines.Add ("Great!");
		lines.Add ("I have some trouble now...Can you help me to perform?");
		lines.Add ("Don't worry, it's easy. I'll teach you first.");
		lines.Add ("*end");
		lines.RemoveAt (0);
		plotText.text = lines [0];
		lines.RemoveAt (0);
	}

	public void choiceTwoOnClick(){
		faderPanel.SetActive (false);
		choosePanel.SetActive (false);
//		choiceNumber = 2;
		Analytics.CustomEvent ("plot 1",new Dictionary<string,object>
		{
			{"choice",2}
		});

		PlayerPrefs.SetInt ("plotChoice0_1", 2);
		Debug.Log ("two");
		lines.Add ("I am a...musician and I have some trouble now. Can you help me to perform?");
		lines.Add ("Don't worry, it's easy. I'll teach you first.");
		lines.Add ("*end");
		lines.RemoveAt (0);
		plotText.text = lines [0];
		lines.RemoveAt (0);
	}

	public void choiceThreeOnClick(){
		faderPanel.SetActive (false);
		choosePanel.SetActive (false);
//		choiceNumber = 3;
		Analytics.CustomEvent ("plot 1",new Dictionary<string,object> {
			{"choice",3}
		});
		PlayerPrefs.SetInt ("plotChoice0_1", 3);
		Debug.Log ("three");
		lines.Add ("Wow.....That's great");
		lines.Add ("Because I'm in trouble now.");
		lines.Add ("...Can you help me to perform?");
		lines.Add ("Don't worry, it's easy. I'll teach you first.");
		lines.Add ("*end");
		lines.RemoveAt (0);
		plotText.text = lines [0];
		lines.RemoveAt (0);
	}


//
//	public GameObject faderPanel;
//	public GameObject choosePanel;
//
//	private int currentProgress = 0;
//	private int currentChooseProgress = 1;
//	private List<Plot> plots = new List<Plot>();
//
//	void OnEnable(){
//		if (PlayerPrefs.GetInt ("plotProgress") == 0) {
//			Debug.Log ("progress = 0");
//			plots.Add (new DialogueLine("one1", "one2", "one3"));
//			plots.Add (new DialogueLine("two1", "two2", "two3"));
//			plots.Add (new DialogueChoose ("choice1", "choice2", "choice3"));
//			plots.Add (new DialogueLine("three1", "three2", "three3"));
//		}
//	}
//
//	public void showNextPlot() {
//		if (plots[currentProgress+1].returnPlotType () == 0) {
//			Debug.Log ("line");
//			if (PlayerPrefs.GetInt ("plotChoice" + PlayerPrefs.GetInt ("plotProgress") + "_" + currentChooseProgress) == 1) {
//				plotText.text = ((DialogueLine)plots [currentProgress]).getText1 ();
//			}
//			else if (PlayerPrefs.GetInt ("plotChoice" + PlayerPrefs.GetInt ("plotProgress") + "_" + currentChooseProgress) == 2) {
//				plotText.text = ((DialogueLine)plots [currentProgress]).getText2 ();
//			}
//			else if (PlayerPrefs.GetInt ("plotChoice" + PlayerPrefs.GetInt ("plotProgress") + "_" + currentChooseProgress) == 3) {
//				plotText.text = ((DialogueLine)plots [currentProgress]).getText3 ();
//			}
//		} else {
//			Debug.Log ("choose");
//		}
//
//	}
//
//	public void showChooser(){
//		faderPanel.SetActive (true);
//		choosePanel.SetActive (true);
//	}
//
//	void updateInterfaceInformation(){
//
//	}
}
