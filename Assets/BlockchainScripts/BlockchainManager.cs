using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;

public class BlockchainManager : MonoBehaviour
{
    public Button buttonNFTTokenGate;
    public Button buttonStartGame;
    public Button buttonShop;
    public Button buttonBlockchain;

    public Text description;
    public string Address { get; private set; }

    private string NFTSmartContractAddress = "0xEa3071E38de66B78f360245f8fC5d21b2e357590";

    // Start is called before the first frame update
    private void ResetAllButton()
    {
        buttonNFTTokenGate.gameObject.SetActive(false);
        buttonStartGame.gameObject.SetActive(false);
        buttonShop.gameObject.SetActive(false);
        buttonBlockchain.gameObject.SetActive(false);

        buttonNFTTokenGate.interactable = true;
        buttonStartGame.interactable = true;
        buttonShop.interactable = true;
        buttonBlockchain.interactable = true;

        description.text = "Please help the poor little frog find ETH coins";
    }

    public async void Login()
    {
        ResetAllButton();
        Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
        Debug.Log(Address);
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(NFTSmartContractAddress);
        List<NFT> nftList = await contract.ERC721.GetOwned(Address);
        if (nftList.Count == 0)
        {
            description.text = "Claim 1 Token Gate NFT for Playing";
            buttonNFTTokenGate.gameObject.SetActive(true);
            buttonStartGame.gameObject.SetActive(false);
            buttonShop.gameObject.SetActive(false);
            buttonBlockchain.gameObject.SetActive(false);
        }
        else
        {
            description.text = "Please help the poor little frog find ETH coins";
            buttonNFTTokenGate.gameObject.SetActive(false);
            buttonStartGame.gameObject.SetActive(true);
            buttonShop.gameObject.SetActive(true);
            buttonBlockchain.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFTPass()
    {
        buttonNFTTokenGate.interactable = false;
        var contract = ThirdwebManager.Instance.SDK.GetContract(NFTSmartContractAddress);
        var result = await contract.ERC721.ClaimTo(Address, 1);
        buttonNFTTokenGate.gameObject.SetActive(false);
        buttonStartGame.gameObject.SetActive(true);
        buttonShop.gameObject.SetActive(true);
        buttonBlockchain.gameObject.SetActive(true);
        description.text = "Please help the poor little frog find ETH coins";
    }
}
