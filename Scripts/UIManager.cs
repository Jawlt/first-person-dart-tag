using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI itStatusText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI endGameMessageText;

    public void UpdateItStatus(string playerName)
    {
        itStatusText.text = $"{playerName} is IT!";
    }

    public void UpdateTimerDisplay(float remainingTime)
    {
        timerText.text = $"Time Left: {Mathf.CeilToInt(remainingTime)}s";
    }

    public void ShowEndGameMessage(string message)
    {
        endGameMessageText.text = message;
        endGameMessageText.gameObject.SetActive(true);
    }

    public void ClearItStatus()
    {
        itStatusText.text = "";
    }
}
