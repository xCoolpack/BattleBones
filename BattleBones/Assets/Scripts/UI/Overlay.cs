using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Overlay : MonoBehaviour
{
    public ResourceManager ResourceManager;
    public TurnHandler TurnHandler;

    private Button _nextTurnButton;
    private Label _turnCounterLabel;
    private Label _goldLabel;
    private Label _woodLabel;
    private Label _stoneLabel;
    private Label _doggiumLabel;
    private Label _boneLabel;
    private Label _goldIncomeLabel;
    private Label _woodIncomeLabel;
    private Label _stoneIncomeLabel;
    private Label _doggiumIncomeLabel;
    private Label _boneIncomeLabel;
    private VisualElement _lowerContainer;

    private void OnEnable()
    {
        // Get references from UI
        var uiDocument = GetComponent<UIDocument>();
        _nextTurnButton = uiDocument.rootVisualElement.Q<Button>("NextTurnButton");
        _turnCounterLabel = uiDocument.rootVisualElement.Q<Label>("TurnCounterLabel");

        _goldLabel = uiDocument.rootVisualElement.Q<Label>("GoldLabel");
        _woodLabel = uiDocument.rootVisualElement.Q<Label>("WoodLabel");
        _stoneLabel = uiDocument.rootVisualElement.Q<Label>("StoneLabel");
        _doggiumLabel = uiDocument.rootVisualElement.Q<Label>("DoggiumLabel");
        _boneLabel = uiDocument.rootVisualElement.Q<Label>("BoneLabel");

        _goldIncomeLabel = uiDocument.rootVisualElement.Q<Label>("GoldIncomeLabel");
        _woodIncomeLabel = uiDocument.rootVisualElement.Q<Label>("WoodIncomeLabel");
        _stoneIncomeLabel = uiDocument.rootVisualElement.Q<Label>("StoneIncomeLabel");
        _doggiumIncomeLabel = uiDocument.rootVisualElement.Q<Label>("DoggiumIncomeLabel");
        _boneIncomeLabel = uiDocument.rootVisualElement.Q<Label>("BoneIncomeLabel");

        _lowerContainer = uiDocument.rootVisualElement.Q<VisualElement>("LowerContainer");

        // Binding turn handler
        var tunHandlerSerializedObject = new SerializedObject(TurnHandler);
        _turnCounterLabel.bindingPath = "TurnCounter";
        uiDocument.rootVisualElement.Bind(tunHandlerSerializedObject);

        // Binding resource manager
        var resourcesAmount = new SerializedObject(ResourceManager);
        _goldLabel.bindingPath = "ResourcesAmount.Gold";
        _goldIncomeLabel.bindingPath = "ResourcesIncome.Gold";
        _woodLabel.bindingPath = "ResourcesAmount.Wood";
        _woodIncomeLabel.bindingPath = "ResourcesIncome.Wood";
        _stoneLabel.bindingPath = "ResourcesAmount.Stone";
        _stoneIncomeLabel.bindingPath = "ResourcesIncome.Stone";
        _doggiumLabel.bindingPath = "ResourcesAmount.Doggium";
        _doggiumIncomeLabel.bindingPath = "ResourcesIncome.Doggium";
        _boneLabel.bindingPath = "ResourcesAmount.Bone";
        _boneIncomeLabel.bindingPath = "ResourcesIncome.Bone";
        uiDocument.rootVisualElement.Bind(resourcesAmount);

        // Registering callbacks
        _nextTurnButton.RegisterCallback<ClickEvent>(_ => TurnHandler.NextTurn());

        // temp
        uiDocument.rootVisualElement.Q<Button>("Button1").RegisterCallback<ClickEvent>(_ => RemoveInfoBox());
        uiDocument.rootVisualElement.Q<Button>("Button2").RegisterCallback<ClickEvent>(_ => UnitInfoBox(GameObject.Find("TestUnit").GetComponent<Unit>()));
        RemoveInfoBox();
    }

    private void TestDefensiveBuilding()
    {
        var go = GameObject.Find("TestBuilding");
        var b = go.GetComponent<Building>();
        var db = go.GetComponent<DefensiveBuilding>();
        DefensiveBuildingInfoBox(db, b);
    }

    /// <summary>
    /// Removes InfoBox if it exists
    /// </summary>
    public void RemoveInfoBox()
    {
        var infoBox = _lowerContainer.Q<VisualElement>("InfoBox");
        if (infoBox is null)
            return;

        _lowerContainer.Remove(infoBox);
    }

    /// <summary>
    /// Creates base info box with title. Removes existing info box
    /// </summary>
    /// <param name="title"></param>
    private VisualElement CreateBasicInfoBox(string title)
    {
        RemoveInfoBox();
        var infoBox = new VisualElement();
        var titleBox = new VisualElement();
        var titleLabel = new Label(title);

        titleLabel.AddToClassList("TitleLabel");
        titleBox.Add(titleLabel);
        titleBox.AddToClassList("InnerInfoBox");
        titleBox.AddToClassList("TitleInfoBox");
        infoBox.Add(titleBox);
        infoBox.AddToClassList("InfoBox");
        infoBox.name = "InfoBox";
        _lowerContainer.Add(infoBox);

        return infoBox;
    }

    /// <summary>
    /// Creates info box for the given unit
    /// </summary>
    /// <param name="unit"></param>
    public void UnitInfoBox(Unit unit)
    {
        var infoBox = CreateBasicInfoBox(unit.BaseUnitStats.UnitName);
        var statsBox = new VisualElement();
        var statsBoxLeft = new VisualElement();
        var statsBoxRight = new VisualElement();
        var buttonsBox = new VisualElement();
        var movePointsLabel = new Label($"Movement points: {unit.CurrentMovementPoints}/{unit.MaxMovementPoints}");
        var damageLabel = new Label($"Damage: {unit.CurrentDamage}");
        var defenseLabel = new Label($"Defense: {unit.CurrentDefense}");
        var attackRangeLabel = new Label($"Attack range: {unit.AttackRange}");
        var sightRangeLabel = new Label($"Sight range: {unit.SightRange}");

        var healButton = new Button(() =>
        {
            unit.BeginHealing();
            UnitInfoBox(unit);
        })
        {
            text = "Heal"
        };

        var defendButton = new Button(() =>
        {
            unit.BeginDefending();
            UnitInfoBox(unit);
        })
        {
            text = "Defend"
        };

        var deleteButton = new Button(() =>
        {
            unit.Delete();
            RemoveInfoBox();
        })
        {
            text = "Delete"
        };

        var hpBar = new HpBar(unit.CurrentHealth, unit.MaxHealth);

        healButton.AddToClassList("InfoBoxButton");
        defendButton.AddToClassList("InfoBoxButton");
        deleteButton.AddToClassList("InfoBoxButton");
        hpBar.AddToClassList("InnerInfoBox");
        
        statsBoxLeft.Add(damageLabel);
        statsBoxLeft.Add(defenseLabel);
        statsBoxLeft.Add(movePointsLabel);
        statsBoxRight.Add(attackRangeLabel);
        statsBoxRight.Add(sightRangeLabel);

        foreach (var label in statsBoxLeft.Children())
        {
            label.AddToClassList("StatsLabel");
        }

        foreach (var label in statsBoxRight.Children())
        {
            label.AddToClassList("StatsLabel");
        }

        buttonsBox.Add(healButton);
        buttonsBox.Add(defendButton);
        buttonsBox.Add(deleteButton);

        statsBox.AddToClassList("InnerInfoBox");
        statsBox.AddToClassList("StatsInfoBox");
        buttonsBox.AddToClassList("InnerInfoBox");
        buttonsBox.AddToClassList("ButtonsInfoBox");
        statsBoxLeft.AddToClassList("InnerStatsInfoBox");
        statsBoxRight.AddToClassList("InnerStatsInfoBox");

        
        statsBox.Add(statsBoxLeft);
        statsBox.Add(statsBoxRight);
        infoBox.Add(hpBar);
        infoBox.Add(statsBox);
        infoBox.Add(buttonsBox);
    }

    /// <summary>
    /// Creates info box for the given building
    /// </summary>
    /// <param name="building"></param>
    /// <returns>Pair (statsBox, buttonsBox)</returns>
    private (VisualElement, VisualElement) CreateBuildingInfoBox(Building building)
    {
        var infoBox = CreateBasicInfoBox(building.BaseBuildingStats.BuildingName);
        var sightRangeLabel = new Label($"Sight range: {building.SightRange}");
        var repairLabel = new Label($"Repair cooldown: {building.CurrentRepairCooldown}");
        var statsBox = new VisualElement();
        var buttonsBox = new VisualElement();

        statsBox.AddToClassList("InnerInfoBox");
        statsBox.AddToClassList("StatsInfoBox");
        buttonsBox.AddToClassList("InnerInfoBox");
        buttonsBox.AddToClassList("ButtonsInfoBox");

        statsBox.Add(sightRangeLabel);
        statsBox.Add(repairLabel);


        if (building.BuildingState == BuildingState.Plundered)
        {
            var repairButton = new Button(building.BeginRepair)
            {
                text = "Repair"
            };
            repairButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(repairButton);
        }

        var destroyButton = new Button(building.Destroy)
        {
            text = "Destroy"
        };
        destroyButton.AddToClassList("InfoBoxButton");

        buttonsBox.Add(destroyButton);

        infoBox.Add(statsBox);
        infoBox.Add(buttonsBox);

        return (statsBox, buttonsBox);
    }

    private (VisualElement, VisualElement) CreateDefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding, Building building)
    {
        var (statsBox, buttonsBox) = CreateBuildingInfoBox(building);
        var damageLabel = new Label($"Damage: {defensiveBuilding.CurrentDamage}");
        var attackRangeLabel = new Label($"Attack range: {defensiveBuilding.AttackRange}");
        //var hpLabel = new Label($"Health: {building.CurrentHealth}/{building.MaxHealth}");
        var defenseLabel = new Label($"Defense: {building.CurrentDefense}/{building.MaxDefense}");
        var hpBar = new HpBar(2, 3);

        //statsBox.
        //statsBox.Add(defenseLabel);
        statsBox.Add(damageLabel);
        statsBox.Add(attackRangeLabel);

        foreach (var label in statsBox.Children())
        {
            label.AddToClassList("StatsLabel");
        }

        return (statsBox, buttonsBox);
    }

    public void DefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding, Building building)
    {
        CreateDefensiveBuildingInfoBox(defensiveBuilding, building);
    }

    public void OutpostInfoBox(DefensiveBuilding defensiveBuilding, Building building, Outpost outpost)
    {
        var (_, buttonsBox) = CreateDefensiveBuildingInfoBox(defensiveBuilding, building);
        var recruitButton = new Button(() => CreateRecruitmentBox(outpost));
        recruitButton.AddToClassList("InfoBoxButton");
    }

    private void CreateRecruitmentBox(Outpost outpost)
    {
        throw new NotImplementedException();
    }

    public void IncomeBuildingInfoBox(IncomeBuilding incomeBuilding, Building building)
    {
        var (statsBox, _) = CreateBuildingInfoBox(building);

        if (incomeBuilding.ResourcesIncome.Gold > 0)
            statsBox.Add(new Label($"Gold income: {incomeBuilding.ResourcesIncome.Gold}"));

        if (incomeBuilding.ResourcesIncome.Wood > 0)
            statsBox.Add(new Label($"Wood income: {incomeBuilding.ResourcesIncome.Wood}"));

        if (incomeBuilding.ResourcesIncome.Stone > 0)
            statsBox.Add(new Label($"Stone income: {incomeBuilding.ResourcesIncome.Stone}"));

        if (incomeBuilding.ResourcesIncome.Doggium > 0)
            statsBox.Add(new Label($"Doggium income: {incomeBuilding.ResourcesIncome.Doggium}"));

        if (incomeBuilding.ResourcesIncome.Bone > 0)
            statsBox.Add(new Label($"Bone income: {incomeBuilding.ResourcesIncome.Bone}"));
    }
}
