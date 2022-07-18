using UnityEngine;

// PlayerController는 플레이어 캐릭터로서 Player 게임 오브젝트를 제어한다.
public class PlayerController : MonoBehaviour {
    public AudioClip DeathClip; // 사망시 재생할 오디오 클립
    public float JumpForce = 700f; // 점프 힘
    public int MaxJumpCount = 2;

    private int _jumpCount = 0; // 누적 점프 횟수
    private bool _isOnGround = false; // 바닥에 닿았는지 나타냄
    private bool _isDead = false; // 사망 상태

    private Rigidbody2D _rigidbody; // 사용할 리지드바디 컴포넌트
    private Animator _animator; // 사용할 애니메이터 컴포넌트
    private AudioSource _audioSource; // 사용할 오디오 소스 컴포넌트
    private Vector2 _zero;


    // 정적 클래스는 인스턴스화 할 수 없다
    // 인스턴스 멤버를 만들 수 없다는 말
    private static class AnimationID
    {
        public static readonly int IS_ON_GROUND = Animator.StringToHash("IsOnGround");
        public static readonly int DIE = Animator.StringToHash("Die");
        
    }

    private static readonly float MIN_NORMAL_Y = Mathf.Sin(45f * Mathf.Deg2Rad);
    // C#에서 상수 만드는 방법 - 평가 시점의 차이가 다름
    // 1. const : 컴파일. 빌드를 할 때 값을 계산할 수 있어야 함. const를 쓰면 오류가 남. 매직넘버는 괜찮은데 그 외는 readonly라고 보면 됨
    // 2. readonly : 런타임. 빌드를 할 때 계산할 수 없음. 한 번 쓰면 값을 수정할 수 없음

    // C#에서도 정적 멤버를 지원함 - static
    // static을 붙이지 않은 것은 인스턴스 멤버
    // 똑같은 데이터를 클래스마다 들고있어 메모리를 낭비하는 것보다는 
    // 메모리를 고정해주는 것이 좋음
    // C++에서는 함수 내부에서도 static을 사용할 수 있었는데
    // C#에서는 사용할 수 없음

    // 상수를 묶을 수 있는 방법이 두 가지가 있는데
    // 1. class
    // 2. enum
    // enum은 컴파일 때 생성되기 때문에 ANIM어쩌고를 만들어주는데에 적합하지 않음

    private void Awake() {
        // 초기화
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _zero = Vector2.zero;
    }

    private void Update() {
        // 사용자 입력을 감지하고 점프하는 처리
        if (_isDead)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // 최대 점프에 도달했으면 아무것도 안함
            if (_jumpCount >= MaxJumpCount)
            {
                return;
            }

            ++_jumpCount;
            _rigidbody.velocity = _zero;
            _rigidbody.AddForce(new Vector2(0f, JumpForce));
            // Play - 내가 가지고있는 현재 클립으로 재생함. 기본으로 넣어둔 클립 재생
            _audioSource.Play();
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_rigidbody.velocity.y > 0)
            {
                _rigidbody.velocity *= 0.5f;
            }
        }

        _animator.SetBool(AnimationID.IS_ON_GROUND, _isOnGround);
    }

    private void Die() {
        // 사망 처리
        // 1. _isDead = true
        _isDead = true;
        // 2. 애니메이션 업데이트
        _animator.SetTrigger(AnimationID.DIE);
        // 3. 플레이어 캐릭터 멈추기
        _rigidbody.velocity = _zero;
        // 3. 죽을 때 소리도 재생
        // PlayOneShot - 한 번만 일회용적으로 재생한다고 보면 됨
        _audioSource.PlayOneShot(DeathClip);

        GameManager.Instance.End();
    }    

    private void OnTriggerEnter2D(Collider2D other) {
        // 트리거 콜라이더를 가진 장애물과의 충돌을 감지
        if (other.tag == "Dead")
        {
            if (_isDead == false)
                Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // 바닥에 닿았음을 감지하는 처리

        // 플랫폼 위로 안착했다면
        ContactPoint2D point = collision.GetContact(0);
        if (point.normal.y >= MIN_NORMAL_Y)
        {
            _isOnGround = true;
            _jumpCount = 0;

            GameManager.Instance.AddScore();
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        // 바닥에서 벗어났음을 감지하는 처리
        _isOnGround = false;
    }
}