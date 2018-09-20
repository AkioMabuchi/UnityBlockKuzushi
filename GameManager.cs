using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public const float LEFTEST_POSITION = -3.4f;
    public const float RIGHTEST_POSITION = 1.4f;

	public GameObject ball;

    public Text textStage;
    public Text textScore;
    public Text textNumberOfBall;
    public Text textNumberOfBlock;
    public Text textStart;

    public GameObject[] blocksPrefab = new GameObject[7];

	public GameObject racket;

    public GameObject imageGameOver;
    public GameObject imageStageClear;

    public int numberOfBlock = 0;
    public int numberOfBall = 2;

    public int theStage = 1;
    public int theScore = 0;

    public bool isBallMoving = false;

	// Use this for initialization
    void Start () {
        for (int i = 0; i < 10; i++){
            for (int j = 0; j < 7;j++){
                float positionX = i * 0.5f - 3.25f;
                float positionY = j * -0.2f + 2.5f;
                CreateBlock(blocksPrefab[j], positionX, positionY);
            }
        }
        ChangeTextNumberOfBall();
        ChangeTextNumberOfBlock();
    }

	// Update is called once per frame
	void Update () {
        if (isBallMoving){
            float temporaryMousePositionX = (Input.mousePosition.x - 400.0f) / 100.0f;

            if (temporaryMousePositionX < LEFTEST_POSITION){
                temporaryMousePositionX = LEFTEST_POSITION;
            }
            else if (temporaryMousePositionX > RIGHTEST_POSITION){
                temporaryMousePositionX = RIGHTEST_POSITION;
            }

            racket.transform.position = new Vector3(temporaryMousePositionX, -2.5f, 0.0f);
        }
	}

    public void PushGoToTitleButton(){
        SceneManager.LoadScene("TitleScene");
    }

    public void PushStartBallButton(){
        if (!isBallMoving){
            isBallMoving = true;
            ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, -1.0f), ForceMode2D.Impulse);
            textStart.enabled = false;
        }
    }

    public void CreateBlock(GameObject block, float positionX, float positionY){
        Instantiate(block, new Vector3(positionX, positionY, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        numberOfBlock++;
    }

    public void DestroyBlock(GameObject block){
        Destroy(block);
        numberOfBlock--;
        if(numberOfBlock <= 0){
            StopBall();
            Debug.Log("CLEAR");
            AddScore(300);
            imageStageClear.SetActive(true);
        }
        ChangeTextNumberOfBlock();
    }

    public void StopBall(){
        isBallMoving = false;
        ball.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
    }

    public void FallBall(){
        StopBall();

        if(numberOfBall > 0){
            numberOfBall--;
            ChangeTextNumberOfBall();
            ball.transform.position = new Vector3(-1.0f, -2.0f, 0.0f);
            racket.transform.position = new Vector3(-1.0f, -2.5f, 0.0f);
            textStart.enabled = true;
        }else{
            Destroy(ball);
            Debug.Log("GAME OVER");
            imageGameOver.SetActive(true);
        }
    }



    public void ChangeTextNumberOfBall(){
        textNumberOfBall.text = "BALL   " + numberOfBall;
    }

    public void ChangeTextNumberOfBlock(){
        textNumberOfBlock.text = "BLOCK   " + numberOfBlock;
    }

    public void AddScore(int score){
        theScore += score;
        textScore.text = "SCORE   " + theScore;
    }
}
