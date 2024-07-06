using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTable : MonoBehaviour
{
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

    private Transform entryContainer;
    private Transform entryTemplate;
    public Dropdown levelDropdown;
    private ScoreData scoreData;

    private void Awake()
    {
        entryContainer = transform.Find("ScoreContainer");
        entryTemplate = entryContainer.Find("Template");
        entryTemplate.gameObject.SetActive(false);

        string jsonPath = Path.Combine(Application.streamingAssetsPath, "Ranking.json");
        if (File.Exists(jsonPath))
        {
            string jsonData = File.ReadAllText(jsonPath);
            scoreData = JsonUtility.FromJson<ScoreData>(jsonData);
            PopulateDropdown();
            UpdateScoreTable(0);
            levelDropdown.onValueChanged.AddListener(UpdateScoreTable);
        }
        else
        {
            Debug.LogError("Cannot find scores.json file");
        }
    }

    private void PopulateDropdown()
    {
        List<string> levelNames = new List<string>();
        foreach (var level in scoreData.levels)
        {
            levelNames.Add(level.level);
        }
        levelDropdown.ClearOptions();
        levelDropdown.AddOptions(levelNames);
    }

    private void UpdateScoreTable(int levelIndex)
    {
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate) Destroy(child.gameObject);
        }

        float templateHeight = 45f;
        var scores = scoreData.levels[levelIndex].scores;
        for (int i = 0; i < scores.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("Pos").GetComponent<Text>().text = GetRankString(scores[i].rank);
            entryTransform.Find("Name").GetComponent<Text>().text = scores[i].name;
            entryTransform.Find("Score").GetComponent<Text>().text = scores[i].score.ToString();
            entryTransform.Find("Time").GetComponent<Text>().text = scores[i].time;
        }
    }

    private string GetRankString(int rank)
    {
        switch (rank)
        {
            default: return rank + "TH";
            case 1: return "1ST";
            case 2: return "2ND";
            case 3: return "3RD";
        }
    }
}
