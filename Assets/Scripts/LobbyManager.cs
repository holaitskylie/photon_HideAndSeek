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
    }

    public void Connect()
    {
        //�� ���� �õ�
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        //(�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
    }

    public override void OnJoinedRoom()
    {
        //�뿡 ���� �Ϸ�� ��� �ڵ� ����
    }
}
