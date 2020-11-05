using UIElements.OffScreenIndicator;
using System;
using System.Collections.Generic;
using UnityEngine;

// Attach the script to the off screen indicator panel.
[DefaultExecutionOrder(-1)]
public class OffScreenIndicator : MonoBehaviour
{
    [Range(0.5f, 1f)] [Tooltip("Distance offset of the indicators from the centre of the screen")] [SerializeField]
    private float screenBoundOffset = 1f;

    private Camera _mainCamera;
    private Vector3 _screenCentre;
    private Vector3 _screenBounds;

    private List<IndicatorTarget> targets = new List<IndicatorTarget>();

    public static Action<IndicatorTarget, bool> TargetStateChanged;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        _screenBounds = _screenCentre * screenBoundOffset;
        TargetStateChanged += HandleTargetStateChanged;
    }

    private void LateUpdate()
    {
        DrawIndicators();
    }

    // Draw the indicators on the screen and set their position and rotation and other properties.
    private void DrawIndicators()
    {
        foreach (IndicatorTarget target in targets)
        {
            Vector3 screenPosition = OffScreenIndicatorCore.GetScreenPosition(_mainCamera, target.transform.position);
            bool isTargetVisible = OffScreenIndicatorCore.IsTargetVisible(screenPosition);
            float distanceFromCamera = target.NeedDistanceText
                ? target.GetDistanceFromCamera(_mainCamera.transform.position)
                : float.MinValue; // Gets the target distance from the camera.
            Indicator indicator = null;

            if (target.NeedBoxIndicator && isTargetVisible)
            {
                screenPosition.z = 0;
                indicator = GetIndicator(ref target.indicator,
                    IndicatorType.Box); // Gets the box indicator from the pool.
            }
            else if (target.NeedArrowIndicator && !isTargetVisible)
            {
                float angle = float.MinValue;
                OffScreenIndicatorCore.GetArrowIndicatorPositionAndAngle(ref screenPosition, ref angle, _screenCentre,
                    _screenBounds);
                indicator = GetIndicator(ref target.indicator,
                    IndicatorType.Arrow); // Gets the arrow indicator from the pool.
                indicator.transform.rotation =
                    Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg); // Sets the rotation for the arrow indicator.
            }
            else
            {
                target.indicator?.Activate(false);
                target.indicator = null;
            }

            if (indicator)
            {
                if (!(indicator is null))
                {
                    indicator.SetImageColor(target.TargetColor); // Sets the image color of the indicator.
                    indicator.SetDistanceText(distanceFromCamera); //Set the distance text for the indicator.
                    indicator.transform.position = screenPosition; //Sets the position of the indicator on the screen.
                    indicator.SetTextRotation(Quaternion
                        .identity); // Sets the rotation of the distance text of the indicator.
                }
            }
        }
    }

    // Add the target to targets list if active is true.
    // If active is false deactivate the targets indicator, 
    // set its reference null and remove it from the targets list.
    private void HandleTargetStateChanged(IndicatorTarget target, bool active)
    {
        if (active)
        {
            targets.Add(target);
        }
        else
        {
            target.indicator?.Activate(false);
            target.indicator = null;
            targets.Remove(target);
        }
    }

    // Get the indicator for the target.
    // If it's not null and of the same required type then return the same indicator
    // If it's not null but is of different type from type then deactivate the old reference so that it returns to the pool and request one of another type from pool.
    // If it's null then request one from the pool of type
    private Indicator GetIndicator(ref Indicator indicator, IndicatorType type)
    {
        if (indicator != null)
        {
            if (indicator.Type != type)
            {
                indicator.Activate(false);
                indicator = type == IndicatorType.Box
                    ? BoxObjectPool.Current.GetPooledObject()
                    : ArrowObjectPool.Current.GetPooledObject();
                indicator.Activate(true); // Sets the indicator as active.
            }
        }
        else
        {
            indicator = type == IndicatorType.Box
                ? BoxObjectPool.Current.GetPooledObject()
                : ArrowObjectPool.Current.GetPooledObject();
            indicator.Activate(true); // Sets the indicator as active.
        }

        return indicator;
    }

    private void OnDestroy()
    {
        TargetStateChanged -= HandleTargetStateChanged;
    }
}