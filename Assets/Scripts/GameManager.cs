using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GameAnalyticsSDK;

public class GameManager : MonoBehaviour
{

    public GameObject GameOverPanel;
    public GameObject LevelCompletePanel;
    public GameObject scoreItems;
    public Text scoreText;
    public Text bestScoreText;
    public Text LevelDoneText;
    public Text lifeText;
    public GameObject continueBtn;

    public int amoutOfLevelsEverPlayed = 0;

    AdManager adManager;


    public int lifes = 3;

    GameObject player;

    public bool isBonus = false;

    int level = 1;
    int currentLevel = 1;
    static int score = 0;
    float hueValue;

	private void Awake()
	{
        GameAnalytics.Initialize();
        amoutOfLevelsEverPlayed = PlayerPrefs.GetInt("AmountOfLevels", 0);
	}

	private void Start()
	{
        

        adManager = GameObject.FindGameObjectWithTag("IronSourceManager").GetComponent<AdManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        scoreText.text = score.ToString();
        bestScoreText.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        hueValue = Random.Range(0,10) / 10.0f;
        SetBackgroundColor();
        lifeText.text = lifes.ToString();
      
	}


	public void LevelDone()
    {
        level++;

        amoutOfLevelsEverPlayed++;
        PlayerPrefs.SetInt("AmountOfLevels", amoutOfLevelsEverPlayed);

        Vibration.Vibrate(100);
        PlayerPrefs.SetInt("CurrentLevel", level);

        LevelCompletePanel.SetActive(true);
        LevelDoneText.text = "Your Score "+score;
        scoreItems.SetActive(false);

        GameAnalytics.NewProgressionEvent( GAProgressionStatus.Complete, level.ToString());
 
    }

	public void GameOver()
    {
        Handheld.Vibrate();
        GameOverPanel.SetActive(true);

        adManager.PingNetwork((isConnect)=>{
            if (adManager.CheckAvailability() && isConnect)
            {
                continueBtn.SetActive(true);
            }
            
        });
       
    }



    public void Restart()
    {
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void addScore()
    {
        Debug.Log("Called ");
        score++;
        //Handheld.Vibrate();
        Vibration.Vibrate(55);
        if (score > PlayerPrefs.GetInt("BestScore",0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreText.text = score.ToString();
        }

        scoreText.text = score.ToString();
        SetBackgroundColor();
    }

    public void SetBackgroundColor()
    {
        Camera.main.backgroundColor = Color.HSVToRGB(hueValue, 0.6f, 0.8f);

        hueValue += 0.1f;
        if (hueValue >= 1)
        {
            hueValue = 0;
        }
    }

    public void minusLife()
    {
        lifes--;
        lifeText.text = lifes.ToString();
    }

    public void plusLife()
    {
        lifes++;
        lifeText.text = lifes.ToString();
    }


    public void restoreGame()
    {
        if (!player.GetComponent<Player>().isDead)
            return;
        
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            player.GetComponent<Player>().isDead = false;
            rb.velocity = Vector2.zero;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3f);
            plusLife();
            GameOverPanel.SetActive(false);
            rb.isKinematic = false;
    }

    public void ResetData()
    {
        PlayerPrefs.SetInt("BestScore", 0);
        PlayerPrefs.SetInt("AmountOfLevels", 0);

    }



}