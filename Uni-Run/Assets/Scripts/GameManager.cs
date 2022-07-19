using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 게임 오버 상태를 표현하고, 게임 점수와 UI를 관리하는 게임 매니저
// 씬에는 단 하나의 게임 매니저만 존재할 수 있다.
public class GameManager : SingletonBehaviour<GameManager>
{
    public int ScoreIncreaseAmount = 1;
    //public ScoreText ScoreText; // 점수를 출력할 UI 텍스트
    //public GameObject GameOverUI; // 게임 오버시 활성화 할 UI 게임 오브젝트
    // public class ScoreChangeEvent : UnityEvent<int> { }
    // public ScoreChangeEvent OnScoreChanged = new ScoreChangeEvent();

    public UnityEvent OnGameEnd = new UnityEvent();
    public event UnityAction ONGameEnd2;

    public UnityEvent<int> OnScoreChanged = new UnityEvent<int>();
    public event UnityAction<int> OnScoreChanged2;
    // 스코어 프로퍼티
    public int CurrentScore
    {
        get
        {
            return _currentScore;
        }
        set
        {
            // value - 프로퍼티 구현할 때 사용할 수 있음. 일관된 이름으로 처리할 수 있도록 해줌
            _currentScore = value;
            //노티파이
            OnScoreChanged.Invoke(_currentScore);
            OnScoreChanged2?.Invoke(_currentScore); // 구독자가 있는지 null 체크
        }
    }

    private int _currentScore = 0; // 게임 점수
    public bool _isEnd = false; // 게임 오버 상태

    void Update() {
        // 게임 오버 상태에서 게임을 재시작할 수 있게 하는 처리
        if (_isEnd && Input.GetKeyDown(KeyCode.R))
        {
            reset();
            SceneManager.LoadScene(0);
        }
    }

    // 점수를 증가시키는 메서드
    public void AddScore() 
    {
        CurrentScore += ScoreIncreaseAmount;
    }

    public void End()
    {
        // GameObject gameOverUI = GameObject.Find("GameOverUI");
        // gameOverUI.SetActive(true);
        _isEnd = true;
        OnGameEnd.Invoke();
        OnGameEnd?.Invoke();
    }

    private void reset()
    {
        _currentScore = 0;
        _isEnd = false;
    }
}