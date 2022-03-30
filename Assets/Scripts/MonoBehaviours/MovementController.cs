using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UnityEngine Ŭ���� MonoBehaviour�� ��ӹ���
/// MovementController Ŭ����
/// </summary>
/// <remarks>
/// MonoBehaviour Ŭ����
///     ��� ��ũ��Ʈ�� ��ӹ޴� �⺻ Ŭ����
///     Start(), Awake(), Update() ���� �Լ��� ����� �� �ִ�.
/// </remarks>
public class MovementController : MonoBehaviour
{
    public float movementSpaeed = 3.0f;         // ĳ������ �ӵ��� ������ ����, public���� �����Ͽ� ����Ƽ���� �ٷ� ������ �����ϴ�.
    
    Vector2 movement = new Vector2();           // 2D ��ǥ�� �����ϱ� ���� Vector2 ����
    Animator animator;                          // Animator ������Ʈ�� �����ϱ� ���� ����
    Rigidbody2D rb2D;                           // Rigidbody2D ������Ʈ�� �����ϱ� ���� ����


    /// <summary>
    /// ��ũ��Ʈ�� �������ڸ��� �Ҹ��� �޼���
    /// �ʱ�ȭ ������ ���ȴ�.
    /// </summary>
    /// <remarks>
    /// GetComponent<Type>(): Type
    ///     �ش� ��ũ��Ʈ ������Ʈ�� ���� ���� ������Ʈ�� Type ������Ʈ��
    ///     ã�Ƽ� ��ȯ�Ѵ�.
    /// </remarks>
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// �����Ӵ� ȣ��Ǵ� �޼���
    /// ����� �������� ���� �������� �������� ���
    /// �������� ������ ��ŭ ȣ�� �󵵰� �پ���.
    /// </summary>
    private void Update()
    {
        UpdateState();
    }

    /// <summary>
    /// ������ �������� ȣ��Ǵ� �޼���
    /// Update()�� �޸� �����ӿ� ������� �����ϰ� ȣ��ȴ�.
    /// </summary>
    private void FixedUpdate()
    {
        MoveCharacter();
    }

    /// <summary>
    /// ĳ���͸� �����̴� �޼���
    /// </summary>
    /// <remarks>
    /// Input.GetAxisRaw(axisName : string) : float
    ///     axisName:
    ///         ���� ��(����, ����)
    ///     ��ȯ ��:
    ///         Ű����� ���̽�ƽ �Է°��� ���� -1 ~ 1
    ///         Ű������ ��� -1, 0, 1 �� �ϳ�
    ///     axisName�� ���� ��ȯ ��
    ///         "Horizontal(����)": 
    ///              1: ��
    ///              0: Non
    ///             -1: ��
    ///         "Vertical(����)":
    ///              1: ��
    ///              0: Non
    ///             -1: ��
    /// 
    /// movement(Vector2).Normalize()
    ///     ���͸� ����ȭ(normalize)�ؼ� �÷��̾ �밢��, ����, ����, ��� ��������
    ///     �����̵� ������ �ӷ��� �����ϰ� �Ѵ�.
    ///     
    /// rb2D.velocity = movement * movementSpaeed
    ///     Rigidbody2D.velocity�� �ӵ��� �����ϴ� Vector2 ������ get, set �޼����̴�.
    ///     movement * movementSpeed = Vector2(x * movementSpeed, y * movementSpeed)
    ///     
    /// </remarks>
    private void MoveCharacter()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        rb2D.velocity = movement * movementSpaeed;
    }

    /// <summary>
    /// �ִϸ��̼� ��ȯ�� ���� ���¸� ������Ʈ�ϴ� �޼���
    /// </summary>
    /// <remarks>
    /// if(Mathf.Approximately(...) && ...)
    ///     Mathf.Approximately(float a, float b) : bool
    ///     Mathf.Approximately(movement.x, 0f)
    ///     Mathf.Approximately(movement.y, 0f)
    ///         a, b �ε� �Ҽ��� ���� ���ϴ� �޼���� x�� y ��� 0�̸�
    ///         �÷��̾ ���� �ִ� ���̹Ƿ� isWalking ���� false�� �����Ѵ�.
    ///
    ///     movement.x or y: 
    ///         Input.GetAxisRaw() �޼���� �Էµ� ����Ű�� �� �� ����
    ///         ����⿡ �°� �ִϸ��̼��� ��ȯ��
    ///         
    /// animator.SetFloat(string name, float value) : void
    /// animator.SetFloat("xDir", movement.x)
    /// animator.SetFloat("yDir", movement.y)
    ///     ���ο� movement ������ anumator�� ������Ʈ �Ѵ�.
    /// </remarks>
    private void UpdateState()
    {
        if(Mathf.Approximately(movement.x, 0f) && Mathf.Approximately(movement.y, 0f))
        {
            animator.SetBool("isWalking", false);
        }
        else
        {
            animator.SetBool("isWalking", true);
        }

        animator.SetFloat("xDir", movement.x);
        animator.SetFloat("yDir", movement.y);
        //print(animator.GetBool("isWalking"));
        //print("x: " + animator.GetFloat("xDir") + " y: " + animator.GetFloat("yDir"));
    }
}