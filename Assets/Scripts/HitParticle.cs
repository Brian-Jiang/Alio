using UnityEngine;

public class HitParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		
	}

	void OnEnable() {
		Destroy (gameObject, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}





