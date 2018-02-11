using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text highScoreText;
    public Text restartText;
    public Text gameOverText;
    
    private bool gameOver;
    private bool restart;
    private int score;
 
    private string highScoreString = "High Score: ";

    void Start(){
        gameOver = false;
        restart = false;

        restartText.text = "";
        gameOverText.text = "";

        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves ());
    }

    void Update()
    {
        float targetTime = 5.0f;
        if (restart) {
            targetTime -= Time.deltaTime;
            UpdateHighScore(score);
            if (Input.GetKeyDown(KeyCode.R)){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
            }
        }
    }
        
    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (true) { 
            for (int i=0; i < hazardCount; i++) { 
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x,spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver) {
                restartText.text = "Press 'R' for restart!";
                restart = true;
                yield return new WaitForSeconds(waveWait);
                SceneManager.LoadScene("UI");
                break;
            }
        }
    }

    public void AddScore(int newScoreValue) {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "score: " + score;
    }

    void UpdateHighScore(int score)
    {
        if(score > PlayerPrefs.GetInt("HighScoreText", 0))
        {
            PlayerPrefs.SetInt("HighScoreText", score);
            highScoreText.text = highScoreString + score;
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over!";
        highScoreText.text = highScoreString + PlayerPrefs.GetInt("HighScoreText", 0).ToString();
        gameOver = true;
    }
}
