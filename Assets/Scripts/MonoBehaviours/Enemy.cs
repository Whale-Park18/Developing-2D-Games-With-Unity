using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;            // flost 형식으로 단수화한 hitPoints 변수

    public int damageStrength;  // 적에게 입힐 피해량

    Coroutine damageCoroutine;  // 실행 중인 코루틴을 가리키는 참조를 저장
                                // 나중에 코루틴을 중지할 때도 사용할 수 있다.

    private void OnEnable()
    {
        ResetCharacter();
    }

    /// <summary>
    /// 현재 오브젝트의 콜라이더 2D가 다른 오브젝트의 콜라이더 2D와 닿았을 때
    /// 유니티 엔진에서 호출하는 메서드
    /// </summary>
    /// <param name="collision">충돌한 세부 정보</param>
    /// <remarks>
    /// if (damageCoroutine == null)
    ///     이 적이 이미 DamageCharacter() 코루틴을 실행 중인지 확인한다.
    ///     실행 중이 아니면 플레이어 오브젝트의 코루틴을 시작한다.
    /// </remarks>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (damageCoroutine == null)
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrength, 1.0f));
        }
    }

    /// <summary>
    /// 현재 오브젝트의 콜라이더 2D와 다른 오브젝트의 콜라이더 2D가 접촉을 끝낼 때
    /// 유니티 엔진에서 호출하는 메서드
    /// </summary>
    /// <param name="collision">충돌한 세부 정보</param>
    /// <remarks>
    /// if(damageCoroutine != null)
    ///     코루틴이 활성화 되었다면 해당 코루틴을 중지하고 null로 설정한다.
    /// </remarks>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    /// <summary>
    /// 현재 캐릭터에게 피해를 주려고 다른 캐릭터가 호출하는 메서드
    /// </summary>
    /// <param name="damage">피해량</param>
    /// <param name="interval">피해 사이의 대기 시간</param>
    /// <returns>코루틴 반환 형식</returns>
    /// <remarks>
    /// public override ...
    ///     추상 메서드를 구현할 땐 override 키워드를 써서 재정의해야 한다.
    ///     
    /// while(true)
    ///     캐릭터가 죽을 때까지 계속 피해를 준다. interval이 0이면 루프를
    ///     빠져나온다.
    ///     
    /// if(hitPoints <= float.Epsilon)
    ///     hitPoints가 0이하인지 확인하여 적이 패배했는지 확인한다.
    ///     
    ///     * 왜 0이 아닌 float.Epsilon인가?
    ///         float의 내부 구현 방식 때문에 float 계산은 반올림 오류가 일어나기 쉽다.
    ///         때문에 현재 시스템에서 "0보다 큰 가장 작은 양수 값"으로 정의한
    ///         float.Epsilon과 float과 비교하는 게 낫다. 적의 생사를 판단하려는 목적상 
    ///         hitPoints가 float.Epsilon보다 작으면 캐릭터의 체력은 0이다.
    ///         
    /// if(interval > float.Epsilon)
    ///     interval 역시 float로 선언된 값이기 때문에 0과 비교하는 것이 아닌
    ///     float.Epsilon으로 비교한다.
    ///     
    /// yield return new WaitForSeconds(interval)
    ///     양보(yield)하고 interval 초만큼 대기한 뒤에 while() 루프의 실행을 재개한다.
    ///     이 상태에서는 캐릭터가 죽어야만 푸르가 끝난다.
    /// </remarks>
    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while(true)
        {
            StartCoroutine(FlickerCharacter());

            hitPoints -= damage;

            if(hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if(interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 캐릭터를 다시 사용할 수 있게 원래 시작 상태로 되돌리는 메서드
    /// 이 메서드는 캐릭터를 처음 만들고 변수를 설정할 때도 사용할 수 있다.
    /// </summary>
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
}
