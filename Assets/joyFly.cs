using UnityEngine;
using System.Collections;

public class joyFly : MonoBehaviour
{
	public float throttle = 0f; 
	private float xMove = 0f;	// x-axis orientation previously pitch
	private float yMove = 0f;	// y-axis orientation previously yaw
	private const float zMove = .1f;	// z-axis orientation previously roll
	private const float acceleration = .1f;
	private const float maxSpeed = 20;
	private const int maxZRot = 60;
	private bool celerate = false; 
	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(celerate)
		{
			if(acceleration + throttle < maxSpeed)
			{
				throttle+=acceleration;
			}	
		}
		else
		{
			if(throttle - acceleration > 0)
			{
				throttle-=acceleration;
			}
		}
		
		transform.parent.rigidbody.velocity = transform.parent.forward * -throttle;
		Vector3 rot = transform.parent.localRotation.eulerAngles;
		rot.x += xMove * .90f;
		rot.y += yMove * .90f;
		if(xMove > 0)
		{
			if(rot.z < maxZRot && rot.z > -1*maxZRot) {
				rot.z +=zMove;
			}
		}
		else if(xMove < 0) 
		{
			if(rot.z < maxZRot && rot.z > -1*maxZRot) {
				rot.z = -zMove;
			}
		}
		transform.parent.eulerAngles = rot;
	}
	void fireLaser ()
	{
		gameObject.SendMessage("shootLaser");
		
	}
	void accelerate ()
	{
		celerate = true;
		
	}
	void decelerate ()
	{
		celerate = false; 
		
	}
	void turnX (double angleX)
	{
		yMove+= (float) angleX;
	}
	void turnY (double angleY)
	{
		xMove+= (float) angleY;
	}
	//instead of roll, pitch and yaw make them in terms of x and y. 
	// in turnx and turny we will want to switch ^these values. 
}

