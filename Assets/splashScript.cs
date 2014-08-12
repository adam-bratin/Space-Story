using UnityEngine;
using System.Collections;

public class splashScript : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time>=5){
			Application.LoadLevel("launch");
		}
	}
}
