using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damgeInflicted;  // 적에게 입힐 피해량

    /// <summary>
    /// 현재 게임 오브젝트의 콜라이더 2D 트리거에 다른 오브젝트가 닿았을 때
    /// 유니티 엔진에서 호출하는 메서드
    /// </summary>
    /// <param name="collision">충돌한 세부 정보</param>
    /// <remarks>
    /// gameObject.SetActive(false)
    ///     게임 오브젝트를 Destroy로 삭제하는 것이 아닌 비활성화 시키는 이유
    ///         AmmoObject를 비황성화 상태로 설정하면 오브젝트 풀링이라는 기법을 사용해서
    ///         게임 성능을 좋게 유지할 수 있다.
    /// </remarks>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision is BoxCollider2D)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            StartCoroutine(enemy.DamageCharacter(damgeInflicted, 0.0f));

            gameObject.SetActive(false);
        }
    }
}
