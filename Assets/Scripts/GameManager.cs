using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

//<summary>
//네트워크 플레이어 간의 통신 클래스 역할
//게임 씬에 들어서면 플레이어를 생성한다
//플레이어의 데이터 관리, 스폰, 플레이어 사망
//다른 플레이어의 정보를 받아온다
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
        //씬이 로드되었을 때 호출되는 메서드를 sceneLoaded 이벤트에 구독
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
            Debug.Log("플레이어 생성");
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero, Quaternion.identity);
        }
    }     
    
}
