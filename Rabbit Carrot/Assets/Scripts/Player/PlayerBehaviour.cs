using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float rabbitSpeed;
    public float moleSpeed;
    public GameObject mole;

    void OperationHandle(E_PlayerOperation[] operations)
    {
        foreach (E_PlayerOperation operation in operations)
        {
            switch (operation)
            {
                case E_PlayerOperation.ClimbUp:
                    //兔子角色向上移动,并且y轴不超过9.5f
                    if (this.transform.position.y <= 9.5f)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //兔子角色向下移动,并且y轴不低于-8f
                    if (this.transform.position.y >= -8f)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //兔子角色以两倍的速度向上移动,并且y轴不超过9.5f
                    if (this.transform.position.y <= 9.5f)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * 2, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //鼹鼠角色向左移动,并且y轴不小于-7f
                    if (mole.transform.position.x>-7)
                    {
                        mole.transform.position -= new Vector3(moleSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //鼹鼠角色向右移动,并且y轴不超过7f
                    if (mole.transform.position.x < 7)
                    {
                        mole.transform.position += new Vector3(moleSpeed, 0);
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
    }

    // Update is called once per frame
    void Update()
    {
        //OperationHandle();
    }






}
