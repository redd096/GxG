using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public PlayerSelector playerSelector { get; private set; }

    void Awake()
    {
        CheckInstance();
    }

    void CheckInstance()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

        instance.SetDefaults();
    }

    void SetDefaults()
    {
        playerSelector = FindObjectOfType<PlayerSelector>();
    }
}
