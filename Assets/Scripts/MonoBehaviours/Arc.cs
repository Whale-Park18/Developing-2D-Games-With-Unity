using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 실제로 AmmoObject가 움직이는 역할
/// </summary>
public class Arc : MonoBehaviour
{
    /// <summary>
    /// 포물선을 따라 게임 오브젝트를 움직이는 메서드
    /// </summary>
    /// <param name="destination">최종 위치</param>
    /// <param name="duration">시작 위치에서 최종 위치까지 움직이는 시간</param>
    /// <returns></returns>
    /// <remarks>
    /// TraveArc() 메서드는 여러 프레임에 걸쳐 실행해야 하므로 코루틴으로 만들어야 제격이다.
    /// 
    /// startPosition
    ///     Player::Weapon의 오브젝트 풀에서 할당된 AmmoObject의 위치
    ///     
    /// percentComplete += Time.deltaTime / duration
    ///     AmmoObject를 목적지 방향으로 매끄럽게 움직이기 위해서 Time.deltaTime을 이용한다.
    ///     프레임마다 움직이게 된다면 프레임에 따라 움직이는 거리가 달라지게 된다.
    ///     지난 프레임 이후로 흐른 시간을 총알을 움직이려는 시간으로 나누면 현재 프레임에서의
    ///     진행률을 구할 수 있다.
    ///     Time.deltaTime은 지난 프레임을 그린 이후로 흐른 시간이다. 이 행의 결과인 
    ///     percentageComplte는 이전까지의 진행률에 현재 프레임에서의 진행률을 더한 현재까지의 
    ///     총 진행률이다.
    ///     
    /// currentHeight : 포물선의 현재 높이
    /// currentHeight = Mathf.Sin(float f)
    /// currentHeight = Mathf.Sin(Mathf.PI * percentComplete)
    ///     파이(PI)는 반주기인180도를 뜻한다. 즉, 파이 * percentComplete는 0 ~ 180 사잇값이기
    ///     때문에 Sin(Mathf.PI * percentCompete)는 포물선을 그리게 된다.
    ///     
    /// Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up * currentHeight
    ///     Vector3.Lerp(Vector3 a, Vector3 b, float t) : Vector3
    ///     Vector3.Lerp(startPosition, destination, percentComplete)
    ///         AmmoObject가 두 지점 사이를 일정한 속력으로 매끄럽게 움직이는 효과를 내려면
    ///         게임 프로그래밍에 널리 쓰이는 기법인 선형보간을 사용해야 한다. 선형보간을 하려면
    ///         시작 지점, 종료 지점, 0에서 1사이의 백분율이 필요하다. 선형보간으로 프레임당 이동 거리를
    ///         구할 때 총 진행률(percentCOmplete)을 선형보간 메서드인 Lerp의 백분율 매개변수로 사용한다.
    ///         
    ///         Lerp() 메서드에 총 진행률을 사용한다는 말은 AmmoObject를 화면의 어디로 발사하더라도
    ///         도달하는 시간이 같다는 뜻이다. 분명히 현실 세계에는 있을 수 없는 비현실적인 현상이다.
    ///         하지만 비디오 게임은 현실 세계의 규칙을 완벽하게 따르지 않아도 상관 없다.
    ///         
    ///     Vector3.up : 유니티가 제공하는 변수로 Vector3(0, 1, 0)을 나타낸다.
    ///     Vector3.up * currentHeight
    ///         포물선의 높이를 설정한다.
    ///   
    /// yield return null
    ///      다음 프레임까지 코루틴의 실행을 일시 정지한다.
    /// 
    /// SetActive(false)
    ///     목적지에 도착하면 오브젝트를 비활성화한다.
    /// </remarks>
    public IEnumerator TraveArc(Vector3 destination, float duration)
    {
        var startPosition = transform.position; // 현재 게임오브젝트의 시작 위치
        var percentComplete = 0.0f;             // 선형보간을 계산할 때 사용되는 변수

        while(percentComplete < 1.0f)
        {
            percentComplete += Time.deltaTime / duration;
            
            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);

            transform.position = Vector3.Lerp(startPosition, destination, percentComplete)
                + Vector3.up * currentHeight;
            
            yield return null;
        }

        gameObject.SetActive(false);
    }    
}
