using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public sealed class Coroutines : MonoBehaviour
{
    private static Coroutines m_instance;
    
    private static Coroutines instance
    {
        get
        {
            if (m_instance is null)
            {
                var go = new GameObject("[COROUTINE MANAGER]");
                m_instance = go.AddComponent<Coroutines>();
                DontDestroyOnLoad(go);
                
            }

            return m_instance;
        }
    }

    public static Coroutine StartRoutine(IEnumerator enumerator)
    {
        return instance.StartCoroutine(enumerator);
    }

    public static void StopRoutine(Coroutine coroutine)
    {
        instance.StopCoroutine(coroutine);
    }
}
