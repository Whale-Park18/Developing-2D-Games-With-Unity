using UnityEngine;
using Cinemachine;  // 시네머신 네임스페이스

/// <summary>
/// 시네머신의 프로세싱 파이프라인과 연결할 컴포넌트는
/// CinemachineExtension을 상속해야 한다.
/// </summary>
public class RoundCameraPos : CinemachineExtension
{
    /// <summary>
    /// 단위당 픽셀
    /// </summary>
    public float PixelsPerUnit = 32;

    /// <summary>
    /// CinemachineExtension을 상속한 클래스라면 반드시 구현해야 하는 메서드로
    /// 제한자의 처리가 끝나고 시네머신이 호출하는 메서드다.
    /// </summary>
    /// <param name="vcam">Unity 장면 내에서 가상 카메라를 나타내는 Monobehaviour의 기본 클래스</param>
    /// <param name="stage"></param>
    /// <param name="state"></param>
    /// <param name="deltaTime"></param>
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        /// 시네머신 가상 카메라는 여러 단계로 이뤄진 포스트 프로세싱 파이프라인을 지니고 있다.
        /// 이 코드는 현재 카메라의 포스트 프로세싱 단계가 "Body" 단계인지 확인한다.
        /// 맞으면 가상 카메라의 공간 위치를 설정할 수 있다.
        if (stage == CinemachineCore.Stage.Body)
        {
            /// 가상 카메라의 최종 위치를 얻는다.
            Vector3 fianlPos = state.FinalPosition;

            /// 나중에 작성할 반올림 메서드를 호출해서 위치를 반올림하고,
            /// 반올림한 결과로 세로운 벡터를 생성한다. 이 벡터는 새로운 픽셀 단위 위치다.
            Vector3 pos2 = new Vector3(Round(fianlPos.x), Round(fianlPos.y), fianlPos.z);

            /// 원래 위치와 방금 반올림해서 계산한 위치의 차이를 가상 카메라의 위치에 반영한다.
            state.PositionCorrection += pos2 - fianlPos;
        }
    }

    /// <summary>
    /// 입력값을 반올림하는 메서드
    /// 이 메서드를 사용해서 카메라가 늘 픽셀 단위에 머물게 한다.
    /// </summary>
    /// <param name="x">반올림 할 값</param>
    /// <returns></returns>
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}
