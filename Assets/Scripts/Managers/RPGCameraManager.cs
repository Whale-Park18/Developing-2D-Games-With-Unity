using UnityEngine;
using Cinemachine; // �ó׸ӽ� Ŭ������ ������ ���� ����ϱ� ���� ����Ʈ

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;  // �ó׸ӽ� ���� ī�޶� ����,
                                                    // �ٸ� Ŭ�������� ����� �� �ֵ��� public���� ����,
                                                    // ������ �ڵ带 ���� ������ �Ӽ��̹Ƿ� �ν����Ϳ�
                                                    // ��Ÿ���� �ʵ��� �������ش�.

    /// <summary>
    /// �̱��� ���� �����ϴ� �޼���
    /// </summary>
    /// <remarks>
    /// if(sharedInstance != null && ...)
    ///     �̱��� ����
    ///     
    /// GameObject.FindWithTag(string tag) : GameObject
    ///     ���� ���� tag��� �±װ� �ִ� ���� ������Ʈ�� ��ȯ�ϴ� �޼���
    ///     
    ///     �ϳ��� ���� ������Ʈ�� ���� �ٸ� ����� �����ϴ� ������Ʈ ���� ����
    ///     �߰��� �� �ִٴ� ���� ��������. �̷� ����� ��������(Composition) �����������̶� �Ѵ�.
    ///     
    /// virtualCamera = ...
    ///     ���� ũ��, Follow �� ���� ī�޶��� ��� �Ӽ��� ����Ƽ �����ͻӸ� �ƴ϶�
    ///     ��ũ��Ʈ�� ���ؼ��� ������ �� �ִ� ���� ī�޶� ������Ʈ�� ������ �θ� �ڵ带 ����
    ///     ���� ī�޶��� �Ӽ��� ������ �� �ִ�.
    /// </remarks>
    private void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }

        GameObject vCamGameObject = GameObject.FindWithTag("VirtualCamera");

        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
}
