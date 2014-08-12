using UnityEngine;
using System.Collections;

public class crashShipMoon : MonoBehaviour {
	public float speed = 300.0f;
	public AudioClip engine;
	public AudioClip crash;
	public AudioClip arrived;
	
	private GameObject ship;
	private GameObject dust;
	private Vector3 startPos;
	private Vector3 endPos;
	private Transform shipPos;
	private Transform dustPos;
	private float distance;
	private float acceleration = -.2f;
	private float distCovered;
	private bool crashed = false;
	private bool dustOn = false;
	private bool playedArrival = false;
	private bool quit = false;
	private float startQuit;
	private bool stopTurn = false;
	
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

		distCovered += Time.deltaTime * speed;
		float fracJourney = distCovered / distance;
		if((ship.transform.eulerAngles.z >329 || ship.transform.eulerAngles.z<1) && !stopTurn){
			ship.transform.eulerAngles += new Vector3(0f,0f,Time.deltaTime*3f);
		}
		ship.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);
		if(speed>0){
			speed+=acceleration;
		}
		else{
			speed=0;
		}
		if(stopTurn && !crashed){
			audio.clip = crash;
			audio.loop = false;
			audio.Play();
			crashed = true;
		}
		if (crashed && !audio.isPlaying && !playedArrival) {
			Debug.Log (crashed);
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
		if(dist<2){
			ParticleEmitter emit2 = GameObject.Find("REInnerCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend2 = GameObject.Find("REInnerCore").GetComponent<ParticleRenderer>();
			ParticleEmitter emit3 = GameObject.Find("REOuterCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend3 = GameObject.Find("REOuterCore").GetComponent<ParticleRenderer>();
			ParticleEmitter emit4 = GameObject.Find("CInnerCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend4 = GameObject.Find("CInnerCore").GetComponent<ParticleRenderer>();
			ParticleEmitter emit5 = GameObject.Find("COuterCore").GetComponent<ParticleEmitter>();
			ParticleRenderer rend5 = GameObject.Find("COuterCore").GetComponent<ParticleRenderer>();
			ParticleEmitter emit6 = GameObject.Find("CSmoke").GetComponent<ParticleEmitter>();
			ParticleRenderer rend6 = GameObject.Find("CSmoke").GetComponent<ParticleRenderer>();
			Light light = GameObject.Find("CLightsource").GetComponent<Light>();
			
			emit2.enabled = false;
			rend2.enabled = false;
			emit3.enabled = false;
			rend3.enabled = false;
			emit4.enabled = true;
			rend4.enabled = true;
			emit5.enabled = true;
			rend5.enabled = true;
			emit6.enabled = true;
			rend6.enabled = true;
			light.enabled = true;
			stopTurn = true;

		}
	}
}
