#pragma strict

// pxs modify for Leap
// store a variable to indicate whether we are using Leap as the input here


var forceMult : float = 20.0;

private var throttle : float = 0; 
private var pitch : float = 0;	// x-axis orientation
private var yaw : float = 0;	// y-axis orientation
private var roll : float = 0;	// z-axis
private var speed : float;

function Start () {
	#if !UNITY_IPHONE
		if(pxsLeapInput.leapEnabled()){
			speed = 1;
		}
		else{
			speed = 0;
		}
	#else
		speed = 1;
	#endif
}

function Update () {

	GetInput();
	
	// apply velocity factor
	transform.parent.rigidbody.velocity = transform.parent.forward * -throttle * forceMult;
	//transform.parent.localRotation.eulerAngles.x = pitch * 20;
	transform.parent.localRotation.eulerAngles.y += yaw*.90;
	transform.parent.localRotation.eulerAngles.z = roll * 20.0;
}

function GetInput()
{
	var ship = GameObject.Find("ship_body");
	
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
		  throttle = .01;
		  roll = 0;
		  //yaw = 0;
		  pitch = 0;
		  
		}
		
		if(throttle < -0.1)
		{
		  throttle = -0.1;
		}
	}
	else{
		//keyboardControl();
	}
#endif
}	