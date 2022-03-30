using UnityEngine;
using Cinemachine; // 시네머신 클래스와 데이터 형식 사용하기 위해 임포트

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;  // 시네머신 가상 카메라를 참조,
                                                    // 다른 클래스에서 사용할 수 있도록 public으로 설정,
                                                    // 하지만 코드를 통해 설정할 속성이므로 인스펙터에
                                                    // 나타나지 않도록 설정해준다.

    /// <summary>
    /// 싱글톤 패턴 구현하는 메서드
    /// </summary>
    /// <remarks>
    /// if(sharedInstance != null && ...)
    ///     싱글톤 구현
    ///     
    /// GameObject.FindWithTag(string tag) : GameObject
    ///     현재 씬에 tag라는 태그가 있는 게임 오브젝트를 반환하는 메서드
    ///     
    ///     하나의 게임 오브젝트에 각각 다른 기능을 제공하는 컴포넌트 여러 개를
    ///     추가할 수 있다는 점을 생각하자. 이런 방식을 컴포지션(Composition) 디자인패턴이라 한다.
    ///     
    /// virtualCamera = ...
    ///     직교 크기, Follow 등 가상 카메라의 모든 속성은 유니티 에디터뿐만 아니라
    ///     스크립트를 통해서도 설정할 수 있다 가상 카메라 컴포넌트를 저장해 두면 코드를 통해
    ///     가상 카메라의 속성을 제어할 수 있다.
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
