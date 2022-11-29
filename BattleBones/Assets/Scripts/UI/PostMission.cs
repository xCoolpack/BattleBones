using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PostMission : MonoBehaviour
{
    private UIDocument UiDocument;

    // Start is called before the first frame update
    void Start(){
        UiDocument = GetComponent<UIDocument>();

        var menuButton = UiDocument.rootVisualElement.Q<Button>("MenuButton");

        menuButton.RegisterCallback<ClickEvent>(_ =>
        {
            SceneManager.LoadScene("CampaignMapScene");
        });
    }
}
