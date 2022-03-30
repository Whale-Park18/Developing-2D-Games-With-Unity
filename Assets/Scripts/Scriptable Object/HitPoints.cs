using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CreateAssetMenu: ���� �޴� �ȿ� ���� �޴��� ����� �ش�.
/// �޴��� ���� ��ũ���� ������ ������Ʈ�� Item�� �ν��Ͻ���
/// ���� ���� �� �ִ�.
/// </summary>
[CreateAssetMenu(menuName = "HitPoints")]


/// ����Ƽ���� �����ϴ� �뷮�� �����͸� �����ϴµ� ����� �� �ִ� ������ �����̳�
/// ScriptableObject�� ����ϸ� �纻�� �����Ǵ� ���� �����Ͽ� ������Ʈ�� �޸� ��뷮��
/// ���� �� ������, MonoBehaviour ��ũ��Ʈ�� ������� �ʴ� �����͸� �����ϴ� �������� ����ϴ�
/// ������Ʈ���� �����ϴ�.
/// ��, ScriptableObject�� ���� ������Ʈ�� ������Ʈ�� ������ �� ����, ������Ʈ�� �������� ����ȴ�.
public class HitPoints : ScriptableObject
{
    // HeartBar�� ���� Meter �̹��� ������Ʈ�� ä��� �� �Ӽ��� ���� �����ؾ�
    // ������ ó������ ü�� ���� float�� ����� ������ ���ϴ�.
    public float value;
}
