using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeCoinFinish : MonoBehaviour
{
    [SerializeField] private Text txtCoin; // Text component displaying coins
    [SerializeField] private Text txtTime; // Text component displaying time
    [SerializeField] private TextMeshProUGUI displayCoin; // Text component to display coin value
    [SerializeField] private TextMeshProUGUI displayTime; // Text component to display time value

    void Start()
    {
        // Ensure the Text references are assigned
        if (txtCoin == null)
        {
            Debug.LogError("txtCoin is not assigned. Please assign it in the Inspector.");
        }

        if (txtTime == null)
        {
            Debug.LogError("txtTime is not assigned. Please assign it in the Inspector.");
        }

        if (displayCoin == null)
        {
            Debug.LogError("displayCoin is not assigned. Please assign it in the Inspector.");
        }

        if (displayTime == null)
        {
            Debug.LogError("displayTime is not assigned. Please assign it in the Inspector.");
        }
    }

    void Update()
    {
        // Update the displayCoin and displayTime with values from txtCoin and txtTime
        if (txtCoin != null && txtTime != null && displayCoin != null && displayTime != null)
        {
            string coinValue = txtCoin.text;
            string timeValue = txtTime.text;
            displayCoin.text = $"{coinValue}";
            displayTime.text = $"Time: {timeValue}";
        }
    }
}
