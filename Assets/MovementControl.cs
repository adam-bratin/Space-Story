using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {
	private GameObject ship;
	private GameObject turret;
	private const string accelerate = "accelerate";
	private const string decelerate = "decelerate";
	private const string fireLaser = "fireLaser";
	private const string fireTurret = "fireTurret";
	private const string speedAxis = "shipSpeed";
	private const string shipXAxis = "shipX";
	private const string shipYAxis = "shipY";
	private const string turretXAxis = "turretX";
	private const string turretYAxis = "turretY";
	private const string shipName = "SciFi_Fighter_MK";
	private const string turretName = "gun TBS 001C";
	private const string turnX = "turnX";
	private const string turnY = "turnY";
	private const int maxTurn = 1;

	// Use this for initialization
	void Start () {
		ship = GameObject.Find(shipName);
		turret = GameObject.Find ("/" + shipName + "/" + turretName);
	}
	
	// Update is called once per frame
	void Update () {
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
		if(Input.GetButtonDown(fireLaser)) {
			Debug.Log("fireLaser");
			ship.SendMessage(fireLaser);
		}
		if (Input.GetButtonDown(speedAxis)) {
			Debug.Log("accelerate");
			ship.SendMessage(accelerate);
		}
		if (Input.GetButtonUp(speedAxis)) {
			Debug.Log("decelerate");
			ship.SendMessage(decelerate);
		}
		if (Input.GetButtonDown(fireTurret)) {
			//turret.SendMessage(fireTurret);
		}
		float shipXAngle = Input.GetAxis(shipXAxis) * maxTurn;
		float shipYAngle = Input.GetAxis(shipYAxis) * maxTurn;
		//float turretXAngle = Input.GetAxis(turretXAxis) * maxTurn;
		//float turretYAngle = Input.GetAxis(turretYAxis) * maxTurn;
		ship.SendMessage(turnX, shipXAngle);
		ship.SendMessage(turnY, shipYAngle);
		//turret.SendMessage(turnX, turretXAngle);
		//turret.SendMessage(turnY, turretYAngle);
		#endif
	}
}