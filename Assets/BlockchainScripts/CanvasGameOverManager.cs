using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using System;

namespace RoadCrossing
{
    public class CanvasGameOverManager : MonoBehaviour
    {
        public Button buttonRestart;
        public Button buttonMainMenu;
        public Button buttonTokenClaim;

        public Text claimingStatusTxt;
        public Text tokenClaimBalanceText;

        public string Address { get; private set; }

        private string tokenContractAddress = "0x292134AF9AC3E3Dc3c6c73cFef3Ce823ca76f03e";

        void Start()
        {
            buttonRestart.gameObject.SetActive(true);
            buttonRestart.interactable = true;
            buttonMainMenu.gameObject.SetActive(true);
            buttonMainMenu.interactable = false;
            buttonTokenClaim.gameObject.SetActive(true);
            buttonTokenClaim.interactable = true;
            claimingStatusTxt.text = "";
            GetTokenBalance();
        }

        private void HideAllButtons() {
            buttonRestart.interactable = false;
            buttonMainMenu.interactable = false;
            buttonTokenClaim.interactable = false;
        }

        private void ShowAllButtons()
        {
            buttonRestart.interactable = true;
            buttonMainMenu.interactable = true;
            buttonTokenClaim.interactable = true;
        }

        public async void GetTokenBalance()
        {
            var sdk = ThirdwebManager.Instance.SDK;
            string address = await sdk.Wallet.GetAddress();
            Contract contract = sdk.GetContract(tokenContractAddress);
            Debug.Log(contract);
            var data = await contract.ERC20.BalanceOf(address);
            Debug.Log("GetTokenBalance" + data.displayValue);
            tokenClaimBalanceText.text = "Token Balance: " + data.displayValue;
        }

        public async void ClaimERC20Token()
        {
            GameObject gameController = GameObject.Find("GameController");
            if (gameController != null)
            {
                RCGGameController rcgGameController = gameController.GetComponent<RCGGameController>();
                if (rcgGameController != null)
                {
                    if (rcgGameController.score <= 0) {
                        return;
                    }
                    HideAllButtons();
                    claimingStatusTxt.text = "Claimming...";
                    claimingStatusTxt.gameObject.SetActive(true);

                    var contract = ThirdwebManager.Instance.SDK.GetContract(tokenContractAddress);
                    try
                    {
                        var result = await contract.ERC20.Claim(rcgGameController.score.ToString());
                        Debug.Log("Tokens were claimed!");
                        claimingStatusTxt.text = "Tokens were claimed!";
                        GetTokenBalance(); // Cập nhật lại số dư token sau khi claim thành công
                        buttonRestart.interactable = true;
                        buttonMainMenu.interactable = true;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error occurred while claiming tokens: {ex.Message}");
                        // Xử lý lỗi nếu có ngoại lệ xảy ra
                        claimingStatusTxt.text = "Claiming Fail!";
                        ShowAllButtons();
                    }
                }
                else
                {
                    Debug.LogError("rcgGameController Error");
                    ShowAllButtons();
                }
            }
            else
            {
                Debug.LogError("gameController Error");
                ShowAllButtons();
            }
        }
    }
}


