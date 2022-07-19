using UnityEngine;

// 발판으로서 필요한 동작을 담은 스크립트
public class Platform : MonoBehaviour {
    private int _obstacleCount;
    public GameObject[] _obstacles; // 장애물 오브젝트들
    private bool _isStepped = false; // 플레이어 캐릭터가 밟았었는가

    void Awake()
    {
        _obstacleCount = transform.childCount;
        _obstacles = new GameObject[_obstacleCount];
        for (int i = 0; i < _obstacleCount; ++i)
        {
            _obstacles[i] = transform.GetChild(i).gameObject;
        }
    }
    // 컴포넌트가 활성화될때 마다 매번 실행되는 메서드
    private void OnEnable() 
    {
        // 발판을 리셋하는 처리
        _isStepped = false;

        // 장애물을 활성화 비활성화 해야함, 확률은 본인 하고싶은 대로
        // Ex. 25% 확률로 활성화됨

        for (int i = 0; i < _obstacleCount; ++i)
        {
            if (0 == Random.Range(0, 2))
            {
                _obstacles[i].SetActive(true);
            }
            else
            {
                _obstacles[i].SetActive(false);
            }
        }



    }

    void OnCollisionEnter2D(Collision2D collision) 
    {
        // 플레이어 캐릭터가 자신을 밟았을때 점수를 추가하는 처리
        if (collision.gameObject.tag == "Player")
        {
            if (_isStepped == false)
            {
                _isStepped = true;
                GameManager.Instance.AddScore();
            }
        }
    }
}