using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("兔子移动的普通速度")]
    public float rabbitSpeed;
    [Header("兔子快速移动的加成倍数")]
    public float extraSpeedPercent;
    [Header("鼹鼠移动速度")]
    public float moleSpeed;
    [SerializeField]
    [Header("鼹鼠预制体")]
    private GameObject molePrefab;


    [SerializeField]
    [Header("根")]
    private Root root;
    [SerializeField]
    [Header("玩家角色")]
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
        root.RootLength -= deltaDistance; //向上移动意味着根要缩短
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
                    //兔子角色向上移动,并且y轴不超过9.5f
                    if (playerBody.transform.position.y < worldAreaRect.yMax)
                    {
                        Climb(rabbitSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //兔子角色向下移动,并且y轴不低于-8f
                    if (playerBody.transform.position.y > worldAreaRect.yMin)
                    {
                        Climb(-rabbitSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //兔子角色以两倍的速度向上移动,并且y轴不超过9.5f
                    if (playerBody.transform.position.y < worldAreaRect.yMax)
                    {
                        Climb(rabbitSpeed * extraSpeedPercent * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.ClimbDownQuick:
                    //兔子角色向下移动,并且y轴不低于-8f
                    if (playerBody.transform.position.y > worldAreaRect.yMin)
                    {
                        Climb(-rabbitSpeed * extraSpeedPercent * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //鼹鼠角色向左移动,并且y轴不小于-7f
                    if (moleInstance.transform.position.x > worldAreaRect.xMin)
                    {
                        MoveMole(-moleSpeed * Time.deltaTime);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //鼹鼠角色向右移动,并且y轴不超过7f
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
