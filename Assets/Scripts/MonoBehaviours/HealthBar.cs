using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 스크립트에서 UI 요소를 사용하기 위해 임포트 해야하는 네임스페이스

public class HealthBar : MonoBehaviour
{
    public HitPoints hitPoints; // 스크립팅 가능한 오브젝트 HitPoints 에셋을 가리키는 참조

    [HideInInspector] // 인스펙터에서 값이 나오지 않도록 특성 선언(대괄호([])는 특성을 나타냄)
    public Player character;    // maxHitPoints를 얻기 위해 Player 오브젝트의 참조가 필요
                                // 이 참조는 유니티 에디터가 아니라 코드를 통해 설정해야 함으로
                                // 헷갈리지 않게 인스펙터에서 감춘다.

    public Image meterImage;    // Meter 이미지 오브젝트를 찾을 필요없게 만든 속성

    public Text hpText;         // 편의상 만든 속성

    float maxHitPoints;         // 예제 게임의 설계상 체력의 최대값이 바뀌지 않아 로컬에 저장

    /// <summary>
    /// 스크립트 활성화 되었을 때, 딱 한번 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// maxHitPoints = character.maxHitPoints
    ///     character에서 최대 체력 값을 가져와서 저장한다.
    /// </remarks>
    void Start()
    {
        maxHitPoints = character.maxHitPoints;
    }

    /// <summary>
    /// 프레임당 호출되는 메서드
    /// </summary>
    /// <remarks>
    /// meterImage.fillAmount = hitPoints.value / maxHitPoints
    ///     Meter의 Image 컴포넌트의 "채우기 양" 값을 설정한다.
    /// 
    /// hpText.text = "HP:" + (meterImage.fillAmount * 100)
    ///     HPText의 텍스트 속성을 수정해 남아있는 체력을 정수로 보여준다.
    ///     fillAmount가 실수형이기 때문에 *100을 해준다.
    /// </remarks>
    void Update()
    {
        // character가 null이면 참조하지 못한 것
        if(character != null)
        {
            meterImage.fillAmount = hitPoints.value / maxHitPoints;

            hpText.text = "HP:" + (meterImage.fillAmount * 100);
        }
    }
}
