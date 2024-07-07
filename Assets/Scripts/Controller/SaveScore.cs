using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SaveScore : MonoBehaviour
{
    public InputField nameInput;
    public Text scoreText;
    public Text timeText;
    public Button confirmButton;

    private string jsonPath;
    private ScoreData scoreData;
    private ScoreEntry lastSavedEntry;

    [System.Serializable]
    public class ScoreEntry
    {
        public int rank;
        public string name;
        public int score;
        public string time;
    }

    [System.Serializable]
    public class LevelScores
    {
        public string level;
        public List<ScoreEntry> scores;
    }

    [System.Serializable]
    public class ScoreData
    {
        public List<LevelScores> levels;
    }

    void Start()
    {
        // Xây dựng đường dẫn tới file JSON trong thư mục StreamingAssets
        jsonPath = Path.Combine(Application.streamingAssetsPath, "Ranking.json");

        // Load dữ liệu từ file JSON vào scoreData
        LoadScores();

        // Đăng ký sự kiện cho nút xác nhận
        confirmButton.onClick.AddListener(OnConfirmButtonClick);
    }

    void LoadScores()
    {
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            scoreData = JsonUtility.FromJson<ScoreData>(jsonData);
        }
        else
        {
            // Nếu không tìm thấy file JSON, tạo mới scoreData
            scoreData = new ScoreData
            {
                levels = new List<LevelScores>()
            };
        }
    }

    void OnConfirmButtonClick()
    {
        // Kiểm tra các UI element đã được gán hay chưa
        if (nameInput == null || scoreText == null || timeText == null)
        {
            Debug.LogError("One or more UI elements are not assigned in the Inspector.");
            return;
        }

        // Lấy dữ liệu từ các UI elements
        string playerName = nameInput.text;
        int playerScore;

        // Kiểm tra và chuyển đổi điểm số từ scoreText
        if (!int.TryParse(scoreText.text.Replace("Score: ", ""), out playerScore))
        {
            Debug.LogError("Invalid score format or scoreText is null.");
            return;
        }

        // Lấy thời gian từ timeText
        string playerTime = timeText.text.Replace("Time: ", "");

        // Lưu thông tin vào PlayerPrefs và chuyển scene
        lastSavedEntry = new ScoreEntry
        {
            name = playerName,
            score = playerScore,
            time = playerTime
        };

        // Lấy tên của scene hiện tại và lưu dữ liệu vào level tương ứng
        string currentSceneName = SceneManager.GetActiveScene().name;
        AddScoreEntry(playerName, playerScore, playerTime, currentSceneName);
        SaveScores();

        // Chuyển sang scene "RankingSave"
        PlayerPrefs.SetString("LastSavedName", lastSavedEntry.name);
        PlayerPrefs.SetInt("LastSavedScore", lastSavedEntry.score);
        PlayerPrefs.SetString("LastSavedTime", lastSavedEntry.time);
        SceneManager.LoadScene("RankingSave");
    }

    void AddScoreEntry(string name, int score, string time, string level)
    {
        // Tìm level tương ứng hoặc tạo mới nếu chưa tồn tại
        LevelScores currentLevel = scoreData.levels.Find(l => l.level == level);
        if (currentLevel == null)
        {
            currentLevel = new LevelScores
            {
                level = level,
                scores = new List<ScoreEntry>()
            };
            scoreData.levels.Add(currentLevel);
        }

        // Thêm mới entry vào level tương ứng
        ScoreEntry newEntry = new ScoreEntry
        {
            name = name,
            score = score,
            time = time
        };

        currentLevel.scores.Add(newEntry);
        SortScores(currentLevel.scores);
    }

    void SortScores(List<ScoreEntry> scores)
    {
        // Sắp xếp điểm số và thời gian
        scores.Sort((a, b) =>
        {
            int scoreComparison = b.score.CompareTo(a.score);
            if (scoreComparison == 0)
            {
                TimeSpan timeA = TimeSpan.Parse(a.time);
                TimeSpan timeB = TimeSpan.Parse(b.time);
                return timeA.CompareTo(timeB);
            }
            return scoreComparison;
        });

        // Cập nhật lại rank sau khi sắp xếp
        for (int i = 0; i < scores.Count; i++)
        {
            scores[i].rank = i + 1;
        }
    }

    void SaveScores()
    {
        // Lưu scoreData vào file JSON
        string jsonData = JsonUtility.ToJson(scoreData, true);
        File.WriteAllText(jsonPath, jsonData);
    }
}
