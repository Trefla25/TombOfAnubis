using UnityEngine;
using UnityEngine.InputSystem;

public static class MouseUtils 
{
    public static Camera camera = Camera.main;
    public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
    {
        Plane dragPlane = new(camera.transform.forward, new Vector3(0, 0, zValue));
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = camera.ScreenPointToRay(mouseScreenPos);
        if (dragPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}
