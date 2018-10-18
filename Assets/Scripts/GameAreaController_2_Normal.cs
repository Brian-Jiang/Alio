using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAreaController_2_Normal : MonoBehaviour {

	Vector3[] path1;
	Vector3[] path2;
	Vector3[] path3;
	Vector3[] path4;
	Vector3[] path5;
	Vector3[] path6;
//
	public GameObject GameArea;
//
	void Start () {
		path1 = iTweenPath.GetPath ("GameAreaPath_1");
		path2 = iTweenPath.GetPath ("GameAreaPath_2");
		path3 = iTweenPath.GetPath ("GameAreaPath_3");
		path4 = iTweenPath.GetPath ("GameAreaPath_4");
		path5 = iTweenPath.GetPath ("GameAreaPath_5");
		path6 = iTweenPath.GetPath ("GameAreaPath_6");
	}
//
	public void animateGameArea(){
		iTween.MoveTo (GameArea, iTween.Hash ("path", path1, "time", 0.7f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 11.86f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path2, "time", 1.15f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 22.5f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path3, "time", 10f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 23.955f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path4, "time", 10.2f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 33.957f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path5, "time", 4f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 61.865f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path6, "time", 4.2f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 83.356f));
	}

}
