using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;                   // Slot �������� ������ �����Ѵ�. ���߿� 
                                                    // ����Ƽ �����Ϳ��� �߰��Ѵ�.
                                                    // Inventory ��ũ��Ʈ�� �� ���������� ���� ����
                                                    // ���纻�� �ν��Ͻ�ȭ�ؼ� �κ��丮 �������� ����Ѵ�.

    public const int numSloths = 5;                 // �κ��丮 ���� ���� ����

    Image[] itemImages = new Image[numSloths];      // sprite �Ӽ��� ���� Image ������Ʈ �迭
                                                    // �κ��丮�� �������� �߰��� �� �� sprite �Ӽ���
                                                    // �������� �����ϴ� ��������Ʈ�� �����Ѵ�.

    Item[] items = new Item[numSloths];             // �÷��̾ �ֿ� �������� ��ũ���� ������ ������Ʈ
                                                    // Item�� ������ �����ϴ� �迭

    GameObject[] slots = new GameObject[numSloths]; // Slot �������� ���� �߿� �������� �ν��Ͻ���
                                                    // �����Ѵ�.

    /// <summary>
    /// ��ũ��Ʈ�� �������ڸ��� �Ҹ��� �޼���
    /// </summary>
    void Start()
    {
        CreateSlots();
    }

    /// <summary>
    /// ������ �����ϴ� �޼���
    /// </summary>
    /// <remarks>
    /// slotPrefab != null
    ///     ����Ƽ �����͸� ���� �����ߴ��� Ȯ��
    /// 
    /// Instantiate(GameObject original) : GameObject
    ///     original ���ӿ�����Ʈ�� �ν��Ͻ��� ��ȯ�ϴ� �޼���
    /// 
    /// newSlot.transform.SetParent(Transform p) : void
    ///     Transform�� �θ� �����ϴ� �޼���
    /// 
    ///     �Ű�����:
    ///         gameObject.transform.GetChild(0).transform
    ///             gameObject:
    ///                 ��ũ��Ʈ�� ����Ʈ�� ���� ������Ʈ,
    ///                 Inventory ��ũ��Ʈ�� InventoryObject�� ����Ʈ �� ���̱� ������
    ///                 gameObject�� InventoryObject�� ����Ų��.
    ///             
    ///             GameObject.transform: 
    ///                 ���� ������Ʈ�� Transform ������Ʈ
    ///             
    ///             GetChild(int index) : Transform
    ///                 index�� �ش��ϴ� �ڽ� Transform ��ȯ�ϴ� �޼���
    ///                 gameObject.transform.GetChild(0)�� ���
    ///                 InventoryObject�� �ڽ� ������Ʈ�� InventoryBackground ���̹Ƿ�
    ///                 0��° �ڽ��� InventoryBackgorund�̴�.
    ///                 
    /// newSlot.transform.GetChild(1).GetComponent<Image>();
    ///     newSlot�� Slot �������� �ν��Ͻ�ȭ�� ���̴�.
    ///     Slot�� ���� ������
    ///         Slot
    ///           > Background
    ///           > ItemImage
    ///     �̱� ������ 1��° �ڽ��� ItemImage�̴�.
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
    /// �κ��丮�� �������� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="itemToAdd">�߰��� Item������ ������</param>
    /// <returns>������ �߰� ���� ����</returns>
    /// <remarks>
    /// if(items[i] != null && ...)
    ///     ����:
    ///         1. items[i] != null
    ///             item[i]�� ������ Item�� �ִ°�?
    ///             
    ///         2. items[i].itemType == itemToAdd.itemType
    ///             items[i]�� ������ Item�� �߰��� Item�� Ÿ���� ������?
    ///             
    ///         3. itemToAdd.stackable == true
    ///             �߰��� Item�� stackable �Ӽ��� Ȱ��ȭ �Ǿ� �ִ°�?
    ///             
    ///     if�� ��:
    ///         slots[i].gameObject.GetComponent<Slot>()
    ///             slots[i]�� ������ ���ӿ�����Ʈ���� Slot ��ũ��Ʈ�� ����Ʈ��
    ///             ������Ʈ�� ��ȯ�Ѵ�.
    ///         
    ///         quantityText.enable = true
    ///             ��Ȱ��ȭ �س��� Slot::Background::Tray::QtuText ������Ʈ�� Ȱ��ȭ �Ѵ�.
    ///     
    /// if(items[i] == null)
    ///     ����:
    ///         items[i]�� ������ ���� ���°�?
    ///         
    ///     if�� ��:
    ///         items[i] = Instantiate(itemToAdd_)        
    ///             items[i]�� itemToAdd�� �ν��Ͻ�ȭ �Ͽ� �Ҵ��Ѵ�.
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
                print("items[i] Ȱ��ȭ");
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
