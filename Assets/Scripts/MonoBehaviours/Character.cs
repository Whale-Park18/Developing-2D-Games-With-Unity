using System.Collections; // 코루틴의 반환 형식인 IEnumerator이 정의된 네임스페이스
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoints;      // 최대 체력
    public float startingHitPoints; // 최초 체력

    /// <summary>
    /// 캐릭터의 체력이 0으로 떨어질 때 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// 캐릭터가 죽을 때 Destory(gameObject)를 호출하면
    /// 현재 게임 오브젝트를 제거하고 씬에서 삭제한다.
    /// </remarks>
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 캐릭터를 다시 사용할 수 있게 원래 시작 상태로 되돌리는 메서드
    /// 이 메서드는 캐릭터를 처음 만들고 변수를 설정할 때도 사용할 수 있다.
    /// </summary>
    public abstract void ResetCharacter();

    /// <summary>
    /// 현재 캐릭터에게 피해를 주려고 다른 캐릭터가 호출하는 메서드
    /// </summary>
    /// <param name="damage">피해량</param>
    /// <param name="interval">피해 사이의 대기 시간</param>
    /// <returns>코루틴의 반환 형식</returns>
    public abstract IEnumerator DamageCharacter(int damage, float interval);

    /// <summary>
    /// 캐릭터가 피해를 입었을 때의 효과
    /// </summary>
    /// <returns>코루틴 반환</returns>
    /// <remarks>
    /// 스프라이트 렌더러를 통해 캐릭터를 빨갛게 물들인 후, 기본 틴트 색상인 하얀색으로 
    /// 다시 설정한다.
    /// </remarks>
    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
