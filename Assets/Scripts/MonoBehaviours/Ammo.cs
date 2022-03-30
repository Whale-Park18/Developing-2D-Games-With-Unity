using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int damgeInflicted;  // ������ ���� ���ط�

    /// <summary>
    /// ���� ���� ������Ʈ�� �ݶ��̴� 2D Ʈ���ſ� �ٸ� ������Ʈ�� ����� ��
    /// ����Ƽ �������� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="collision">�浹�� ���� ����</param>
    /// <remarks>
    /// gameObject.SetActive(false)
    ///     ���� ������Ʈ�� Destroy�� �����ϴ� ���� �ƴ� ��Ȱ��ȭ ��Ű�� ����
    ///         AmmoObject�� ��Ȳ��ȭ ���·� �����ϸ� ������Ʈ Ǯ���̶�� ����� ����ؼ�
    ///         ���� ������ ���� ������ �� �ִ�.
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
