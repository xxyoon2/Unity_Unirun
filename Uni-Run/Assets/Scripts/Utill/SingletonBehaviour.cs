using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유니티 컴포넌트 대상으로 타입을 만드는 것이기 때문에 아무 타입(int, float, ...)이 오면 안됨
// 오로지 컴포넌트만 T를 사용할 수 있어야 함

// where 절은 T에 대한 제약 조건을 나타냄
// 제네릭을 썻을 때만 사용할 수 있는 문법
// 제네릭 : C#에서 지원하는 일반화 프로그래밍
// 일반화 - 타입과 관계없이 코드를 작성할 수 있게 해주는 것
public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour // 제네릭 T는 MonoBehaviour을 상속받은 타입이라고 제한
{
    // MonoBehaviour을 상속받으면 유니티가 알아서 생성시키고 소멸시키기 때문에 굳이 생성자를 만들어 줄 필요가 없음
    // 그럼에도 생성자를 만들면 잘못된 값이 들어가 심각한 프로젝트에 큰 문제가 생길 수도 있음

    // 여기서 사용할 수 있는 것은 유니티에서 제공하는 이벤트 함수를 이용해 초기화시킬 수 있음
    // 그 중 Awake를 사용 - 스크립트가 활성화 되어있지 않아도 초기화 시켜주기 때문
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    protected void Awake()
    {
         if (_instance != null)
         {
            if (_instance != this)
            {
                Destroy(gameObject);
            }

            return;
         }
        
        
        _instance = GetComponent<T>();
        // 씬이 전환이 되어도 소멸(파괴)되면 안됨
        // 그러기에 그것을 방지하는 함수를 사용해야 함
        DontDestroyOnLoad(gameObject);
    }
}
