using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private Animator _animator;

    private bool _isSelected;

    private EventSystem _eventSystem;
    private PointerEventData _eventData;

    // public Tile leftTile;
    // public Tile rightTile;
    public Tile upTile;
    public Tile downTile;

    [SerializeField] private Transform icon;

    private RectTransform _rectTransform;

    // Return the height & current position of the tile
    public float Width => _rectTransform.sizeDelta.x;
    public float Height => _rectTransform.sizeDelta.y;

    public Vector2 Position => _rectTransform.anchoredPosition;

    private TileManager _tileManager;
    private static readonly int Selected = Animator.StringToHash("selected");

    // Awake is called when an object is initialized
    private void Awake()
    {
        // Get references to components
        _button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _rectTransform = GetComponent<RectTransform>();

        _eventSystem = EventSystem.current;
        _eventData = new PointerEventData(_eventSystem);

        _tileManager = transform.parent.GetComponent<TileManager>();
    }

    // When mouse hover over tile, select it
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _tileManager.Select(this);
    }

    // Keep selected tile even after mouse exit
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _tileManager.Select(this);
    }

    #region Select Methods

    // When a tile is selected
    public void OnSelected()
    {
        if (_isSelected) return;

        _button.OnPointerEnter(_eventData);
        icon.gameObject.SetActive(true);

        _animator.SetTrigger(Selected);

        _isSelected = true;
    }

    // When a tile is not selected
    public void OnDeselected()
    {
        _button.OnPointerExit(_eventData);
        icon.gameObject.SetActive(false);

        _isSelected = false;
    }

    #endregion

    // When the tile is clicked
    public void OnClicked()
    {
        _button.OnPointerClick(_eventData);
    }
}