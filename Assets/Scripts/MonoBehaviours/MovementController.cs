using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnityEngine 클래스 MonoBehaviour를 상속받은
/// MovementController 클래스
/// </summary>
/// <remarks>
/// MonoBehaviour 클래스
///     모든 스크립트가 상속받는 기본 클래스
///     Start(), Awake(), Update() 등의 함수를 사용할 수 있다.
/// </remarks>
public class MovementController : MonoBehaviour
{
    public float movementSpaeed = 3.0f;         // 캐릭터의 속도를 저장한 변수, public으로 지정하여 유니티에서 바로 접근이 가능하다.
    
    Vector2 movement = new Vector2();           // 2D 좌표를 저장하기 위한 Vector2 변수
    Animator animator;                          // Animator 컴포넌트를 참조하기 위한 변수
    Rigidbody2D rb2D;                           // Rigidbody2D 컴포넌트를 참조하기 위한 변수


    /// <summary>
    /// 스크립트를 시작하자마자 불리는 메서드
    /// 초기화 용으로 사용된다.
    /// </summary>
    /// <remarks>
    /// GetComponent<Type>(): Type
    ///     해당 스크립트 컴포넌트를 가진 게임 오브젝트의 Type 컴포넌트를
    ///     찾아서 반환한다.
    /// </remarks>
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// 프레임당 호출되는 메서드
    /// 기기의 성능으로 인해 프레임이 낮아지는 경우
    /// 낮아지는 프레임 만큼 호출 빈도가 줄어든다.
    /// </summary>
    private void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// 일정한 간격으로 호출되는 메서드
    /// Update()와 달리 프레임에 상관없이 일정하게 호출된다.
    /// </summary>
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    /// <summary>
    /// 캐릭터를 움직이는 메서드
    /// </summary>
    /// <remarks>
    /// Input.GetAxisRaw(axisName : string) : float
    ///     axisName:
    ///         가상 축(가로, 세로)
    ///     반환 값:
    ///         키보드와 조이스틱 입력값에 대해 -1 ~ 1
    ///         키보드의 경우 -1, 0, 1 중 하나
    ///     axisName에 따른 반환 값
    ///         "Horizontal(수평)": 
    ///              1: →
    ///              0: Non
    ///             -1: ←
    ///         "Vertical(수직)":
    ///              1: ↑
    ///              0: Non
    ///             -1: ↓
    /// 
    /// movement(Vector2).Normalize()
    ///     벡터를 정규화(normalize)해서 플레이어가 대각선, 수직, 수평, 어느 방향으로
    ///     움직이든 일정한 속력을 우지하게 한다.
    ///     
    /// rb2D.velocity = movement * movementSpaeed
    ///     Rigidbody2D.velocity는 속도를 관여하는 Vector2 변수의 get, set 메서드이다.
    ///     movement * movementSpeed = Vector2(x * movementSpeed, y * movementSpeed)
    ///     
    /// </remarks>
    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        rb2D.velocity = movement * movementSpaeed;
    }

    /// <summary>
    /// 애니메이션 전환을 위해 상태를 업데이트하는 메서드
    /// </summary>
    /// <remarks>
    /// if(Mathf.Approximately(...) && ...)
    ///     Mathf.Approximately(float a, float b) : bool
    ///     Mathf.Approximately(movement.x, 0f)
    ///     Mathf.Approximately(movement.y, 0f)
    ///         a, b 부동 소수점 값을 비교하는 메서드로 x와 y 모두 0이면
    ///         플레이어가 멈춰 있는 것이므로 isWalking 값을 false로 설정한다.
    ///
    ///     movement.x or y: 
    ///         Input.GetAxisRaw() 메서드로 입력된 방향키를 알 수 있음
    ///         방향기에 맞게 애니메이션을 전환함
    ///         
    /// animator.SetFloat(string name, float value) : void
    /// animator.SetFloat("xDir", movement.x)
    /// animator.SetFloat("yDir", movement.y)
    ///     새로운 movement 값으로 anumator를 업데이트 한다.
    /// </remarks>
    private void UpdateState()
    {
        if(Mathf.Approximately(movement.x, 0f) && Mathf.Approximately(movement.y, 0f))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);
        //print(animator.GetBool("isWalking"));
        //print("x: " + animator.GetFloat("xDir") + " y: " + animator.GetFloat("yDir"));
    }
}