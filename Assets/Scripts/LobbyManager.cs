using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject uI_LoginGameobject;

    [Header("Lobby UI")]
    public GameObject uI_LobbyGameobject;
    public GameObject uI_3DGameobject; 

    [Header("Connect Success UI")]
    public GameObject uI_ConnectionStatusGameobject;   
    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    #region  UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            //activating on lobby ui
             uI_LobbyGameobject.SetActive(true);
        uI_3DGameobject.SetActive(true);
        uI_ConnectionStatusGameobject.SetActive(false);

        //showConnectionStatus = true;    
        uI_LoginGameobject.SetActive(false);
        }

        else
        {
            //activating only login ui since we did not connect to photon yet
        uI_LobbyGameobject.SetActive(false);
        uI_3DGameobject.SetActive(false);
        uI_ConnectionStatusGameobject.SetActive(false);

        //showConnectionStatus = true;    
        uI_LoginGameobject.SetActive(true);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
        
    }

    #endregion 


    #region Unity callback methods
    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;
        if(!string.IsNullOrEmpty(playerName))
        {
             uI_LobbyGameobject.SetActive(false);
             uI_3DGameobject.SetActive(false);
             uI_LoginGameobject.SetActive(false);

             uI_ConnectionStatusGameobject.SetActive(true);

            if(!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        else{
            Debug.Log("Player name is invalid");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
    }

    #endregion

    #region Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to the internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+ " connected to the Photon Server");

         uI_LobbyGameobject.SetActive(true);
        uI_3DGameobject.SetActive(true);
        uI_LoginGameobject.SetActive(false);
        uI_ConnectionStatusGameobject.SetActive(false);
    }

    #endregion
}
