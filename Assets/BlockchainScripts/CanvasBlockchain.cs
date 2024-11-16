using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using System;

public class CanvasBlockchain : MonoBehaviour
{
    public Button buttonGold;
    public Button buttonHP;
    public Button buttonx2Gold;
    public Button buttonAccept;

    public Text ClaimingStatusText;

    public Button claimTokenBtn;

    public Text tokenBalanceText;

    public Text coinsText;
    public Text currentHPText;
    public Text x2GoldText;

    public Text coinsTextShop;

    public string Address { get; private set; }

    private string TokenAddressSmartContract = "0xd5a967Ee6089cfadB16ea16DCF706CF103063131";

    private string coinsPlayerPrefs = "Coins";

    // Start is called before the first frame update
    public void OnWalletConnected()
    {
        buttonGold.gameObject.SetActive(true);
        buttonHP.gameObject.SetActive(true);
        buttonx2Gold.gameObject.SetActive(true);
        buttonAccept.gameObject.SetActive(true);

        buttonGold.interactable = true;
        buttonHP.interactable = true;
        buttonx2Gold.interactable = true;
        buttonAccept.interactable = true;

        ClaimingStatusText.text = "";

        x2GoldText.gameObject.SetActive(false);

        coinsText.text = PlayerPrefs.GetInt(coinsPlayerPrefs).ToString();

        TokenBalance();
    }

    public async void TokenBalance()
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var result = await contract.ERC20.BalanceOf(Address);
        tokenBalanceText.text = "Token Owned: " + result.displayValue;
        tokenBalanceText.gameObject.SetActive(true);
    }

    public async void ClaimToken()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        int tokenclaimed = 10;

        try
        {
            // Attempt to claim tokens
            var result = await contract.ERC20.ClaimTo(Address, tokenclaimed.ToString());

            // Try to fetch the updated token balance after claiming
            try
            {
                var resultBalance = await contract.ERC20.BalanceOf(Address);
                tokenBalanceText.text = "Token Owned: " + resultBalance.displayValue;
                tokenBalanceText.gameObject.SetActive(true);

                // Update UI after claiming tokens
                ClaimingStatusText.text = "+10 Token";
                ButtonManager.TriggerButtonRestore();
            }
            catch (Exception ex)
            {
                // Handle failure to fetch token balance
                Debug.LogError("Failed to fetch token balance: " + ex.Message);
                ClaimingStatusText.text = "Failed to fetch balance!";
                return; // Stop further execution if fetching balance fails
            }
        }
        catch (Exception ex)
        {
            // Handle failure to claim tokens
            Debug.LogError("Failed to claim token: " + ex.Message);
            ClaimingStatusText.text = "Failed to claim Token!";
        }
        finally
        {
            // Ensure button is re-enabled regardless of success or failure
            buttonAccept.interactable = true;
        }
    }


    public static int ConvertStringToRoundedInt(string numberStr)
    {
        // Convert the string to a double
        double number = double.Parse(numberStr);

        // Round the number
        double roundedNumber = Math.Round(number);

        // Convert to int and return
        return (int)roundedNumber;
    }

    public async void ClaimGold()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);

        // Fetch the initial token balance
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);

        if (roundedInt <= 0)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            buttonAccept.interactable = true;
            return;
        }

        try
        {
            // Attempt to burn 1 token
            await contract.ERC20.Burn("1");

            // Update gold amount after successful burn
            UpdateGold(50);

            // Try to fetch the updated token balance after burning
            try
            {
                var resultBalance = await contract.ERC20.BalanceOf(Address);
                tokenBalanceText.text = "Token Owned: " + resultBalance.displayValue;
                tokenBalanceText.gameObject.SetActive(true);
                // Update the UI for gold and coins
                coinsText.text = PlayerPrefs.GetInt(coinsPlayerPrefs).ToString();
                coinsTextShop.text = PlayerPrefs.GetInt(coinsPlayerPrefs).ToString();
                Debug.Log("Gold claimed");
                ClaimingStatusText.text = "+50 Gold";
                ButtonManager.TriggerButtonRestore();
            }
            catch (Exception ex)
            {
                // Handle failure to fetch token balance
                Debug.LogError("Failed to fetch token balance: " + ex.Message);
                ClaimingStatusText.text = "Failed to fetch balance!";
                return; // Stop further execution if fetching balance fails
            }
        }
        catch (Exception ex)
        {
            // Handle failure to burn the token
            Debug.LogError("Failed to burn token: " + ex.Message);
            ClaimingStatusText.text = "Failed to claim Gold!";
        }
        finally
        {
            // Ensure button is re-enabled regardless of success or failure
            buttonAccept.interactable = true;
        }
    }


    public async void ClaimHP()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);
        if (roundedInt <= 1)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            buttonAccept.interactable = true;
            return;
        }
        try
        {
            await contract.ERC20.Burn("2");
            ResourceBoost.Instance.hp = 1;

            try
            {
                var resultBalance = await contract.ERC20.BalanceOf(Address);
                tokenBalanceText.text = "Token Owned: " + resultBalance.displayValue;
                tokenBalanceText.gameObject.SetActive(true);

                currentHPText.text = "4";
                Debug.Log("HP claimed");
                ClaimingStatusText.text = "+1 HP";
                ButtonManager.TriggerButtonRestore();
                buttonAccept.interactable = true;
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to fetch token balance: " + ex.Message);
                ClaimingStatusText.text = "Failed to fetch balance!";
                return;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to burn token: " + ex.Message);
            ClaimingStatusText.text = "Failed to claim HP!";
        }
        finally
        {
            buttonAccept.interactable = true;
        }

    }

    public async void Claimx2Gold()
    {
        buttonAccept.interactable = false;
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        ClaimingStatusText.text = "Claiming!";
        var contract = ThirdwebManager.Instance.SDK.GetContract(TokenAddressSmartContract);

        // Fetch the initial token balance
        var numberStr = await contract.ERC20.BalanceOf(Address);
        int roundedInt = ConvertStringToRoundedInt(numberStr.displayValue);

        if (roundedInt <= 2)
        {
            ClaimingStatusText.text = "Not Enough Token!";
            buttonAccept.interactable = true;
            return;
        }

        try
        {
            // Attempt to burn 3 tokens
            await contract.ERC20.Burn("3");

            // Update goldx2 status if the burn is successful
            ResourceBoost.Instance.goldx2 = 2;

            // Fetch the updated token balance after burning
            try
            {
                var resultBalance = await contract.ERC20.BalanceOf(Address);
                tokenBalanceText.text = "Token Owned: " + resultBalance.displayValue;
                tokenBalanceText.gameObject.SetActive(true);
                // Show and update the UI for x2Gold
                x2GoldText.gameObject.SetActive(true);
                Debug.Log("X2 Gold claimed");
                ClaimingStatusText.text = "X2 GOLD";
                ButtonManager.TriggerButtonRestore();
            }
            catch (Exception ex)
            {
                // Handle the case where fetching the token balance fails
                Debug.LogError("Failed to fetch token balance: " + ex.Message);
                ClaimingStatusText.text = "Failed to fetch balance!";
                return; // Stop further execution if balance fetch fails
            }
        }
        catch (Exception ex)
        {
            // Handle the case where burning tokens fails
            Debug.LogError("Failed to burn token: " + ex.Message);
            ClaimingStatusText.text = "Failed to claim X2 Gold!";
        }
        finally
        {
            // Re-enable the button regardless of success or failure
            buttonAccept.interactable = true;
        }
    }

    private void UpdateGold(int coinAdded) {
        int coinsLeft = PlayerPrefs.GetInt(coinsPlayerPrefs);
        //Register the item lock state in the player prefs
        PlayerPrefs.SetInt(coinsPlayerPrefs, coinsLeft + coinAdded);
    }
}
