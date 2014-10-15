using UnityEngine;
using System.Collections;

public class MovementControl : MonoBehaviour {
	private GameObject ship;
	private GameObject turret;
	private const string accelerate = "accelerate";
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

	// Use this for initialization
	void Start () {
		ship = GameObject.Find(shipName);
		turret = GameObject.Find ("/" + shipName + "/" + turretName);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown(fireLaser)) {
			ship.SendMessage(fireLaser);
		}

		if (Input.GetButtonDown(fireTurret)) {
			//turret.SendMessage(fireTurret);
		}
		float shipXAngle = Input.GetAxis(shipXAxis);
		float shipYAngle = Input.GetAxis(shipYAxis);

		ship.SendMessage(turnX, shipXAngle);
		ship.SendMessage(turnY, shipYAngle);

		float acceleration = Input.GetAxis (accelerate);
		ship.SendMessage (accelerate, acceleration);

		//float turretXAngle = Input.GetAxis(turretXAxis) * maxTurn;
		//float turretYAngle = Input.GetAxis(turretYAxis) * maxTurn;
		//turret.SendMessage(turnX, turretXAngle);
		//turret.SendMessage(turnY, turretYAngle);
	}
}