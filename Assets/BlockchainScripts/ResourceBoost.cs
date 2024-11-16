using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;

public class ResourceBoost : MonoBehaviour
{
    private static ResourceBoost _instance;

    public static ResourceBoost Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceBoost>();

                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject("CanvasBlockchain");
                    _instance = singletonObject.AddComponent<ResourceBoost>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public int gold;
    public int hp;
    public int goldx2;
    public int piggy;
    public int doggy;
    public int goaty;
    public int turtle;
    public int koala;
    public int sheep;
    public int duck;

    public bool piggyNFT;
    public bool doggyNFT;
    public bool goatyNFT;
    public bool turtleNFT;
    public bool koalaNFT;
    public bool sheepNFT;
    public bool duckNFT;

    private void Start()
    {
        gold = 0;
        hp = 0;
        goldx2 = 1;
    }

}
