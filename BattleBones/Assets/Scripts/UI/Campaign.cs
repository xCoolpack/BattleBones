using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Campaign : MonoBehaviour
{
    public UIDocument UiDocument;
    public MissionHandler MissionHandler;

    private void Start()
    {
        UiDocument = GetComponent<UIDocument>();
        var trophiesLabel = UiDocument.rootVisualElement.Q<Label>("TrophiesLabel");
        trophiesLabel.text = $"Trophies: {MissionHandler.CurrentTrophies}";

        var backButton = UiDocument.rootVisualElement.Q<Button>("BackButton");

        backButton.RegisterCallback<ClickEvent>(_ =>
        {
            SceneManager.LoadScene("MenuScene");
        });

        var buttons = new List<Button>()
        {
            UiDocument.rootVisualElement.Q<Button>("Mission1Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission2Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission3Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission4Button")
        };

        var labels = new List<Label>()
        {
            UiDocument.rootVisualElement.Q<Label>("Mission1Label"),
            UiDocument.rootVisualElement.Q<Label>("Mission2Label"),
            UiDocument.rootVisualElement.Q<Label>("Mission3Label"),
            UiDocument.rootVisualElement.Q<Label>("Mission4Label")
        };

        for (int i = 0; i < buttons.Count; i++)
        {
            var mission = MissionHandler.MissionList[i];

            if (!mission.CanStartMission())
                buttons[i].SetEnabled(false);

            buttons[i].text = mission.MissionName;
            buttons[i].tooltip = mission.MissionDescription;

            if(mission.CanStartMission())
                buttons[i].RegisterCallback<ClickEvent>(_ =>
                {
                    mission.StartMission();
                });

            var objectivesDone = mission.Objectives.Sum(o => PlayerPrefs.GetInt(o.ObjectiveId.ToString()));
            var objectivesSum = mission.Objectives.Count;

            labels[i].text = $"Progress: {objectivesDone}/{objectivesSum}";
        }
    }
}
