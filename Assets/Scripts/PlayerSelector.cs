using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypesOfPlayer
{
    knight,
    wizard
}

public class PlayerSelector : MonoBehaviour
{
    [Header("Defaults")]
    [SerializeField] TypesOfPlayer startPlayer = TypesOfPlayer.knight;
    //si può fare un array di struct (TypesOfPlayer, Player) così nel CreatePlayers controlla tutto l'array - non possono esserci duplicati di TypesOfPLayer
    [SerializeField] Player knightPrefab = default;
    [SerializeField] Player wizardPrefab = default;

    [Header("Switch")]
    [SerializeField] float delaySwitch = 2;

    float timeLastSwitch;

    Dictionary<TypesOfPlayer, GameObject> players = new Dictionary<TypesOfPlayer, GameObject>();

    //for multiplayer
    bool twoPlayers = default;

    void Start()
    {
        CreatePlayers();
    }

    #region private API

    #region start

    void CreatePlayers()
    {
        //find spawn
        Transform playerSpawn = FindObjectOfType<Player>().transform;

        InstantiateOnePlayer(TypesOfPlayer.knight, knightPrefab, playerSpawn);
        InstantiateOnePlayer(TypesOfPlayer.wizard, wizardPrefab, playerSpawn);

        //destroy spawn
        Destroy(playerSpawn.gameObject);
    }

    void InstantiateOnePlayer(TypesOfPlayer typeOfPlayer, Player prefab, Transform spawn)
    {
        //instantiate and add to dictionary
        players[typeOfPlayer] = Instantiate(prefab).gameObject;

        //set position
        players[typeOfPlayer].transform.position = spawn.position;

        //if not start player, deactivate it
        if (typeOfPlayer != startPlayer)
            players[typeOfPlayer].SetActive(false);
    }

    #endregion

    void SwitchToNextOne(TypesOfPlayer activePlayer)
    {
        //get enum array
        System.Array enums = System.Enum.GetValues(typeof(TypesOfPlayer));

        //then get next one, or if exceed length get the first
        int nextOne = (int)activePlayer + 1;
        if (nextOne >= enums.Length)
            nextOne = 0;

        //toActivate (next one) and toDeactivate (this active player)
        GameObject toActivate = players[(TypesOfPlayer)nextOne];
        GameObject toDeactivate = players[activePlayer];

        //set position and sprite flip
        toActivate.transform.position = toDeactivate.transform.position;
        toActivate.GetComponentInChildren<SpriteRenderer>().flipX = toDeactivate.GetComponentInChildren<SpriteRenderer>().flipX;

        //deactivate one player and active another
        toDeactivate.SetActive(false);
        toActivate.SetActive(true);
    }

    #endregion

    public void SwitchPlayer(TypesOfPlayer activePlayer)
    {
        //only if one player
        if (twoPlayers)
            return;

        //check delay
        if (Time.time < timeLastSwitch)
            return;

        timeLastSwitch = Time.time + delaySwitch;

        //deactivate one and active another
        SwitchToNextOne(activePlayer);
    }

    /*
    #region multiplayer

    //void DeactivateOnePlayer(TypesOfPlayer typeOfPlayer)
    //{
    //    //deactivate one player
    //    players[typeOfPlayer].SetActive(false);
    //}

    public void StartSecondPlayer()
    {
        //set two players
        twoPlayers = true;

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
    */
}
