using UnityEngine;
using UnityEngine.UIElements;

public class HpBar : VisualElement
{
    public HpBar(int currentHp, int maxHp)
    {
        AddToClassList("HpBar");
        var green = new VisualElement();
        var red = new VisualElement();
        var label = new Label($"{currentHp} / {maxHp}");
        label.ClearClassList();
        label.AddToClassList("HpLabel");
        green.AddToClassList("HpBarGreen");
        red.AddToClassList("HpBarRed");

        var hp = (float)currentHp / (float)maxHp;
        green.style.width = new StyleLength(Length.Percent(hp*100));
        red.style.width = new StyleLength(Length.Percent((1-hp)*100));

        Add(green);
        Add(red);
        Add(label);
    }
}
