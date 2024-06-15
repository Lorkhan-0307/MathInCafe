using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StableUtility : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
