using UnityEngine;
using System.Collections;

public class displayCharacterInfo : MonoBehaviour {
	private float distanceEarth = 0.0f;
	private float distanceMoon = 0.0f;
	private float fuel = 1000;
	public float health = 10;
	public float shots = 10;
	private GameObject ship;
	private GameObject moon;
	private GameObject earth;
	private GameObject distanceEText;
	private GameObject distanceMText;
	private GameObject fuelText;
	private GameObject healthText;
	private GameObject shotsText;
	private Detonator det;
	
	private RaycastHit hit;
    public float range = 200.0f;
    private LineRenderer line;
	private GameObject laser;
	private float lineStartTime = 0;
	private float cooldown = 3f;
	private bool laserOn = false;
	public AudioClip engine_normal;
	public AudioClip engine_slight_damage;
	public AudioClip engine_medium_damage;
	public AudioClip engine_large_damage;
	private float height = Screen.height;
	private float width = Screen.width;
	private bool shotsOn = false;

	// Use this for initialization
	void Start () {
		ship = GameObject.Find("Ship");
		moon = GameObject.Find("Moon");
		earth = GameObject.Find("Earth");
		setupScreenText ();

		det = ship.GetComponentInChildren<Detonator>();
		laser = GameObject.Find("laser");
		line = GetComponentInChildren<LineRenderer>();
		audio.clip = engine_normal;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update(){
		distanceEarth = distance("earth");
		distanceMoon = distance("moon");
		if(distanceEarth<=20){
			Application.LoadLevel("landing earth");
		}
		else if(distanceMoon<=20){
			Application.LoadLevel("landing moon");
		}
		if(fuel>0){
			fuel -= Time.deltaTime;
		}
		else{
			fuel=0;
			string[] options = {"fuel"};
			ship.SendMessage("playSound",options);
		}
		//laserCheck();
		if(laserOn){
			if(Time.time - lineStartTime>=.3){
				line.enabled=false;
			}
			else{
				line.enabled = true;
				Vector3 direction = laser.transform.TransformDirection(0,0,1);
				if(Physics.Raycast(laser.transform.position,direction, out hit,range)){
					hit.transform.gameObject.SendMessage("asteroidExplode");
				}
			}
		}

		distanceEText.guiText.text = "Miles to Earth: " + distanceEarth;
		distanceMText.guiText.text = "Miles to Moon: " + distanceMoon;
		fuelText.guiText.text = "Remaining Fuel: " + fuel;
		healthText.guiText.text = "Health: " + health;
		shotsText.guiText.text = "Shots: " + shots;
	}
	
//	void OnGUI(){
//		/*guiText.fontSize = Screen.height*.05;*/
//		GUI.Label(new Rect(.9f*width,.05f*height,.1f*width,.05f*height), "Miles to Earth: " + distanceEarth);
//		GUI.Label(new Rect(.9f*width,.11f*height,.1f*width,.05f*height), "Miles to Moon: " + distanceMoon);
//		GUI.Label(new Rect(.9f*width,.17f*height,.1f*width,.05f*height), "Remaining Fuel: " + fuel);
//		GUI.Label(new Rect(.9f*width,.23f*height,.1f*width,.05f*height), "Health: " + health);
//		GUI.Label(new Rect(.9f*width,.29f*height,.1f*width,.05f*height), "Shots: " + shots);
//	}
	
	float distance(string type){
		GameObject temp = null;
		if(type == ("earth")){
			temp = earth;
		}
		else if(type == "moon"){
			temp = moon;
		}
		float xdist = Mathf.Pow((ship.transform.position.x - temp.transform.position.x),2);
		float ydist = Mathf.Pow((ship.transform.position.y - temp.transform.position.y),2);
		float zdist = Mathf.Pow((ship.transform.position.z - temp.transform.position.z),2);
		float distance = Mathf.Sqrt(xdist + ydist + zdist);
		distance-= (temp.renderer.bounds.size.z)/2;
		return distance;
	}
	
	void loseHealth(){
		health-=1;
		if(health==5){
			turnFlameOn("Left");
			audio.clip = engine_slight_damage;
			audio.Play();
			string[] options = {"health","5"};
			ship.SendMessage("playSound",options);
		}
		else if(health==3){
			turnFlameOn("Right");
			string[] options = {"health","3"};
			ship.SendMessage("playSound",options);
		}
		else if(health==1){
			turnFlameOn("Top");
			string[] options = {"health","1"};
			ship.SendMessage("playSound",options);
		}
		else if(health==0){
			det.Explode();
			renderer.enabled = false;
			GameObject.Find("Ship").GetComponent<BoxCollider>().enabled = false;
			string[] options = {"health","0"};
			ship.SendMessage("playSound",options);
		}
	}
	
	void turnFlameOn(string name){
		GameObject flame = GameObject.Find(name);
		ParticleEmitter[] emit = flame.GetComponentsInChildren<ParticleEmitter>();
		ParticleRenderer[] rend = flame.GetComponentsInChildren<ParticleRenderer>();
		for(int i=0; i<emit.Length; i++){
			emit[i].enabled = true;
			rend[i].enabled = true;
		}
	}
	
	void shootLaser(){
		if(Time.time - lineStartTime>=cooldown && shots>0){
			laserOn=true;
			laser.audio.Play();
			lineStartTime = Time.time;
			if(shotsOn){
				shots--;
			}
		}
	}
	
	/*void laserCheck(){
		if(Input.GetKeyDown(KeyCode.Space) && shots>0){
			laserOn = true;
			laser.audio.Play();
			lineStartTime = Time.time;
			shots--;
		}
	}*/

	void setShotsOn(bool on){
		shotsOn = on;
	}

	void setupScreenText(){
		distanceEText = GameObject.Find ("Distance Earth");
		distanceMText = GameObject.Find ("Distance Moon");
		fuelText = GameObject.Find ("Fuel");
		healthText = GameObject.Find ("Health");
		shotsText = GameObject.Find ("Shots");

		distanceEText.guiText.fontSize = (int)(Mathf.Ceil (45 * width / 3028));
		distanceMText.guiText.fontSize = (int)(Mathf.Ceil (45 * width / 3028));
		fuelText.guiText.fontSize = (int)(Mathf.Ceil (45 * width / 3028));
		healthText.guiText.fontSize = (int)(Mathf.Ceil (45 * width / 3028));
		shotsText.guiText.fontSize = (int)(Mathf.Ceil (45 * width / 3028));

		distanceEText.guiText.pixelOffset = new Vector2 (width/1.25f, height/1.05f);
		distanceMText.guiText.pixelOffset = new Vector2 (width/1.25f, height/1.11f);
		fuelText.guiText.pixelOffset = new Vector2 (width / 1.25f, height / 1.17f);
		healthText.guiText.pixelOffset = new Vector2 (width / 1.25f, height / 1.23f);
		shotsText.guiText.pixelOffset = new Vector2 (width / 1.25f, height / 1.29f);

		distanceEText.guiText.text = "Miles to Earth: " + distanceEarth;
		distanceMText.guiText.text = "Miles to Moon: " + distanceMoon;
		fuelText.guiText.text = "Remaining Fuel: " + fuel;
		healthText.guiText.text = "Health: " + health;
		shotsText.guiText.text = "Shots: " + shots;
		}
}