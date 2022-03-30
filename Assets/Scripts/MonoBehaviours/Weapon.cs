using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject ammoPrefab;

    static List<GameObject> ammoPool;   // AmmoObject�� �����ϴ� ������Ʈ Ǯ
    public int poolSize;                // AmmoObject�� �����ϴ� ������Ʈ Ǯ�� ũ��

    public float weaponVelocity;        // �߻��� �Ѿ��� �ӷ�

    bool isFiring;                      // �÷��̾ ������ ��� ������ Ȯ���ϴ� �÷���

    [HideInInspector]
    public Animator animator;           // �ִϸ����� ����

    Camera localCamera;                 // ī�޶� ����

    float positiveSlope;                // ��и� ��꿡 ����� �� ������ ����
    float negativeSlope;

    /// <summary>
    /// �÷��̾ ������ �߻��� ������ ������ �� ����� ������
    /// </summary>
    enum Quadrant
    {
        East,
        South,
        West,
        North
    }

    /// <summary>
    /// ��ũ��Ʈ �ʱ�ȭ�� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// if(ammoPool == null)
    ///     ������Ʈ Ǯ�� �������� �ʾ����� ������Ʈ Ǯ�� ������
    ///     
    /// for(int i = 0; ...)
    ///     poolSize ��ŭ ������Ʈ�� ������ ��, ������Ʈ Ǯ�� �ִ´�.
    ///     ��, ������Ʈ Ǯ�� ���� ������Ʈ�� ��Ȱ��ȭ �� ä�� ����.
    /// </remarks>
    private void Awake()
    {
        if(ammoPool == null)
        {
            ammoPool = new List<GameObject> ();
        }

        for(int i = 0; i < poolSize; i++)
        {
            GameObject ammoObject = Instantiate(ammoPrefab);
            ammoObject.SetActive(false);
            ammoPool.Add(ammoObject);
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isFiring = false;

        localCamera = Camera.main;

        Vector2 lowerLeft = localCamera.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 upperRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 upperLeft = localCamera.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 lowerRight = localCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0));

        positiveSlope = GetSlope(lowerLeft, upperRight);
        negativeSlope = GetSlope(upperLeft, lowerRight);
    }

    /// <summary>
    /// ������Ʈ Ǯ ammoPool���� AmmoObject�� �����ͼ� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="location">AmmoObject�� ���� ��ġ</param>
    /// <returns>������Ʈ Ǯ���� ������ Ȱ��ȭ�� AmmoObject</returns>
    /// <remarks>
    /// return null
    ///     ��Ȱ��ȭ�� ������Ʈ�� �߰����� ����. ��, Ǯ�� ��� ������Ʈ�� �������
    /// </remarks>
    GameObject SpawnAmmo(Vector3 location)
    {
        foreach(GameObject ammo in ammoPool)
        {
            if(ammo.activeSelf == false)
            {
                ammo.SetActive(true);

                ammo.transform.position = location;

                return ammo;
            }
        }

        return null;
    }

    /// <summary>
    /// AmmoObject�� ������ ��ġ���� ���콺 ��ư�� Ŭ���� ��ġ�� �ű�� �޼���
    /// </summary>
    /// <remarks>
    /// Camera.main.ScreenToWorldPoint(Vector3 positon) : Vector3
    /// Camera.main.ScreenToWorldPoint(Input.mousePosition)
    ///     ���콺�� ȭ�� ������ ����ϹǷ� ȭ�� ������ ���콺 ��ġ�� ���� ���� ��ġ�� ��ȯ�Ѵ�.
    ///     
    /// SpawnAmmo(transform.position)
    ///     ������Ʈ Ǯ���� Ȱ��ȭ�� AmmoObject�� ��´�.
    ///     
    /// travelDuration : AmmoObject�� �� �̵� �ð�
    /// travelDuration = 1.0f / weaponVelocity
    ///     weaponVelocity�� 2.0�̸� travelDuration�� 1.0 / 2.0 �̹Ƿ� 0.5�ʸ��� ȭ����
    ///     �������� �������� �����Ѵ�. �� �Ŀ� ������ �������� �� �ּ��� �Ѿ��� �ӷ��� ��������.
    ///     
    ///     �Ѿ��� �̵� �Ÿ��� ������� 0.5�ʶ�� �̵� �ð��� �������� �ʴ´ٸ� 
    ///     �÷��̾ ���� ���� ����� ������ ������ �߻� ���� ��, ���ѿ��� �� �Ѿ��� ���İ���
    ///     ���� �����Ͽ� �ƿ� ������ ���� ���ɼ��� �ִ�. ����Ī ���� ������ ����� �ִٸ�
    ///     ��� ������ ������ RPG�̹Ƿ� �׻� ���ѿ��� �߻��� �Ѿ��� ���� ���̴� ���� �� ����ְ�
    ///     ��������.
    /// </remarks>
    void FireAmmo()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        GameObject ammo = SpawnAmmo(transform.position);

        if (ammo != null) 
        {
            Arc arcScript = ammo.GetComponent<Arc>();

            float travelDuration = 1.0f / weaponVelocity;

            StartCoroutine(arcScript.TraveArc(mousePosition, travelDuration));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// �����Ӵ� ȣ��Ǵ� �޼���
    /// </summary>
    /// <remarks>
    /// Input.GetMouseButtonDown(int button)
    /// Input.GetMouseButtonDown(0)
    ///     ���콺 ��ư�� �������� �ô��� Ȯ���ϴ� �޼���
    ///     
    ///     �Ű�����
    ///         0: ����
    ///         1: ������
    ///         2: ���
    /// </remarks>
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isFiring = true;
            FireAmmo();
        }

        print("Weapon::Update");
        UpdateState();
    }

    /// <summary>
    /// ���⸦ ����ϴ� �޼���
    /// </summary>
    /// <param name="pointOne">ù ��° ��ǥ</param>
    /// <param name="pointTwo">�� ��° ��ǥ</param>
    /// <returns>����</returns>
    float GetSlope(Vector2 pointOne, Vector2 pointTwo)
    {
        return (pointTwo.y - pointOne.y) / (pointTwo.x - pointOne.x);
    }

    /// <summary>
    /// ���콺�� Ŭ���� ��ġ�� �÷��̾ �������� �ϴ� ���⺸�� ���� ���� ���� �ִ���
    /// ���ϴ� �޼���
    /// </summary>
    /// <param name="inputPosition">���콺�� Ŭ���� ��ġ</param>
    /// <returns>���콺 Ŭ���� y ������ ���� ������ y ������ ���ΰ�?</returns>
    /// <remarks>
    /// y = mx + n
    ///     m : ����
    ///     n : y ����
    ///     
    /// n = y - mx
    /// </remarks>
    bool HigherThanPositiveSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        float yIntercept = playerPosition.y - (positiveSlope * playerPosition.x);

        float inputIntercept = mousePosition.y - (positiveSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    /// <summary>
    /// ���콺�� Ŭ���� ��ġ�� �÷��̾ �������� �ϴ� ���⺸�� ���� ���� ���� �ִ���
    /// ���ϴ� �޼���
    /// </summary>
    /// <param name="inputPosition">���콺�� Ŭ���� ��ġ</param>
    /// <returns>���콺 Ŭ���� y ������ ���� ������ y ������ ���ΰ�?</returns>
    bool HigherThanNegativeSlopeLine(Vector2 inputPosition)
    {
        Vector2 playerPosition = gameObject.transform.position;
        Vector2 mousePosition = localCamera.ScreenToWorldPoint(inputPosition);

        float yIntercept = playerPosition.y - (negativeSlope * playerPosition.x);

        float inputIntercept = mousePosition.y - (negativeSlope * mousePosition.x);

        return inputIntercept > yIntercept;
    }

    /// <summary>
    /// ���콺�� Ŭ���� ��и��� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>���콺�� Ŭ���� ��и�</returns>
    Quadrant GetQuadrant()
    {
        bool higherThanPositiveSlopeLine = HigherThanPositiveSlopeLine(Input.mousePosition);
        bool higherThanNegativeSlopeLine = HigherThanNegativeSlopeLine(Input.mousePosition);

        if(!higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.East;
        }
        else if(higherThanPositiveSlopeLine && higherThanNegativeSlopeLine)
        {
            return Quadrant.South;
        }
        else if(higherThanPositiveSlopeLine && !higherThanNegativeSlopeLine)
        {
            return Quadrant.West;
        }
        else
        {
            return Quadrant.North;
        }
    }

    void UpdateState()
    {
        print("Weapon::UpdateState");
        if(isFiring)
        {
            Vector2 quadrantVector;

            Quadrant quadEnum = GetQuadrant();

            switch(quadEnum)
            {
                case Quadrant.East:
                    quadrantVector = new Vector2(1.0f, 0.0f);
                    break;

                case Quadrant.South:
                    quadrantVector = new Vector2(0.0f, 1.0f);
                    break;

                case Quadrant.West:
                    quadrantVector = new Vector2(-1.0f, 0.0f);
                    break;

                case Quadrant.North:
                    quadrantVector = new Vector2(0.0f, -1.0f);
                    break;

                default:
                    quadrantVector = new Vector2(0.0f, 0.0f);
                    break;
            }

            animator.SetBool("isFiring", true);

            animator.SetFloat("fireXDir", quadrantVector.x);
            animator.SetFloat("fireYDir", quadrantVector.y);

            isFiring = false;
        }
        else
        {
            animator.SetBool("isFiring", false);
        }

    }
}
