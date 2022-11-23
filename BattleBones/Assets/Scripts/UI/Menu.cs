using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public UIDocument UiDocument;

    private Button _newGameButton;
    private Button _loadButton;
    private Button _quitButton;

    private void Start()
    {
        UiDocument = GetComponent<UIDocument>();
        _newGameButton = UiDocument.rootVisualElement.Q<Button>("NewGameButton");
        _loadButton = UiDocument.rootVisualElement.Q<Button>("LoadGameButton");
        _quitButton = UiDocument.rootVisualElement.Q<Button>("QuitButton");

        _newGameButton.RegisterCallback<ClickEvent>(_ =>
        {
            StartNewGame();
        });

        _loadButton.RegisterCallback<ClickEvent>(_ =>
        {
            LoadGame();
        });

        _quitButton.RegisterCallback<ClickEvent>(_ =>
        {
            Quit();
        });
    }

    private void StartNewGame()
    {
        SceneManager.LoadScene("CampaignMapScene");
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("CampaignMapScene");
    }

    private void Quit()
    {
        // does not work in editor
        Application.Quit();
    }
}
