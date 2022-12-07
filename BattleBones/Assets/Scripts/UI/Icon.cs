using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class Icon
{
    public static Image Sword => GetImage("Sword");
    public static Image Gold => GetImage("Gold");
    public static Image Wood => GetImage("Wood");
    public static Image Stone => GetImage("Stone");
    public static Image Shield => GetImage("Shield");
    public static Image Doggium => GetImage("Doggium");
    public static Image Bone => GetImage("Bone");
    public static Image Target => GetImage("Target");
    public static Image Eye => GetImage("Eye");
    public static Image Boot => GetImage("Boot");
    public static Image Hourglass => GetImage("hourglass");

    private static Image GetImage(string name)
    {
        return new Image
        {
            sprite = LoadNewSprite($".\\Assets\\Sprites\\Icons\\{name}.png")
        };
    }

    private static Sprite LoadNewSprite(string filePath, float pixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D spriteTexture = LoadTexture(filePath) ?? throw new System.Exception($"Can't load image {filePath}.");
        Sprite newSprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit, 0, spriteType);

        return newSprite;
    }

    private static Texture2D LoadTexture(string filePath)
    {
        Texture2D tex2D;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex2D = new Texture2D(2, 2);           
            if (tex2D.LoadImage(fileData)) 
                return tex2D;            
        }
        return null; // Return null if load failed
    }
}

