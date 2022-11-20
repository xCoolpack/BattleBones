using Unity.VisualScripting;
using UnityEngine;

public static class Logger
{
    private static bool _saveToFile = false;
    private static string _filePath;

    public static void Log(string msg)
    {
        if (_saveToFile)
        {

        }
        else
        {
            Overlay.LogMessage(msg);
        }
    }
}
