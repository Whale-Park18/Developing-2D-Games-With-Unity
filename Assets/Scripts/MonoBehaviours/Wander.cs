using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 나중에 Wander 스크립트를 추가할 게임 오브젝트에는
// 리지드바디 2D, 써클 콜라이더 2D, 애니메이터가 있어야 한다.
// RequireComponent를 사용해서 이 스크립트를 추가한 게임 오브젝트에
// 필요한 컴포넌트가 없으면 자동으로 추가한다.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    public float pursuitSpeed;  // 적이 플레이어를 추적하는 속력
    public float wanderSpeed;   // 추적할 떄가 아닌 평상시 새회 속력
    float currentSpeed;         // 앞의 둘 중에서 선택할 현재 속력

    public float directionChangeInterval;   // 배회할 방향의 전환 빈도

    public bool followPlayer;               // 플레이어를 추적하는 기능을 켜거나 끌 수 있는
                                            // 플래그

    Coroutine moveCoroutine;                // 이동 코루틴 참조
    
    Rigidbody2D rb2d;
    Animator animator;
    CircleCollider2D circleCollider;

    Transform targetTransform = null;       // 적이 플레이어를 추적할 때 사용
    
    Vector3 endPosition;                    // 배회하고 있는 적의 목적지

    float currentAngle = 0;                 // 배회할 방향을 바꿀 떈 기존 각도에서 새로운
                                            // 각도를 더한다. 이 각도를 사용해서 목저리를
                                            // 나타내는 벡터를 만든다.

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        currentSpeed = wanderSpeed;

        StartCoroutine(WanderRoutine());
    }

    /// <summary>
    /// 프레임당 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// Debug.DrawLine(vector3 start, vector3 end, Color color)
    /// Debug.DrawLine(rb2d.position, endPosition, Color.red)
    /// </remarks>
    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    /// <summary>
    /// 현재 오브젝트의 콜라이더 2D 트리거에 다른 오브젝트 콜라이더 2D와 닿았을 때
    /// 유니티 엔진에서 호출하는 메서드
    /// </summary>
    /// <param name="collision">충동할 세부 정보</param>
    /// <remarks>
    /// collision.gameObject.CompareTag("Player") && followPlayer
    ///     충돌한 게임 오브젝트가 Player && 현재 플레이어 추적중인지 확인
    ///     ! followPlayer를 초기화 하지 않는데 왜 사용하는지 의문임 저거 땜에 추적 안됨
    ///     
    ///     currentSpeed = pursuitSpeed
    ///     targetTransform = collision.gameObject.transform
    ///         현재 속도를 추적 스피드로 초기화 하고 
    ///         추적을 위핸 Transform을 플레이어의 transform으로 초기화 한다.
    ///         
    ///     moveCoroutine != null
    ///         현재 배회 중이라면 중지시킨다.
    ///         
    ///     moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed))
    ///         transform을 설정한 후 호출하는 Move() 메서드는 플레이어를 추적하게 된다.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        //if(collision.gameObject.CompareTag("Player") && followPlayer)
        {
            print("Wander::OnTriggerEnter2D");
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }

    /// <summary>
    /// 현재 오브젝트의 콜라이더 2D 트리거에 충돌했던 다른 오브젝트 콜라이더 2D가 떨어졌을 때
    /// 유니티 엔진에서 호출하는 메서드
    /// </summary>
    /// <param name="collision">충돌한 세부 정보</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Wander::OnTriggerExit2D");
            animator.SetBool("isWalking", false);

            currentSpeed = wanderSpeed;

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
    }

    /// <summary>
    /// 추적을 제외한 배회 로직
    /// </summary>
    /// <returns>코루틴</returns>
    /// <remarks>
    /// ChooseNewEndpoint()
    ///     새로운 목적지 설정
    ///     
    /// if(moveCoroutine != null)
    ///     코루틴이 이미 실행 중이라면 해당 코루틴을 중지한다.
    ///     
    /// StartCoroutine(Move(rb2d, currentSpeed))
    ///     Move 코루틴을 활성화 시킨다.
    ///     
    /// yield return new WaitForSeconds(directionChangeInterval)
    ///     directionChangeInterval 초를 대기하는 코루틴
    /// </remarks>
    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// 새로운 목적지를 우작위로 선택하는 메서드
    /// </summary>
    /// <remarks>
    /// Random.Range(0, 360)
    ///     0~360도 사이의 값을 무작위 선택, 적이 움직일 새로운 방향을 나타낸다.
    ///     "도" 단위는 각도 값이다.
    /// 
    /// Mathf.Repeat(float t, float length)
    /// Mathf.Repeat(currentAngle, 360)
    ///     Mathf.Repeat() 메서드는 % 연산자와 마찬가지로 주어진 값이 지정한 값의 범위 안에
    ///     들 때까지 반복하므로 반환 값이 절대 0보다 작거나 360보다 클 수 없다.
    ///     즉, 0에서 360 사이의 새로운 각도를 반환한다.
    ///     
    /// endPosition += Vector3FromAngle(currentAngle)
    ///     새로운 방향으로 입력된 각도를 Vector3로 환산한 후 endPosition에 더한다.
    /// </remarks>
    public void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);

        endPosition += Vector3FromAngle(currentAngle);
    }

    /// <summary>
    /// 인수로 받은 각도를 호도로 변환하고 방향 벡터 Vector3를 반환하는 메서드
    /// </summary>
    /// <param name="inputAngleDegrees">각도</param>
    /// <returns>방향 벡터</returns>
    /// <remarks>
    /// inputAngleDegrees * Mathf.Deg2Rad
    ///     입력받은 각도에 유니티가 제공하는 변환 상수 Mathf.Deg2Rad로 곱해서
    ///     각도에서 호도로 바꾼다.
    ///     
    /// Vector3(...)
    ///     변환한 호도를 사용해서 적의 방향으로 사용할 방향 벡터를 만든다.
    /// </remarks>
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    /// <summary>
    /// endP
    /// </summary>
    /// <param name="rigidBodyToMove">움직일 리지드바디 2D</param>
    /// <param name="speed">속도</param>
    /// <returns>코루틴</returns>
    /// <remarks>
    /// remainingDistance
    ///     현재 위치와 endPosition 사이에 남근 거리를 저장하는 변수
    ///     
    ///     (transform.position - endPosition).sqrMagnitude
    ///         transform.position - endPosition
    ///             두 피연산자의 타입이 Vector3이다
    ///         
    ///     sqrMagnitude
    ///         Vector3의 속성, 적의 현재 위치와 목적지 사이의 대략적인 거리를 구한다.
    ///         
    /// targetTransform != null
    ///     플레이어를 추적중이라면 endPosition을 플레이어의 위치로 갱신시킨다.
    ///     
    /// rigidBodyToMove != null
    ///     Move() 메서드는 리지드바디 2D를 사용해서 적을 움직이므로 리지드바디 2D가 꼭 필요하다.
    ///     
    ///     animator.SetBool(string name, bool value)
    ///     animator.SetBool("isWalking", true)
    ///         애니메이션 파라이터 isWalking을 true로 설정
    ///         
    ///     Vector3.MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    ///         current: 현재 위치, target: 최종 위치, maxDistanceDelta: 프레임 안에 이동할 거리
    ///     Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed* Time.deltaTime)
    ///         MoveTowards() 메서드는 리지드바디 2D의 움직임을 계산할 때 사용한다. 실제로 
    ///         리지드바디 2D는 움직이진 않는다. 적이 추적 중인지, 씬을 느긋하게 배회 중인지에 따라
    ///         speed 값이 바뀔 수 있다.
    ///         
    ///     Rigidbody2D.MovePosition(Vector2 position)
    ///     rb2d.MovePosition(newPosition)
    ///         MovePosition()을 사용하여 리지드바디 2D를 앞서 계산한 newPosition으로 옮긴다.
    ///         
    ///     remainingDistance = ...
    ///         남은 거리를 다시 계산한다.
    ///         
    /// yield return new WaitForFixedUpdate()
    ///     다음 고정 프레임 업데이트까지 실행을 양보한다.
    ///     
    /// animator.SetBool("isWalking", false)
    ///     적이 endPosition에 도착해서 새로운 방향의 선택을 기다린다. 따라서 애니메이션 상태를
    ///     대기 상태로 변경한다.
    /// </remarks>
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while(remainingDistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if(rigidBodyToMove != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, 
                    speed * Time.deltaTime);

                rb2d.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// 기즈모를 구현하는 메서드
    /// </summary>
    /// <remarks>
    /// Gizmos.DrawWireSphere(Vector3 center, float radius)
    /// Gizmos.DrawWireSphere(transform.position, circleCollider.radius)
    /// </remarks>
    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
