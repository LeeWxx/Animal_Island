using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    private static AdventureManager instance;

    public static AdventureManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AdventureManager>();
            }

            return instance;
        }
    }

    public int finalStageIndex;

    private int coin;
    private int score;

    private int gameOverCoin;

    public int GameOverCoin
    {
        get { return gameOverCoin; }
    }

    private int gameOverScore;

    private bool isRevival;

    public bool IsRevival
    {
        get { return isRevival; }
    }

    private int reviveWaitCount = 3;

    private BridgeEndZone lastExitZone;
    
    public BridgeEndZone LastExitZone
    {
        get { return lastExitZone; }
    }

    private bool isGameOver;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }

        isGameOver = false;
        isRevival = false;
        AdventureSetting.Instance.AdventureSet();
        coin = 0;
        score = 0;

        lastExitZone = null;
    }

    public void CoinUp(int plusCoin)
    {
        coin += plusCoin;
        AdventureUIManager.Instance.CoinTextUpdate(coin);
    }

    public void ScoreUpdate(int scoreParameter)
    {
        score = scoreParameter;
        AdventureUIManager.Instance.ScoreTextUpdate(score);
    }

    public void GameOver()
    {
        if(isGameOver == false)
        {
            gameOverScore = score;
            gameOverCoin = coin;
            FindObjectOfType<PlayerItemController>().BuffAllOff();
            AdventureUIManager.Instance.RewardPanelOn(gameOverScore, gameOverCoin);
            isGameOver = true;
        }
    }

    public void LastExitZoneUpdate(BridgeEndZone lastEndZone)
    {
        lastExitZone = lastEndZone;
    }

    public IEnumerator Revive()
    {
        AdventureUIManager.Instance.revivePanel.gameObject.SetActive(false);
        isRevival = true;


        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        PlayerMovement playerMovement = playerHealth.GetComponent<PlayerMovement>();
        PlayerItemController playerItemController = playerHealth.GetComponent<PlayerItemController>();

        playerHealth.MaxHpSet(playerHealth.MaxHp);
        playerMovement.RevivePosSet(lastExitZone);

        AdventureUIManager.Instance.reviveWaitText.gameObject.SetActive(true);
        AdventureUIManager.Instance.reviveWaitText.text = "" + reviveWaitCount;

        while (reviveWaitCount >= 1)
        {
            yield return new WaitForSeconds(1f);
            reviveWaitCount -= 1;
            AdventureUIManager.Instance.reviveWaitText.text = "" + reviveWaitCount;
        }
        AdventureUIManager.Instance.reviveWaitText.gameObject.SetActive(false);

        playerMovement.onPlayerMoving.enabled = true;
        playerMovement.rigidBody.constraints = RigidbodyConstraints.None;
        playerMovement.rigidBody.constraints = RigidbodyConstraints.FreezeRotation;

        playerItemController.Booster();

        playerHealth.GetComponent<PlayerInput>().jumpPossible = true;
    }

    public void ReviveReady()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.GetComponent<PlayerItemController>().BuffAllOff();
        playerMovement.onPlayerMoving.enabled = false;
        playerMovement.rigidBody.constraints = RigidbodyConstraints.FreezeAll;

        AdventureUIManager.Instance.RevivePanelOn();
    }

    public void Boost()
    {
        FindObjectOfType<PlayerItemController>().Booster();
    }
}
