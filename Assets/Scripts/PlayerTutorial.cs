using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;


public class PlayerTutorial : MonoBehaviour
{
    float angel = 0;
    float bounceFactor = 3;

    int xSpeed = 3;
    int xSpeedStaic = 20;
    int ySpeed = 10;

    bool isDone = false;

    public GameObject tutorialStepOneImg;
    public GameObject tutorialStepTwoImg;
    public GameObject itemEffectObj;
    public GameObject Dead_Effect;
    public GameObject levelCompletePanel;


    Rigidbody2D rd;

	private void Awake()
	{
        GameAnalytics.Initialize();
	}
	void Start()
    {
        rd = gameObject.GetComponent<Rigidbody2D>();

    }

  
    void Update()
    {
        MovePlayer();
        GetInput();
    }

    void MovePlayer()
    {
        if (isDone)
            return;
        
            Vector2 pos = transform.position;
            pos.x = Mathf.Cos(angel) * 3;
            // pos.y = 0;
            transform.position = pos;
            angel += Time.deltaTime * xSpeed;
    }

    void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            rd.AddForce(new Vector2(0, ySpeed));

            if (tutorialStepOneImg.activeSelf && rd.velocity.y > 4)
            {
                tutorialStepOneImg.SetActive(false);
            }
        }
        else
        {
            if (rd.velocity.y > 0)
            {
                rd.AddForce(new Vector2(0, -ySpeed / 1f));
            }
            else
            {
                rd.velocity = new Vector2(rd.velocity.x, 0);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.tag == "Obstacle")
        {
            GameOver();
        }

        if (collision.gameObject.tag == "TutorialStep2")
        {
            tutorialStepTwoImg.SetActive(true);
        }

        if (collision.gameObject.tag == "SkipTrigger")
        {
            tutorialStepTwoImg.SetActive(false);
        }

        if (collision.gameObject.tag == "Item")
        {
            GetItem(collision);
        }

        if (collision.gameObject.tag == "Finish")
        {
            levelCompletePanel.SetActive(true);
            rd.velocity = Vector2.zero;
            rd.isKinematic = true;
            isDone = true;
        }


    }

    void GameOver()
    {
        rd.velocity = Vector2.zero;
       
        transform.position = new Vector3(transform.position.x, transform.position.y - bounceFactor);
        Destroy(Instantiate(Dead_Effect, transform.position, Quaternion.identity), 1.5f);
        Handheld.Vibrate();
    }

    void GetItem(Collider2D other)
    {
        Vibration.Vibrate(55);
        Destroy(Instantiate(itemEffectObj, other.gameObject.transform.position, Quaternion.identity), 0.5f);
        Destroy(other.gameObject);
    }

    public void LevelDone()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Copmlete Tutorial");
        Vibration.Vibrate(100);
        SceneManager.LoadScene(1);
    }
}
