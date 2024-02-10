using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

//<summary>
//Lobby Scene�� ��Ʈ��ũ �κ�� �����ϴ� ��ũ��Ʈ
//��ġ����ŷ ������ �� ������ ���
//</summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    private string gameVersion = "v1.0";
    public InputField userName; //�÷��̾� �г��� ����
    public Text connectionInfoText; //���� ��Ʈ��ũ ���� ���� ǥ��
    public Button joinButton; //��ġ����ŷ ������ ���� �뿡 ����

    void Start()
    {
        //���� ����� ���ÿ� ������ ���� ���� �õ�
        //�����ϴ� ���� �� ������ �õ��� �� ������ ���� ��ư ��Ȱ��ȭ

        //���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;

        //���� ���� ����
        PhotonNetwork.ConnectUsingSettings();
                
        joinButton.interactable = false;
        connectionInfoText.text = "���� ���� ��";

    }

    public override void OnConnectedToMaster()
    {
        //Ŭ���̾�Ʈ�� ������ ������ ����Ǹ� ��ġ����ŷ �۾��� ������ �غ� �Ǹ� �ڵ� ȣ��
        Debug.Log("���� ���� ���� ����");

        joinButton.interactable = true;
        connectionInfoText.text = "������ �����";
             
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        //������ ���� ���� ���� �� �ڵ� ����
        //������ ������ ������ DisconnectCause�� ����
        Debug.Log("���� ���� ���� ����");

        joinButton.interactable = false;
        connectionInfoText.text = "������ ������� ����\n���� �õ� ��";

        //������ �������� ������ �õ�
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Connect()
    {
        //�� ���� �õ�
        //��ġ����ŷ ������ ���� �� ������ �뿡 ������ �õ�
        Debug.Log("���� �뿡 ������ �õ�");

        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            //���� ������ ����� ���¶�� ���� �뿡 ���� �õ�
            connectionInfoText.text = "�뿡 ���� ��";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            //���� ������ ������� �ʾҴٸ� ���� ������ ���� �õ�
            connectionInfoText.text = "������ ������� ����\n���� �õ� ��";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //(�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
        Debug.Log("����ִ� �� ����. ���ο� �� ����");
        connectionInfoText.text = "���ο� ���� �����մϴ�.";

        //�� �Ӽ� ����
        RoomOptions options = new RoomOptions();
        options.IsOpen = true;
        options.IsVisible = true;
        options.MaxPlayers = 4;

        //���ο� �� ����
        //������ �� �̸��� string Ÿ��, �� �ɼ��� RoomOptions Ÿ������ �޴´�
        //������ ���� ���� ���� ������� �����ϸ� ���� ������ Ŭ���̾�Ʈ�� ȣ��Ʈ ������ �ô´�
        PhotonNetwork.CreateRoom("room1", options);
    }
       
    public override void OnJoinedRoom()
    {
        //�뿡 ���� �Ϸ�� ��� �ڵ� ����
        Debug.Log(PhotonNetwork.NickName + "�� ���� �Ϸ�");

        //�÷��̾��� �г��� ����
        PhotonNetwork.NickName = userName.text;
        connectionInfoText.text = PhotonNetwork.NickName + " �����ϼ̽��ϴ�.";

        //�� ������ ��ΰ� �ش� ���� �ε��ϰ� ��
        //PhotonNetwork.LoadLevel("Main");
    }
}
