using UnityEngine;
using System.Collections;

public class launchRocket : MonoBehaviour {
	private GameObject rocket;
	private GameObject player;
	private Transform rocketPos;
	private bool launchOn = false;
	private Vector3 startPos;
	private Vector3 endPos;
	private float startTime;
	private float distance;
	private float speed = 1.0f;
	public float acceleration = .1f;
	public float maxspeed = 75.0f;

	// Use this for initialization
	void Start () {
		rocket = GameObject.Find("rocket");
		player = GameObject.Find("Player");
		rocketPos = rocket.transform;
		startPos = rocketPos.position;
		endPos = startPos;
		endPos.y +=10000;
		distance = Vector3.Distance(startPos, endPos);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(launchOn){
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / distance;
			rocketPos.position = Vector3.Lerp(startPos, endPos, fracJourney);
			if(speed<maxspeed){
				speed+=acceleration;
			}
			if(player.audio.isPlaying==false){
				Application.LoadLevel("Game");
			}
		}
	}
	
	void launch(){
		audio.Play();
		launchOn = true;
		turnEngineOn();
		
	}
	
	void turnEngineOn(){
		GameObject engine = GameObject.Find("Engine");
		ParticleEmitter emit = engine.GetComponent<ParticleEmitter>();
		ParticleRenderer rend = engine.GetComponent<ParticleRenderer>();
		emit.enabled = true;
		rend.enabled = true;
	}
}
