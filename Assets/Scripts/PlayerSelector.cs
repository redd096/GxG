using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    [Header("Defaults")]
    [SerializeField] bool startWithKnight = true;
    [SerializeField] Player knightPrefab = default;
    [SerializeField] Player wizardPrefab = default;

    [Header("Switch")]
    public float delaySwitch = 2;
    [HideInInspector] public float timeLastSwitch;

    GameObject knight;
    GameObject wizard;

    //for multiplayer
    bool twoPlayers;
    bool knightIsFirstPlayer;

    void Start()
    {
        CreatePlayers();

        DeactivateOnePlayer(startWithKnight);
    }

    #region private API

    void CreatePlayers()
    {
        //find spawn
        Transform playerSpawn = FindObjectOfType<Player>().transform;

        //instantiate
        knight = Instantiate(knightPrefab).gameObject;
        wizard = Instantiate(wizardPrefab).gameObject;

        //set positions
        knight.transform.position = playerSpawn.position;
        wizard.transform.position = playerSpawn.position;

        //destroy spawn
        Destroy(playerSpawn.gameObject);
    }

    void DeactivateOnePlayer(bool deactivateWizard)
    {
        //deactivate knight or wizard
        if (deactivateWizard)
        {
            wizard.SetActive(false);
        }
        else
        {
            knight.SetActive(false);
        }
    }

    void Switch(GameObject toDeactivate, GameObject toActivate)
    {
        //set position and sprite flip
        toActivate.transform.position = toDeactivate.transform.position;
        toActivate.GetComponentInChildren<SpriteRenderer>().flipX = toDeactivate.GetComponentInChildren<SpriteRenderer>().flipX;

        //deactivate one player and active another
        toDeactivate.SetActive(false);
        toActivate.SetActive(true);
    }

    #endregion

    public void SwitchPlayer(bool playerIsKnight)
    {
        //only if one player
        if (twoPlayers)
            return;

        //deactivate one and active another
        if(playerIsKnight)
        {
            Switch(knight, wizard);
        }
        else
        {
            Switch(wizard, knight);
        }
    }

    #region multiplayer

    public void StartSecondPlayer()
    {
        //set two players
        twoPlayers = true;
        bool isKnightActive = knight.activeSelf;

        if(isKnightActive)
        {
            //activate wizard for second player
            knightIsFirstPlayer = true;

            wizard.transform.position = knight.transform.position;
            wizard.SetActive(true);
        }
        else
        {
            //activate knight for second player
            knightIsFirstPlayer = false;

            knight.transform.position = wizard.transform.position;
            knight.SetActive(true);
        }
    }

    public void EndSecondPlayer()
    {
        //set one player only
        twoPlayers = false;

        //deactivate wizard or knight
        DeactivateOnePlayer(knightIsFirstPlayer);
    }

    #endregion
}
