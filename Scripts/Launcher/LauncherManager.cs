using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LauncherManager : MonoBehaviourPunCallbacks
{
    public static LauncherManager lm;
    public TMP_InputField inputField;
    public TMP_InputField inputPassword;
    public Text loginTips;
    [SerializeField] public string playerName;

    private void Awake(){
        if(lm == null){
            lm = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    public void OnClickOnline(){
        playerName = inputField.text;
        AuthenticationValues authValues = new AuthenticationValues();
        authValues.AuthType = CustomAuthenticationType.Custom;
        authValues.AddAuthParameter("account", playerName);
        authValues.AddAuthParameter("password", inputPassword.text);
        authValues.AddAuthParameter("mode", "login");
        PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = playerName;
        print("ClickStart");
    }


    public void OnClickOffline(){
        playerName = inputField.text;
        SceneManager.LoadScene("BattleScene");
    }

    public override void OnConnectedToMaster(){
        print("Connected");
        SceneManager.LoadScene("Lobby");
    }

    public void onClickCreateLogin() {
        playerName = inputField.text;
        AuthenticationValues authValues = new AuthenticationValues();
        authValues.AuthType = CustomAuthenticationType.Custom;
        authValues.AddAuthParameter("account", playerName);
        authValues.AddAuthParameter("password", inputPassword.text);
        authValues.AddAuthParameter("mode", "create");
        PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.LocalPlayer.NickName = playerName;
        print("ClickStart");
    }


    public override void OnCustomAuthenticationFailed(string debugMessage)
    {
        loginTips.gameObject.SetActive(true);
        loginTips.text = debugMessage;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear() {
        yield return new WaitForSeconds(2);
        loginTips.gameObject.SetActive(false);
    }
}
