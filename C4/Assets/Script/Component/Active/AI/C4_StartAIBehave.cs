﻿using UnityEngine;
using System.Collections;

public class C4_StartAIBehave : MonoBehaviour {

    public int checkBound;
    public int attackOrMovePercent;
    public int attackSuccessOrFailPercent;
    int attackPercent;
    int successAttackPercent;
    C4_Player shortestDistancePlayer;
    double distanceWithPlayer;
    double checkDistanceEachPlayer;
    C4_BoatFeature boatFeature;
    C4_Enemy enemy;

    Vector3 toMove;
    Vector3 perpendicularAtPlayerVector;
    Vector3 playerPositionVector;
    float angleToPlayer;
    float angleToPerpendicular;
    float tempValue;

    void Start()
    {
        attackPercent = 0;
        successAttackPercent = 0;
        distanceWithPlayer = 0;
        boatFeature = GetComponent<C4_BoatFeature>();
        enemy = GetComponent<C4_Enemy>();
    }

    public void startBehave()
    {
        checkDistanceWithPlayer();
        if (distanceWithPlayer < checkBound && (C4_EnemyManager.Instance.action == C4_EnemyManager.Action.Move))
        {
            if (boatFeature.stackCount == 3)
            {
                C4_EnemyManager.Instance.showSelect();
                Invoke("moveToPlayer", 1f);
            }
        }
        else
        {
            if (attackPercent > attackOrMovePercent && (C4_EnemyManager.Instance.action == C4_EnemyManager.Action.Attack))
            {
                C4_EnemyManager.Instance.showSelect();
                Invoke("attackPlayer", 1f);
            }
            else
            {
                C4_EnemyManager.Instance.showSelect();
                Invoke("moveBesidePlayer", 1f);
            }

        }
    }

    void attackPlayer()
    {
        if (successAttackPercent > attackSuccessOrFailPercent)
        {
            toMove = shortestDistancePlayer.transform.position;
        }
        else
        {
            playerPositionVector = shortestDistancePlayer.transform.position - transform.position;
            perpendicularAtPlayerVector = new Vector3(playerPositionVector.z, 0, -playerPositionVector.x) - transform.position;
            tempValue = Random.Range(0, 2);
            if (tempValue > 1)
            {
                angleToPerpendicular = Random.Range(1, 2);
            }
            else
            {
                angleToPerpendicular = Random.Range(-2, -1);
            }
            tempValue = Random.Range(0, 2);
            if (tempValue > 1)
            {
                angleToPlayer = Random.Range(0, 2);
            }
            else
            {
                angleToPlayer = Random.Range(-2, 0);
            }
            tempValue = Random.Range(boatFeature.moveRange / 2, boatFeature.moveRange * 2 + boatFeature.moveRange / 2);
            toMove = (playerPositionVector * angleToPlayer + angleToPerpendicular * perpendicularAtPlayerVector).normalized * tempValue + shortestDistancePlayer.transform.position;
        }
        enemy.shot(toMove);
        C4_EnemyManager.Instance.resetSelect();
    }

    void moveBesidePlayer()
    {
        playerPositionVector = shortestDistancePlayer.transform.position - transform.position;
        perpendicularAtPlayerVector = new Vector3(playerPositionVector.z, 0, -playerPositionVector.x) - transform.position;
        tempValue = Random.Range(0,2);
        if(tempValue >1)
        {
            angleToPerpendicular = Random.Range(1, 2);
        }
        else
        {
            angleToPerpendicular = Random.Range(-2,-1);
        }
        tempValue = Random.Range(0,2);
        if(tempValue>1)
        {
            angleToPlayer = Random.Range(0, 2);
        }
        else
        {
            angleToPlayer = Random.Range(-2,0);
        }
        tempValue = Random.Range(boatFeature.moveRange / 2,boatFeature.moveRange * 2 + boatFeature.moveRange / 2);
        toMove = (playerPositionVector * angleToPlayer + angleToPerpendicular * perpendicularAtPlayerVector).normalized * tempValue + transform.position;
        enemy.move(toMove);
        C4_EnemyManager.Instance.resetSelect();
    }

    void moveToPlayer()
    {
        toMove = shortestDistancePlayer.transform.position.normalized * boatFeature.moveRange * 3 + transform.position;
        enemy.move(toMove);
        C4_EnemyManager.Instance.resetSelect();
    }

    void checkDistanceWithPlayer()
    {
        for (int i = 0; i < C4_PlayManager.Instance.objectList.Count; i++)
        {
            checkDistanceEachPlayer = Vector3.Distance(C4_PlayManager.Instance.objectList[i].transform.position, transform.position);
            if (distanceWithPlayer > checkDistanceEachPlayer)
            {
                distanceWithPlayer = checkDistanceEachPlayer;
                shortestDistancePlayer = C4_PlayManager.Instance.objectList[i].GetComponent<C4_Player>();
            }
        }
    }
}