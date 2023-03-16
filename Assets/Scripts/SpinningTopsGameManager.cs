using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class SpinningTopsGameManager : MonoBehaviourPunCallbacks
{
    [Header("UI")]
    public GameObject uI_InformPanelGameobject;
    public TextMeshProUGUI uI_InformText;
    public GameObject searchForGamesButtonGameobject;
    // Start is called before the first frame update
    void Start()
    {
        uI_InformPanelGameobject.SetActive(true);
        uI_InformText.text = "Search For Games To Battle!!"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI callback methods

    public void JoinRandomRoom()
    {
        uI_InformText.text = "Searching for available rooms..."; 
        PhotonNetwork.JoinRandomRoom();
        searchForGamesButtonGameobject.SetActive(false);
    }

    #endregion

    #region Photon CallBack methods

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        uI_InformText.text = message; 
        CreateAndJoinRoom();
    }

    public override void OnJoinedRoom()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name +" waiting for other players...";    
        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject,2.0f)); 
        }
        Debug.Log(PhotonNetwork.NickName + " joined to" + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
       Debug.Log(newPlayer.NickName + " joined to" + PhotonNetwork.CurrentRoom.Name +" Player Count "+ PhotonNetwork.CurrentRoom.PlayerCount);
       uI_InformText.text = newPlayer.NickName + " joined to" + PhotonNetwork.CurrentRoom.Name +" Player Count "+ PhotonNetwork.CurrentRoom.PlayerCount;

       StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f)); 
    }

    #endregion

    #region Private Methods

    void CreateAndJoinRoom()
    {

        string randomRoomName = "Room " + Random.Range(0,1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        // Create the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);
    }

    #endregion
}
