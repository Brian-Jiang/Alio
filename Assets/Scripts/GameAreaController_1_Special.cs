using UnityEngine;

public class GameAreaController_1_Special : MonoBehaviour {

	Vector3[] path1;
	Vector3[] path2;
	Vector3[] path3;
	Vector3[] path4;
	Vector3[] path5;

	public GameObject GameArea;

	void Start () {
		path1 = iTweenPath.GetPath ("GameAreaPath_1");
		path2 = iTweenPath.GetPath ("GameAreaPath_2");
		path3 = iTweenPath.GetPath ("GameAreaPath_3");
		path4 = iTweenPath.GetPath ("GameAreaPath_4");
		path5 = iTweenPath.GetPath ("GameAreaPath_5");
	}

	public void animateGameArea(){
		iTween.MoveTo (GameArea, iTween.Hash ("path", path1, "time", 1.739f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 20.421f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path2, "time", 1.747f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 44.976f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path3, "time", 2.038f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 63.968f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path4, "time", 6.138f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 87.28f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path5, "time", 5f, "easetype", iTween.EaseType.easeInOutCubic, "looptype", iTween.LoopType.none, "delay", 94.05f));
	}

}
