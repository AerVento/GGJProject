using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBouncePad : MonoBehaviour, IBouncePad
{
    [Header("�������ӵ����³��ӵ�����Ҫ��ʱ��")]
    [SerializeField]
    private float secondsConsumingBullets;

    private Queue<Coroutine> coroutines = new Queue<Coroutine>();

    public E_Team SourceTeam => E_Team.Carrots;

    public E_Team TargetTeam => E_Team.Player;

    public void Bounce(Bullet bullet)
    {
        AudioManager.Instance.PlayRandomEffectAudio("eat1", "eat2", "eat3");
        coroutines.Enqueue(StartCoroutine(GagBullet(bullet)));
        Main.Instance.Cursor.CursorStatus = CursorController.Status.Targeting;
    }
    private IEnumerator GagBullet(Bullet bullet)
    {
        float originalSpeed = bullet.Speed;
        bullet.gameObject.SetActive(false);
        bullet.Speed = 0;
        yield return new WaitForSeconds(secondsConsumingBullets);
        if(bullet != null) 
        {
            AudioManager.Instance.PlayRandomEffectAudio("spit1", "spit2", "spit3", "spit4", "spit5", "spit6");
            bullet.gameObject.SetActive(true);
            bullet.transform.position = transform.position;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bullet.transform.right = new Vector3(worldPos.x, worldPos.y) - GameController.Instance.PlayerController.Player.PlayerPosition;
            bullet.Speed = originalSpeed;
        }
        //���Ƚ�ȥ�ı�Ȼ���ȳ���
        coroutines.Dequeue();
        if(coroutines.Count == 0)
        {
            Main.Instance.Cursor.CursorStatus = CursorController.Status.None;
        }
    }
}
