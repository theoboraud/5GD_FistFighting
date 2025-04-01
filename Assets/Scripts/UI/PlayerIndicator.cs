using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    private Player _player;    
    public Vector3 offset = new Vector3(0, 1.0f, 0); // offset of this indicator to player
    private RectTransform rectTransform;

    private Image _image; 

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        _player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (_player == null) return;

        Vector3 worldPosition = _player.transform.position + offset;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        rectTransform.position = screenPosition;
    }
    public void SetColor(Color color)
    {
        _image.color = color;
    }
}