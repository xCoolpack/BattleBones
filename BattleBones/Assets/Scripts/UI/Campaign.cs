using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Campaign : MonoBehaviour
{
    public UIDocument UiDocument;
    public MissionHandler MissionHandler;

    private Label _trophiesLabel;
    private Button _mission1Button;
    private Label _mission1Label;
    private Button _mission2Button;
    private Label _mission2Label;
    private Button _mission3Button;
    private Label _mission3Label;
    private Button _mission4Button;
    private Label _mission4Label;

    private void Start()
    {
        UiDocument = GetComponent<UIDocument>();
        _trophiesLabel = UiDocument.rootVisualElement.Q<Label>("TrophiesLabel");
        _trophiesLabel.text = $"Trophies: {MissionHandler.CurrentTrophies}";

        var buttons = new List<Button>()
        {
            UiDocument.rootVisualElement.Q<Button>("Mission1Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission2Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission3Button"),
            UiDocument.rootVisualElement.Q<Button>("Mission4Button")
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
        }
    }
}
