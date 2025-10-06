
using System;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Single<UIManager>
{
    private bool _pauseMenuOn = false;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject[] menuTabs = null;
    [SerializeField] private Button[] menuButtons = null;
    [SerializeField] private UIInventoryBar inventoryBar = null;
    [SerializeField] private PauseMenuInventoryManagement pauseMenuInventoryManagement = null;

    public bool PauseMenuOn { get => _pauseMenuOn; set => _pauseMenuOn = value; }

    protected override void Awake()
    {
        base.Awake();
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        PauseMenu();
    }

    private void PauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(PauseMenuOn)
            {
                DisablePauseMenu();
            }
            else
            {
                EnablePauseMenu();
            }
        }
    }

    private void EnablePauseMenu()
    {
        inventoryBar.DestoryCurrentDraggedItem();
        inventoryBar.ClearCurrentSelectedItem();

        PauseMenuOn = true;
        Player.Instance.DisablePlayerInput();
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        GC.Collect();
        HighlightButtonForSelectedTab();

        
    }

    

    public void DisablePauseMenu()
    {
        pauseMenuInventoryManagement.DestroyCurrentDraggedItems();

        PauseMenuOn = false;
        Player.Instance.EnablePlayerInput();
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

        
    }

    private void HighlightButtonForSelectedTab()
    {
        for(int i = 0; i < menuTabs.Length; i++)
        {
            if(menuTabs[i].activeSelf)
            {
                SetButtonColorToActive(menuButtons[i]);
            }
            else
            {
                SetButtonColorToInactive(menuButtons[i]);
            }
        }
    }

    

    private void SetButtonColorToActive(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = colors.pressedColor;
        button.colors = colors;
    }

    private void SetButtonColorToInactive(Button button)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = colors.disabledColor;
        button.colors = colors;
    }

    public void SwitchPauseMenuTab(int tabIndex)
    {
        for(int i = 0;i < menuTabs.Length;i++)
        {
            if (i != tabIndex)
            {
                menuTabs[i].SetActive(false);
            }
            else
            {
                menuTabs[i].SetActive(true);
            }
        }
        HighlightButtonForSelectedTab();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveGame()
    {
        SaveLoadManager.Instance.SaveDataToFile();
    }
    public void LoadGame()
    {
        SaveLoadManager.Instance.LoadDataFromFile();
    }
}
