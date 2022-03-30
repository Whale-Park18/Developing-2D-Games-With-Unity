using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CreateAssetMenu: 생성 메뉴 안에 하위 메뉴를 만들어 준다.
/// 메뉴를 통해 스크립팅 가능한 오브젝트인 Item의 인스턴스를
/// 쉽게 만들 수 있다.
/// </summary>
[CreateAssetMenu(menuName = "HitPoints")]


/// 유니티에서 제공하는 대량의 데이터를 저장하는데 사용할 수 있는 데이터 컨테이너
/// ScriptableObject를 사용하면 사본이 생성되는 것을 방지하여 프로젝트의 메모리 사용량을
/// 줄일 수 있으며, MonoBehaviour 스크립트에 변경되지 않는 데이터를 저장하는 프리팹을 사용하는
/// 프로젝트에서 유용하다.
/// 단, ScriptableObject는 게임 오브젝트에 컴포넌트로 부착할 수 없고, 프로젝트에 에셋으로 저장된다.
public class HitPoints : ScriptableObject
{
    // HeartBar에 속한 Meter 이미지 오브젝트의 채우기 양 속성에 값을 대입해야
    // 함으로 처음부터 체력 값을 float로 만들어 놓으면 편하다.
    public float value;
}
