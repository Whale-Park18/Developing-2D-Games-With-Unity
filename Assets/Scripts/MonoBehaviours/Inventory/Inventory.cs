using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;                   // Slot 프리팹의 참조를 저장한다. 나중에 
                                                    // 유니티 에디터에서 추가한다.
                                                    // Inventory 스크립트는 이 프리팹으로 여러 개의
                                                    // 복사본을 인스턴스화해서 인벤토리 슬롯으로 사용한다.

    public const int numSloths = 5;                 // 인벤토리 바의 슬롯 개수

    Image[] itemImages = new Image[numSloths];      // sprite 속성을 가진 Image 컴포넌트 배열
                                                    // 인벤토리에 아이템을 추가할 때 이 sprite 속성에
                                                    // 아이템이 참조하는 스프라이트를 설정한다.

    Item[] items = new Item[numSloths];             // 플레이어가 주운 아이템인 스크립팅 가능한 오브젝트
                                                    // Item의 참조를 저장하는 배열

    GameObject[] slots = new GameObject[numSloths]; // Slot 프리팹은 실행 중에 동적으로 인스턴스로
                                                    // 생성한다.

    /// <summary>
    /// 스크립트를 시작하자마자 불리는 메서드
    /// </summary>
    void Start()
    {
        CreateSlots();
    }

    /// <summary>
    /// 슬롯을 생성하는 메서드
    /// </summary>
    /// <remarks>
    /// slotPrefab != null
    ///     유니티 에디터를 통해 설정했는지 확인
    /// 
    /// Instantiate(GameObject original) : GameObject
    ///     original 게임오브젝트의 인스턴스를 반환하는 메서드
    /// 
    /// newSlot.transform.SetParent(Transform p) : void
    ///     Transform의 부모를 설정하는 메서드
    /// 
    ///     매개변수:
    ///         gameObject.transform.GetChild(0).transform
    ///             gameObject:
    ///                 스크립트를 임포트한 게임 오브젝트,
    ///                 Inventory 스크립트는 InventoryObject에 임포트 될 것이기 때문에
    ///                 gameObject는 InventoryObject를 가리킨다.
    ///             
    ///             GameObject.transform: 
    ///                 게임 오브젝트의 Transform 컴포넌트
    ///             
    ///             GetChild(int index) : Transform
    ///                 index에 해당하는 자식 Transform 반환하는 메서드
    ///                 gameObject.transform.GetChild(0)의 경우
    ///                 InventoryObject의 자식 오브젝트는 InventoryBackground 뿐이므로
    ///                 0번째 자식은 InventoryBackgorund이다.
    ///                 
    /// newSlot.transform.GetChild(1).GetComponent<Image>();
    ///     newSlot은 Slot 프리팹을 인스턴스화한 것이다.
    ///     Slot의 계층 구조은
    ///         Slot
    ///           > Background
    ///           > ItemImage
    ///     이기 때문에 1번째 자식은 ItemImage이다.
    ///             
    /// </remarks>
    public void CreateSlots()
    {
        if(slotPrefab != null)
        {
            for(int i = 0; i < numSloths; i++)
            {
                GameObject newSlot = Instantiate(slotPrefab);
                newSlot.name = "ItemSlot_" + i;
                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                itemImages[i] = newSlot.transform.GetChild(1).GetComponent<Image>(); ;
            }
        }
    }

    /// <summary>
    /// 인벤토리에 아이템을 추가하는 메서드
    /// </summary>
    /// <param name="itemToAdd">추가할 Item형식의 아이템</param>
    /// <returns>아이템 추가 성공 여부</returns>
    /// <remarks>
    /// if(items[i] != null && ...)
    ///     조건:
    ///         1. items[i] != null
    ///             item[i]에 참조된 Item이 있는가?
    ///             
    ///         2. items[i].itemType == itemToAdd.itemType
    ///             items[i]에 참조된 Item과 추가할 Item의 타입이 같은가?
    ///             
    ///         3. itemToAdd.stackable == true
    ///             추가할 Item의 stackable 속성이 활성화 되어 있는가?
    ///             
    ///     if문 블럭:
    ///         slots[i].gameObject.GetComponent<Slot>()
    ///             slots[i]에 참조된 게임오브젝트에서 Slot 스크립트가 임포트된
    ///             컴포넌트를 반환한다.
    ///         
    ///         quantityText.enable = true
    ///             비활성화 해놨던 Slot::Background::Tray::QtuText 컴포넌트를 활성화 한다.
    ///     
    /// if(items[i] == null)
    ///     조건:
    ///         items[i]에 참조된 값이 없는가?
    ///         
    ///     if문 블럭:
    ///         items[i] = Instantiate(itemToAdd_)        
    ///             items[i]에 itemToAdd를 인스턴스화 하여 할당한다.
    /// </remarks>
    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null &&
                items[i].itemType == itemToAdd.itemType &&
                itemToAdd.stackable == true)
            {
                items[i].quantity = items[i].quantity + 1;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;

                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();

                return true;
            }

            if(items[i] == null)
            {
                print("items[i] 활성화");
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;

                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;

                return true;
            }
        }

        return false;
    }
}
