﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class C4_BaseObjectManager : MonoBehaviour {

    [System.NonSerialized]
    public List<C4_Object> objectList;
    [System.NonSerialized]
    public Dictionary<ObjectID, C4_Object> objectDictionary;

    public void Awake()
    {
        objectList = new List<C4_Object>();
        objectDictionary = new Dictionary<ObjectID, C4_Object>();
    }

    public void addObject(C4_Object inputObject)
    {
        objectList.Add(inputObject);
        objectDictionary.Add(inputObject.objectID, inputObject);
    }

    public void removeObject(C4_Object removeObject)
    {
        objectList.Remove(removeObject);
        objectDictionary.Remove(removeObject.objectID);
    }
}
