using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScore;
    public GameObject GameOverText;

    private bool m_Started;
    private int m_Points;

    private string playername;
    private int highscore;

    private bool m_GameOver;

    // Start is called before the first frame update
    private void Start()
    {
        const float step = 0.6f;
        var perLine = Mathf.FloorToInt(4.0f / step);

        var pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (var i = 0; i < LineCount; ++i)
        for (var x = 0; x < perLine; ++x)
        {
            var position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
            var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
            brick.PointValue = pointCountArray[i];
            brick.onDestroyed.AddListener(AddPoint);
        }

        LoadScore();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                var randomDirection = Random.Range(-1.0f, 1.0f);
                var forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SaveScore();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void LoadScore()
    {
        var path = Application.persistentDataPath + "/save.json";

        if (!File.Exists(path)) return;
        var json = File.ReadAllText(path);

        var data = JsonUtility.FromJson<SaveData>(json);
        playername = data.name;
        highscore = data.highScore;
        BestScore.text = $"Best Score    =>    {playername} : {highscore}";
    }

    public void SaveScore()
    {
        var data = new SaveData();

        if (m_Points < highscore) return;
        data.highScore = m_Points;
        data.name = MenuManager.Instance.playerNameInput;

        var json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }
}