using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;

    public int m_HighScore;
    private string highscoreName;

    private bool m_GameOver = false;

    private Menu menu;

    
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu").GetComponent<Menu>();

        LoadHighscore();

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
                SceneManager.LoadScene(0);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        UpdateHighscore();
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SaveHighscore();
    }

    void UpdateHighscore()
    {
        if(m_Points > m_HighScore)
        {
            m_HighScore = m_Points;
            highscoreName = menu.Instance.username;
            HighScoreText.text = $"HighScore: {m_HighScore} Name: {highscoreName}";
        }
    }

    [System.Serializable]
    public class SaveHighscoreData
    {
        public string name;
        public int highscore;
    }

    public void SaveHighscore()
    {
        SaveHighscoreData data = new SaveHighscoreData();
        data.name = highscoreName;
        data.highscore = m_HighScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highscoredatafile.json", json);
    }

    public void LoadHighscore()
    {
        string path = (Application.persistentDataPath + "/highscoredatafile.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveHighscoreData data = JsonUtility.FromJson<SaveHighscoreData>(json);

            m_HighScore = data.highscore;
            highscoreName = data.name;
            HighScoreText.text = $"HighScore: {m_HighScore} Name: {highscoreName}";
        }
    }
}
