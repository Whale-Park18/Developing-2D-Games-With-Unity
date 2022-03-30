using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CreateAssetMenu: ���� �޴� �ȿ� ���� �޴��� ����� �ش�.
/// �޴��� ���� ��ũ���� ������ ������Ʈ�� Item�� �ν��Ͻ���
/// ���� ���� �� �ִ�.
/// </summary>
[CreateAssetMenu(menuName = "Item")]

/// ����Ƽ���� �����ϴ� �뷮�� �����͸� �����ϴµ� ����� �� �ִ� ������ �����̳�
/// ScriptableObject�� ����ϸ� �纻�� �����Ǵ� ���� �����Ͽ� ������Ʈ�� �޸� ��뷮��
/// ���� �� ������, MonoBehaviour ��ũ��Ʈ�� ������� �ʴ� �����͸� �����ϴ� �������� ����ϴ�
/// ������Ʈ���� �����ϴ�.
/// ��, ScriptableObject�� ���� ������Ʈ�� ������Ʈ�� ������ �� ����, ������Ʈ�� �������� ����ȴ�.
public class Item : ScriptableObject
{
    
    public string objectName;   // �پ��� �������� ����(�����, ��ǰ��, ��� ��)

    
    public Sprite sprite;       // ���ӿ� ǥ���� �� �ִ� �������� ��������Ʈ�� ����Ű�� ������ ����

    
    public int quantity;        // �������� ����
    
    public bool stackable;      // ������ ��ҿ� �׾Ƶ� �� �����鼭 �÷��̾�� ���ÿ� ��ȣ�ۿ��� �� �ִ�
                                // ������ �����۵��� �纻�� ǥ���ϴ� ���
                                // stackable�� �ƴ� �������� �� ���� ���� ���� ó���� �� ����.

    // objectName�� ��Ȳ�� ���� ���� �ȿ��� �÷��̾�� ���� �� �� ������,
    // ItemType �Ӽ��� ���� �÷��̾�� �������� �ʰ� ���� ���� �ȿ��� ������Ʈ��
    // �ĺ��� ���� ����Ѵ�.
    public enum ItemType
    {
        COIN,
        HEALTH
    }

    public ItemType itemType;
}
