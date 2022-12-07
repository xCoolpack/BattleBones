using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Icon : MonoBehaviour
{
    public Image Sword => GetImage("Sword");
    public Image Gold => GetImage("Gold");
    public Image Wood => GetImage("Wood");
    public Image Stone => GetImage("Stone");
    public Image Shield => GetImage("Shield");
    public Image Doggium => GetImage("Doggium");
    public Image Bone => GetImage("Bone");
    public Image Target => GetImage("Target");
    public Image Eye => GetImage("Eye");
    public Image Boot => GetImage("Boot");
    public Image Hourglass => GetImage("hourglass");

    public List<Sprite> Sprites;

    private Image GetImage(string name)
    {
        return new Image
        {
            sprite = Sprites.Find(s => s.name == name)
        };
    }
}

