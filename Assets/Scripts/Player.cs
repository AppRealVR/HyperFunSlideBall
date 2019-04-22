using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public GameObject Dead_Effect;



    float angel = 0;
    float bounceFactor = 3;

    int xSpeed = 3;
    int xSpeedStaic = 20;
    int ySpeed = 10;

   

    Rigidbody2D rd;
    GameManager gameManager;

    public GameObject itemEffectObj;
    public bool isDead = false;

	private void Awake()
	{
        rd = gameObject.GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

	void Start()
    {
        
    }

    void Update()
    {
        MovePlayer();
        GetInput();
    }

    void MovePlayer()
    {
        if (PlayerPrefs.GetInt("privacyPolicyShown", 0) != 1)
            return;         

        if (!isDead)
        {
            Vector2 pos = transform.position;
            pos.x = Mathf.Cos(angel) * 3;
            // pos.y = 0;
            transform.position = pos;
            angel += Time.deltaTime * xSpeed;
        }else
        {
            rd.velocity = Vector2.zero;
        }
           // return;
        
       
    }

    void GetInput()
    {
        if (PlayerPrefs.GetInt("privacyPolicyShown", 0) != 1)
            return;         

        if(Input.GetMouseButton(0))
        {
            rd.AddForce(new Vector2(0,ySpeed));
        }else
        {
            if (rd.velocity.y > 0)
            {
                rd.AddForce(new Vector2(0, -ySpeed / 1f));   
            }else
            {
                rd.velocity = new Vector2(rd.velocity.x,0);
            }
          
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (isDead)
            return;

        if(collision.gameObject.tag == "Obstacle" && !isDead)
        {
            gameManager.minusLife();
            if (gameManager.lifes > 0)
            { 
                
                bounce(); 
            }
            else
            {
                Dead(); 
            }


        }else if(collision.gameObject.tag == "Item")
        {
            GetItem(collision);
        }else if (collision.gameObject.tag == "Finish")
        {
            LevelComlete();
        }


	}

    void GetItem(Collider2D other)
    {
        Destroy(Instantiate(itemEffectObj, other.gameObject.transform.position, Quaternion.identity), 0.5f);
        Destroy(other.gameObject);
        gameManager.addScore();

    }

    void LevelComlete()
    {
        if (isDead)
            return;
        
        gameManager.LevelDone();
        stopGame();
    }

    void Dead()
    {
        StartCoroutine(Camera.main.GetComponent<CameraShake>().Shake());
        Destroy(Instantiate(Dead_Effect, transform.position, Quaternion.identity), 1.5f);
        gameManager.GameOver();
        isDead = true;
        rd.isKinematic = true;
        //stopGame();
    }

    void stopGame()
    {
        isDead = true;
        rd.velocity = new Vector2(0, 0);
        rd.isKinematic = true;
    }

    void bounce()
    {
        rd.velocity = Vector2.zero; 
        transform.position = new Vector3(transform.position.x, transform.position.y - bounceFactor);
        Destroy(Instantiate(Dead_Effect, transform.position, Quaternion.identity), 1.5f);
        Handheld.Vibrate();
    }
 
}
