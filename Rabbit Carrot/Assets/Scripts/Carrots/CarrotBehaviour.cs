using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CarrotBehaviour : MonoBehaviour
{
    [Header("胡萝卜身体")]
    [SerializeField]
    private GameObject body;
    [Header("根")]
    [SerializeField]
    private Root root;

    [Header("在攻击时子弹目标位置向上偏移的最小值")]
    [SerializeField]
    private float minUpChange;
    [Header("在攻击时子弹目标位置向上偏移的最大值")]
    [SerializeField]
    private float maxUpChange;

    /// <summary>
    /// The status carrot at.
    /// </summary>
    private E_CarrotStatus status;



    /// <summary>
    /// The root of the carrot.
    /// </summary>
    public Root Root => root;
    
    [SerializeField]
    private float growSpeed = 1;
    /// <summary>
    /// The grow speed.
    /// </summary>
    public float GrowSpeed { get => growSpeed; set => growSpeed = value; }

    [SerializeField]
    private float reloadTime = 2;
    /// <summary>
    /// The reload time for carrots to shoot next bullet.
    /// </summary>
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }

    [SerializeField]
    private float lockTime = 2;
    /// <summary>
    /// The time distance when the carrots found player but wait for shots.
    /// </summary>
    public float LockTime { get => lockTime; set => lockTime = value; }

    [SerializeField]
    private float bulletSpeed = 2;
    /// <summary>
    /// The flying speed of bullet.
    /// </summary>
    public float BulletSpeed { get => bulletSpeed; set => bulletSpeed = value; }

    [SerializeField]
    [Range(0, 90)]
    private float guardAngle;
    public float GuardAngle { get => guardAngle; set => guardAngle = value; }
    /// <summary>
    /// The rotate angle of root.
    /// </summary>
    public float Angle
    {
        get
        {
            return transform.eulerAngles.z;
        }
        set
        {
            Vector3 angle = transform.eulerAngles;
            angle.z = value;
            transform.eulerAngles = angle;
            root.Angle = value;
        }
    }

    bool isLocking = false; //whether the carrot was locking on player.
    private void Update()
    {
        //When the carrots was locking, check every frame if the player was still in the shoot area.
        IEnumerator CheckLockStatus()
        {
            bool isTimeUp = false;
            isLocking = true;
            IEnumerator coroutine = WaitCoroutine(LockTime, () =>
            {
                status = E_CarrotStatus.Shoot;
                isTimeUp = true;
            });
            StartCoroutine(coroutine);
            while (!isTimeUp)
            {
                if (!ShouldShoot())
                {
                    StopCoroutine(coroutine);
                    isLocking = false;
                    status = E_CarrotStatus.Guard;
                }
                yield return 1;
            }
        }
        switch (status)
        {
            case E_CarrotStatus.Guard:
                isLocking = false;
                if (ShouldShoot())
                    status = E_CarrotStatus.Lock;
                break;
            case E_CarrotStatus.Lock:
                if(!isLocking)
                    StartCoroutine(CheckLockStatus());
                break;
            case E_CarrotStatus.Reload:
                if (!ShouldShoot())
                    status = E_CarrotStatus.Guard;
                break;
            case E_CarrotStatus.Shoot:
                if (ShouldShoot())
                {
                    status = E_CarrotStatus.Reload;
                    Shoot();
                    StartCoroutine(WaitCoroutine(ReloadTime, () =>
                    {
                        if (status == E_CarrotStatus.Reload)
                        {
                            status = E_CarrotStatus.Shoot;
                        }
                    }));
                }
                else
                    status = E_CarrotStatus.Guard;
                break;
            case E_CarrotStatus.Die:
                break;
        }
    }
    private IEnumerator WaitCoroutine(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback?.Invoke();
    }
    /// <summary>
    /// Make the carrot grow and start the behaviour of carrot.
    /// </summary>
    public void GrowAndStart(float finalLength)
    {
        void SetLength(float length)
        {
            root.RootLength= length;
            body.transform.position = root.Bottom;
        }
        IEnumerator GrowCoroutine(float finalLength)
        {
            for (float i = 0; i <1 ; i += GrowSpeed * Time.deltaTime)
            {
                SetLength(Mathf.Lerp(0, finalLength, i));
                yield return 1;
            }
            SetLength(finalLength);
            //Grow completed, then the carrot go to guard the area.
            status = E_CarrotStatus.Guard;
        }
        StartCoroutine(GrowCoroutine(finalLength));
        status = E_CarrotStatus.Grow;
    }
    /// <summary>
    /// Returns true to tell the carrots to shoot a bullet in guarding status.
    /// </summary>
    /// <returns></returns>
    private bool ShouldShoot()
    {
        Vector3 playerPosition = GameController.Instance.PlayerController.Player.PlayerPosition;
        Vector3 bodyPosition = body.transform.position;
        float distance = Mathf.Abs(playerPosition.x - bodyPosition.x);
        float allowDeltaY = distance * Mathf.Tan(Mathf.Deg2Rad * GuardAngle);
        if (Mathf.Abs(playerPosition.y - bodyPosition.y) <= allowDeltaY) 
        { 
            return true;
        }
        else
            return false;
    }
    private void Shoot()
    {
        GameController.Instance.FlyingObjectsController.AddFlying<Bullet>(body.transform.position, (bullet) =>
        {
            bullet.Team = E_Team.Carrots;
            Vector3 target = GameController.Instance.PlayerController.Player.PlayerPosition;
            Vector3 random = new Vector3(0, Random.Range(minUpChange,maxUpChange));
            bullet.transform.right = target + random - body.transform.position;
            bullet.Speed = BulletSpeed;
        });
    }
}
