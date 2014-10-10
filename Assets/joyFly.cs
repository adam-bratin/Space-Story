using UnityEngine;
using System.Collections;




public class joyFly : MonoBehaviour
{
	private float throttle = 0f; 
	private float xMove = 0f;	// x-axis orientation previously pitch
	private float yMove = 0f;	// y-axis orientation previously yaw
	private const float zMove = 60f;	// z-axis orientation previously roll
	private const float acceleration = .1f;
	private const float maxSpeed = 20;
	private bool accelerate = false; 
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(accelerate)
		{
			if(acceleration + speed < maxSpeed)
			{
				throttle+=acceleration;
			}	
		}
		else
		{
			if(acceleration - speed > 0)
			{
				throttle-=acceleration;
			}
		}

		transform.parent.rigidbody.velocity = transform.parent.forward * -throttle * forceMult;
		Vector3 rot = transform.parent.localRotation.eulerAngles;
		rot.x += xMove * .90f;
		rot.y += yMove * .90f;
		if(xMove > 0)
		{
			rot.z = zMove;
		}
		else
		{
			rot.z = -zMove;
		}
		transform.parent.eulerAngles = rot;
	}
	void fireLaser ()
	{
		ship.SendMessage("shootLaser");

	}
	void accelerate ()
	{
		accelerate = true;

	}
	void decelerate ()
	{
		accelerte = false; 

	}
	void turnX (double angleX)
	{
			yMove+=angleX;
	}
	void turnY (double angleY)
	{
			xMove+=angleY;
	}
	//instead of roll, pitch and yaw make them in terms of x and y. 
	// in turnx and turny we will want to switch ^these values. 
}

