using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("�����ƶ�����ͨ�ٶ�")]
    public float rabbitSpeed;
    [Header("���ӿ����ƶ��ļӳɱ���")]
    public float extraSpeedPercent;
    [Header("�����ƶ��ٶ�")]
    public float moleSpeed;
    [SerializeField]
    [Header("��������")]
    private GameObject mole;

    private InputCalculator calculator;

    void OperationHandle(E_PlayerOperation[] operations)
    {
        Rect worldAreaRect = GameController.Instance.MapController.MapWorldRect;
        worldAreaRect.yMin += GameController.Instance.MapController.Grid.cellSize.y; //������������������궼�ڸ��ӵĵױߣ��������ƶ�ʱ�ᴩ�����飬��˼�1�����Ӹ߶�����ֹ����
        foreach (E_PlayerOperation operation in operations)
        {
            switch (operation)
            {
                case E_PlayerOperation.ClimbUp:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����9.5f
                    if (this.transform.position.y < worldAreaRect.yMax)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDown:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����-8f
                    if (this.transform.position.y > worldAreaRect.yMin)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbUpQuick:
                    //���ӽ�ɫ���������ٶ������ƶ�,����y�᲻����9.5f
                    if (this.transform.position.y < worldAreaRect.yMax)
                    {
                        this.transform.position += new Vector3(0, rabbitSpeed * extraSpeedPercent * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.ClimbDownQuick:
                    //���ӽ�ɫ�����ƶ�,����y�᲻����-8f
                    if (this.transform.position.y > worldAreaRect.yMin)
                    {
                        this.transform.position -= new Vector3(0, rabbitSpeed * extraSpeedPercent * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveLeft:
                    //�����ɫ�����ƶ�,����y�᲻С��-7f
                    if (mole.transform.position.x > worldAreaRect.xMin)
                    {
                        mole.transform.position -= new Vector3(moleSpeed * Time.deltaTime, 0);
                    }
                    break;
                case E_PlayerOperation.MoleMoveRight:
                    //�����ɫ�����ƶ�,����y�᲻����7f
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
