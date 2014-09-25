using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float speed = 0.1f;

	private Vector3 velocity;

	// Use this for initialization
	void Start () 
	{
		velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		velocity = Vector3.zero;

		//Get Key input in 4 directions
		if(Input.GetKey(KeyCode.W))
			velocity.y = speed;

		if(Input.GetKey(KeyCode.A))
			velocity.x = -speed;

		if(Input.GetKey(KeyCode.S))
			velocity.y = -speed;

		if(Input.GetKey(KeyCode.D))
			velocity.x = speed;

		transform.position += velocity;
	}
}
