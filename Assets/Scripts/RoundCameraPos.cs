using UnityEngine;
using Cinemachine;  // �ó׸ӽ� ���ӽ����̽�

/// <summary>
/// �ó׸ӽ��� ���μ��� ���������ΰ� ������ ������Ʈ��
/// CinemachineExtension�� ����ؾ� �Ѵ�.
/// </summary>
public class RoundCameraPos : CinemachineExtension
{
    /// <summary>
    /// ������ �ȼ�
    /// </summary>
    public float PixelsPerUnit = 32;

    /// <summary>
    /// CinemachineExtension�� ����� Ŭ������� �ݵ�� �����ؾ� �ϴ� �޼����
    /// �������� ó���� ������ �ó׸ӽ��� ȣ���ϴ� �޼����.
    /// </summary>
    /// <param name="vcam">Unity ��� ������ ���� ī�޶� ��Ÿ���� Monobehaviour�� �⺻ Ŭ����</param>
    /// <param name="stage"></param>
    /// <param name="state"></param>
    /// <param name="deltaTime"></param>
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        /// �ó׸ӽ� ���� ī�޶�� ���� �ܰ�� �̷��� ����Ʈ ���μ��� ������������ ���ϰ� �ִ�.
        /// �� �ڵ�� ���� ī�޶��� ����Ʈ ���μ��� �ܰ谡 "Body" �ܰ����� Ȯ���Ѵ�.
        /// ������ ���� ī�޶��� ���� ��ġ�� ������ �� �ִ�.
        if (stage == CinemachineCore.Stage.Body)
        {
            /// ���� ī�޶��� ���� ��ġ�� ��´�.
            Vector3 fianlPos = state.FinalPosition;

            /// ���߿� �ۼ��� �ݿø� �޼��带 ȣ���ؼ� ��ġ�� �ݿø��ϰ�,
            /// �ݿø��� ����� ���ο� ���͸� �����Ѵ�. �� ���ʹ� ���ο� �ȼ� ���� ��ġ��.
            Vector3 pos2 = new Vector3(Round(fianlPos.x), Round(fianlPos.y), fianlPos.z);

            /// ���� ��ġ�� ��� �ݿø��ؼ� ����� ��ġ�� ���̸� ���� ī�޶��� ��ġ�� �ݿ��Ѵ�.
            state.PositionCorrection += pos2 - fianlPos;
        }
    }

    /// <summary>
    /// �Է°��� �ݿø��ϴ� �޼���
    /// �� �޼��带 ����ؼ� ī�޶� �� �ȼ� ������ �ӹ��� �Ѵ�.
    /// </summary>
    /// <param name="x">�ݿø� �� ��</param>
    /// <returns></returns>
    float Round(float x)
    {
        return Mathf.Round(x * PixelsPerUnit) / PixelsPerUnit;
    }
}
