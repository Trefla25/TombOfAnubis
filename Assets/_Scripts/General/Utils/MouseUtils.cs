using UnityEngine;
  using UnityEngine.InputSystem;

  public static class MouseUtils
  {
      public static Vector3 GetMousePositionInWorldSpace(float zValue = 0f)
      {
          var cam = Camera.main;
          if (cam == null) return Vector3.zero;

          Plane dragPlane = new(cam.transform.forward, new Vector3(0, 0, zValue));
          Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
          Ray ray = cam.ScreenPointToRay(mouseScreenPos);
          if (dragPlane.Raycast(ray, out float distance))
          {
              return ray.GetPoint(distance);
          }
          return Vector3.zero;
      }
  }