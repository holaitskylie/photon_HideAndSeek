using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    public Text roomInfoText; 

    public void SetupRoom(string roomName, int currPlayer, int maxPlayer)
    {
        roomInfoText.text = roomName + "(" + currPlayer + "/" + maxPlayer + ")";    
    }

}
