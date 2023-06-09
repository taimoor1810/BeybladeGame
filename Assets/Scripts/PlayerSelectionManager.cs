using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Transform playerSwitcherTransform;
     public GameObject[] spinnerTopModels;
    public int playerSelectionNumber;

    [Header("UI")]
    public TextMeshProUGUI playerModelType_Text;
     public Button next_Button;
    public Button prev_Button;
    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;

    #region  Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
        playerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    #endregion

    #region UI Callback Methods

    public void NextPlayerClicked()
    {
        next_Button.enabled = false;
        prev_Button.enabled = false;

        playerSelectionNumber +=1;

        if(playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }

        Debug.Log(playerSelectionNumber);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if(playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //This means the player model is attack
            playerModelType_Text.text = "Attack";
        }

        else
        {
            playerModelType_Text.text = "Defence";
        }
    }

    public void PreviousPlayerClicked()
    {
         next_Button.enabled = false;
        prev_Button.enabled = false;

        playerSelectionNumber -=1;

        if(playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }

        Debug.Log(playerSelectionNumber);
        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

          if(playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //This means the player model is attack
            playerModelType_Text.text = "Attack";
        }

        else
        {
            playerModelType_Text.text = "Defence";
        }
    }

    public void OnSelectButtonClicked()
    {
        next_Button.enabled = false;
        prev_Button.enabled = false;
        uI_Selection.SetActive(false);
        uI_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable { {MultiPlayerARSpinnerTopGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
         next_Button.enabled = true;
        prev_Button.enabled = true;
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }

    #endregion 

    #region Private Mehtods

    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation*Quaternion.Euler(axis*angle);

        float elapsedTime = 0.0f;

        while(elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;
         next_Button.enabled = true;
        prev_Button.enabled = true;
    }

    #endregion
}
