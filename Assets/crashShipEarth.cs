using UnityEngine;
using System.Collections;

public class crashShipEarth : MonoBehaviour {
	public float speed = 300.0f;
	public AudioClip engine;
	public AudioClip crash;
	public AudioClip arrived;
	
	private GameObject ship;
	private GameObject dust;
	private Vector3 startPos;
	private Vector3 endPos;
	private float distance;
	private float acceleration = -.6f;
	private float hoverSpeed = 10f;
	private float distCovered;
	private bool crashed = false;
	private bool dustOn = false;
	private bool play = false;
	private bool playedArrival = false;
	private float startQuit;
	private bool quit = false;
	
	// Use this for initialization
	void Start () {
		ship = GameObject.Find("Ship");
		dust = GameObject.Find("Dust Storm");
		startPos = new Vector3(ship.transform.position.x,ship.transform.position.y,ship.transform.position.z);
		endPos = new Vector3(dust.transform.position.x,dust.transform.position.y,dust.transform.position.z);
		distance = Vector3.Distance(startPos, endPos);
		audio.clip = engine;
		audio.Play();	
	}
	
	// Update is called once per frame
	void Update () {
		if(speed>0){
			distCovered += (Time.deltaTime) * speed;
		}
		else{
			distCovered += (Time.deltaTime) * hoverSpeed;
			ParticleEmitter emit2 = GameObject.Find("InnerCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend2 = GameObject.Find("InnerCore").GetComponent<ParticleRenderer>();
			ParticleEmitter emit3 = GameObject.Find("OuterCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend3 = GameObject.Find("OuterCore").GetComponent<ParticleRenderer>();
			
			emit2.enabled = false;
			rend2.enabled = false;
			emit3.enabled = false;
			rend3.enabled = false;
		}
		float fracJourney = distCovered / distance;
		ship.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
		if((ship.transform.eulerAngles.z >=300 && ship.transform.eulerAngles.z<360)){
			ship.transform.eulerAngles += new Vector3(0f,0f,Time.deltaTime*6f);
		}
		if(speed>0){
			speed+=acceleration;
		}
		else{
			speed=0;
		}
		if(ship.transform.position.y<=5 && !play){
			audio.clip = crash;
			audio.loop = false;
			audio.Play();
			play = true;
			crashed = true;
		}

		if (crashed == true && !audio.isPlaying && !playedArrival) {
			audio.clip = arrived;
			audio.Play();
			playedArrival = true;
		}
		if (!audio.isPlaying && playedArrival && !quit) {
			startQuit = Time.time;
			quit = true;
		}
		if (quit && (Time.time - startQuit >= 2f)) {
			Application.Quit();
		}
		float dist = Mathf.Abs(Vector3.Distance(ship.transform.position,endPos));
		if(dist<225 && !dustOn){
			ParticleEmitter emit = dust.GetComponent<ParticleEmitter>();
			ParticleRenderer rend = dust.GetComponent<ParticleRenderer>();
			
			emit.enabled = true;
			rend.enabled = true;
			dustOn = true;
		}
	}
}
