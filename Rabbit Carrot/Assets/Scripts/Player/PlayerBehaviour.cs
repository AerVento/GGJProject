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
                    //���ӽ�ɫ�����ƶ�,����y�᲻����9.5f
                    if (this.transform.position.y <= 9.5f)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����-8f
                    if (this.transform.position.y >= -8f)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //���ӽ�ɫ���������ٶ������ƶ�,����y�᲻����9.5f
                    if (this.transform.position.y <= 9.5f)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * 2, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //�����ɫ�����ƶ�,����y�᲻С��-7f
                    if (mole.transform.position.x>-7)
                    {
                        mole.transform.position -= new Vector3(moleSpeed, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //�����ɫ�����ƶ�,����y�᲻����7f
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
