using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("�����ƶ�����ͨ�ٶ�")]
    public float rabbitSpeed;
    [Header("���ӿ����ƶ��ļӳɱ���")]
    public float extraSpeedPercent;
    [Header("�����ƶ��ٶ�")]
    public float moleSpeed;
    [SerializeField]
    [Header("����Ԥ����")]
    private GameObject molePrefab;


    [SerializeField]
    [Header("��")]
    private Root root;
    [SerializeField]
    [Header("��ҽ�ɫ")]
    private GameObject playerBody;

    private GameObject moleInstance;
    private InputCalculator calculator;

    /// <summary>
    /// The world position of player body.
    /// </summary>
    public Vector3 PlayerPosition { get => playerBody.transform.position; }

    public Mole Mole
    {
        get => moleInstance.GetComponent<Mole>();
    }

    public void Climb(float deltaDistance)
    {
        root.RootLength -= deltaDistance; //�����ƶ���ζ�Ÿ�Ҫ����
        playerBody.transform.position += Vector3.up * deltaDistance;
    }
    public void MoveMole(float offset)
    {
        moleInstance.transform.position += Vector3.right * offset;
    }

    void OperationHandle(E_PlayerOperation[] operations)
    {

        Rect worldAreaRect = GameController.Instance.MapController.MapWorldRect;

        foreach (E_PlayerOperation operation in operations)
        {
            switch (operation)
            {
                case E_PlayerOperation.ClimbUp:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����9.5f
                    if (playerBody.transform.position.y < worldAreaRect.yMax)
                    {
                        Climb(rabbitSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����-8f
                    if (playerBody.transform.position.y > worldAreaRect.yMin)
                    {
                        Climb(-rabbitSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //���ӽ�ɫ���������ٶ������ƶ�,����y�᲻����9.5f
                    if (playerBody.transform.position.y < worldAreaRect.yMax)
                    {
                        Climb(rabbitSpeed * extraSpeedPercent * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbDownQuick:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����-8f
                    if (playerBody.transform.position.y > worldAreaRect.yMin)
                    {
                        Climb(-rabbitSpeed * extraSpeedPercent * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //�����ɫ�����ƶ�,����y�᲻С��-7f
                    if (moleInstance.transform.position.x > worldAreaRect.xMin)
                    {
                        MoveMole(-moleSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //�����ɫ�����ƶ�,����y�᲻����7f
                    if (moleInstance.transform.position.x < worldAreaRect.xMax)
                    {
                        MoveMole(moleSpeed * Time.deltaTime);
                    }
                    break;
                default:
                    break;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        moleInstance = Instantiate(molePrefab);

        calculator = new InputCalculator();

        //calculator.ClimbUpKey = (MouseSource)0;
        //calculator.ClimbDownKey = (MouseSource)1;
        calculator.ClimbUpKey = (KeyCodeSource)KeyCode.W;
        calculator.ClimbDownKey = (KeyCodeSource)KeyCode.S;
        calculator.MoveLeftKey = (KeyCodeSource)KeyCode.LeftArrow;
        calculator.MoveRightKey = (KeyCodeSource)KeyCode.RightArrow;
        calculator.StartListening();

    }

    // Update is called once per frame
    void Update()
    {
        OperationHandle(calculator.GetOperation());
    }






}
