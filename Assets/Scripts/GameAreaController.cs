using UnityEngine;

public class GameAreaController : MonoBehaviour {

	Vector3[] path;
	Vector3[] path2;
	Vector3[] path3;
	Vector3[] path4;
	Vector3[] path5;
//
//	Hashtable ha;
	public GameObject GameArea;
//
//	iTweenPath thisiTweenPath;




	void Start () {
		path = iTweenPath.GetPath ("GameAreaPath");
		path2 = iTweenPath.GetPath ("GameAreaPath_2");
		path3 = iTweenPath.GetPath ("GameAreaPath_transition");
		path4 = iTweenPath.GetPath ("GameAreaPath_3");
		path5 = iTweenPath.GetPath ("GameAreaPath_4");
//		Debug.Log (path.ToString ());
//
//		ha.Add ("path", path);

//		Debug.Log (path.Length);

	}
	

	void Update () {

	}

	public void animateGameArea(){
		iTween.MoveTo (GameArea, iTween.Hash ("path", path, "time", 4f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 30f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path2, "time", 11f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 43.69f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path3, "time", 0.5f, "easetype", iTween.EaseType.easeInOutQuad, "looptype", iTween.LoopType.none, "delay", 55f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path4, "time", 20.658f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 56.104f));
		iTween.MoveTo (GameArea, iTween.Hash ("path", path5, "time", 10.992f, "easetype", iTween.EaseType.linear, "looptype", iTween.LoopType.none, "delay", 76.762f));
	}
}
