using UnityEngine;

public class TileSelector : MonoBehaviour
{
    private RectTransform _rectTransform;

    private float Width => _rectTransform.sizeDelta.x;

    private const float InterpolationRatio = 0.15f;
    private bool _isLerping;
    private Vector2 _lerpPosition;

    // Target to rotate to
    private Transform _lookTarget;

    public Animator Animator => GetComponent<Animator>();

    // Awake is called when an object is initialized
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update()
    {
        // If current position is close enough to lerp position
        // Then stop lerping to save on memory
        if (GlobalController.CloseTo(_rectTransform.position.x, _lerpPosition.x, 0.01f) &&
            GlobalController.CloseTo(_rectTransform.position.y, _lerpPosition.y, 0.01f)) _isLerping = false;

        // If is lerping then lerp to position
        if (_isLerping)
            _rectTransform.anchoredPosition =
                Vector2.Lerp(_rectTransform.anchoredPosition, _lerpPosition, InterpolationRatio);

        Transform transform1 = transform;
        transform.right = Vector2.Lerp(transform1.right, _lookTarget.transform.position - transform1.position,
            InterpolationRatio);
    }

    // When a tile is selected
    public void Select(Tile tile)
    {
        // Set lerp position and start lerping
        _lerpPosition = new Vector2(tile.Position.x - tile.Width / 2f - Width / 2f - 15f, tile.Position.y);
        _isLerping = true;

        _lookTarget = tile.transform;
    }
}