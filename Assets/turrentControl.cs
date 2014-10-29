using UnityEngine;
using System.Collections;

public class turrentControl : MonoBehaviour {
	private float xMove = 0f;
	private float yMove = 0f;
	private const float maxTurn = .1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rot = transform.parent.localRotation.eulerAngles;
		rot.x += xMove * .90f;
		rot.y += yMove * .90f;
		transform.parent.eulerAngles = rot;
	}

	void fireLaser ()
	{
		gameObject.SendMessage("shootLaser");        
	}
	
	void turnX (float angleX)
	{
		yMove=angleX * maxTurn;
	}
	
	void turnY (float angleY)
	{
		xMove=angleY * maxTurn;
	}
}
