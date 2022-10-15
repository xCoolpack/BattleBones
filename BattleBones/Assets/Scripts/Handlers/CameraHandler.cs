using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CameraHandler : MonoBehaviour
{
    public float CameraSpeed; 
    public float ScrollSpeed;
    public float BorderThickness;

    public DoubleTuple XLimit;
    public DoubleTuple YLimit;
    public DoubleTuple ZLimit;
    
    private Controls _controls;
    private InputAction _cameraMovement;
    private InputAction _mousePosition;
    private InputAction _scroll;

    private void Awake()
    {
        _controls = new Controls();
        _cameraMovement = _controls.CameraControls.CameraMovement;
        _mousePosition = _controls.CameraControls.MousePosition;
        _scroll = _controls.CameraControls.Scroll;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }


    private void Update()
    {
        Vector2 value = _cameraMovement.ReadValue<Vector2>();
        Vector3 pos = transform.position;

        // Movement for wasd
        value *= CameraSpeed * Time.deltaTime;

        // Movement for mouse
        //Vector2 mousePos = _mousePosition.ReadValue<Vector2>();
        //// Up
        //if (mousePos.y >= Screen.height - BorderThickness)
        //    value.y += CameraSpeed * Time.deltaTime;
        //// Down
        //if (mousePos.y <= BorderThickness)
        //    value.y -= CameraSpeed * Time.deltaTime;
        //// Left
        //if (mousePos.x <= BorderThickness)
        //    value.x -= CameraSpeed * Time.deltaTime;
        //// Right
        //if (mousePos.x >= Screen.width - BorderThickness)
        //    value.x += CameraSpeed * Time.deltaTime;

        // Scroll
        float scroll = _scroll.ReadValue<float>();
        scroll *= ScrollSpeed * Time.deltaTime;
        pos = new Vector3(
            Math.Clamp(pos.x + value.x, XLimit.Min, XLimit.Max),
            Math.Clamp(pos.y + value.y, YLimit.Min, YLimit.Max), 
            Math.Clamp(pos.z + scroll, ZLimit.Min, ZLimit.Max));
        transform.position = pos;
    }
}
[System.Serializable]
public struct DoubleTuple
{
    public float Min;
    public float Max;

    public DoubleTuple(float min, float max)
    {
        Min = min;
        Max = max;
    }
}
