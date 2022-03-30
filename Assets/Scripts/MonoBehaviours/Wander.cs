using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���߿� Wander ��ũ��Ʈ�� �߰��� ���� ������Ʈ����
// ������ٵ� 2D, ��Ŭ �ݶ��̴� 2D, �ִϸ����Ͱ� �־�� �Ѵ�.
// RequireComponent�� ����ؼ� �� ��ũ��Ʈ�� �߰��� ���� ������Ʈ��
// �ʿ��� ������Ʈ�� ������ �ڵ����� �߰��Ѵ�.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Wander : MonoBehaviour
{
    public float pursuitSpeed;  // ���� �÷��̾ �����ϴ� �ӷ�
    public float wanderSpeed;   // ������ ���� �ƴ� ���� ��ȸ �ӷ�
    float currentSpeed;         // ���� �� �߿��� ������ ���� �ӷ�

    public float directionChangeInterval;   // ��ȸ�� ������ ��ȯ ��

    public bool followPlayer;               // �÷��̾ �����ϴ� ����� �Ѱų� �� �� �ִ�
                                            // �÷���

    Coroutine moveCoroutine;                // �̵� �ڷ�ƾ ����
    
    Rigidbody2D rb2d;
    Animator animator;
    CircleCollider2D circleCollider;

    Transform targetTransform = null;       // ���� �÷��̾ ������ �� ���
    
    Vector3 endPosition;                    // ��ȸ�ϰ� �ִ� ���� ������

    float currentAngle = 0;                 // ��ȸ�� ������ �ٲ� �� ���� �������� ���ο�
                                            // ������ ���Ѵ�. �� ������ ����ؼ� ��������
                                            // ��Ÿ���� ���͸� �����.

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        currentSpeed = wanderSpeed;

        StartCoroutine(WanderRoutine());
    }

    /// <summary>
    /// �����Ӵ� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// Debug.DrawLine(vector3 start, vector3 end, Color color)
    /// Debug.DrawLine(rb2d.position, endPosition, Color.red)
    /// </remarks>
    private void Update()
    {
        Debug.DrawLine(rb2d.position, endPosition, Color.red);
    }

    /// <summary>
    /// ���� ������Ʈ�� �ݶ��̴� 2D Ʈ���ſ� �ٸ� ������Ʈ �ݶ��̴� 2D�� ����� ��
    /// ����Ƽ �������� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="collision">�浿�� ���� ����</param>
    /// <remarks>
    /// collision.gameObject.CompareTag("Player") && followPlayer
    ///     �浹�� ���� ������Ʈ�� Player && ���� �÷��̾� ���������� Ȯ��
    ///     ! followPlayer�� �ʱ�ȭ ���� �ʴµ� �� ����ϴ��� �ǹ��� ���� ���� ���� �ȵ�
    ///     
    ///     currentSpeed = pursuitSpeed
    ///     targetTransform = collision.gameObject.transform
    ///         ���� �ӵ��� ���� ���ǵ�� �ʱ�ȭ �ϰ� 
    ///         ������ ���� Transform�� �÷��̾��� transform���� �ʱ�ȭ �Ѵ�.
    ///         
    ///     moveCoroutine != null
    ///         ���� ��ȸ ���̶�� ������Ų��.
    ///         
    ///     moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed))
    ///         transform�� ������ �� ȣ���ϴ� Move() �޼���� �÷��̾ �����ϰ� �ȴ�.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        //if(collision.gameObject.CompareTag("Player") && followPlayer)
        {
            print("Wander::OnTriggerEnter2D");
            currentSpeed = pursuitSpeed;

            targetTransform = collision.gameObject.transform;

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� �ݶ��̴� 2D Ʈ���ſ� �浹�ߴ� �ٸ� ������Ʈ �ݶ��̴� 2D�� �������� ��
    /// ����Ƽ �������� ȣ���ϴ� �޼���
    /// </summary>
    /// <param name="collision">�浹�� ���� ����</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Wander::OnTriggerExit2D");
            animator.SetBool("isWalking", false);

            currentSpeed = wanderSpeed;

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            targetTransform = null;
        }
    }

    /// <summary>
    /// ������ ������ ��ȸ ����
    /// </summary>
    /// <returns>�ڷ�ƾ</returns>
    /// <remarks>
    /// ChooseNewEndpoint()
    ///     ���ο� ������ ����
    ///     
    /// if(moveCoroutine != null)
    ///     �ڷ�ƾ�� �̹� ���� ���̶�� �ش� �ڷ�ƾ�� �����Ѵ�.
    ///     
    /// StartCoroutine(Move(rb2d, currentSpeed))
    ///     Move �ڷ�ƾ�� Ȱ��ȭ ��Ų��.
    ///     
    /// yield return new WaitForSeconds(directionChangeInterval)
    ///     directionChangeInterval �ʸ� ����ϴ� �ڷ�ƾ
    /// </remarks>
    public IEnumerator WanderRoutine()
    {
        while(true)
        {
            ChooseNewEndpoint();

            if(moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = StartCoroutine(Move(rb2d, currentSpeed));

            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    /// <summary>
    /// ���ο� �������� �������� �����ϴ� �޼���
    /// </summary>
    /// <remarks>
    /// Random.Range(0, 360)
    ///     0~360�� ������ ���� ������ ����, ���� ������ ���ο� ������ ��Ÿ����.
    ///     "��" ������ ���� ���̴�.
    /// 
    /// Mathf.Repeat(float t, float length)
    /// Mathf.Repeat(currentAngle, 360)
    ///     Mathf.Repeat() �޼���� % �����ڿ� ���������� �־��� ���� ������ ���� ���� �ȿ�
    ///     �� ������ �ݺ��ϹǷ� ��ȯ ���� ���� 0���� �۰ų� 360���� Ŭ �� ����.
    ///     ��, 0���� 360 ������ ���ο� ������ ��ȯ�Ѵ�.
    ///     
    /// endPosition += Vector3FromAngle(currentAngle)
    ///     ���ο� �������� �Էµ� ������ Vector3�� ȯ���� �� endPosition�� ���Ѵ�.
    /// </remarks>
    public void ChooseNewEndpoint()
    {
        currentAngle += Random.Range(0, 360);
        currentAngle = Mathf.Repeat(currentAngle, 360);

        endPosition += Vector3FromAngle(currentAngle);
    }

    /// <summary>
    /// �μ��� ���� ������ ȣ���� ��ȯ�ϰ� ���� ���� Vector3�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="inputAngleDegrees">����</param>
    /// <returns>���� ����</returns>
    /// <remarks>
    /// inputAngleDegrees * Mathf.Deg2Rad
    ///     �Է¹��� ������ ����Ƽ�� �����ϴ� ��ȯ ��� Mathf.Deg2Rad�� ���ؼ�
    ///     �������� ȣ���� �ٲ۴�.
    ///     
    /// Vector3(...)
    ///     ��ȯ�� ȣ���� ����ؼ� ���� �������� ����� ���� ���͸� �����.
    /// </remarks>
    Vector3 Vector3FromAngle(float inputAngleDegrees)
    {
        float inputAngleRadians = inputAngleDegrees * Mathf.Deg2Rad;

        return new Vector3(Mathf.Cos(inputAngleRadians), Mathf.Sin(inputAngleRadians), 0);
    }

    /// <summary>
    /// endP
    /// </summary>
    /// <param name="rigidBodyToMove">������ ������ٵ� 2D</param>
    /// <param name="speed">�ӵ�</param>
    /// <returns>�ڷ�ƾ</returns>
    /// <remarks>
    /// remainingDistance
    ///     ���� ��ġ�� endPosition ���̿� ���� �Ÿ��� �����ϴ� ����
    ///     
    ///     (transform.position - endPosition).sqrMagnitude
    ///         transform.position - endPosition
    ///             �� �ǿ������� Ÿ���� Vector3�̴�
    ///         
    ///     sqrMagnitude
    ///         Vector3�� �Ӽ�, ���� ���� ��ġ�� ������ ������ �뷫���� �Ÿ��� ���Ѵ�.
    ///         
    /// targetTransform != null
    ///     �÷��̾ �������̶�� endPosition�� �÷��̾��� ��ġ�� ���Ž�Ų��.
    ///     
    /// rigidBodyToMove != null
    ///     Move() �޼���� ������ٵ� 2D�� ����ؼ� ���� �����̹Ƿ� ������ٵ� 2D�� �� �ʿ��ϴ�.
    ///     
    ///     animator.SetBool(string name, bool value)
    ///     animator.SetBool("isWalking", true)
    ///         �ִϸ��̼� �Ķ����� isWalking�� true�� ����
    ///         
    ///     Vector3.MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
    ///         current: ���� ��ġ, target: ���� ��ġ, maxDistanceDelta: ������ �ȿ� �̵��� �Ÿ�
    ///     Vector3.MoveTowards(rigidBodyToMove.position, endPosition, speed* Time.deltaTime)
    ///         MoveTowards() �޼���� ������ٵ� 2D�� �������� ����� �� ����Ѵ�. ������ 
    ///         ������ٵ� 2D�� �������� �ʴ´�. ���� ���� ������, ���� �����ϰ� ��ȸ �������� ����
    ///         speed ���� �ٲ� �� �ִ�.
    ///         
    ///     Rigidbody2D.MovePosition(Vector2 position)
    ///     rb2d.MovePosition(newPosition)
    ///         MovePosition()�� ����Ͽ� ������ٵ� 2D�� �ռ� ����� newPosition���� �ű��.
    ///         
    ///     remainingDistance = ...
    ///         ���� �Ÿ��� �ٽ� ����Ѵ�.
    ///         
    /// yield return new WaitForFixedUpdate()
    ///     ���� ���� ������ ������Ʈ���� ������ �纸�Ѵ�.
    ///     
    /// animator.SetBool("isWalking", false)
    ///     ���� endPosition�� �����ؼ� ���ο� ������ ������ ��ٸ���. ���� �ִϸ��̼� ���¸�
    ///     ��� ���·� �����Ѵ�.
    /// </remarks>
    public IEnumerator Move(Rigidbody2D rigidBodyToMove, float speed)
    {
        float remainingDistance = (transform.position - endPosition).sqrMagnitude;

        while(remainingDistance > float.Epsilon)
        {
            if(targetTransform != null)
            {
                endPosition = targetTransform.position;
            }

            if(rigidBodyToMove != null)
            {
                animator.SetBool("isWalking", true);

                Vector3 newPosition = Vector3.MoveTowards(rigidBodyToMove.position, endPosition, 
                    speed * Time.deltaTime);

                rb2d.MovePosition(newPosition);

                remainingDistance = (transform.position - endPosition).sqrMagnitude;
            }

            yield return new WaitForFixedUpdate();
        }

        animator.SetBool("isWalking", false);
    }

    /// <summary>
    /// ����� �����ϴ� �޼���
    /// </summary>
    /// <remarks>
    /// Gizmos.DrawWireSphere(Vector3 center, float radius)
    /// Gizmos.DrawWireSphere(transform.position, circleCollider.radius)
    /// </remarks>
    private void OnDrawGizmos()
    {
        if(circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
}
