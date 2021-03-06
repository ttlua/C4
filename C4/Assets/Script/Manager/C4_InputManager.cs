﻿using UnityEngine;
using System.Collections;

/// <summary>
///  Input에 대한 것을 처리하는 Manager
///  - click Down, cliking, click Up의 경우로 나누어 처리한다
///  - input에 대한 Data를 수집하여 Camera와 Play Manager에게 전송한다
/// </summary>

public class C4_InputManager : MonoBehaviour, C4_IntInitInstance {

    private static C4_InputManager _instance;
    public static C4_InputManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = GameObject.FindObjectOfType(typeof(C4_InputManager)) as C4_InputManager;
                if (!_instance)
                {
                    GameObject container = new GameObject();
                    container.name = "C4_InputManager";
                    _instance = container.AddComponent(typeof(C4_InputManager)) as C4_InputManager;
                }
            }

            return _instance;
        }
    }
    
    public void initInstance()
    {
        if (!_instance)
        {
            _instance = GameObject.FindObjectOfType(typeof(C4_InputManager)) as C4_InputManager;
            if (!_instance)
            {
                GameObject container = new GameObject();
                container.name = "C4_InputManager";
                _instance = container.AddComponent(typeof(C4_InputManager)) as C4_InputManager;
            }
        }
    }

    InputData inputData;
    C4_Camera camObject;

    bool isClick;
    RaycastHit hit;
    C4_Object clickObject;

	void Start () {
        isClick = false;
        camObject = Camera.main.transform.root.GetComponent<C4_Camera>();
	}
	
	void Update () {

        if (isClick)
        {
            onClick();
            if (inputData.clickObjectID.type == ObjectID.Type.Water)
            {
                camObject.cameraMove(inputData);
            }
            else
            {
                C4_PlayManager.Instance.dispatchData(inputData);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            onClickDown();
        }

        if (isClick&&Input.GetMouseButtonUp(0))
        {
            onClickUp();
            C4_PlayManager.Instance.dispatchData(inputData);
        }
    }


    /* 버튼을 눌렀을 때의 Data 처리 */
    void onClickDown()
    {
        isClick = true;

        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
        clickObject = hit.collider.transform.root.gameObject.GetComponent<C4_Object>();
        inputData.clickObjectID = clickObject.objectID;
        inputData.dragObjectID = clickObject.objectID;
        inputData.clickPosition = hit.point;
        inputData.dragPosition = hit.point;
        inputData.clickPosition.y = 0;
        inputData.dragPosition.y = 0;
        inputData.keyState = InputData.KeyState.Down;

        if (inputData.clickObjectID.type == ObjectID.Type.Player)
        {
            C4_PlayManager.Instance.setBoatScript(hit.collider.transform.root.gameObject);
        }
    }



    /* 계속 클릭했을 때(드래그)의 Data 처리 */
    void onClick()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
        clickObject = hit.collider.transform.root.gameObject.GetComponent<C4_Object>();
        inputData.dragPosition = hit.point;
        inputData.dragPosition.y = 0;
        inputData.dragObjectID = clickObject.objectID;
    }



    /* 버튼을 올렸을 때의 Data 처리 */
    void onClickUp()
    {
        inputData.keyState = InputData.KeyState.Up;
        isClick = false;
    }
}
