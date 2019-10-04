using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public static MainGameManager instance;
    public GameObject[] tankPrefab;
    int tankID;

    public int score;
    public TextMeshProUGUI scoreText;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        scoreText.text = score.ToString();
        tankID = PlayerPrefs.GetInt("TankSelected");
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("MainMenuScene");
            return;
        }

        if (PlayerTankManager.LocalPlayerInstance == null)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Instantiating Player 1");
                GameObject player1 = PhotonNetwork.Instantiate(this.tankPrefab[tankID].name, new Vector3(0, 0, 0), Quaternion.identity, 0);            
            }
            else
            {
                GameObject player2 = PhotonNetwork.Instantiate(this.tankPrefab[tankID].name, new Vector3(0, 0, 0), Quaternion.identity, 0);
            }
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenuScene");
    }

    public void SetScore(int plusScore)
    {
        score += plusScore;
        scoreText.text = score.ToString();
    }
}
