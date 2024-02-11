using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    LobbyManager lobbyManager;
    public Text roomInfoText;

    private void Start()
    {
        lobbyManager = FindObjectOfType<LobbyManager>();
    }

    public void SetupRoom(string roomName, int currPlayer, int maxPlayer)
    {
        roomInfoText.text = roomName + "(" + currPlayer + "/" + maxPlayer + ")";
        GetComponent<Button>().onClick.AddListener(() => OnClickRoom(roomName));
    }

    public void OnClickRoom(string roomName)
    {
        Debug.Log(roomName + "에 진입합니다.");
        lobbyManager.JoinRoom(roomName);
    }

}
