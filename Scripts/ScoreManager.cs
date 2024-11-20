using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ScoreManger : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputScore;
    [SerializeField]
    private TMP_InputField inputName;

    public UnityEvent<string, int> submiteScoreEvent;

    public void SubmitScore()
    {
        submiteScoreEvent.Invoke(inputName.text, int.Parse(inputScore.text));
    }
}