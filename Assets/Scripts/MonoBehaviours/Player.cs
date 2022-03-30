using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;     // ���� ü���� �����ϴ� ��ũ���� ������ ������Ʈ

    public HealthBar healthBarPrefab;   // HealthBar �������� ������ ����, �� �μ��� 
                                        // ����ؼ� �������� ���纻�� �ν��ͽ�ȭ �Ѵ�.

    HealthBar healthBar;                // �ν��Ͻ�ȭ�� HealthBar�� ������ �����Ѵ�.

    public Inventory inventoryPrefab;   // Inventory �������� ������ ����

    Inventory inventory;                // �ν��Ͻ�ȭ�� Inventory�� ������ �����Ѵ�.

    private void OnEnable()
    {
        ResetCharacter();
    }

    /// <summary>
    /// ��ũ��Ʈ�� �������ڸ��� �Ҹ��� �޼���
    /// </summary>
    /// <remarks>
    /// inventory = Instantiate(inventoryPrefab)
    ///     inventoryPrefab�� �ν��Ͻ�ȭ �Ͽ� inventory�� �Ҵ��Ѵ�.
    ///     
    /// hitPoints.value = startingHitPoint
    ///     ��ũ���� ������ HitPoints�� �����ϴ� hitPoints�� value �Ӽ���
    ///     startingHitPoints�� �ʱ�ȭ �Ѵ�.
    ///     
    /// healthBar = Instantiate(healthBarPrefab)
    ///     healthBarPrefab�� �ν��Ͻ�ȭ �Ͽ� healthBar�� �Ҵ��Ѵ�.
    ///     
    /// healthBar.character = this
    ///     healthBar�� character �Ӽ��� 
    ///     Class Player �ڽ��� �����ϴ� this�� �Ҵ��Ѵ�.
    /// </remarks>
    private void Start()
    {
        inventory = Instantiate(inventoryPrefab);

        hitPoints.value = startingHitPoints;
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;
    }

    /// <summary>
    /// ������Ʈ�� Ʈ���� �ݶ��̴��� ��ġ�� ȣ��Ǵ� �޼���
    /// </summary>
    /// <param name="collision">�浹�� Collider 2D ������Ʈ</param>
    /// <remarks>
    /// collision.gameObject.ComapreTag("TagName"): bool
    ///     collision.gameObject: GameObject
    ///         collision�� ������Ʈ�� ���� ������Ʈ
    /// 
    ///     gameObject.CompareTag("TagName")
    ///         ���� ������Ʈ�� �±׿� "TagName"�� �� 
    ///     
    /// collision.gameObject.GetComponent<Type>(): Type
    ///     gameObject�� Type ������Ʈ�� ã�Ƽ� ��ȯ��
    ///     
    ///     GetComponent<Consumable>.item
    ///         Consumalbe ��ũ��Ʈ�� ��� ���� item
    ///         
    /// collision.gameObject.SetActive(bool value): void
    ///     gameObject�� Ȱ��ȭ�� ������
    ///     
    ///     value:
    ///         Ȱ��ȭ: true
    ///         ��Ȱ��ȭ: false
    ///     
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            // null�̸� hitObject�� ����� �� ���� ��
            if(hitObject != null)
            {
                bool shouldDisappear = false; // �浹�� ������Ʈ�� ����� ���� ��Ÿ���� ��

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
    /// ü���� �����ϴ� �޼ҵ�
    /// </summary>
    /// <param name="amount">������ ü�� ��</param>
    /// <remarks>
    /// ���� ü�°��� �ִ� ��뺸�� ������ Ȯ���Ѵ�.
    /// 
    /// switch�� �ȿ� �ڵ带 ���� �������μ� �� ���� ������ �ִ�.
    ///     1. ü�� ������ �Լ��� �������μ� �ڵ带 ��Ȯ�ϰ� ������ �� �־� ���׸� ���� �� �ִ�.
    ///     2. HEALTH �����۰��� ��ȣ �ۿ� ���� �÷��̾��� ü���� �����ؾ� �� ��Ȳ�� �ִ�.
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
    /// ���� ĳ���Ϳ��� ���ظ� �ַ��� �ٸ� ĳ���Ͱ� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="damage">���ط�</param>
    /// <param name="interval">���� ������ ��� �ð�</param>
    /// <returns>�ڷ�ƾ ��ȯ ����</returns>
    /// <remarks>
    /// public override ...
    ///     �߻� �޼��带 ������ �� override Ű���带 �Ἥ �������ؾ� �Ѵ�.
    ///     
    /// while(true)
    ///     ĳ���Ͱ� ���� ������ ��� ���ظ� �ش�. interval�� 0�̸� ������
    ///     �������´�.
    ///     
    /// if(hitPoints.value <= float.Epsilon)
    ///     hitPoints�� 0�������� Ȯ���Ͽ� ���� �й��ߴ��� Ȯ���Ѵ�.
    ///     
    ///     * �� 0�� �ƴ� float.Epsilon�ΰ�?
    ///         float�� ���� ���� ��� ������ float ����� �ݿø� ������ �Ͼ�� ����.
    ///         ������ ���� �ý��ۿ��� "0���� ū ���� ���� ��� ��"���� ������
    ///         float.Epsilon�� float�� ���ϴ� �� ����. ���� ���縦 �Ǵ��Ϸ��� ������ 
    ///         hitPoints�� float.Epsilon���� ������ ĳ������ ü���� 0�̴�.
    ///         
    /// if(interval > float.Epsilon)
    ///     interval ���� float�� ����� ���̱� ������ 0�� ���ϴ� ���� �ƴ�
    ///     float.Epsilon���� ���Ѵ�.
    ///     
    /// yield return new WaitForSeconds(interval)
    ///     �纸(yield)�ϰ� interval �ʸ�ŭ ����� �ڿ� while() ������ ������ �簳�Ѵ�.
    ///     �� ���¿����� ĳ���Ͱ� �׾�߸� Ǫ���� ������.
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
    /// ĳ���͸� �ٽ� ����� �� �ְ� ���� ���� ���·� �ǵ����� �޼���
    /// �� �޼���� ĳ���͸� ó�� ����� ������ ������ ���� ����� �� �ִ�.
    /// </summary>
    /// <remarks>
    /// �κ��丮�� ü�¹ٸ� �ʱ�ȭ �Ѵ�.
    /// </remarks>
    public override void ResetCharacter()
    {
        inventory = Instantiate(inventory);
        healthBar = Instantiate(healthBar);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }

    /// <summary>
    /// ĳ������ ü���� 0���� ������ �� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// base.KillCharacter()
    ///     base Ű����� ���� Ŭ������ ����� �θ� �Ǵ� "�⺻" Ŭ������ ������ �� ����Ѵ�.
    ///     base.KillCharacter()�� ȣ���ϸ� �θ� Ŭ������ KillCharacter() �޼��尡 ȣ��ȴ�.
    ///     
    /// Destroy(healthBar/inventory.gameObject)
    ///     ü�¹�, �̺��丮 ����
    /// </remarks>
    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }
}
