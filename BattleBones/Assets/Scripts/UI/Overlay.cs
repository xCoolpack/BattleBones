using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
//using UnityEditor;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.EventSystems;
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
    private VisualElement _aspectRatioPanel;
    private VisualElement _goldContainer;
    private VisualElement _woodContainer;
    private VisualElement _stoneContainer;
    private VisualElement _doggiumContainer;
    private VisualElement _boneContainer;

    private static VisualElement _loggerBody;
    private static ScrollView _logger;
    private static VisualElement _loggerEnd;
    private Button _loggerButton;

    public Building PickedBuilding;
    public Unit PickedUnit;
    public Field PickedField;
    public UIDocument UiDocument;

    private bool _isOnObjectivesMenu;
    public bool IsPointerOverUI { get; private set; }

    private bool _isLoggerBig = false;

    private void OnEnable()
    {
        // Get references from UI
        UiDocument = GetComponent<UIDocument>();
        _nextTurnButton = UiDocument.rootVisualElement.Q<Button>("NextTurnButton");
        _turnCounterLabel = UiDocument.rootVisualElement.Q<Label>("TurnCounterLabel");

        _goldLabel = UiDocument.rootVisualElement.Q<Label>("GoldLabel");
        _woodLabel = UiDocument.rootVisualElement.Q<Label>("WoodLabel");
        _stoneLabel = UiDocument.rootVisualElement.Q<Label>("StoneLabel");
        _doggiumLabel = UiDocument.rootVisualElement.Q<Label>("DoggiumLabel");
        _boneLabel = UiDocument.rootVisualElement.Q<Label>("BoneLabel");

        _goldIncomeLabel = UiDocument.rootVisualElement.Q<Label>("GoldIncomeLabel");
        _woodIncomeLabel = UiDocument.rootVisualElement.Q<Label>("WoodIncomeLabel");
        _stoneIncomeLabel = UiDocument.rootVisualElement.Q<Label>("StoneIncomeLabel");
        _doggiumIncomeLabel = UiDocument.rootVisualElement.Q<Label>("DoggiumIncomeLabel");
        _boneIncomeLabel = UiDocument.rootVisualElement.Q<Label>("BoneIncomeLabel");

        _goldContainer = UiDocument.rootVisualElement.Q<VisualElement>("GoldContainer");
        _stoneContainer = UiDocument.rootVisualElement.Q<VisualElement>("StoneContainer");
        _woodContainer = UiDocument.rootVisualElement.Q<VisualElement>("WoodContainer");
        _doggiumContainer = UiDocument.rootVisualElement.Q<VisualElement>("DoggiumContainer");
        _boneContainer = UiDocument.rootVisualElement.Q<VisualElement>("BoneContainer");

        _lowerContainer = UiDocument.rootVisualElement.Q<VisualElement>("LowerContainer");
        _objectivesMenu = UiDocument.rootVisualElement.Q<VisualElement>("ObjectivesMenu");

        _aspectRatioPanel = UiDocument.rootVisualElement.Q<VisualElement>("AspectRatioPanel");

        _loggerBody = UiDocument.rootVisualElement.Q<VisualElement>("LoggerBody");
        _loggerEnd = UiDocument.rootVisualElement.Q<VisualElement>("LoggerEnd");
        _logger = UiDocument.rootVisualElement.Q<ScrollView>("Logger");
        _loggerButton = UiDocument.rootVisualElement.Q<Button>("LoggerButton");

        // Binding turn handler
        //var tunHandlerSerializedObject = new SerializedObject(TurnHandler);
        //_turnCounterLabel.bindingPath = "TurnCounter";
        //UiDocument.rootVisualElement.Bind(tunHandlerSerializedObject);

        //// Binding resource manager
        //var resourcesAmount = new SerializedObject(ResourceManager);
        //_goldLabel.bindingPath = "ResourcesAmount.Gold";
        //_goldIncomeLabel.bindingPath = "ResourcesIncome.Gold";
        //_woodLabel.bindingPath = "ResourcesAmount.Wood";
        //_woodIncomeLabel.bindingPath = "ResourcesIncome.Wood";
        //_stoneLabel.bindingPath = "ResourcesAmount.Stone";
        //_stoneIncomeLabel.bindingPath = "ResourcesIncome.Stone";
        //_doggiumLabel.bindingPath = "ResourcesAmount.Doggium";
        //_doggiumIncomeLabel.bindingPath = "ResourcesIncome.Doggium";
        //_boneLabel.bindingPath = "ResourcesAmount.Bone";
        //_boneIncomeLabel.bindingPath = "ResourcesIncome.Bone";
        //UiDocument.rootVisualElement.Bind(resourcesAmount);

        // Add images to resources
        _goldContainer.Insert(0, Icon.Gold);
        _stoneContainer.Insert(0, Icon.Stone);
        _woodContainer.Insert(0, Icon.Wood);
        _doggiumContainer.Insert(0, Icon.Doggium);
        _boneContainer.Insert(0, Icon.Bone);

        _nextTurnButton.Add(Icon.Hourglass);

        // Registering callbacks
        _nextTurnButton.RegisterCallback<ClickEvent>(_ =>
        {
            TurnHandler.NextTurn();
            RemoveInfoBox();
            ClearPicked();
        });

        _objectivesMenu.Q<VisualElement>("OpenButton").RegisterCallback<ClickEvent>(_ =>
        {
            HandleObjectivesMenuClick();
        });

        _loggerButton.RegisterCallback<ClickEvent>(_ =>
        {
            HandleLoggerButtonClick();
        });

        foreach (var child in _aspectRatioPanel.Children())
        {
            child.RegisterCallback<MouseEnterEvent>(_ => IsPointerOverUI = true);
            child.RegisterCallback<MouseLeaveEvent>(_ => IsPointerOverUI = false);
        }

        RemoveInfoBox();
    }

    private void Update()
    {
        _logger.ScrollTo(_loggerEnd);
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
        IsPointerOverUI = false;
    }

    /// <summary>
    /// Clears picked objects and marked fields
    /// </summary>
    public void ClearPicked()
    {
        var field = PickedField ?? PickedBuilding?.Field ?? PickedUnit?.Field;

        if (field != null)
        {
            field.Mark_ = Field.Mark.Unmarked;
            field.ChosenMark.SetActive(false);
        }

        PickedUnit?.ToggleOffAllMarks();
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
        IsPointerOverUI = false;
    }

    /// <summary>
    /// Creates base info box with title. Removes existing info box-
    /// </summary>
    /// <param name="title"></param>
    private VisualElement CreateBasicInfoBox(string title)
    {
        RemoveInfoBox();

        if (title.Length > 33)
        {
            title = title[..33] + '.';
        }

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
        if (PickedField is null) throw new ArgumentNullException(nameof(PickedField), "PickedField has to be set");
        var title = $"{PickedField.Type.FieldName} {PickedField.ThreeAxisCoordinates} - cost {PickedField.Type.MovementPointsCost}";
        var infoBox = CreateBasicInfoBox(title);
        var buildings = PickedField.Type.AllowableBuildings;
        var container = new VisualElement();
        container.AddToClassList("BuildBoxContainer");

        foreach (var building in buildings)
        {
            var buyBox = new VisualElement();
            var buildingName = new Label(building.BuildingName);
            var costs = new List<VisualElement>();

            var cost = building.BaseCost;

            if (cost.Bone > 0)
                costs.Add(Combine(Icon.Bone, new Label(cost.Bone.ToString())));

            if (cost.Doggium > 0)
                costs.Add(Combine(Icon.Doggium, new Label(cost.Doggium.ToString())));

            if (cost.Gold > 0)
                costs.Add(Combine(Icon.Gold, new Label(cost.Gold.ToString())));

            if (cost.Stone > 0)
                costs.Add(Combine(Icon.Stone, new Label(cost.Stone.ToString())));

            if (cost.Wood > 0)
                costs.Add(Combine(Icon.Wood, new Label(cost.Wood.ToString())));

            buyBox.Add(buildingName);
            costs.ForEach(c => 
            {
                c.AddToClassList("CostContainer");
                buyBox.Add(c);
            });

            if (PickedField.CanConstruct(TurnHandler.CurrentPlayer, building.BuildingName))
            {
                var buyButton = new Button(() =>
                {
                    PickedField.BeginBuildingConstruction(TurnHandler.CurrentPlayer, building.BuildingName);
                    RemoveInfoBox();
                    ClearPicked();
                })
                {
                    text = "Build"
                };
                buyButton.AddToClassList("InfoBoxButton");
                buyButton.AddToClassList("BuildButton");
                buyBox.Add(buyButton);
            }
            else
            {
                var cantAffordLabel = new Label("Can't afford");
                cantAffordLabel.AddToClassList("BuildButton");
                buyBox.Add(cantAffordLabel);
            }

            buyBox.AddToClassList("BuildBox");
            container.Add(buyBox);
        }

        infoBox.AddToClassList("BuildInfoBox");
        infoBox.Add(container);
    }

    /// <summary>
    /// Creates info box for the given unit
    /// </summary>
    /// <param name="showButtons"></param>
    public void UnitInfoBox(bool showButtons)
    {
        if (PickedUnit is null) throw new ArgumentNullException(nameof(PickedUnit), "PickedUnit has to be set");
        var infoBox = CreateBasicInfoBox($"{PickedUnit.BaseUnitStats.UnitName} {PickedUnit.Field.ThreeAxisCoordinates}");
        var statsBox = new VisualElement();
        var statsBoxLeft = new VisualElement();
        var statsBoxRight = new VisualElement();
        var buttonsBox = new VisualElement();

        var movePointsLabel = new Label($"{PickedUnit.CurrentMovementPoints}/{PickedUnit.MaxMovementPoints}");
        var damageLabel = new Label(PickedUnit.CurrentDamage.ToString());
        var defenseLabel = new Label(PickedUnit.CurrentDefense.ToString());
        var attackRangeLabel = new Label(PickedUnit.AttackRange.ToString());
        var sightRangeLabel = new Label(PickedUnit.SightRange.ToString());

        var movePoints = Combine(Icon.Boot, movePointsLabel);
        var damage = Combine(Icon.Sword, damageLabel);
        var defense = Combine(Icon.Shield, defenseLabel);
        var attackRange = Combine(Icon.Target, attackRangeLabel);
        var sightRange = Combine(Icon.Eye, sightRangeLabel);

        movePoints.AddToClassList("CostContainer");
        damage.AddToClassList("CostContainer");
        defense.AddToClassList("CostContainer");
        attackRange.AddToClassList("CostContainer");
        sightRange.AddToClassList("CostContainer");

        if (showButtons)
        {
            if (PickedUnit.CanHeal())
            {
                var healButton = new Button(() =>
                {
                    PickedUnit.BeginHealing();
                    UnitInfoBox(showButtons);
                })
                {
                    text = "Heal"
                };
                healButton.AddToClassList("InfoBoxButton");
                buttonsBox.Add(healButton);
            }

            if (PickedUnit.CanDefend())
            {
                var defendButton = new Button(() =>
                {
                    PickedUnit.BeginDefending();
                    UnitInfoBox(showButtons);
                })
                {
                    text = "Defend"
                };
                defendButton.AddToClassList("InfoBoxButton");
                buttonsBox.Add(defendButton);
            }

            if (PickedUnit.CanPlunder())
            {
                var plunderButton = new Button(() =>
                {
                    PickedUnit.Plunder();
                    UnitInfoBox(showButtons);
                })
                {
                    text = "Plunder"
                };
                plunderButton.AddToClassList("InfoBoxButton");
                buttonsBox.Add(plunderButton);
            }

            var deleteButton = new Button(() =>
            {
                PickedUnit.Field.TurnOffChosenMark();
                PickedUnit.Delete();
                RemoveInfoBox();
                ClearPicked();
            })
            {
                text = "Delete"
            };

            deleteButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(deleteButton);
        }

        var hpBar = new HpBar(PickedUnit.CurrentHealth, PickedUnit.MaxHealth);
        hpBar.AddToClassList("InnerInfoBox");

        statsBoxLeft.Add(damage);
        statsBoxLeft.Add(defense);
        statsBoxLeft.Add(movePoints);
        statsBoxRight.Add(attackRange);
        statsBoxRight.Add(sightRange);

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
        var infoBox = CreateBasicInfoBox($"{PickedBuilding.BaseBuildingStats.BuildingName} {PickedBuilding.Field.ThreeAxisCoordinates}-{PickedBuilding.GetBuildingStateName()}");
        var sightRangeLabel = new Label(PickedBuilding.SightRange.ToString());
        var sightRange = Combine(Icon.Eye, sightRangeLabel);
        sightRange.AddToClassList("CostContainer");
        var statsBox = new VisualElement();
        var buttonsBox = new VisualElement();
        var statsBoxLeft = new VisualElement();
        var statsBoxRight = new VisualElement();

        statsBox.AddToClassList("InnerInfoBox");
        statsBox.AddToClassList("StatsInfoBox");
        buttonsBox.AddToClassList("InnerInfoBox");
        buttonsBox.AddToClassList("ButtonsInfoBox");

        statsBoxLeft.Add(sightRange);

        if (showButtons)
        {
            var destroyButton = new Button(() =>
            {
                PickedBuilding.Field.TurnOffChosenMark();
                PickedBuilding.Destroy();
                RemoveInfoBox();
                ClearPicked();
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

    private (VisualElement, VisualElement) CreateDefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding, bool showButtons)
    {
        var (statsBox, buttonsBox) = CreateBuildingInfoBox(showButtons);
        var damageLabel = new Label(defensiveBuilding.CurrentDamage.ToString());
        var attackRangeLabel = new Label(defensiveBuilding.AttackRange.ToString());
        var damage = Combine(Icon.Sword, damageLabel);
        var attackRange = Combine(Icon.Target, attackRangeLabel);
        damage.AddToClassList("CostContainer");
        attackRange.AddToClassList("CostContainer");
        var hpBar = new HpBar(PickedBuilding.CurrentHealth, PickedBuilding.MaxHealth);

        statsBox.parent.Insert(1, hpBar);
        VisualElement statsBoxLeft = ((List<VisualElement>)statsBox.Children())[0];
        VisualElement statsBoxRight = ((List<VisualElement>)statsBox.Children())[1];

        statsBoxRight.Add(damage);
        statsBoxRight.Add(attackRange);

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
    /// <param name="showButtons"></param>
    public void DefensiveBuildingInfoBox(DefensiveBuilding defensiveBuilding, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException(nameof(PickedBuilding), "PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateDefensiveBuildingInfoBox(defensiveBuilding, showButtons);

        if (showButtons && PickedBuilding.CanRepair())
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
        if (PickedBuilding is null) throw new ArgumentNullException(nameof(PickedBuilding), "PickedBuilding has to be set");

        var (_, buttonsBox) = CreateDefensiveBuildingInfoBox(defensiveBuilding, showButtons);

        if (showButtons && PickedBuilding.CanRepair())
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
            var recruitButton = new Button(() => CreateRecruitmentBox(outpost, defensiveBuilding)) { text = "Recruit" };
            recruitButton.AddToClassList("InfoBoxButton");
            buttonsBox.Add(recruitButton);
        }
    }

    public void WatchtowerInfobox(bool showButtons)
    {
        CreateBuildingInfoBox(showButtons);
    }

    /// <summary>
    /// Creates units recruitment box
    /// </summary>
    /// <param name="outpost"></param>
    private void CreateRecruitmentBox(Outpost outpost, DefensiveBuilding defensiveBuilding)
    {
        var recruitmentBox = _lowerContainer.Q<VisualElement>("RecruitmentBox");
        if (recruitmentBox is not null)
            _lowerContainer.Remove(recruitmentBox);

        var unlockedUnits = outpost.Building.Player.UnlockedUnits;
        recruitmentBox = new VisualElement() { name = "RecruitmentBox" };

        foreach (var unitObject in unlockedUnits)
        {
            var unit = unitObject.GetComponent<Unit>();

            var name = new Label(unit.BaseUnitStats.UnitName);

            var buyBox = new VisualElement();
            var costs = new List<VisualElement>();

            buyBox.AddToClassList("InnerRecruitmentBox");

            var cost = unit.BaseUnitStats.BaseCost;

            if (cost.Bone > 0)
                costs.Add(Combine(Icon.Bone, new Label(cost.Bone.ToString())));

            if (cost.Doggium > 0)
                costs.Add(Combine(Icon.Doggium, new Label(cost.Doggium.ToString())));

            if (cost.Gold > 0)
                costs.Add(Combine(Icon.Gold, new Label(cost.Gold.ToString())));

            if (cost.Stone > 0)
                costs.Add(Combine(Icon.Stone, new Label(cost.Stone.ToString())));

            if (cost.Wood > 0)
                costs.Add(Combine(Icon.Wood, new Label(cost.Wood.ToString())));

            buyBox.Add(name);
            costs.ForEach(c =>
            {
                c.AddToClassList("CostContainer");
                buyBox.Add(c);
            });

            if (outpost.CanRecruit(unit.BaseUnitStats.UnitName))
            {
                var button = new Button(() =>
                {
                    outpost.BeginUnitRecruitment(unit.BaseUnitStats.UnitName);
                    OutpostInfoBox(defensiveBuilding, outpost, true);
                })
                {
                    text = "Buy"
                };
                button.AddToClassList("InfoBoxButton");
                button.AddToClassList("BuildButton");
                buyBox.Add(button);
            }
            else
            {
                var cantAffordLabel = new Label("Can't afford");
                cantAffordLabel.AddToClassList("BuildButton");
                buyBox.Add(cantAffordLabel);
            }

            recruitmentBox.Add(buyBox);
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
        if (PickedBuilding is null) throw new ArgumentNullException(nameof(PickedBuilding), "PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateBuildingInfoBox(showButtons);
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

        if (showButtons && PickedBuilding.CanRepair())
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
        if (PickedBuilding is null) throw new ArgumentNullException(nameof(PickedBuilding), "PickedBuilding has to be set");

        var (statsBox, buttonsBox) = CreateBuildingInfoBox(showButtons);

        if (showButtons && PickedBuilding.CanRepair())
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
    /// Creates an info box for the given Barricade
    /// </summary>
    /// <param name="barricade"></param>
    /// <param name="showButtons"></param>
    public void BarricadeInfoBox(Barricade barricade, bool showButtons)
    {
        if (PickedBuilding is null) throw new ArgumentNullException(nameof(PickedBuilding), "PickedBuilding has to be set");
        var (statsBox, buttonsBox) = CreateBuildingInfoBox(showButtons);
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
        var list = new VisualElement
        {
            name = "ObjectivesList"
        };

        var listOfLists = new List<List<IObjective>>()
        {
            ObjectiveHandler.Objectives,
            ObjectiveHandler.FailObjectives
        };

        foreach (var objectives in listOfLists)
        {
            foreach (var objective in objectives)
            {
                var box = new VisualElement();
                var primary = new Label(objective.IsPrimary ? "Primary" : "Side");
                var completed = new Label(objective.IsCompleted ? "Completed" : "Not completed");
                var info = new Label(objective.ObjectiveInfo);
                box.Add(primary);
                box.Add(info);
                box.Add(completed);
                box.AddToClassList("ObjectiveInfoBox");
                list.Add(box);
            }
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

    /// <summary>
    /// Logs message on the on screen logger for the player
    /// </summary>
    /// <param name="message"></param>
    public static void LogMessage(string message)
    {
        var label = new Label(message);
        label.AddToClassList("LogMessage");
        _loggerBody.Add(label);
    }

    private VisualElement Combine(VisualElement v1, VisualElement v2)
    {
        var v = new VisualElement();
        v.Add(v1);
        v.Add(v2);
        v.AddToClassList("FlexRow");
        return v;
    }

    private void HandleLoggerButtonClick()
    {
        if (_isLoggerBig)
        {
            _logger.style.height = new StyleLength(100);
            _loggerButton.text = "^";
            _isLoggerBig = false;
        }
        else
        {
            _logger.style.height = new StyleLength(500);
            _loggerButton.text = "v";
            _isLoggerBig = true;
        }
    }
}
