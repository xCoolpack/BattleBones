using System.IO;
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

    private static Image GetImage(string name)
    {
        return new Image
        {
            sprite = LoadNewSprite($".\\Assets\\Sprites\\Icons\\{name}.png")
        };
    }

    private static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100.0f, SpriteMeshType spriteType = SpriteMeshType.Tight)
    {
        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference
        Texture2D SpriteTexture = LoadTexture(FilePath) ?? throw new System.Exception($"Can't load image {FilePath}.");
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), PixelsPerUnit, 0, spriteType);

        return NewSprite;
    }

    private static Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           
            if (Tex2D.LoadImage(FileData)) 
                return Tex2D;            
        }
        return null; // Return null if load failed
    }
}

