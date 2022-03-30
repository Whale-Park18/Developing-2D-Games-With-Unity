using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CreateAssetMenu: 생성 메뉴 안에 하위 메뉴를 만들어 준다.
/// 메뉴를 통해 스크립팅 가능한 오브젝트인 Item의 인스턴스를
/// 쉽게 만들 수 있다.
/// </summary>
[CreateAssetMenu(menuName = "Item")]

/// 유니티에서 제공하는 대량의 데이터를 저장하는데 사용할 수 있는 데이터 컨테이너
/// ScriptableObject를 사용하면 사본이 생성되는 것을 방지하여 프로젝트의 메모리 사용량을
/// 줄일 수 있으며, MonoBehaviour 스크립트에 변경되지 않는 데이터를 저장하는 프리팹을 사용하는
/// 프로젝트에서 유용하다.
/// 단, ScriptableObject는 게임 오브젝트에 컴포넌트로 부착할 수 없고, 프로젝트에 에셋으로 저장된다.
public class Item : ScriptableObject
{
    
    public string objectName;   // 다양한 목적으로 사용됨(디버깅, 상품명, 대사 등)

    
    public Sprite sprite;       // 게임에 표시할 수 있는 아이템의 스프라이트를 가리키는 참조를 저장

    
    public int quantity;        // 아이템의 수량
    
    public bool stackable;      // 동일한 장소에 쌓아둘 수 있으면서 플레이어와 동시에 상호작용할 수 있는
                                // 동일한 아이템들의 사본을 표현하는 용어
                                // stackable이 아닌 아이템은 한 번에 여러 개를 처리할 수 없다.

    // objectName은 상황에 따라 게임 안에서 플레이어에게 보여 줄 수 있지만,
    // ItemType 속성은 절대 플레이어에게 보여주지 않고 게임 로직 안에서 오브젝트를
    // 식별할 때만 사용한다.
    public enum ItemType
    {
        COIN,
        HEALTH
    }

    public ItemType itemType;
}
