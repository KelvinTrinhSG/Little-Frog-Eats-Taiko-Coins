using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGameStart : MonoBehaviour
{
    public void ToShopScene()
    {
        SceneManager.LoadScene("ShopAndPlay");
    }
}
