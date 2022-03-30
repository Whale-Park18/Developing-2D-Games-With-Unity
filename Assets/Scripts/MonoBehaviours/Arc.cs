using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ AmmoObject�� �����̴� ����
/// </summary>
public class Arc : MonoBehaviour
{
    /// <summary>
    /// �������� ���� ���� ������Ʈ�� �����̴� �޼���
    /// </summary>
    /// <param name="destination">���� ��ġ</param>
    /// <param name="duration">���� ��ġ���� ���� ��ġ���� �����̴� �ð�</param>
    /// <returns></returns>
    /// <remarks>
    /// TraveArc() �޼���� ���� �����ӿ� ���� �����ؾ� �ϹǷ� �ڷ�ƾ���� ������ �����̴�.
    /// 
    /// startPosition
    ///     Player::Weapon�� ������Ʈ Ǯ���� �Ҵ�� AmmoObject�� ��ġ
    ///     
    /// percentComplete += Time.deltaTime / duration
    ///     AmmoObject�� ������ �������� �Ų����� �����̱� ���ؼ� Time.deltaTime�� �̿��Ѵ�.
    ///     �����Ӹ��� �����̰� �ȴٸ� �����ӿ� ���� �����̴� �Ÿ��� �޶����� �ȴ�.
    ///     ���� ������ ���ķ� �帥 �ð��� �Ѿ��� �����̷��� �ð����� ������ ���� �����ӿ�����
    ///     ������� ���� �� �ִ�.
    ///     Time.deltaTime�� ���� �������� �׸� ���ķ� �帥 �ð��̴�. �� ���� ����� 
    ///     percentageComplte�� ���������� ������� ���� �����ӿ����� ������� ���� ��������� 
    ///     �� ������̴�.
    ///     
    /// currentHeight : �������� ���� ����
    /// currentHeight = Mathf.Sin(float f)
    /// currentHeight = Mathf.Sin(Mathf.PI * percentComplete)
    ///     ����(PI)�� ���ֱ���180���� ���Ѵ�. ��, ���� * percentComplete�� 0 ~ 180 ���հ��̱�
    ///     ������ Sin(Mathf.PI * percentCompete)�� �������� �׸��� �ȴ�.
    ///     
    /// Vector3.Lerp(startPosition, destination, percentComplete) + Vector3.up * currentHeight
    ///     Vector3.Lerp(Vector3 a, Vector3 b, float t) : Vector3
    ///     Vector3.Lerp(startPosition, destination, percentComplete)
    ///         AmmoObject�� �� ���� ���̸� ������ �ӷ����� �Ų����� �����̴� ȿ���� ������
    ///         ���� ���α׷��ֿ� �θ� ���̴� ����� ���������� ����ؾ� �Ѵ�. ���������� �Ϸ���
    ///         ���� ����, ���� ����, 0���� 1������ ������� �ʿ��ϴ�. ������������ �����Ӵ� �̵� �Ÿ���
    ///         ���� �� �� �����(percentCOmplete)�� �������� �޼����� Lerp�� ����� �Ű������� ����Ѵ�.
    ///         
    ///         Lerp() �޼��忡 �� ������� ����Ѵٴ� ���� AmmoObject�� ȭ���� ���� �߻��ϴ���
    ///         �����ϴ� �ð��� ���ٴ� ���̴�. �и��� ���� ���迡�� ���� �� ���� ���������� �����̴�.
    ///         ������ ���� ������ ���� ������ ��Ģ�� �Ϻ��ϰ� ������ �ʾƵ� ��� ����.
    ///         
    ///     Vector3.up : ����Ƽ�� �����ϴ� ������ Vector3(0, 1, 0)�� ��Ÿ����.
    ///     Vector3.up * currentHeight
    ///         �������� ���̸� �����Ѵ�.
    ///   
    /// yield return null
    ///      ���� �����ӱ��� �ڷ�ƾ�� ������ �Ͻ� �����Ѵ�.
    /// 
    /// SetActive(false)
    ///     �������� �����ϸ� ������Ʈ�� ��Ȱ��ȭ�Ѵ�.
    /// </remarks>
    public IEnumerator TraveArc(Vector3 destination, float duration)
    {
        var startPosition = transform.position; // ���� ���ӿ�����Ʈ�� ���� ��ġ
        var percentComplete = 0.0f;             // ���������� ����� �� ���Ǵ� ����

        while(percentComplete < 1.0f)
        {
            percentComplete += Time.deltaTime / duration;
            
            var currentHeight = Mathf.Sin(Mathf.PI * percentComplete);

            transform.position = Vector3.Lerp(startPosition, destination, percentComplete)
                + Vector3.up * currentHeight;
            
            yield return null;
        }

        gameObject.SetActive(false);
    }    
}
