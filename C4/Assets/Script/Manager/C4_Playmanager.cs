﻿using UnityEngine;
using System.Collections;

/// <summary>
///  선택한 배를 플레이 하는 것을 관리하는 Manager
///  Input Manager로부터 받은 Data를 dispatchData에서 분석하여 선택된 배에게 행동을 명령한다.
///  aiming : 조준(드래그상태)
///  orderShot : 발포
///  orderMove : 이동
///  setBoatScript : 배 선택(Input Manager가 호출)
///  activeDone : 행동을 완료하여 상태를 reset
///  dispatchData : 전달받은 Data 분석
/// </summary>

public class C4_PlayManager : MonoBehaviour, C4_IntInitInstance{

    private static C4_PlayManager _instance;
    public static C4_PlayManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(C4_PlayManager)) as C4_PlayManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "C4_PlayManager";
                    _instance = container.AddComponent(typeof(C4_PlayManager)) as C4_PlayManager;
                }
            }

            return _instance;
        } 
    }

    public void initInstance()
    {
        if (!_instance)
        {
            _instance = GameObject.FindObjectOfType(typeof(C4_PlayManager)) as C4_PlayManager;
            if (!_instance)
            {
                GameObject container = new GameObject();
                container.name = "C4_PlayManager";
                _instance = container.AddComponent(typeof(C4_PlayManager)) as C4_PlayManager;
            }
        }
    }

    [System.NonSerialized]
    public GameObject selectedBoat;
    C4_Player character;
    C4_BoatFeature boatFeature;

    bool isAim;

    /* 조준하고 있는 방향으로 회전하고 UI를 출력할 함수 */
    void aiming(Vector3 clickPosition)
    {
        hideMoveUI();
        float distance = Vector3.Distance(selectedBoat.transform.position, clickPosition);
        Vector3 aimDirection = (selectedBoat.transform.position - clickPosition).normalized;
        aimDirection.y = 0;
        character.GetComponent<C4_BoatFeature>().aimUI.transform.position = selectedBoat.transform.position;
        character.GetComponent<C4_BoatFeature>().aimUI.transform.rotation = Quaternion.LookRotation(aimDirection);
        character.GetComponent<C4_BoatFeature>().aimUI.transform.localScale = new Vector3(distance, 1, 1);

        character.GetComponent<C4_BoatFeature>().aimUI.fillAmount = 1;
        
        character.startTurn(clickPosition);
    }


    /* 발포하고 상태를 초기화할 함수 */
    void orderShot(Vector3 shotDirection)
    {
        character.GetComponent<C4_BoatFeature>().aimUI.fillAmount = 0;
        character.shot(shotDirection);
        activeDone();
    }


    /* 움직임을 명령할 함수 */
    void orderMove(Vector3 toMove)
    {
        character.startMove(toMove);
        character.startTurn(toMove);
        activeDone();
    }


    /* 배를 선택하는 함수 */
    public void setBoatScript(GameObject clickBoat)
    {
        selectedBoat = clickBoat;
        character = selectedBoat.GetComponent<C4_Player>();
    }

    /* 선택 정보를 초기화 */
    void activeDone()
    {
        isAim = false;
        character = null;
        hideMoveUI(); // selectedBoat 초기화전에 moveUI 끔
        selectedBoat = null;
    }

    /* moveUI를 보여주는 함수 */
    void showMoveUI()
    {
        boatFeature = selectedBoat.GetComponent<C4_BoatFeature>();
        for (int i = 0; i < boatFeature.moveUI.Length; i++)
        {
            boatFeature.moveUI[i].transform.localScale = new Vector3(boatFeature.moveRange * (i+1), (boatFeature.moveRange * (i+1))/ 2, 1);
        }
        
        switch (boatFeature.stackCount)
        {
            case 0:
                boatFeature.moveUI[0].fillAmount = 0;
                boatFeature.moveUI[1].fillAmount = 0;
                boatFeature.moveUI[2].fillAmount = 0;
                break;
            case 1:
                boatFeature.moveUI[0].fillAmount = 1;
                boatFeature.moveUI[1].fillAmount = 0;
                boatFeature.moveUI[2].fillAmount = 0;
                break;
            case 2:
                boatFeature.moveUI[0].fillAmount = 1;
                boatFeature.moveUI[1].fillAmount = 1;
                boatFeature.moveUI[2].fillAmount = 0;
                break;
            case 3:
                boatFeature.moveUI[0].fillAmount = 1;
                boatFeature.moveUI[1].fillAmount = 1;
                boatFeature.moveUI[2].fillAmount = 1;
                break;
        }
    }

    /* MoveUI를 감추는 함수 */
    void hideMoveUI()
    {
        boatFeature = selectedBoat.GetComponent<C4_BoatFeature>();
        for (int i = 0; i < boatFeature.moveUI.Length; i++)
        {
            boatFeature.moveUI[i].fillAmount = 0;
        }
    }        

    /* InputManager로부터 전해받은 InputData를 분석하고 행동을 명령하는 함수 */
    public void dispatchData(InputData inputData)
    {

        if (selectedBoat != null)
        {
            showMoveUI(); // 배가 선택되었을 때 MoveUI를 보여줌
            if (inputData.keyState == InputData.KeyState.Down)
            {
                if (isAim)
                {
                    if (inputData.clickObjectID.id == inputData.dragObjectID.id)
                    {
                        isAim = false;
                    }
                    aiming(inputData.dragPosition);
                }
                else
                {
                    if ((inputData.clickObjectID.type == ObjectID.Type.Player) && inputData.clickObjectID.id != inputData.dragObjectID.id)
                    {
                        isAim = true;
                    }
                }
            }
            else
            {
                if (isAim)
                {
                    orderShot(inputData.dragPosition);
                }
                else
                {

                    if (inputData.clickObjectID.type == ObjectID.Type.Water)
                    {
                        if(inputData.clickPosition == inputData.dragPosition)
                        {
                            orderMove(inputData.clickPosition);
                        }
                    }
                }
            }
        }
    }
}
