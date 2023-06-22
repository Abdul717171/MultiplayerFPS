using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public GameObject enemy;
    [Space]
    public Transform spwantPoint;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("connecting..");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("connected to server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("You're in the Lobby");
        PhotonNetwork.JoinOrCreateRoom("Falcons", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("You're in room Falcon's");
        GameObject _enemy = PhotonNetwork.Instantiate(enemy.name, spwantPoint.position, Quaternion.identity);
    }
}
