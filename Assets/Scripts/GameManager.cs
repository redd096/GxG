using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public PlayerSelector playerSelector { get; private set; }

    void Start()
    {
        CheckInstance();
    }

    void CheckInstance()
    {
        if (instance)
        {
            instance.SetDefaults();
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            SetDefaults();
        }
    }

    void SetDefaults()
    {
        DontDestroyOnLoad(this);

        playerSelector = FindObjectOfType<PlayerSelector>();
    }
}
