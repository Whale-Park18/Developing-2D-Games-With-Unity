using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;     // 현재 체력을 저장하는 스크립팅 가능한 오브젝트

    public HealthBar healthBarPrefab;   // HealthBar 프리팹의 참조를 저장, 이 인수를 
                                        // 사용해서 프리팹의 복사본을 인스터스화 한다.

    HealthBar healthBar;                // 인스턴스화한 HealthBar의 참조를 저장한다.

    public Inventory inventoryPrefab;   // Inventory 프리팹의 참조를 저장

    Inventory inventory;                // 인스턴스화한 Inventory의 참조를 저장한다.

    private void OnEnable()
    {
        ResetCharacter();
    }

    /// <summary>
    /// 스크립트를 시작하자마자 불리는 메서드
    /// </summary>
    /// <remarks>
    /// inventory = Instantiate(inventoryPrefab)
    ///     inventoryPrefab을 인스턴스화 하여 inventory에 할당한다.
    ///     
    /// hitPoints.value = startingHitPoint
    ///     스크립팅 가능한 HitPoints를 참조하는 hitPoints의 value 속성을
    ///     startingHitPoints로 초기화 한다.
    ///     
    /// healthBar = Instantiate(healthBarPrefab)
    ///     healthBarPrefab을 인스턴스화 하여 healthBar에 할당한다.
    ///     
    /// healthBar.character = this
    ///     healthBar의 character 속성에 
    ///     Class Player 자신을 참조하는 this를 할당한다.
    /// </remarks>
    private void Start()
    {
        inventory = Instantiate(inventoryPrefab);

        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }

    /// <summary>
    /// 오브젝트가 트리거 콜라이더와 겹치면 호출되는 메서드
    /// </summary>
    /// <param name="collision">충돌한 Collider 2D 컴포넌트</param>
    /// <remarks>
    /// collision.gameObject.ComapreTag("TagName"): bool
    ///     collision.gameObject: GameObject
    ///         collision을 컴포넌트한 게임 오브젝트
    /// 
    ///     gameObject.CompareTag("TagName")
    ///         게임 오브젝트의 태그와 "TagName"를 비교 
    ///     
    /// collision.gameObject.GetComponent<Type>(): Type
    ///     gameObject의 Type 컴포넌트를 찾아서 반환함
    ///     
    ///     GetComponent<Consumable>.item
    ///         Consumalbe 스크립트의 멤버 변수 item
    ///         
    /// collision.gameObject.SetActive(bool value): void
    ///     gameObject의 활성화를 설정함
    ///     
    ///     value:
    ///         활성화: true
    ///         비활성화: false
    ///     
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            // null이면 hitObject를 제대로 못 얻은 것
            if(hitObject != null)
            {
                bool shouldDisappear = false; // 충돌한 오브젝트를 감춰야 할지 나타내는 값

                switch(hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisappear = inventory.AddItem(hitObject);
                        break;

                    case Item.ItemType.HEALTH:
                        shouldDisappear = AdjustHitPoints(hitObject.quantity);
                        break;

                    default:
                        break;
                }

                if(shouldDisappear)
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// 체력을 조정하는 메소드
    /// </summary>
    /// <param name="amount">조정할 체력 값</param>
    /// <remarks>
    /// 현재 체력값이 최대 허용보다 낮은지 확인한다.
    /// 
    /// switch문 안에 코드를 넣지 않음으로서 두 가지 장점이 있다.
    ///     1. 체력 조정용 함수를 만듬으로서 코드를 명확하게 이해할 수 있어 버그를 줄일 수 있다.
    ///     2. HEALTH 아이템과의 상호 작용 말고도 플레이어의 체력을 변경해야 할 상황이 있다.
    /// </remarks>
    public bool AdjustHitPoints(int amount)
    {
        if(hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints);

            return true;
        }

        return false;
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
    /// if(hitPoints.value <= float.Epsilon)
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
        while (true)
        {
            StartCoroutine(FlickerCharacter());

            hitPoints.value -= damage;

            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
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
    /// <remarks>
    /// 인벤토리와 체력바를 초기화 한다.
    /// </remarks>
    public override void ResetCharacter()
    {
        inventory = Instantiate(inventory);
        healthBar = Instantiate(healthBar);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }

    /// <summary>
    /// 캐릭터의 체력이 0으로 떨어질 때 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// base.KillCharacter()
    ///     base 키워드는 현재 클래스가 상속한 부모 또는 "기본" 클래스를 참조할 때 사용한다.
    ///     base.KillCharacter()를 호출하면 부모 클래스의 KillCharacter() 메서드가 호출된다.
    ///     
    /// Destroy(healthBar/inventory.gameObject)
    ///     체력바, 이벤토리 제거
    /// </remarks>
    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
}
