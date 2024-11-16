using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Thirdweb;
using UnityEngine.UI;
using TMPro;
using System;

public class BlockchainShopManager : MonoBehaviour
{
    public string Address { get; private set; }

    string piggyAddress = "0x7C85421cfE2721bc915Af734Ad41e2ac53f1b585";
    string doggyAddress = "0x3C3141ACc8f3570e84E24208178162bDd8cCaD81";
    string goatyAddress = "0x9C41a21deCE117da9dE589f14f7640800a6c7276";
    string turtleAddress = "0x298A9f30415b5266302b1099f47112A0aEA292a8";
    string koalaAddress = "0x780451eaaA73b6725fFAA276B25d0f10Bf59F5f0";
    string sheepAddress = "0x51Dec8E8F4F3CD98389eF6329a09a76EeAd9ed8d";
    string duckAddress = "0x5e5D7d9b5bAE37e04F7d05eD05D6Df4F887608BC";

    string tokenContractAddress = "0x292134AF9AC3E3Dc3c6c73cFef3Ce823ca76f03e";

    public Button piggyButton;
    public Button doggyButton;
    public Button goatyButton;
    public Button turtleButton;
    public Button koalaButton;
    public Button sheepButton;
    public Button duckButton;

    public Button coinButton;
    public Button heartButton;

    public Button backButton;

    public Button shopButton;
    public Button playButton;

    public TMP_Text piggyBalanceValue;
    public TMP_Text doggyBalanceValue;
    public TMP_Text goatyBalanceValue;
    public TMP_Text turtleBalanceValue;
    public TMP_Text koalaBalanceValue;
    public TMP_Text sheepBalanceValue;
    public TMP_Text duckBalanceValue;

    public Text heartTextPrice;
    public Text coinx2TextPrice;

    public TextMeshProUGUI nftClaimingStatusText;

    public Text coinsText;

    public TMP_Text heartText;
    public TMP_Text x2CoinText;

    private void Start()
    {
        UpdateAllNFTBalance();
        GetTokenBalance();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CS_StartMenu");
    }

    private void HideAllButton()
    {
        piggyButton.interactable = false;
        doggyButton.interactable = false;
        goatyButton.interactable = false;
        turtleButton.interactable = false;
        koalaButton.interactable = false;
        sheepButton.interactable = false;
        duckButton.interactable = false;
        coinButton.interactable = false;
        heartButton.interactable = false;
        backButton.interactable = false;
    }

    private void ShowAllButton()
    {
        piggyButton.interactable = true;
        doggyButton.interactable = true;
        goatyButton.interactable = true;
        turtleButton.interactable = true;
        koalaButton.interactable = true;
        sheepButton.interactable = true;
        duckButton.interactable = true;
        if (ResourceBoost.Instance.hp == 0)
        {
            heartButton.interactable = true;
        }
        else {
            heartButton.interactable = false;
        }
        if (ResourceBoost.Instance.goldx2 == 1)
        {
            coinButton.interactable = true;
        }
        else
        {
            coinButton.interactable = false;
        }

        backButton.interactable = true;
    }

    private void SetSingleTon(int indexValue)
    {
        if (indexValue == 1)
        {
            ResourceBoost.Instance.piggy = 1;
        }
        else if (indexValue == 2)
        {
            ResourceBoost.Instance.doggy = 1;
        }
        else if (indexValue == 3)
        {
            ResourceBoost.Instance.goaty = 1;

        }
        else if (indexValue == 4)
        {
            ResourceBoost.Instance.turtle = 1;
        }
        else if (indexValue == 5)
        {
            ResourceBoost.Instance.koala = 1;
        }
        else if (indexValue == 6)
        {
            ResourceBoost.Instance.sheep = 1;
        }
        else if (indexValue == 7)
        {
            ResourceBoost.Instance.duck = 1;
        }
    }

    private void UpdateBalanceText(int indexValue, string nftCount, int nftCountValue)
    {
        if (nftCountValue >= 1)
        {
            SetSingleTon(indexValue);
        }
        if (indexValue == 1)
        {
            piggyBalanceValue.text = nftCount;
            if (nftCountValue >= 1) {
                PlayerPrefs.SetInt("PiggyCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.piggy = 1;
            }
        }
        else if (indexValue == 2)
        {
            doggyBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("DoggyCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.doggy = 1;
            }
        }
        else if (indexValue == 3)
        {
            goatyBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("GoatyCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.goaty = 1;
            }
        }
        else if (indexValue == 4)
        {
            turtleBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("TurtleCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.turtle = 1;
            }
        }
        else if (indexValue == 5)
        {
            koalaBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("KoalaCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.koala = 1;
            }
        }
        else if (indexValue == 6)
        {
            sheepBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("SheepCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.sheep = 1;
            }
        }
        else if (indexValue == 7)
        {
            duckBalanceValue.text = nftCount;
            if (nftCountValue >= 1)
            {
                PlayerPrefs.SetInt("DuckCount", 1);
                PlayerPrefs.Save();
                ResourceBoost.Instance.duck = 1;
            }
        }       
    }

    private async void UpdateNFTBalance(string NFTAddressSmartContract, int indexValue)
    {
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        nftClaimingStatusText.text = "Updating NFT Balance...";
        nftClaimingStatusText.gameObject.SetActive(true);
        try
        {
            List<NFT> nftList = await contract.ERC721.GetOwned(Address);

            UpdateBalanceText(indexValue, nftList.Count.ToString(), nftList.Count);
            nftClaimingStatusText.text = "Updating Completed";

            shopButton.interactable = true;
            playButton.interactable = true;

        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while fetching NFTs: {ex.Message}");
            // Handle the error, e.g., show an error message to the user or retry the operation
        }
    }

    public void UpdateAllNFTBalance()
    {
        shopButton.interactable = false;
        playButton.interactable = false;
        UpdateNFTBalance(piggyAddress, 1);
        UpdateNFTBalance(doggyAddress, 2);
        UpdateNFTBalance(goatyAddress, 3);
        UpdateNFTBalance(turtleAddress, 4);
        UpdateNFTBalance(koalaAddress, 5);
        UpdateNFTBalance(sheepAddress, 6);
        UpdateNFTBalance(duckAddress, 7);
    }

    public async void ClaimNFTPass(int indexValue)
    {
        string NFTAddressSmartContract = "";
        if (indexValue == 1)
        {
            NFTAddressSmartContract = piggyAddress;
        }
        else if (indexValue == 2)
        {
            NFTAddressSmartContract = doggyAddress;
        }
        else if (indexValue == 3)
        {
            NFTAddressSmartContract = goatyAddress;
        }
        else if (indexValue == 4)
        {
            NFTAddressSmartContract = turtleAddress;
        }
        else if (indexValue == 5)
        {
            NFTAddressSmartContract = koalaAddress;
        }
        else if (indexValue == 6)
        {
            NFTAddressSmartContract = sheepAddress;
        }
        else if (indexValue == 7)
        {
            NFTAddressSmartContract = duckAddress;
        }
        
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        nftClaimingStatusText.text = "Claiming...";
        nftClaimingStatusText.gameObject.SetActive(true);
        HideAllButton();
        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTAddressSmartContract);
        try
        {
            var result = await contract.ERC721.ClaimTo(Address, 1);
            nftClaimingStatusText.text = "Claimed NFT Pass!";

            if (indexValue == 1)
            {
                ResourceBoost.Instance.piggyNFT = false;
            }
            else if (indexValue == 2)
            {
                ResourceBoost.Instance.doggyNFT = false;
            }
            else if (indexValue == 3)
            {
                ResourceBoost.Instance.goatyNFT = false;
            }
            else if (indexValue == 4)
            {
                ResourceBoost.Instance.turtleNFT = false;
            }
            else if (indexValue == 5)
            {
                ResourceBoost.Instance.koalaNFT = false;
            }
            else if (indexValue == 6)
            {
                ResourceBoost.Instance.sheepNFT = false;
            }
            else if (indexValue == 7)
            {
                ResourceBoost.Instance.duckNFT = false;
            }

            ShowAllButton();
            SetSingleTon(indexValue);
            UpdateAllNFTBalance();
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred while claiming the NFT: {ex.Message}");
            // Optionally, update the UI to inform the user of the error
            nftClaimingStatusText.text = "Failed to claim NFT. Please try again.";
            ShowAllButton();
        }
    }

    private static float ConvertStringToFloat(string numberStr)
    {
        // Convert the string to a float
        float number = float.Parse(numberStr);

        // Return the float value
        return number;
    }

    public async void GetTokenBalance()
    {
        var sdk = ThirdwebManager.Instance.SDK;
        string address = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(tokenContractAddress);
        Debug.Log(contract);
        var data = await contract.ERC20.BalanceOf(address);
        Debug.Log("GetTokenBalance" + data.displayValue);
        coinsText.text = data.displayValue;
        ShowAllButton();
    }

    public async void SpendTokenToBuyNFT(int indexValue)
    {
        int costValue = 0;
        if (indexValue == 1) {
            costValue = 200;
        }
        else if (indexValue == 2) {
            costValue = 400;
        }
        else if (indexValue == 3)
        {
            costValue = 600;
        }
        else if (indexValue == 4)
        {
            costValue = 800;
        }
        else if (indexValue == 5)
        {
            costValue = 1000;
        }
        else if (indexValue == 6)
        {
            costValue = 2000;
        }
        else if (indexValue == 7)
        {
            costValue = 4000;
        }

        HideAllButton();

        if (indexValue == 1)
        {
            if (ResourceBoost.Instance.piggyNFT == true) {
                ClaimNFTPass(1);
            }
        }
        else if (indexValue == 2)
        {
            if (ResourceBoost.Instance.doggyNFT == true)
            {
                ClaimNFTPass(2);
            }
        }
        else if (indexValue == 3)
        {
            if (ResourceBoost.Instance.goatyNFT == true)
            {
                ClaimNFTPass(3);
            }
        }
        else if (indexValue == 4)
        {
            if (ResourceBoost.Instance.turtleNFT == true)
            {
                ClaimNFTPass(4);
            }
        }
        else if (indexValue == 5)
        {
            if (ResourceBoost.Instance.koalaNFT == true)
            {
                ClaimNFTPass(5);
            }
        }
        else if (indexValue == 6)
        {
            if (ResourceBoost.Instance.sheepNFT == true)
            {
                ClaimNFTPass(6);
            }
        }
        else if (indexValue == 7)
        {
            if (ResourceBoost.Instance.duckNFT == true)
            {
                ClaimNFTPass(7);
            }
        }

        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(tokenContractAddress);
        var balance = await contract.ERC20.BalanceOf(Address);
        float balanceValue = float.Parse(balance.displayValue);
        if (balanceValue >= costValue)
        {
            nftClaimingStatusText.text = "Buying...";
            nftClaimingStatusText.gameObject.SetActive(true);
            try
            {
                var data = await contract.ERC20.Burn(costValue.ToString());

                if (indexValue == 1)
                {
                    ResourceBoost.Instance.piggyNFT = true;
                    nftClaimingStatusText.text = "Bought Piggy";
                    ClaimNFTPass(1);
                }
                else if (indexValue == 2)
                {
                    ResourceBoost.Instance.doggyNFT = true;
                    nftClaimingStatusText.text = "Bought Doggy";
                    ClaimNFTPass(2);
                }
                else if (indexValue == 3)
                {
                    ResourceBoost.Instance.goatyNFT = true;
                    nftClaimingStatusText.text = "Bought Goaty";
                    ClaimNFTPass(3);
                }
                else if (indexValue == 4)
                {
                    ResourceBoost.Instance.turtleNFT = true;
                    nftClaimingStatusText.text = "Bought Turtle";
                    ClaimNFTPass(4);
                }
                else if (indexValue == 5)
                {
                    ResourceBoost.Instance.koalaNFT = true;
                    nftClaimingStatusText.text = "Bought Koala";
                    ClaimNFTPass(5);
                }
                else if (indexValue == 6)
                {
                    ResourceBoost.Instance.sheepNFT = true;
                    nftClaimingStatusText.text = "Bought Sheep";
                    ClaimNFTPass(6);
                }
                else if (indexValue == 7)
                {
                    ResourceBoost.Instance.duckNFT = true;
                    nftClaimingStatusText.text = "Bought Duck";
                    ClaimNFTPass(7);
                }
                GetTokenBalance();
                ShowAllButton();
            }
            catch (System.Exception ex)
            {
                // Xử lý lỗi
                Debug.LogError("Error burning tokens: " + ex.Message);
                ShowAllButton();
            }                    
        }
        else
        {
            nftClaimingStatusText.text = "Get More Token";
            ShowAllButton();
        }
    }
    public async void SpendTokenToBuyItem(int indexValue) {
        int costValue = 200;
        HideAllButton();
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        var contract = ThirdwebManager.Instance.SDK.GetContract(tokenContractAddress);
        var balance = await contract.ERC20.BalanceOf(Address);
        float balanceValue = float.Parse(balance.displayValue);
        if (balanceValue >= costValue)
        {
            nftClaimingStatusText.text = "Buying...";
            nftClaimingStatusText.gameObject.SetActive(true);
            try
            {
                var data = await contract.ERC20.Burn(costValue.ToString());

                if (indexValue == 1)
                {
                    ResourceBoost.Instance.hp = 1;
                    ShowAllButton();
                    nftClaimingStatusText.text = "+1 Heart";
                    heartText.gameObject.SetActive(true);
                    heartButton.interactable = false;
                }
                else if (indexValue == 2)
                {
                    ResourceBoost.Instance.goldx2 = 2;
                    ShowAllButton();
                    nftClaimingStatusText.text = "x2 Coin";
                    x2CoinText.gameObject.SetActive(true);
                    coinButton.interactable = false;
                }                
                GetTokenBalance();
            }
            catch (System.Exception ex)
            {
                // Xử lý lỗi
                Debug.LogError("Error burning tokens: " + ex.Message);
                ShowAllButton();
            }
        }
        else
        {
            nftClaimingStatusText.text = "Get More Token";
            ShowAllButton();
        }

    }
}
