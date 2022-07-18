using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    private TextMeshProUGUI _ui;
    // Update is called once per frame

    void Awake()
    {
        _ui = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int score)
    {
        _ui.text = $"Score: {score}";    
    }

}
