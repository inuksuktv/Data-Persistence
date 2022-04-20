using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public int highScore;
    private string playerName;
    private string championName;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Awake()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        playerName = GameObject.Find("DataContainer").GetComponent<DataContainer>().input;
        ScoreText.text = playerName + $"'s score : {m_Points}";
        LoadHighScore();
        HighScoreText.text = "High Score: " + championName + ": " + highScore;
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = playerName + $"'s score : {m_Points}";
        if (m_Points > highScore)
        {
            HighScoreText.text = "High Score: " + playerName + ": " + m_Points;
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        if (m_Points > highScore)
        {
            SaveHighScore(m_Points);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string championName;
    }

    public void SaveHighScore(int newHighScore)
    {
        SaveData data = new SaveData();
        data.highScore = newHighScore;
        data.championName = playerName;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log("Saving as JSON: " + json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.highScore;
            championName = data.championName;
        }
        else { championName = "default"; highScore = 0; }
    }
}
