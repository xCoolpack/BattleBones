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
    private Label _playerLabel;


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
    }
}
