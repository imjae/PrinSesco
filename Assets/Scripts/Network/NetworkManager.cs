using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    public TextMeshProUGUI connectionInfoText;
    public Button joinButton;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        this.joinButton.interactable = false;
        this.connectionInfoText.text = "마스터 서버에 접속중...";
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster()
    {
        this.joinButton.interactable = true;
        this.connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        Connect();
    }

    // 마스터 서버와 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause)
    {
        this.joinButton.interactable = false;
        this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n 접속 재시도중...";

        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect()
    {
        this.joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            this.connectionInfoText.text = "룸에 접속중...";
            PhotonNetwork.JoinRandomRoom();
        }

        else
        {
            this.connectionInfoText.text = "오프라인 : 마스터 서버와 연결 끊김 \n 다시 접속 시도합니다.";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        this.connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        this.connectionInfoText.text = "방 참가 성공!";
        PhotonNetwork.LoadLevel("NetworkMain");
    }
}
