using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {
	private float trackLength;
	public GameObject[] track;
	public Transform player;
	public float generateDistance;
	private List <GameObject> allTracks;
	private int i;

	void Start () {
		i = 0;
		allTracks = new List<GameObject>();
		trackLength = 0f;
		trackGenerator ();
		trackGenerator ();
		trackGenerator ();
		trackGenerator ();
		trackGenerator ();
	}

	void Update () {
		if ((trackLength - player.position.z) <= generateDistance) {
			trackGenerator ();
			deleteTrack();
		}
	}

	void trackGenerator(){
		GameObject obj;
		if (i < 3) {
			obj = track [0];
			i++;
		}else
			obj = track [Random.Range (1, track.Length)];
		allTracks.Add (Instantiate (obj,new Vector3(0,0,trackLength),obj.transform.rotation));
		trackLength += 30f;
	}

	void deleteTrack(){
		Destroy (allTracks[0]);
		allTracks.RemoveAt (0);
	}
}
