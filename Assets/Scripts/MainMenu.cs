using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    MainMenu menu;
    public Button Tesla, RocketList;
    void Start()
    {
        Tesla.onClick.AddListener(()=>LoadScene(1));
        RocketList.onClick.AddListener(() => LoadScene(2));
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        menu = this;
        if (menu != this)
        {
            Destroy(menu);
            menu = this;
        }
    }
    void Update()
    {
        
    }

    void LoadScene(int num)
    {
        SceneManager.LoadScene(num);
    }
    
}
