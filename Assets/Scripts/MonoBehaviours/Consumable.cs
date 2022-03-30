using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스크립팅 가능한 오브젝트는 MonoBehaviour를 상속하지 않으므로 게임 오브젝트에 추가할 수 없다.
/// 따라서 MonoBehaviour를 상속하며 Item의 참조를 저장하는 스크립트를 이용한다.
/// </summary>
public class Consumable : MonoBehaviour
{
    public Item item;
}
