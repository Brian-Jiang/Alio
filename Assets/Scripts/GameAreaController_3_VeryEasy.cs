using UnityEngine;

public class GameAreaController_3_VeryEasy : MonoBehaviour {

	Vector3[] path1;
	Vector3[] path2;
	Vector3[] path3;
	Vector3[] path4;
	Vector3[] path5;
	Vector3[] path6;
	Vector3[] path7;
	Vector3[] path8;
	//	//
	public GameObject GameArea;
	//	//
	void Start () {
		path1 = iTweenPath.GetPath ("GameAreaPath_1");
//		path2 = iTweenPath.GetPath ("GameAreaPath_2");
//		path3 = iTweenPath.GetPath ("GameAreaPath_3");
//		path4 = iTweenPath.GetPath ("GameAreaPath_4");
//		path5 = iTweenPath.GetPath ("GameAreaPath_5");
//		path6 = iTweenPath.GetPath ("GameAreaPath_6");
//		path7 = iTweenPath.GetPath ("GameAreaPath_7");
//		path8 = iTweenPath.GetPath ("GameAreaPath_8");
	}
	//	//
	public void animateGameArea(){
		iTween.MoveTo (GameArea, iTween.Hash ("path", path1, "speed", 0.3f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 0f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path2, "time", 2.5f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 20.4f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path3, "time", 7f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 38f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path4, "time", 4f, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "delay", 51f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path5, "time", 2.3f, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "delay", 56f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path6, "time", 4f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 62f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path7, "time", 2.3f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 87.28f));
//		iTween.MoveTo (GameArea, iTween.Hash ("path", path8, "time", 2.3f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 90.41f));


	}
}
