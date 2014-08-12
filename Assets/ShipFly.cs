using UnityEngine;
using System.Collections;

public class ShipFly : MonoBehaviour {
	#pragma strict
	// pxs modify for Leap
	// store a variable to indicate whether we are using Leap as the input here
	
	
	public float forceMult = 20.0f;
	
	private float throttle = 0f; 
	private float pitch = 0f;	// x-axis orientation
	private float yaw = 0f;	// y-axis orientation
	private float roll = 0f;	// z-axis
	private float acceleration = .1f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		GetInput();
		// apply velocity factor

		transform.parent.rigidbody.velocity = transform.parent.forward * -throttle * forceMult;
		Vector3 rot = transform.parent.localRotation.eulerAngles;
		rot.y += yaw*.90f;
		rot.z = roll * 60.0f;
		transform.parent.eulerAngles = rot;
	}

	void GetInput(){
		GameObject ship = GameObject.Find("ship_body");
		
		#if UNITY_IPHONE
			// laser
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
				ship.SendMessage("shootLaser");
			}
			
			// navigation with acceleration
			throttle = -Input.acceleration.y * 2.0;	// move forward/backword
			yaw = Input.acceleration.x * 2.0;			// roll
			roll = Input.acceleration.x * 2.0;		//
		
		#else
			if(pxsLeapInput.leapEnabled()){
				// pxs Modify for Leap
				if(pxsLeapInput.getFingerCount()!=0)
				{
					if(pxsLeapInput.getFingerCount()==1)
					{
						ship.SendMessage("shootLaser");
					}
					throttle = pxsLeapInput.GetHandAxis("Depth");
					//pitch = pxsLeapInput.GetHandAxis("Tilt");	// -1~1
					yaw = pxsLeapInput.GetHandAxis("Horizontal");	// -1~1
					roll = -pxsLeapInput.GetHandAxis("Rotation");	// -1~1
					
				}
				else
				{
					throttle = .01f;
					roll = 0f;
					//yaw = 0;
					pitch = 0f;
					
				}
				
				if(throttle < -0.1)
				{
					throttle = -0.1f;
				}
			}
			else{
				keyboardControl(ship);
			}
		#endif
	}

	void keyboardControl(GameObject ship){
		if(Input.GetKeyDown(KeyCode.Space)){
			ship.SendMessage("shootLaser");
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			if(throttle<1){
				throttle+=acceleration;
			}
			else{
				throttle = 1f;
			}
		}
		if(Input.GetKeyUp(KeyCode.UpArrow)){
			throttle = 0f;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			if(yaw<1){
				yaw+=acceleration;
				roll+=acceleration;
			}
			else{
				yaw = 1f ;
				roll=1f;
			}
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			if(yaw>-1){
				yaw-=acceleration;
				roll-=acceleration;
			}
			else{
				yaw = -1f;
				roll = -1f;
			}
		}
		if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)){
			yaw = 0f;
			roll = 0f;
		}
	}
}
