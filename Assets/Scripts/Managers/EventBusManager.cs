using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusManager : MonoBehaviour
{
    private static EventBusManager instance;
    public static EventBusManager I => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
