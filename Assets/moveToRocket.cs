using UnityEngine;
using System.Collections;

public class moveToRocket : MonoBehaviour {
	private GameObject player;
	private GameObject rocket;
	private Transform rocketPos;
	private Transform playerPos;
	private Vector3 startPos;
	private Vector3 endPos;
	private float startTime;
	private float distance;
	private int calls=0;
	public float speed = 200.0f;
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		rocket = GameObject.Find("rocket");
		rocketPos = rocket.transform;
		playerPos = player.transform;
		startPos = playerPos.position;
		endPos = rocketPos.position;
		endPos.z+=500;
		distance = Vector3.Distance(startPos, endPos);
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / distance;
		playerPos.position = Vector3.Lerp(startPos, endPos, fracJourney);
		if(playerPos.position==endPos){
			if(calls==0){
				rocket.SendMessage("launch");
				//GameObject.Find("BGM").audio.Stop();
			}
			if(playerPos.eulerAngles.x==0 || playerPos.eulerAngles.x>290){
				playerPos.Rotate(-Time.deltaTime*7,0,0);
			}
			calls++;
		}
	}
}
