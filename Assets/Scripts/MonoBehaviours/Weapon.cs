using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;

    static List<GameObject> ammoPool;   // AmmoObject를 관리하는 오브젝트 풀
    public int poolSize;                // AmmoObject를 관리하는 오브젝트 풀의 크기

    public float weaponVelocity;        // 발사한 총알의 속력

    bool isFiring;                      // 플레이어가 새총을 쏘는 중인지 확인하는 플래그

    [HideInInspector]
    public Animator animator;           // 애니메이터 참조

    Camera localCamera;                 // 카메라 참조

    float positiveSlope;                // 사분면 계산에 사용할 두 직선의 기울기
    float negativeSlope;

    /// <summary>
    /// 플레이어가 새총을 발사할 방향을 지정할 떄 사용할 열거형
    /// </summary>
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    /// <summary>
    /// 스크립트 초기화시 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// if(ammoPool == null)
    ///     오브젝트 풀이 생성되지 않았으면 오브젝트 풀을 생성함
    ///     
    /// for(int i = 0; ...)
    ///     poolSize 만큼 오브젝트를 생성한 후, 오브젝트 풀에 넣는다.
    ///     단, 오브젝트 풀에 들어가는 오브젝트는 비활성화 된 채로 들어간다.
    /// </remarks>
    private void Awake()
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject> ();
        }

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isFiring = false;

        localCamera = Camera.main;

        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    /// <summary>
    /// 오브젝트 풀 ammoPool에서 AmmoObject를 가져와서 반환하는 메서드
    /// </summary>
    /// <param name="location">AmmoObject를 놓을 위치</param>
    /// <returns>오브젝트 풀에서 가져와 활성화한 AmmoObject</returns>
    /// <remarks>
    /// return null
    ///     비활성화된 오브젝트를 발견하지 못함. 즉, 풀의 모든 오브젝트를 사용중임
    /// </remarks>
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.SetActive(true);

                ammo.transform.position = location;

                return ammo;
            }
        }

        return null;
    }

    /// <summary>
    /// AmmoObject를 스폰한 위치에서 마우스 버튼을 클릭한 위치로 옮기는 메서드
    /// </summary>
    /// <remarks>
    /// Camera.main.ScreenToWorldPoint(Vector3 positon) : Vector3
    /// Camera.main.ScreenToWorldPoint(Input.mousePosition)
    ///     마우스는 화면 공간을 사용하므로 화면 공간의 마우스 위치를 월드 공간 위치로 변환한다.
    ///     
    /// SpawnAmmo(transform.position)
    ///     오브젝트 풀에서 활성화한 AmmoObject를 얻는다.
    ///     
    /// travelDuration : AmmoObject의 총 이동 시간
    /// travelDuration = 1.0f / weaponVelocity
    ///     weaponVelocity가 2.0이면 travelDuration은 1.0 / 2.0 이므로 0.5초만에 화면을
    ///     가로질러 목적지에 도달한다. 이 식에 따르면 목적지가 더 멀수록 총알의 속력이 빨라진다.
    ///     
    ///     총알의 이동 거리에 상관없이 0.5초라는 이동 시간을 보장하지 않는다면 
    ///     플레이어가 적과 아주 가까운 곳에서 새총을 발사 했을 때, 새총에서 쏜 총알이 순식간에
    ///     적에 도달하여 아예 보이지 않을 가능성도 있다. 일인칭 슈팅 게임을 만들고 있다면
    ///     상관 없지만 예제는 RPG이므로 항상 새총에서 발사한 총알이 눈에 보이는 편이 더 재미있게
    ///     느껴진다.
    /// </remarks>
    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject ammo = SpawnAmmo(transform.position);

        if (ammo != null) 
        {
            Arc arcScript = ammo.GetComponent<Arc>();

            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TraveArc(mousePosition, travelDuration));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// 프레임당 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// Input.GetMouseButtonDown(int button)
    /// Input.GetMouseButtonDown(0)
    ///     마우스 버튼을 눌렀는지 뗐는지 확인하는 메서드
    ///     
    ///     매개변수
    ///         0: 왼쪽
    ///         1: 오른쪽
    ///         2: 가운데
    /// </remarks>
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            FireAmmo();
        }

        print("Weapon::Update");
        UpdateState();
    }

    /// <summary>
    /// 기울기를 계산하는 메서드
    /// </summary>
    /// <param name="pointOne">첫 번째 좌표</param>
    /// <param name="pointTwo">두 번째 좌표</param>
    /// <returns>기울기</returns>
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    /// <summary>
    /// 마우스로 클릭한 위치가 플레이어를 기준으로 하는 기울기보다 양의 직선 위에 있는지
    /// 비교하는 메서드
    /// </summary>
    /// <param name="inputPosition">마우스로 클릭한 위치</param>
    /// <returns>마우스 클릭한 y 절편이 양의 직선의 y 절편보다 위인가?</returns>
    /// <remarks>
    /// y = mx + n
    ///     m : 기울기
    ///     n : y 절편
    ///     
    /// n = y - mx
    /// </remarks>
    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);

        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    /// <summary>
    /// 마우스로 클릭한 위치가 플레이어를 기준으로 하는 기울기보다 음의 직선 위에 있는지
    /// 비교하는 메서드
    /// </summary>
    /// <param name="inputPosition">마우스로 클릭한 위치</param>
    /// <returns>마우스 클릭한 y 절편이 음의 직선의 y 절편보다 위인가?</returns>
    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);

        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    /// <summary>
    /// 마우스로 클릭한 사분면을 반환하는 메서드
    /// </summary>
    /// <returns>마우스로 클릭한 사분면</returns>
    Quadrant GetQuadrant()
    {
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);

        if(!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if(higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if(higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }

    void UpdateState()
    {
        print("Weapon::UpdateState");
        if(isFiring)
        {
            Vector2 quadrantVector;

            Quadrant quadEnum = GetQuadrant();

            switch(quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;

                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;

                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;

                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;

                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);

            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }

    }
}
