using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {
	
	private Vector3 cam;
	private float offset;
	public Transform player;

	void Start(){
		cam = transform.position;
		offset = transform.position.z - player.position.z;
	}
	void LateUpdate () {
		transform.position = new Vector3 (
			cam.x,
			cam.y,
			offset + player.position.z
		);
	}
}
