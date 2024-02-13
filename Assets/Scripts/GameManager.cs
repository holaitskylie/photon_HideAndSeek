using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

//<summary>
//��Ʈ��ũ �÷��̾� ���� ��� Ŭ���� ����
//���� ���� ���� �÷��̾ �����Ѵ�
//�÷��̾��� ������ ����, ����, �÷��̾� ���
//�ٸ� �÷��̾��� ������ �޾ƿ´�
//</summary>
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;    

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        //���� �ε�Ǿ��� �� ȣ��Ǵ� �޼��带 sceneLoaded �̺�Ʈ�� ����
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex == 1)
        {
            Debug.Log("�÷��̾� ����");
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
        }
    }     
    
}
