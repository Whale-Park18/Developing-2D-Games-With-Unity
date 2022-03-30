using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{

    public static RPGGameManager sharedInstance = null; // static Ű���带 �̿��ؼ� �̱��� ������Ʈ��
                                                        // ������ �� ����Ѵ�. �̱����� �� �Ӽ��� ���ؼ���
                                                        // ������ �� �ִ�.
   
    public SpawnPoint playerSpawnPoint;                 // �÷��̾� ���� ��ġ�� ������ ����

    public RPGCameraManager cameraManager;              // ī�޶� �Ŵ��� ����

    /// <summary>
    /// �̱��� �޼���
    /// </summary>
    /// <remarks>
    /// if(sharedInstance != null && sharedInstance != this)
    ///     ����:
    ///         1. sharedInstance != null
    ///         2. sharedInstance != this
    ///         
    /// Destroy(Object obj): void
    ///     sharedInstance�� �̹� �����ϰ� �ٸ��ٸ� �����Ѵ�.
    ///     
    /// sharedInstance = this
    ///     ���� �ν��Ͻ��� ������ �ν��Ͻ��� sharedInstance ������
    ///     ���� ������Ʈ�� �����Ѵ�.
    /// </remarks>
    private void Awake()
    {
        if(sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetScene();
    }

    private void Update()
    {
        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// ���� ������ ���� �޼���
    /// </summary>
    public void SetScene()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// �÷��̾ ������Ű�� �޼���
    /// </summary>
    /// <remarks>
    /// cameraManager.virtualCamera.Follow = player.transform
    ///     virtualCamera.Follow �Ӽ��� �÷��̾� ������Ʈ�� Ʈ�������� �����ϴ� �ڵ�
    ///     ���� ī�޶� �÷��̾ ����ٸ��� �ȴ�.
    /// </remarks>
    public void SpawnPlayer()
    {
        if(playerSpawnPoint != null)
        {
            GameObject player = playerSpawnPoint.SpawnObject();

            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
