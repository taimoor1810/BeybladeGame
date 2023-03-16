using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;


public class BattleScript : MonoBehaviour
{
    public Spinner spinnerScript;
    private float startSpinSpeed;
    private float currentSpinSpeed;
    public Image spinSpeedBar_Image;
    public TextMeshProUGUI spinSpeedRatio_Text; 
    public bool isAttacker;
    public bool isDefender;

    [Header("Player Type Damage Coefficients")]
    public float doDamage_Coefficient_Attacker = 10f;
    public float getDamage_Coefficient_Attacker = 1.2f;
     public float doDamage_Coefficient_Defender = 0.75f;
    public float getDamage_Coefficient_Defender = 0.2f;
    public float common_Damage_Coefficient = 0.04f;


    private void Awake() {
        startSpinSpeed = spinnerScript.spinSpeed;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
    }

    private void CheckPlayerType()
    {
        if(gameObject.name.Contains("Attaker"))
        {
            isAttacker = true;
            isDefender = false;
        }

        else if(gameObject.name.Contains("Defender"))
        {
            isAttacker = false;
            isDefender = true;
            spinnerScript.spinSpeed = 4400;
            startSpinSpeed = spinnerScript.spinSpeed;
            currentSpinSpeed = spinnerScript.spinSpeed;

            spinSpeedRatio_Text.text = currentSpinSpeed + "/" + startSpinSpeed;
        }
    }


    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Compare speed of two beyblades
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

            Debug.Log("My Speed "+ mySpeed+ "----Other player speed "+otherPlayerSpeed);

            if(mySpeed>otherPlayerSpeed)
            {
                Debug.Log("You Damaged the other player");

                float default_Damage_Amount = gameObject.GetComponent<Rigidbody>().velocity.magnitude*3600f*common_Damage_Coefficient;

                    if(isAttacker)
                    {
                        default_Damage_Amount *= doDamage_Coefficient_Attacker;
                    }
                    else if(isDefender)
                    {
                        default_Damage_Amount *= doDamage_Coefficient_Defender;
                    }   

                if(collision.collider.gameObject.GetComponent<PhotonView>().IsMine)
                {
                    //Apply Damage to other player
                collision.collider.gameObject.GetComponent<PhotonView>().RPC("DoDamage", RpcTarget.AllBuffered, default_Damage_Amount);
            
                }

                }

        }
    }

    [PunRPC]
    public void DoDamage(float _damageAmount)
    {
        if(isAttacker)
        {
            _damageAmount *= getDamage_Coefficient_Attacker;
        }
        else if(isDefender)
        {
            _damageAmount *= getDamage_Coefficient_Defender;
        }

        spinnerScript.spinSpeed -= _damageAmount;
        currentSpinSpeed = spinnerScript.spinSpeed;

        spinSpeedBar_Image.fillAmount = currentSpinSpeed / startSpinSpeed;
        spinSpeedRatio_Text.text = currentSpinSpeed.ToString("F0") + "/" + startSpinSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckPlayerType();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
