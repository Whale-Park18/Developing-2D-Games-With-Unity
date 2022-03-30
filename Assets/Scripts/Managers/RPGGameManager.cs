using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{

    public static RPGGameManager sharedInstance = null; // static 키워드를 이용해서 싱글톤 오브젝트에
                                                        // 접글할 때 사용한다. 싱글톤은 이 속성을 통해서만
                                                        // 접근할 수 있다.
   
    public SpawnPoint playerSpawnPoint;                 // 플레이어 스폰 위치의 참조를 저장

    public RPGCameraManager cameraManager;              // 카메라 매니저 참조

    /// <summary>
    /// 싱글톤 메서드
    /// </summary>
    /// <remarks>
    /// if(sharedInstance != null && sharedInstance != this)
    ///     조건:
    ///         1. sharedInstance != null
    ///         2. sharedInstance != this
    ///         
    /// Destroy(Object obj): void
    ///     sharedInstance가 이미 존재하고 다르다면 제거한다.
    ///     
    /// sharedInstance = this
    ///     현재 인스턴스가 유일한 인스턴스면 sharedInstance 변수에
    ///     현재 오브젝트를 대입한다.
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
    /// 씬의 설정에 관한 메서드
    /// </summary>
    public void SetScene()
    {
        SpawnPlayer();
    }

    /// <summary>
    /// 플레이어를 스폰시키는 메서드
    /// </summary>
    /// <remarks>
    /// cameraManager.virtualCamera.Follow = player.transform
    ///     virtualCamera.Follow 속성에 플레이어 오브젝트의 트랜스폼을 설정하는 코드
    ///     가상 카메라가 플레이어를 따라다리게 된다.
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
