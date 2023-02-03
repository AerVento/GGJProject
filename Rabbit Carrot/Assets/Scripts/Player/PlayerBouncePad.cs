using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBouncePad : MonoBehaviour, IBouncePad
{
    [Header("在吸收子弹后吐出子弹所需要的时间")]
    [SerializeField]
    private float secondsConsumingBullets;

    public E_Team SourceTeam => E_Team.Carrots;

    public E_Team TargetTeam => E_Team.Player;

    public void Bounce(Bullet bullet)
    {
        StartCoroutine(GagBullet(bullet));
    }
    private IEnumerator GagBullet(Bullet bullet)
    {
        float originalSpeed = bullet.Speed;
        bullet.gameObject.SetActive(false);
        bullet.Speed = 0;
        yield return new WaitForSeconds(secondsConsumingBullets);
        if(bullet != null) 
        {
            bullet.gameObject.SetActive(true);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bullet.transform.right = new Vector3(worldPos.x, worldPos.y) - GameController.Instance.PlayerController.Player.PlayerPosition;
            bullet.Speed = originalSpeed;
        }
    }
}
