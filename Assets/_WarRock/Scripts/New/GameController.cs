using GameSparks.RT;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {

    private Player[] playerList;
    public GameObject[] playerPrefabs;
    public GameObject shellPrefabRef;
    // Todo: make dynamic (non-inspector)
    public Text[] playerKillsHUDList, playerNamesHUDList;

    private SpawnPoint[] spawnPoints;


    private void Start() {
        StartCoroutine(SendTimeStamp());
        LoadSpawnPoints();
        SetupPlayers();
    }

    
    /// <summary>
    /// Sends a Unix timestamp in milliseconds to the server
    /// </summary>
    private IEnumerator SendTimeStamp() {
        using (RTData data = RTData.Get()) {
            // Current time as timestamp
            data.SetLong(1, (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds);
            // Send data with OpCode 101 to peerId 0, aka the server
            //GameSparksManager.Instance.GetRTSession().SendData(101, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data, new int[] { 0 });
        }

        // Sync every X sec
        yield return new WaitForSeconds(5f);
        StartCoroutine(SendTimeStamp());
    }

    private void LoadSpawnPoints() {
        spawnPoints = FindObjectsOfType(typeof(SpawnPoint)) as SpawnPoint[];
    }

    private void SetupPlayers() {
        //#region Setup Players
        //playerList = new Player[(int)GameSparksManager.Instance().GetSessionInfo().GetPlayerList().Count];

        //Debug.Log("GC| Found " + playerList.Length + " Players...");
        //for (int i = 0; i < GameSparksManager.Instance().GetSessionInfo().GetPlayerList().Count; i++) {
        //    for (int j = 0; j < allSpawnpoints.Length; j++) {
        //        if (allSpawnpoints[j].teamPeerId == GameSparksManager.Instance().GetSessionInfo().GetPlayerList()[i].peerID) {
        //            GameObject playerObj = Instantiate(playerPrefabs[i], allSpawnpoints[j].gameObject.transform.position, allSpawnpoints[j].gameObject.transform.rotation) as GameObject;
        //            playerObj.name = GameSparksManager.Instance().GetSessionInfo().GetPlayerList()[i].peerID.ToString();
        //            //playerObj.transform.SetParent(this.transform);

        //            if (GameSparksManager.Instance().GetSessionInfo().GetPlayerList()[i].peerID == GameSparksManager.Instance().GetRTSession().PeerId) {
        //                playerObj.GetComponent<Player>().SetupPlayer(true);
        //            }
        //            else {
        //                playerObj.GetComponent<Player>().SetupPlayer(false);
        //            }

        //            playerList[i] = playerObj.GetComponent<Player>(); // add the new tank object to the corresponding reference in the list

        //            // Todo: Get dynamicly generated HUD with player
        //            playerNamesHUDList[i].text = GameSparksManager.Instance().GetSessionInfo().GetPlayerList()[i].displayName; // set the HUD of this player to be the display name
        //            break;
        //        }
        //    }
        //}

        //// lastly, go through the list of HUD elements, starting with the number of players and clear any HUD for players that aren't in the session //
        //for (int i = GameSparksManager.Instance().GetSessionInfo().GetPlayerList().Count; i < playerKillsHUDList.Length; i++) {
        //    playerKillsHUDList[i].text = playerNamesHUDList[i].text = string.Empty;
        //}
        //#endregion
    }



    /// <summary>
    /// Updates the opponent player's position, rotation, and if they have been reset
    /// </summary>
    /// <param name="_packet">Packet Received From Opponent Player</param>
    public void UpdateOpponentPlayers(RTPacket _packet) {
        //
    }

    /// <summary>
    /// This is called when an opponent has registered a raycast hit.
    /// It will reset the opponent's Player, and update the
    /// score of the owner of the gun that hit.
    /// </summary>
    /// <param name="_packet">Packet Received From Opponent Player</param>
    public void RegisterOpponentHit(RTPacket _packet) {
        //
    }

    /// <summary>
    /// This method is called when a player disconnects from the RT session
    /// </summary>
    /// <param name="_peerId">Peer Id of the player who disconnected</param>
    public void OnOpponentDisconnected(int _peerId) {
        Debug.Log("User with peerId " + _peerId + " has left the RT session.");
    }


    public Player[] GetAllPlayers() {
        return playerList;
    }
}
