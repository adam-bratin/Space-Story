using UnityEngine;
using System.Collections;

public class asteroidControl : MonoBehaviour {
	private GameObject currentobj;
	private GameObject earth;
	private GameObject moon;
	private Detonator det;
	private Vector3 startPos;
	private Vector3 endPos;
	private float distance;
	private float startTime;
	private float speed = 1.0f;
	public float acceleration = 0.1f;
	public float maxspeed = 20.0f;
	private GameObject ship;
	private bool explode = false;
	
	// Use this for initialization
	void Start () {
		ship = GameObject.Find("Ship");
		earth = GameObject.Find("Earth");
		moon = GameObject.Find("Moon");
		currentobj = gameObject;
		det = currentobj.GetComponent<Detonator>();
		startPos = currentobj.transform.position;
		endPos = startPos;
		//faces earth
		if(ship.transform.eulerAngles.y<=90 || ship.transform.eulerAngles.y>270){
			endPos.z = 15000;
		}
		//faces moon
		else{
			endPos.z=-4000;
		}
		distance = Vector3.Distance(startPos, endPos);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.parent.localScale.x<1.5f){
			transform.parent.localScale+= new Vector3(.005f,.005f,.005f);
		}
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / distance;
		Vector3 move = Vector3.Lerp(startPos, endPos, fracJourney);
		currentobj.transform.parent.transform.position = move;
		if(speed<maxspeed){
			speed+=acceleration;
		}
		if(currentobj.transform.parent.transform.position.z<= earth.transform.position.z || currentobj.transform.parent.transform.position.z>=moon.transform.position.z){
			asteroidExplode();
//			det.Explode();
//			audio.Play();
//			currentobj.GetComponent<MeshRenderer>().enabled = false;
//			explode = true;
		}
		if(!audio.isPlaying && explode){
			Destroy(currentobj.transform.parent.gameObject);
		}
	}
	
	void OnTriggerEnter(Collider col){
		if (explode == false) {
			if (col.gameObject.name == "Ship") {
				GameObject.Find ("ship_body").SendMessage ("loseHealth");
			}
			asteroidExplode();
		}
	}
	
	void asteroidExplode(){
		if(explode==false){
			det.Explode();
			audio.Play();
			currentobj.GetComponent<MeshRenderer>().enabled = false;
			explode = true;
		}
	}
}
