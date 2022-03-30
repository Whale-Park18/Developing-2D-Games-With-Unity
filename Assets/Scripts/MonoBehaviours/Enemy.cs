using System.Collections;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;            // flost �������� �ܼ�ȭ�� hitPoints ����

    public int damageStrength;  // ������ ���� ���ط�

    Coroutine damageCoroutine;  // ���� ���� �ڷ�ƾ�� ����Ű�� ������ ����
                                // ���߿� �ڷ�ƾ�� ������ ���� ����� �� �ִ�.

    private void OnEnable()
    {
        ResetCharacter();
    }

    /// <summary>
    /// ���� ������Ʈ�� �ݶ��̴� 2D�� �ٸ� ������Ʈ�� �ݶ��̴� 2D�� ����� ��
    /// ����Ƽ �������� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="collision">�浹�� ���� ����</param>
    /// <remarks>
    /// if (damageCoroutine == null)
    ///     �� ���� �̹� DamageCharacter() �ڷ�ƾ�� ���� ������ Ȯ���Ѵ�.
    ///     ���� ���� �ƴϸ� �÷��̾� ������Ʈ�� �ڷ�ƾ�� �����Ѵ�.
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
    /// ���� ������Ʈ�� �ݶ��̴� 2D�� �ٸ� ������Ʈ�� �ݶ��̴� 2D�� ������ ���� ��
    /// ����Ƽ �������� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="collision">�浹�� ���� ����</param>
    /// <remarks>
    /// if(damageCoroutine != null)
    ///     �ڷ�ƾ�� Ȱ��ȭ �Ǿ��ٸ� �ش� �ڷ�ƾ�� �����ϰ� null�� �����Ѵ�.
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
    /// if(hitPoints <= float.Epsilon)
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
    /// ĳ���͸� �ٽ� ����� �� �ְ� ���� ���� ���·� �ǵ����� �޼���
    /// �� �޼���� ĳ���͸� ó�� ����� ������ ������ ���� ����� �� �ִ�.
    /// </summary>
    public override void ResetCharacter()
    {
        hitPoints = startingHitPoints;
    }
}
