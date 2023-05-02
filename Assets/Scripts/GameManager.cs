using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{

    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MainMenu()
    {
        OnMainMenu?.Invoke();
    }


    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
    }

    public void ARPosition()
    {
        OnARPosition?.Invoke();
    }

    public void CloseApp()
    {
        Application.Quit();
    }




}
