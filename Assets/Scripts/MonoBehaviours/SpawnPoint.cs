 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject prefabToSpawn;    // �ѹ� �Ǵ� ������ ������ �ΰ� ��� ������ ������

    public float repeatInterval;        // ������ ������ �ΰ� �������� �����ϰ� �ʹٸ�
                                        // ����Ƽ���� �� �Ӽ� ���� �����ؾ� �Ѵ�.

    /// <summary>
    /// ��ũ��Ʈ�� �������ڸ��� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// repeatInterval > 0
    ///     repeatInterval > 0�� �� �̸� ������ �ð� ������ �ΰ�
    ///     ��� ������Ʈ�� �����ؾ� �Ѵٴ� ���̴�.
    ///     
    /// InvokeRepeating(string methodName, float time, float repeatRate) : void
    ///     ������ �ֱ�� �ݺ��ؼ� ȣ��Ǵ� �޼���
    ///     
    ///     �Ű�����:
    ///         methodName:
    ///             �Լ���
    ///             
    ///         time:
    ///             ���� ȣ������� ��� �ð�
    ///             
    ///         repeatRate:
    ///             ȣ�� ������ ��� �ð�
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
    /// ������ �������� �ν��Ͻ�ȭ�ؼ� ������Ʈ�� "����"�ϴ� ������ �ϴ� �޼���
    /// </summary>
    /// <returns>������ ������Ʈ�� �ν��Ͻ�</returns>
    /// <remarks>
    /// if(prefabToSpawn != null)
    ///     ������ �����ϱ� ���� �ν��Ͻ�ȭ �ϱ� ���� ����Ƽ �����Ϳ��� ��������
    ///     �����ߴ��� Ȯ���ؾ� �Ѵ�.
    ///     
    /// Instantiate(GameObject original, Vector3 position, Quaternion rotation) : GameObject
    ///     �Ű� ����:
    ///         original:
    ///             �ν��Ͻ��� ���� ������Ʈ
    ///             
    ///         position:
    ///             ������ ��ġ
    ///             
    ///         rotation:
    ///             ȸ��
    ///             Quaternion.identity�� "ȸ���� ����"�� ��Ÿ����.
    /// 
    /// return null
    ///     �����Ϳ��� SpawnPoint�� ����� �������� �ʾҴٴ� ��
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
