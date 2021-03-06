﻿using UnityEngine;
using System.Collections;

public abstract class C4_Character : C4_Object
{
    [System.NonSerialized]
    public bool canMove;
    [System.NonSerialized]
    public bool canShot;
    [System.NonSerialized]
    
    protected C4_BoatFeature boatFeature;
    protected C4_BoatMove moveComponent;
    protected C4_Turn turnComponent;
    protected C4_IntShot shotComponent;

    public void Start()
    {
        moveComponent = GetComponent<C4_BoatMove>();
        turnComponent = GetComponentInChildren<C4_Turn>();
        shotComponent = GetComponent<C4_IntShot>();
        boatFeature = GetComponent<C4_BoatFeature>();
    }

    void Update()
    {
        checkActiveAndStack();
    }

    /* 배의 stack을 체크하여 움직일 수 있나, 발포할 수 있나를 확인 */
    void checkActiveAndStack()
    {
        if (boatFeature.stackCount >= 1)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }

        if (boatFeature.stackCount >= 2)
        {
            canShot = true;
        }
        else
        {
            canShot = false;
        }
    }

    /* 발포 함수 */
    public void shot(Vector3 click)
    {
        if (canShot)
        {
            shotComponent.startShot(click);
        }
    }

    /* 이동함수 */
    public void move(Vector3 toMove)
    {
        if (canMove)
        {
            moveComponent.startMove(toMove);
        }
    }

    /* 방향전환함수 */
    public void turn(Vector3 toMove)
    {
        if (canMove)
        {
            turnComponent.setToTurn(toMove);
        }
    }


    /* 공격을 받았을 때 damage만큼 피해를 입는 함수, 파괴시 true return */
    public bool damaged(int damage)
    {
        boatFeature.hp -= damage;
        return checkHP();
    }

    /* hp Check하여 배가 파괴되면 true return */
    protected abstract bool checkHP();
}
