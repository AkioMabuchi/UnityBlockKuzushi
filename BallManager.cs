using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour {

    private GameObject gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
	}
	
	// Update is called once per frame
	void Update () {
        bool isBallMoving = gameManager.GetComponent<GameManager>().isBallMoving;
        if(isBallMoving){
            float velocityY = GetComponent<Rigidbody2D>().velocity.y;
            if(velocityY < 0.8f && velocityY > 0.0f){
                Debug.Log("低速上昇補正システム、作動中");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 0.5f), ForceMode2D.Force);
            }else if(velocityY > -0.8f && velocityY <= 0.0f){
                Debug.Log("低速落下補正システム、作動中");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -0.5f), ForceMode2D.Force);
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision){
        Debug.Log("Collision");
        switch(collision.gameObject.tag){
            case "Block":
                gameManager.GetComponent<GameManager>().DestroyBlock(collision.gameObject);
                gameManager.GetComponent<GameManager>().AddScore(10);
                break;

            case "Racket":
                Debug.Log("Racket");
                float velocityZero = 5.0f;
                float ballPointX = collision.contacts[0].point.x;
                float racketPointX = collision.gameObject.transform.position.x;
                float pointX = ballPointX - racketPointX;
                float degree = (1.0f - pointX) * Mathf.PI / 2;
                Vector2 force = new Vector2(velocityZero * Mathf.Cos(degree), velocityZero * Mathf.Sin(degree));


                GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
                GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
                break;
            case "OutArea":
                gameManager.GetComponent<GameManager>().FallBall();
                break;

        }
    }
}

// ラケットが左端に衝突した場合は135°の方向に、右端の場合は45°の方向に、真ん中の場合は90°の方向に、
// θ = (1 - x)π/2
// Vx = Vcosθ
// Vy = Vsinθ
