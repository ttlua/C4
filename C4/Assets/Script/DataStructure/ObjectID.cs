﻿using UnityEngine;
using System.Collections;

public struct ObjectID
{
    public enum Type { Character, Enemy, Missile, Water };
    public Type type;
    public int id;
}