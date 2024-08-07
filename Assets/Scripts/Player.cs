using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Vector3 startPosition;
    private Vector3 oldPosition;
    private bool isTurn = false;

    private int moveCnt = 0;
    private int turnCnt = 0;
    private int spawnCnt = 0;

    private bool isDie = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        oldPosition = transform.localPosition;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        CharTurn();
    //    }
    //    else if (Input.GetMouseButtonUp(0))
    //    {
    //        CharMove();
    //    }
    //}

    private void Init()
    {
        anim.SetBool("Die", false);
        startPosition = transform.position;
        oldPosition = transform.localPosition;
        moveCnt = 0;
        spawnCnt = 0;
        turnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = isTurn == true ? false : true; //isTurn이 true라면 false로 변경하고, false면 true로 변경한다

        spriteRenderer.flipX = isTurn;
    }

    public void CharMove()
    {
        if (isDie)
        {
            return;
        }

        moveCnt++;
        MoveDirection();

        if (isFailTurn()) //사망
        {
            CharDie();

            return;
        }

        if (moveCnt > 5) //계단 스폰
        {
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    }

    private void MoveDirection()
    {
        if (isTurn)//left
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }
        transform.position = oldPosition;   //현재캐릭터 위치값에 대입하여 캐릭터 이동
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool resurt = false;

        if (GameManager.Instance.isTurn[turnCnt] != isTurn) //0-19
        {
            resurt = true;
        }
        turnCnt++;

        if (turnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            turnCnt = 0;
        }

        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(spawnCnt);
        spawnCnt++;

        if (spawnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            spawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.Instance.GameOver();
        anim.SetBool("Die", true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.Instance.Init();
        GameManager.Instance.InitStarts();

    }
}
