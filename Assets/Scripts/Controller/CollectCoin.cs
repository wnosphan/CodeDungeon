using UnityEngine;
using UnityEngine.UI;

public class CollectCoin : MonoBehaviour
{
    public Text scoreText; // Reference to the UI Text element for displaying the score
    private int score = 0; // Player's score

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        UpdateScoreText();
    }
        
    void Update()   
    {
        // Any updates if necessary
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            audioManager.PlaySfx(audioManager.coinClip);
            Collect(collision.gameObject); // Collect the coin
        }
    }

    private void Collect(GameObject coin)
    {
        score++; // Increase the score
        Destroy(coin); // Destroy the coin GameObject
        Debug.Log("Coin collected! Current score: " + score);
        UpdateScoreText(); // Update the UI with the new score
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
