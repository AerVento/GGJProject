using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("兔子移动的普通速度")]
    public float rabbitSpeed;
    [Header("兔子快速移动的加成倍数")]
    public float extraSpeedPercent;
    [Header("鼹鼠移动速度")]
    public float moleSpeed;
    [SerializeField]
    [Header("鼹鼠物体")]
    private GameObject mole;

    private InputCalculator calculator;

    void OperationHandle(E_PlayerOperation[] operations)
    {
        Rect worldAreaRect = GameController.Instance.MapController.MapWorldRect;
        worldAreaRect.yMin += GameController.Instance.MapController.Grid.cellSize.y; //格子算出来的世界坐标都在格子的底边，在向下移动时会穿过方块，因此加1个格子高度来防止出界
        foreach (E_PlayerOperation operation in operations)
        {
            switch (operation)
            {
                case E_PlayerOperation.ClimbUp:
                    //兔子角色向上移动,并且y轴不超过9.5f
                    if (this.transform.position.y < worldAreaRect.yMax)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //兔子角色向下移动,并且y轴不低于-8f
                    if (this.transform.position.y > worldAreaRect.yMin)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //兔子角色以两倍的速度向上移动,并且y轴不超过9.5f
                    if (this.transform.position.y < worldAreaRect.yMax)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * extraSpeedPercent * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDownQuick:
                    //兔子角色向下移动,并且y轴不低于-8f
                    if (this.transform.position.y > worldAreaRect.yMin)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed * extraSpeedPercent * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //鼹鼠角色向左移动,并且y轴不小于-7f
                    if (mole.transform.position.x > worldAreaRect.xMin)
                    {
                        mole.transform.position -= new Vector3(moleSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //鼹鼠角色向右移动,并且y轴不超过7f
                    if (mole.transform.position.x < worldAreaRect.xMax)
                    {
                        mole.transform.position += new Vector3(moleSpeed * Time.deltaTime, 0);
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
        calculator = new InputCalculator();

        calculator.ClimbUpKey = (KeyCodeSource)KeyCode.W;
        calculator.ClimbDownKey = (KeyCodeSource)KeyCode.S;
        calculator.MoveLeftKey = (KeyCodeSource)KeyCode.A;
        calculator.MoveRightKey = (KeyCodeSource)KeyCode.D;
        calculator.StartListening();
    }

    // Update is called once per frame
    void Update()
    {
        OperationHandle(calculator.GetOperation());
    }






}
