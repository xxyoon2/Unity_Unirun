using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _ui;


    void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        // OnScoreChanged라는 이벤트를 구독
        //GameManager.Instance.OnScoreChanged.RemoveListener(UpdateText);
        //GameManager.Instance.OnScoreChanged.AddListener(UpdateText);
        //GameManager.Instance.OnScoreChanged.Invoke(10);

        GameManager.Instance.OnScoreChanged2 -= UpdateText; // 구독 해제
        GameManager.Instance.OnScoreChanged2 += UpdateText; // 구독
        //GameManager.Instance.OnScoreChanged2.Invoke(10);
    }

    public void UpdateText(int score)
    {
        _ui.text = $"Score: {score}";    
    }

    void OnDisable()
    {
        // Remove 해서 구독 해제
        //GameManager.Instance.OnScoreChanged.RemoveListener(UpdateText);

        GameManager.Instance.OnScoreChanged2 -= UpdateText;
    }
}
