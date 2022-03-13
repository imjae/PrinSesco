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
    public Button connectButton;
    public InputField nickNameField;
    public Button nickNameButton;

    public Button createRoomButton;
    public GameObject createRoomPanel;
    RoomInfo roomInfo;
    public TextMeshProUGUI peopleInfoText;
    int roomNumber;
    public TextMeshProUGUI roomNumberText;

    public Button joinRoomButton;
    public Button startButton;

    // 시작과 동시에 서버 접속 시도
    private void Start()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
        connectButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속중...";
    }

    // 서버 접속 성공
    public override void OnConnectedToMaster()
    {
        connectButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";

        nickNameField.gameObject.SetActive(true);
        nickNameButton.gameObject.SetActive(true);
        nickNameButton.interactable = false;
    }

    private void Update()
    {
        if (nickNameField.text.Length != 0) nickNameButton.interactable = true;
    }

    // 닉네임 사용 확정 -> 방 만들기
    public void UsingNickname()
    {
        nickNameButton.interactable = false;
        nickNameField.interactable = false;

        createRoomButton.gameObject.SetActive(true);
        joinRoomButton.gameObject.SetActive(true);
    }

    // 서버 접속 실패 시 재시도
    public override void OnDisconnected(DisconnectCause cause)
    {
        connectButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음 \n 접속 재시도";
        PhotonNetwork.ConnectUsingSettings();
    }

    // 방 만들기
    public void CreateRoom()
    {
        createRoomPanel.SetActive(true);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
        startButton.interactable = false;

        // 룸 넘버 랜덤으로 부여
        roomNumber = Random.Range(1000, 9999);

        if (PhotonNetwork.IsConnected)
        {
            roomNumberText.text = "룸 넘버 : " + roomNumber;
            // photonView 연결을 안해서 그런지 오류 발생! (fork 테스트용입니다 무시)
            peopleInfoText.text = roomInfo.PlayerCount + "/ " + roomInfo.MaxPlayers; 
            if (roomInfo.PlayerCount >= 2) startButton.interactable = true;
        }
    }

    // 게임 시작 (씬 이동)
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("NetworkMain");
    }
}
