using UnityEngine;
using UnityEngine.UI;

public class Ranking : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScoreText;
    public Text playerTimeText;

    void Start()
    {
        // Lấy thông tin về đối tượng vừa mới lưu từ PlayerPrefs
        string playerName = PlayerPrefs.GetString("LastSavedName", "");
        int playerScore = PlayerPrefs.GetInt("LastSavedScore", 0);
        string playerTime = PlayerPrefs.GetString("LastSavedTime", "");

        // Hiển thị thông tin lên UI của scene "Ranking"
        playerNameText.text = playerName;
        playerScoreText.text = $"Score: {playerScore}";
        playerTimeText.text = $"Time: {playerTime}";
    }
}
