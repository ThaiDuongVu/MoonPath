using UnityEngine;

namespace UIElements.OffScreenIndicator
{
    public static class OffScreenIndicatorCore
    {
        // Gets the position of the target mapped to screen coordinates.
        public static Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
            return screenPosition;
        }


        // Gets if the target is within the view frustum.
        // screenPosition: Position of the target mapped to screen coordinates
        public static bool IsTargetVisible(Vector3 screenPosition)
        {
            bool isTargetVisible = screenPosition.z > 0 && screenPosition.x > 0 && screenPosition.x < Screen.width &&
                                   screenPosition.y > 0 && screenPosition.y < Screen.height;
            return isTargetVisible;
        }

        // Gets the screen position and angle for the arrow indicator. 
        // screenPosition: Position of the target mapped to screen coordinates
        // angle: Angle of the arrow
        // screenCentre: The screen centre
        // screenBounds: The screen bounds
        public static void GetArrowIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle,
            Vector3 screenCentre, Vector3 screenBounds)
        {
            // Our screenPosition's origin is screen's bottom-left corner.
            // But we have to get the arrow's screenPosition and rotation with respect to screenCentre.
            screenPosition -= screenCentre;

            // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
            // so just invert them.
            if (screenPosition.z < 0) screenPosition *= -1;

            // Angle between the x-axis (bottom of screen) and a vector starting at zero(bottom-left corner of screen) and terminating at screenPosition.
            angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
            // Slope of the line starting from zero and terminating at screenPosition.
            float slope = Mathf.Tan(angle);

            // Two point's line's form is (y2 - y1) = m (x2 - x1) + c, 
            // starting point (x1, y1) is screen bottom-left (0, 0),
            // ending point (x2, y2) is one of the screenBounds,
            // m is the slope
            // c is y intercept which will be 0, as line is passing through origin.
            // Final equation will be y = mx.
            screenPosition = screenPosition.x > 0 ? new Vector3(screenBounds.x, screenBounds.x * slope, 0) : new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);

            // In case the y ScreenPosition exceeds the y screenBounds 
            if (screenPosition.y > screenBounds.y)
                // Keep the y screen position to the maximum y bounds and
                // find the x screen position using x = y/m.
                screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
            else if (screenPosition.y < -screenBounds.y)
                screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
            // Bring the ScreenPosition back to its original reference.
            screenPosition += screenCentre;
        }
    }
}