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

    [Header("UI_Naming")]
    public InputField userName; //플레이어 닉네임 설정
    public InputField roomName; //룸 이름 설정

    public Text connectionInfoText; //현재 네트워크 접속 상태 표시
    public Button createButton; //로비에 룸을 만드는 버튼

    [Header("UI_Room List")]   
    [SerializeField] private RoomData roomPrefab;
    [SerializeField] private Transform scrollArea;
    List<RoomData> roomDataList = new List<RoomData>();


    void Start()
    {
        //게임 실행과 동시에 마스터 서버 접속 시도
        Debug.Log("01. 마스터 서버 접속 시도");

        //게임 버전 지정
        PhotonNetwork.GameVersion = gameVersion;

        //게임 서버 접속
        PhotonNetwork.ConnectUsingSettings();
                
        //접속하는 동안 룸 접속을 시도할 수 없도록 접속 버튼 비활성화
        createButton.interactable = false;
        connectionInfoText.text = "서버 접속 중";

    }

    public override void OnConnectedToMaster()
    {
        //클라이언트가 마스터 서버에 연결되며 매치메이킹 작업을 수행할 준비가 되면 자동 호출
        Debug.Log("02. 포톤 서버 접속 성공");

        createButton.interactable = true;
        connectionInfoText.text = "서버에 연결됨";

        //로비에 접속
        PhotonNetwork.JoinLobby();             
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("03. 로비 접속");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //마스터 서버 접속 실패 시 자동 실행
        //연결이 끊어진 이유는 DisconnectCause로 제공
        Debug.Log("02. 포톤 서버 접속 실패");

        createButton.interactable = false;
        connectionInfoText.text = "서버와 연결되지 않음\n접속 시도 중";

        //마스터 서버로의 재접속 시도
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //룸 접속 시도
        //매치메이킹 서버를 통해 빈 무작위 룸에 접속을 시도
        Debug.Log("랜덤 룸에 접속을 시도");

        createButton.interactable = false;

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

    public void MakeRoom()
    {
        //createButton에 연결하여 로비에 방을 생성하는 메서드
        Debug.Log("03. 방 생성하기");
        createButton.interactable = false;

        //룸 속성 설정
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        if (roomName.text == "")
            roomName.text = "Room" + Random.Range(1, 100);

        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("03. " + roomName.text + "방 생성");
            PhotonNetwork.CreateRoom(roomName.text, options);

            //플레이어의 닉네임 설정
            PhotonNetwork.NickName = userName.text;
            connectionInfoText.text = PhotonNetwork.NickName + " 님이 방을 생성했습니다.";
        }
        else
        {
            //포톤 서버에 연결되지 않았다면 포톤 서버로 연결 시도
            connectionInfoText.text = "서버와 연결되지 않음\n접속 시도 중";
            PhotonNetwork.ConnectUsingSettings();
        }       
               
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("04. 방 생성 완료");        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //(빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
        Debug.Log("03. 비어있는 방 없음. 새로운 방 생성");

        connectionInfoText.text = "새로운 방을 생성합니다.";

        //룸 속성 설정
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        string roomId = "Room" + Random.Range(1, 100);

        //새로운 룸 생성
        //생성할 룸 이름을 string 타입, 룹 옵션을 RoomOptions 타입으로 받는다
        //생성된 룸은 리슨 서버 방식으로 동작하며 룸을 생성한 클라이언트가 호스트 역할을 맡는다
        PhotonNetwork.CreateRoom(roomId, options);
    }
       
    public override void OnJoinedRoom()
    {
        //룸에 참가 완료된 경우 자동 실행
        Debug.Log("05. 방 입장 완료");        

        //플레이어의 닉네임 설정
        PhotonNetwork.NickName = userName.text;
        connectionInfoText.text = PhotonNetwork.NickName + " 입장하셨습니다.";

        //룸 참가자 모두가 해당 씬을 로드하게 함
        PhotonNetwork.LoadLevel("Main");
    }

    public void JoinRoom(string roomName)
    {
        if(PhotonNetwork.IsConnected)
            PhotonNetwork.JoinRoom(roomName);
    }
        

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //로비에 접속하면 자동으로 콜백
        //룸 리스트에 변화(룸 생성, 룸 제거, 룸 속성 변경 등)가 있을 때 마다 자동으로 콜백
        Debug.Log("룸 리스트 업데이트 - 현재 방 갯수 : " + roomList.Count);
        UpdateRoomList(roomList);
        
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {
        //룸 리스트의 모든 요소 삭제
        foreach(RoomData room in roomDataList)
        {
            Destroy(room.gameObject);
        }

        //리스트 초기화
        roomDataList.Clear();

        //현재 사용 가능한 각 방에 대해 인스턴스를 만들고 리스트 추가
        foreach(RoomInfo room in roomList)
        {
            RoomData newRoom = Instantiate(roomPrefab, scrollArea);
            //newRoom.SetRoomName(room.Name);
            newRoom.SetupRoom(room.Name, room.PlayerCount, room.MaxPlayers);
            roomDataList.Add(newRoom);
        }
    }
}
