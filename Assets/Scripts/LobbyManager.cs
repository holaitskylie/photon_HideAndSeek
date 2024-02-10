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
        //연결이 끊어진 이유는 DisconnectCause로 제공
        Debug.Log("포톤 서버 접속 실패");

        joinButton.interactable = false;
        connectionInfoText.text = "서버와 연결되지 않음\n접속 시도 중";

        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //룸 접속 시도
        //매치메이킹 서버를 통해 빈 무작위 룸에 접속을 시도
        Debug.Log("랜덤 룸에 접속을 시도");

        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            //포톤 서버에 연결된 상태라면 랜덤 룸에 접속 시도
            connectionInfoText.text = "룸에 접속 중";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //포톤 서버에 연결되지 않았다면 포톤 서버로 연결 시도
            connectionInfoText.text = "서버와 연결되지 않음\n접속 시도 중";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //(빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
        Debug.Log("비어있는 방 없음. 새로운 방 생성");
        connectionInfoText.text = "새로운 방을 생성합니다.";

        //룸 속성 설정
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        //새로운 룸 생성
        //생성할 룸 이름을 string 타입, 룹 옵션을 RoomOptions 타입으로 받는다
        //생성된 룸은 리슨 서버 방식으로 동작하며 룸을 생성한 클라이언트가 호스트 역할을 맡는다
        PhotonNetwork.CreateRoom("room1", options);
    }
       
    public override void OnJoinedRoom()
    {
        //룸에 참가 완료된 경우 자동 실행
        Debug.Log(PhotonNetwork.NickName + "방 입장 완료");

        //플레이어의 닉네임 설정
        PhotonNetwork.NickName = userName.text;
        connectionInfoText.text = PhotonNetwork.NickName + " 입장하셨습니다.";

        //룸 참가자 모두가 해당 씬을 로드하게 함
        //PhotonNetwork.LoadLevel("Main");
    }
}
