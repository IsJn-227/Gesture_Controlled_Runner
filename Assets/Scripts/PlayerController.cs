using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {
	public float horizontalmoveSpeed;
	private float moveHorizontal;
	private float jump;
	public Rigidbody rb;
	private Animator anim;
	public float speed;
	public float jumpspeed;
	public Text scoretext;
	public Text bigscoretext;

	public Text gameOverText;
	private int score;
	public static PlayerController obj;
	public AudioSource jumpSound;
	public AudioSource gruntSound;


	void Start(){
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();
		gameOverText.enabled = false;
		score = 0;
	}
		
	void FixedUpdate(){
		if (!gameOverText.enabled) {

			float Horizontal = moveHorizontal * horizontalmoveSpeed;

			transform.position = new Vector3 (transform.position.x + Horizontal, transform.position.y, transform.position.z + speed);

			if (transform.position.y < 0.2f) {
				rb.AddForce (new Vector3 (0, jump * jumpspeed, 0), ForceMode.Impulse);
				anim.SetBool ("isRunning", true);
			} else {
				anim.SetBool ("isRunning", false);
			}
			ShowScore ();
			moveHorizontal = Mathf.Lerp (moveHorizontal,0,0.05f);
			jump = 0;
		}
	}

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "ColideWith") {
			gruntSound.Play ();
			anim.SetTrigger ("Die"); 
			gameOverText.enabled = true;
			bigscoretext.text = scoretext.text;
			scoretext.enabled = false;
			StartCoroutine(ReturnToMenu ());
		}
	}

	IEnumerator ReturnToMenu(){
		yield return new WaitForSeconds (4);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}

	void ShowScore(){
		scoretext.text = "SCORE: " + score.ToString();
		score++;
	}

	void Awake(){
		PlayerController.obj = this;
		SimpleGesture.On4AxisFlickSwipeUp(this.Jump);
		SimpleGesture.On4AxisFlickSwipeLeft(this.moveLeft);
		SimpleGesture.On4AxisFlickSwipeRight(this.moveRight);
	}

	void moveRight(){
		moveHorizontal = 1;
	}

	void moveLeft(){
		moveHorizontal = -1;
	}

	void Jump(){
		jump = 1;
		jumpSound.Play ();
	}
}
