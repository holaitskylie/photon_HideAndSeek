using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//<summary>
//Lobby Scene이 네트워크 로비로 동작하는 스크립트
//매치메이킹 서버와 룸 접속을 담당
//</summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "v1.0";
    public InputField userName; //플레이어 닉네임 설정
    public Text connectionInfoText; //현재 네트워크 접속 상태 표시
    public Button joinButton; //매치메이킹 서버를 통해 룸에 접속

    void Start()
    {
        //게임 실행과 동시에 마스터 서버 접속 시도
        //접속하는 동안 룸 접속을 시도할 수 없도록 접속 버튼 비활성화

        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;

        //게임 서버 접속
        PhotonNetwork.ConnectUsingSettings();
                
        joinButton.interactable = false;
        connectionInfoText.text = "서버 접속 중";

    }

    public override void OnConnectedToMaster()
    {
        //클라이언트가 마스터 서버에 연결되며 매치메이킹 작업을 수행할 준비가 되면 자동 호출
        Debug.Log("포톤 서버 접속 성공");

        joinButton.interactable = true;
        connectionInfoText.text = "서버에 연결됨";

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //마스터 서버 접속 실패 시 자동 실행
    }

    public void Connect()
    {
        //룸 접속 시도
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //(빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    }

    public override void OnJoinedRoom()
    {
        //룸에 참가 완료된 경우 자동 실행
    }
}
