using System.Collections; // �ڷ�ƾ�� ��ȯ ������ IEnumerator�� ���ǵ� ���ӽ����̽�
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public float maxHitPoints;      // �ִ� ü��
    public float startingHitPoints; // ���� ü��

    /// <summary>
    /// ĳ������ ü���� 0���� ������ �� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// ĳ���Ͱ� ���� �� Destory(gameObject)�� ȣ���ϸ�
    /// ���� ���� ������Ʈ�� �����ϰ� ������ �����Ѵ�.
    /// </remarks>
    public virtual void KillCharacter()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// ĳ���͸� �ٽ� ����� �� �ְ� ���� ���� ���·� �ǵ����� �޼���
    /// �� �޼���� ĳ���͸� ó�� ����� ������ ������ ���� ����� �� �ִ�.
    /// </summary>
    public abstract void ResetCharacter();

    /// <summary>
    /// ���� ĳ���Ϳ��� ���ظ� �ַ��� �ٸ� ĳ���Ͱ� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="damage">���ط�</param>
    /// <param name="interval">���� ������ ��� �ð�</param>
    /// <returns>�ڷ�ƾ�� ��ȯ ����</returns>
    public abstract IEnumerator DamageCharacter(int damage, float interval);

    /// <summary>
    /// ĳ���Ͱ� ���ظ� �Ծ��� ���� ȿ��
    /// </summary>
    /// <returns>�ڷ�ƾ ��ȯ</returns>
    /// <remarks>
    /// ��������Ʈ �������� ���� ĳ���͸� ������ ������ ��, �⺻ ƾƮ ������ �Ͼ������ 
    /// �ٽ� �����Ѵ�.
    /// </remarks>
    public virtual IEnumerator FlickerCharacter()
    {
        GetComponent<SpriteRenderer>().color = Color.red;

        yield return new WaitForSeconds(0.1f);

        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
