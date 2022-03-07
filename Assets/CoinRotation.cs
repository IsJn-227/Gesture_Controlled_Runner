using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour {
	float Rotation;
	void Start(){
		Rotation = 0f;
	}
	void Update () {
		Quaternion target = Quaternion.Euler(90, 0, Rotation);
		transform.rotation = target;
		Rotation += 10f;
	}
}
