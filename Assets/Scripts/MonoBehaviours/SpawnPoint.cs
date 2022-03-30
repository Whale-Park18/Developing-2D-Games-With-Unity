 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;    // 한번 또는 일정한 간격을 두고 계속 스폰할 프리팹

    public float repeatInterval;        // 일정한 간격을 두고 프리팹을 스폰하고 싶다면
                                        // 유니티에서 이 속성 값을 설정해야 한다.

    /// <summary>
    /// 스크립트를 시작하자마자 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// repeatInterval > 0
    ///     repeatInterval > 0일 때 미리 설정한 시간 간격을 두고
    ///     계속 오브젝트를 스폰해야 한다는 뜻이다.
    ///     
    /// InvokeRepeating(string methodName, float time, float repeatRate) : void
    ///     지정한 주기로 반복해서 호출되는 메서드
    ///     
    ///     매개변수:
    ///         methodName:
    ///             함수명
    ///             
    ///         time:
    ///             최초 호출까지의 대기 시간
    ///             
    ///         repeatRate:
    ///             호출 사이의 대기 시간
    ///     
    /// </remarks>
    void Start()
    {
        if(repeatInterval > 0)
        {
            InvokeRepeating("SpawnObject", 0.0f, repeatInterval);
        }
    }

    /// <summary>
    /// 실제로 프리팹을 인스턴스화해서 오브젝트를 "스폰"하는 역할을 하는 메서드
    /// </summary>
    /// <returns>스폰한 오브젝트의 인스턴스</returns>
    /// <remarks>
    /// if(prefabToSpawn != null)
    ///     에러를 방지하기 위해 인스턴스화 하기 전에 유니티 에디터에서 프리팹을
    ///     설정했는지 확인해야 한다.
    ///     
    /// Instantiate(GameObject original, Vector3 position, Quaternion rotation) : GameObject
    ///     매개 변수:
    ///         original:
    ///             인스턴스할 게임 오브젝트
    ///             
    ///         position:
    ///             생성할 위치
    ///             
    ///         rotation:
    ///             회전
    ///             Quaternion.identity는 "회전이 없음"을 나타낸다.
    /// 
    /// return null
    ///     에디터에서 SpawnPoint를 제대로 설정하지 않았다는 뜻
    /// </remarks>
    public GameObject SpawnObject()
    {
        if(prefabToSpawn != null)
        {
            return Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }

        return null;
    }
}
