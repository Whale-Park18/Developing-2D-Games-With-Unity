using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // ��ũ��Ʈ���� UI ��Ҹ� ����ϱ� ���� ����Ʈ �ؾ��ϴ� ���ӽ����̽�

public class HealthBar : MonoBehaviour
{
    public HitPoints hitPoints; // ��ũ���� ������ ������Ʈ HitPoints ������ ����Ű�� ����

    [HideInInspector] // �ν����Ϳ��� ���� ������ �ʵ��� Ư�� ����(���ȣ([])�� Ư���� ��Ÿ��)
    public Player character;    // maxHitPoints�� ��� ���� Player ������Ʈ�� ������ �ʿ�
                                // �� ������ ����Ƽ �����Ͱ� �ƴ϶� �ڵ带 ���� �����ؾ� ������
                                // �򰥸��� �ʰ� �ν����Ϳ��� �����.

    public Image meterImage;    // Meter �̹��� ������Ʈ�� ã�� �ʿ���� ���� �Ӽ�

    public Text hpText;         // ���ǻ� ���� �Ӽ�

    float maxHitPoints;         // ���� ������ ����� ü���� �ִ밪�� �ٲ��� �ʾ� ���ÿ� ����

    /// <summary>
    /// ��ũ��Ʈ Ȱ��ȭ �Ǿ��� ��, �� �ѹ� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// maxHitPoints = character.maxHitPoints
    ///     character���� �ִ� ü�� ���� �����ͼ� �����Ѵ�.
    /// </remarks>
    void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    /// <summary>
    /// �����Ӵ� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// meterImage.fillAmount = hitPoints.value / maxHitPoints
    ///     Meter�� Image ������Ʈ�� "ä��� ��" ���� �����Ѵ�.
    /// 
    /// hpText.text = "HP:" + (meterImage.fillAmount * 100)
    ///     HPText�� �ؽ�Ʈ �Ӽ��� ������ �����ִ� ü���� ������ �����ش�.
    ///     fillAmount�� �Ǽ����̱� ������ *100�� ���ش�.
    /// </remarks>
    void Update()
    {
        // character�� null�̸� �������� ���� ��
        if(character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;

            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }
}
