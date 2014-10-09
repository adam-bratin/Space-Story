using UnityEngine;
using System.Collections;

public class asteroidSpawn : MonoBehaviour {
	public AudioClip startInstructions;
	public AudioClip distanceEarth250;
	private bool playSoundDistE250 =false;
	public AudioClip distanceEarth500;
	private bool playSoundDistE500 = false;
	public AudioClip distanceEarth1000;
	private bool playSoundDistE1000 = false;
	public AudioClip distanceMoon250;
	private bool playSoundDistM250 =false;
	public AudioClip distanceMoon500;
	private bool playSoundDistM500 =false;
	public AudioClip distanceMoon1000;
	private bool playSoundDistM1000 =false;
	public AudioClip fuelEmpty;
	private bool playSoundFuelEmpty =false;
	public AudioClip health5;
	public AudioClip health3;
	public AudioClip health1;
	public AudioClip exploded;
	private bool quit = false;
	private float startQuit;
	private bool quitEnabled = false;
	
	private GameObject asteriod;
	private GameObject ship;
	private GameObject moon;
	private GameObject earth;
	private Vector3 spawnlocation = new Vector3();
	public float spawnTimeE = .75f;
	public float spawnTimeM = 1.0f;
	private float time;
	private int asteroidNumber = 1;
	private bool startSpawn = false;
	private bool gameOn = false;
	
	// Use this for initialization
	void Start () {
		asteriod = GameObject.Find("asteroid");
		ship = GameObject.Find("Ship");
		earth = GameObject.Find("Earth");
		moon = GameObject.Find("Moon");
		audio.clip = startInstructions;
		audio.Play();
	}
	
	// Update is called once per frame
	void Update () {
		float distanceEarth = distance ("Earth");
		float distanceMoon = distance ("Moon");
		if(startSpawn){
			if(Time.time>time){
				//faces Earth
				if(ship.transform.eulerAngles.y<=90 || ship.transform.eulerAngles.y>270){
					if(distanceEarth<490){
						spawnlocation.z = Random.Range(ship.transform.position.z-100, ship.transform.position.z - 10);
					}
					else{
						spawnlocation.z = Random.Range(ship.transform.position.z-500, ship.transform.position.z - 50);
					}
					time+=spawnTimeE;
				}
				//faces Moon
				else{
					if(distanceMoon<490){
						spawnlocation.z = Random.Range(ship.transform.position.z + 10, ship.transform.position.z + 100);
					}
					else{
						spawnlocation.z = Random.Range(ship.transform.position.z + 50, ship.transform.position.z + 500);
					}
					time+=spawnTimeM;
				}
				spawnlocation.x = Random.Range(ship.transform.position.x-200, ship.transform.position.x + 200);
				spawnlocation.y = Random.Range(ship.transform.position.y-200, ship.transform.position.y + 200);

				GameObject newAsteroid = (GameObject)Instantiate(asteriod, spawnlocation, asteriod.transform.rotation /*Quaternion.identity*/);
				newAsteroid.name = "Asteroid" + asteroidNumber;
				asteroidNumber++;
				newAsteroid.GetComponentInChildren<asteroidControl>().enabled = true;
			}
			if(!audio.isPlaying && quit && !quitEnabled && (Time.time-startQuit)>=2f){
				Application.Quit();
				quitEnabled = true;
			}
		}
		else{
			if(!audio.isPlaying && !gameOn){
				startSpawn = true;
				GameObject.Find("BGM").audio.Play();
				time = Time.time;
				GameObject.Find("ship_body").SendMessage("setShotsOn", true);
				gameOn = true;
			}
		}
	}
	
	float distance(string type){
		GameObject temp = null;
		if(type == ("Earth")){
			temp = earth;
		}
		else if(type == "Moon"){
			temp = moon;
		}
		float xdist = Mathf.Pow((ship.transform.position.x - temp.transform.position.x),2);
		float ydist = Mathf.Pow((ship.transform.position.y - temp.transform.position.y),2);
		float zdist = Mathf.Pow((ship.transform.position.z - temp.transform.position.z),2);
		float distance = Mathf.Sqrt(xdist + ydist + zdist);
		distance -= temp.renderer.bounds.size.z/2;
		if(Mathf.Abs(distance-250)<=20){
			string[] parameters = new string[] {type, "250"};
			playSound(parameters);			
		}
		else if(Mathf.Abs(distance-500)<=20){
			string[] parameters = new string[] {type, "500"};
			playSound(parameters);						
		}
		else if(Mathf.Abs(distance-1000)<=20){
			string[] parameters = new string[] {type, "1000"};
			playSound(parameters);			
		}
		
		return distance;
	}
	
	void playSound(string[] parameters){
		if(parameters[0].Equals("Earth")){
			if(parameters[1].Equals("250") && !playSoundDistE250){
				audio.clip = distanceEarth250;
				audio.Play();
				playSoundDistE250=true;
			}
			else if(parameters[1].Equals("500") && !playSoundDistE500){
				audio.clip = distanceEarth500;
				audio.Play();
				playSoundDistE500=true;
			}
			else if(parameters[1].Equals("1000") && !playSoundDistE1000){
				audio.clip = distanceEarth1000;
				audio.Play();
				playSoundDistE1000=true;
			}
		}
		else if(parameters[0].Equals("Moon")){
			if(parameters[1].Equals("250") && !playSoundDistM250){
				audio.clip = distanceMoon250;
				audio.Play();
				playSoundDistM250=true;
			}
			else if(parameters[1].Equals("500") && !playSoundDistM500){
				audio.clip = distanceMoon500;
				audio.Play();
				playSoundDistM500=true;
			}
			else if(parameters[1].Equals("1000") && !playSoundDistM1000){
				audio.clip = distanceMoon1000;
				audio.Play();
				playSoundDistM1000=true;
			}
		}
		else if(parameters[0].Equals("fuel") && !playSoundFuelEmpty){
			audio.clip = fuelEmpty;
			audio.Play();
			GameObject.Find("BGM").audio.Stop();
			quit = true;
			playSoundFuelEmpty = true;
			startQuit = Time.time;
		}
		else if(parameters[0].Equals("health")){
			if(parameters[1].Equals("5")){
				audio.clip = health5;
				audio.Play();
			}
			else if(parameters[1].Equals("3")){
				audio.clip = health3;
				audio.Play();
			}
			else if(parameters[1].Equals("1")){
				audio.clip = health1;
				audio.Play();
			}
			else if(parameters[1].Equals("0")){
				audio.clip = exploded;
				audio.Play();
				GameObject.Find("BGM").audio.Stop();
				quit = true;
				startQuit = Time.time;
			}
		}
	}
}
