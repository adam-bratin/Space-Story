using UnityEngine;
using System.Collections;
using Leap;

public class LeapFly : MonoBehaviour
{
	private Leap.Controller controller;
	private GameObject ship;
	
	private float forceMult = 20.0f;
	
	private float throttle = 0; 
	private float pitch = 0;	// x-axis orientation
	private float yaw = 0;	// y-axis orientation
	private float roll = 0;	// z-axis
	private float speed;

	// Use this for initialization
	void Start ()
	{
		controller = new Leap.Controller();
		#if !UNITY_IPHONE
			if(controller.IsConnected) {
				speed = 1;
			}
			else{
				speed = 0;
			}
		#else
			speed = 1;
		#endif
		ship = GameObject.Find("ship_body");
	}

	// Update is called once per frame
	void Update ()
	{
		getInput();
		Frame frame = controller.Frame();
	}

	void getInput() {
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
			if(controller.IsConnected) {
				Frame frame = controller.Frame();
				if(frame.Fingers.Count!=0) {
					if(frame.Fingers.Count==1) {
						ship.SendMessage("shootLaser");
					}
			}
		}
		#endif
	}
}
