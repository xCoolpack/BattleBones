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
    public ObjectiveHandler ObjectiveHandler;

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
    private VisualElement _objectivesMenu;

    public Building PickedBuilding;
    public Unit PickedUnit;
    public Field PickedField;

    private bool _isOnObjectivesMenu = false;

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
        _objectivesMenu = uiDocument.rootVisualElement.Q<VisualElement>("ObjectivesMenu");

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
        _nextTurnButton.RegisterCallback<ClickEvent>(_ =>
        {
            TurnHandler.NextTurn();
            RemoveInfoBox();
            ClearPicked();
        });

        _objectivesMenu.Q<VisualElement>("OpenButton").RegisterCallback<ClickEvent>(_ => HandleObjectivesMenuClick());

        RemoveInfoBox();
    }

    /// <summary>
    /// Removes InfoBox if it exists
    /// </summary>
    public void RemoveInfoBox()
    {
        RemoveRecruitmentBox();
        var infoBox = _lowerContainer.Q<VisualElement>("InfoBox");
        if (infoBox is null)
            return;

        _lowerContainer.Remove(infoBox);
    }

    /// <summary>
    /// Clears picked objects and marked fields
    /// </summary>
    public void ClearPicked()
    {
        if(PickedUnit is not null)
        {
            PickedUnit.ToggleOffAllMarks();
        }
        PickedBuilding = null;
        PickedField = null;
        PickedUnit = null;
    }

    /// <summary>
    /// Removes RecruitmentBox if it exists
    /// </summary>
    public void RemoveRecruitmentBox()
    {
        var recruitmentBox = _lowerContainer.Q<VisualElement>("RecruitmentBox");
        if (recruitmentBox is null)
            return;

        _lowerContainer.Remove(recruitmentBox);
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

        var closeButton = new Button(() =>
        {
            RemoveInfoBox();
            ClearPicked();
        })
        {
            text = "x"
        };

        closeButton.AddToClassList("InfoBoxCloseButton");
        titleBox.Add(closeButton);

        return infoBox;
    }

    /// <summary>
    /// Creates info box for the given Field. PickedField has to be set before calling method
    /// </summary>
    public void FieldInfoBox()
    {
        if (PickedField is null) throw new ArgumentNullException("PickedField has to be set");
        var infoBox = CreateBasicInfoBox(PickedField.Type.FieldName);
        // TODO
    }

    /// <summary>
    /// Creates info box for the given unit
    /// </summary>
    /// <param name="showButtons"></param>
    public void UnitInfoBox(bool showButtons)
    {
        if (PickedUnit is null) throw new ArgumentNullException("PickedUnit has to be set");
        var infoBox = CreateBasicInfoBox(PickedUnit.BaseUnitStats.UnitName);
        var statsBox = new VisualElement();
        var statsBoxLeft = new VisualElement();
        var statsBoxRight = new VisualElement();
        var buttonsBox = new VisualElement();
        var movePointsLabel = new Label($"Movement points: {PickedUnit.CurrentMovementPoints}/{PickedUnit.MaxMovementPoints}");
        var damageLabel = new Label($"Damage: {PickedUnit.CurrentDamage}");
        var defenseLabel = new Label($"Defense: {PickedUnit.CurrentDefense}");
        var attackRangeLabel = new Label($"Attack range: {PickedUnit.AttackRange}");
        var sightRangeLabel = new Label($"Sight range: {PickedUnit.SightRange}");

        if (showButtons)
        {
            var healButton = new Button(() =>
            {
                PickedUnit.BeginHealing();
                UnitInfoBox(showButtons);
            })
            {
                text = "Heal"
            };

            var defendButton = new Button(() =>
            {
                PickedUnit.BeginDefending();
                UnitInfoBox(showButtons);
            })
            {
                text = "Defend"
            };

            var deleteButton = new Button(() =>
            {
                PickedUnit.Delete();
                RemoveInfoBox();
            })
            {
                text = "Delete"
            };

            healButton.AddToClassList("InfoBoxButton");
            defendButton.AddToClassList("InfoBoxButton");
            deleteButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(healButton);
            buttonsBox.Add(defendButton);
            buttonsBox.Add(deleteButton);
        }

        var hpBar = new HpBar(PickedUnit.CurrentHealth, PickedUnit.MaxHealth);


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
    /// <returns>Pair (statsBox, buttonsBox)</returns>
    /// <param name="showButtons"></param>
    private (VisualElement, VisualElement) CreateBuildingInfoBox(bool showButtons)
    {
        var infoBox = CreateBasicInfoBox(PickedBuilding.BaseBuildingStats.BuildingName);
        var sightRangeLabel = new Label($"Sight range: {PickedBuilding.SightRange}");
        var repairLabel = new Label($"Repair cooldown: {PickedBuilding.BaseRepairCooldown}");
        var statsBox = new VisualElement();
        var buttonsBox = new VisualElement();
        var statsBoxLeft = new VisualElement();
        var statsBoxRight = new VisualElement();

        statsBox.AddToClassList("InnerInfoBox");
        statsBox.AddToClassList("StatsInfoBox");
        buttonsBox.AddToClassList("InnerInfoBox");
        buttonsBox.AddToClassList("ButtonsInfoBox");

        statsBoxLeft.Add(sightRangeLabel);
        statsBoxLeft.Add(repairLabel);

        if (showButtons)
        {
            var destroyButton = new Button(() =>
            {
                PickedBuilding.Destroy();
                RemoveInfoBox();
            })
            {
                text = "Destroy"
            };
            destroyButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(destroyButton);
        }

        foreach (var label in statsBoxLeft.Children())
        {
            label.AddToClassList("StatsLabel");
        }

        statsBoxLeft.AddToClassList("InnerStatsInfoBox");
        statsBoxRight.AddToClassList("InnerStatsInfoBox");

        statsBox.Add(statsBoxLeft);
        statsBox.Add(statsBoxRight);
        infoBox.Add(statsBox);
        infoBox.Add(buttonsBox);

        return (statsBox, buttonsBox);
    }

    private (VisualElement, VisualElement) CreateDefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding)
    {
        var (statsBox, buttonsBox) = CreateBuildingInfoBox(PickedBuilding);
        var damageLabel = new Label($"Damage: {defensiveBuilding.CurrentDamage}");
        var attackRangeLabel = new Label($"Attack range: {defensiveBuilding.AttackRange}");
        //var defenseLabel = new Label($"Defense: {building.CurrentDefense}/{building.MaxDefense}");
        var hpBar = new HpBar(PickedBuilding.CurrentHealth, PickedBuilding.MaxHealth);

        statsBox.parent.Insert(1, hpBar);
        VisualElement statsBoxLeft = ((List<VisualElement>)statsBox.Children())[0];
        VisualElement statsBoxRight = ((List<VisualElement>)statsBox.Children())[1];

        //defenseLabel.AddToClassList("StatsLabel");
        //statsBoxLeft.Add(defenseLabel);
        statsBoxRight.Add(damageLabel);
        statsBoxRight.Add(attackRangeLabel);

        foreach (var label in statsBoxRight.Children())
        {
            label.AddToClassList("StatsLabel");
        }

        return (statsBox, buttonsBox);
    }


    /// <summary>
    /// Displays InfoBox for the given DefensiveBuilding
    /// </summary>
    /// <param name="defensiveBuilding"></param>
    public void DefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException("PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateDefensiveBuildingInfoBox(defensiveBuilding);

        if (showButtons && PickedBuilding.BuildingState == BuildingState.Plundered)
        {
            var repairButton = new Button(() =>
            {
                PickedBuilding.BeginRepair();
                DefensiveBuildingInfoBox(defensiveBuilding, showButtons);
            })
            {
                text = "Repair"
            };
            repairButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(repairButton);
        }
    }

    /// <summary>
    /// Displays InfoBox for the given Outpost
    /// </summary>
    /// <param name="defensiveBuilding"></param>
    /// <param name="outpost"></param>
    /// <param name="showButtons"></param>
    public void OutpostInfoBox(DefensiveBuilding defensiveBuilding, Outpost outpost, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException("PickedBuilding has to be set");

        var (_, buttonsBox) = CreateDefensiveBuildingInfoBox(defensiveBuilding);

        if (showButtons && PickedBuilding.BuildingState == BuildingState.Plundered)
        {
            var repairButton = new Button(() =>
            {
                PickedBuilding.BeginRepair();
                OutpostInfoBox(defensiveBuilding, outpost, showButtons);
            })
            {
                text = "Repair"
            };
            repairButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(repairButton);
        }

        if (showButtons)
        {
            var recruitButton = new Button(() => CreateRecruitmentBox(outpost)) { text = "Recruit" };
            recruitButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(recruitButton);
        }
    }

    private void CreateRecruitmentBox(Outpost outpost)
    {
        var unlockedUnits = outpost.Building.Player.UnlockedUnits;
        var recruitmentBox = new VisualElement() { name = "RecruitmentBox" };

        foreach (var unitObject in unlockedUnits)
        {
            var unit = unitObject.GetComponent<Unit>();
            var unitBox = new VisualElement();
            unitBox.AddToClassList("InnerRecruitmentBox");
            var name = new Label(unit.name);
            var button = new Button() { text = "Buy" };
            button.AddToClassList("InfoBoxButton");
            unitBox.Add(name);
            unitBox.Add(button);
            recruitmentBox.Add(unitBox);
        }

        recruitmentBox.AddToClassList("RecruitmentBox");
        _lowerContainer.Add(recruitmentBox);
    }

    /// <summary>
    /// Creates an info box for the income building
    /// </summary>
    /// <param name="incomeBuilding"></param>
    /// <param name="showButtons"></param>
    public void IncomeBuildingInfoBox(IncomeBuilding incomeBuilding, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException("PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateBuildingInfoBox(PickedBuilding);
        VisualElement statsBoxLeft = ((List<VisualElement>)statsBox.Children())[0];
        VisualElement statsBoxRight = ((List<VisualElement>)statsBox.Children())[1];

        List<Label> labels = new();

        if (incomeBuilding.ResourcesIncome.Gold > 0)
            labels.Add(new Label($"Gold income: {incomeBuilding.ResourcesIncome.Gold}"));

        if (incomeBuilding.ResourcesIncome.Wood > 0)
            labels.Add(new Label($"Wood income: {incomeBuilding.ResourcesIncome.Wood}"));

        if (incomeBuilding.ResourcesIncome.Stone > 0)
            labels.Add(new Label($"Stone income: {incomeBuilding.ResourcesIncome.Stone}"));

        if (incomeBuilding.ResourcesIncome.Doggium > 0)
            labels.Add(new Label($"Doggium income: {incomeBuilding.ResourcesIncome.Doggium}"));

        if (incomeBuilding.ResourcesIncome.Bone > 0)
            labels.Add(new Label($"Bone income: {incomeBuilding.ResourcesIncome.Bone}"));

        for (int i = 0; i < labels.Count; i++)
        {
            var label = labels[i];
            label.AddToClassList("StatsLabel");

            if (i == 0)
            {
                statsBoxLeft.Add(label);
            }
            else
            {
                statsBoxRight.Add(label);
            }
        }


        if (showButtons && PickedBuilding.BuildingState == BuildingState.Plundered)
        {
            var repairButton = new Button(() =>
            {
                PickedBuilding.BeginRepair();
                IncomeBuildingInfoBox(incomeBuilding, showButtons);
            })
            {
                text = "Repair"
            };
            repairButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(repairButton);
        }
    }

    /// <summary>
    /// Creates an info box for the given housing
    /// </summary>
    /// <param name="housing"></param>
    /// <param name="showButtons"></param>
    public void HousingInfoBox(Housing housing, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException("PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateBuildingInfoBox(PickedBuilding);

        if (showButtons && PickedBuilding.BuildingState == BuildingState.Plundered)
        {
            var repairButton = new Button(() =>
            {
                PickedBuilding.BeginRepair();
                HousingInfoBox(housing, PickedBuilding);
            })
            {
                text = "Repair"
            };
            repairButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(repairButton);
        }

        VisualElement statsBoxLeft = ((List<VisualElement>)statsBox.Children())[0];
        VisualElement statsBoxRight = ((List<VisualElement>)statsBox.Children())[1];

        var capLabel = new Label($"Unit cap: {housing.UnitCap}");
        capLabel.AddToClassList("StatsLabel");
        statsBoxRight.Add(capLabel);
    }

    /// <summary>
    /// Creates an info box for the giveb Barricade
    /// </summary>
    /// <param name="barricade"></param>
    /// <param name="showButtons"></param>
    public void BarricadeInfoBox(Barricade barricade, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException("PickedBuilding has to be set");
        var (statsBox, buttonsBox) = CreateBuildingInfoBox(PickedBuilding);
    }

    private void HandleObjectivesMenuClick()
    {
        if (_isOnObjectivesMenu)
        {
            _isOnObjectivesMenu = false;
            ToggleOffObjectives();
        }
        else
        {
            _isOnObjectivesMenu = true;
            ToggleOnObjectives();
        }
    }

    /// <summary>
    /// Toggles on objectives info
    /// </summary>
    private void ToggleOnObjectives()
    {
        var list = new VisualElement();
        list.name = "ObjectivesList";

        foreach (var objective in ObjectiveHandler.Objectives)
        {
            var box = new VisualElement();
            var primary = new Label(objective.IsPrimary ? "Primary" : "Side");
            var complited = new Label(objective.IsComplited ? "Complited" : "Not complited");
            var info = new Label(objective.ObjectiveInfo);
            box.Add(primary);
            box.Add(info);
            box.Add(complited);
            box.AddToClassList("ObjectiveInfoBox");
            list.Add(box);
        }

        _objectivesMenu.Add(list);
    }

    /// <summary>
    /// Toggles off objectives info
    /// </summary>
    private void ToggleOffObjectives()
    {
        _objectivesMenu.RemoveAt(1);
    }
}
