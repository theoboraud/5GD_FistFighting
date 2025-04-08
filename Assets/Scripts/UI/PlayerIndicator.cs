using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1.0f, 0); // offset of this indicator to player
    private RectTransform rectTransform;
    private Player _player;

    private Image _image; 

    public void Init(Player _myPlayer)
    {
        rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _player = _myPlayer;
    }

    void LateUpdate()
    {
        if (_player == null) return;

        Vector3 targetPosition = Camera.main.WorldToScreenPoint(_player.transform.position + offset);

        rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, Time.deltaTime*30f);
    }
    public void SetColor(Color color)
    {
        _image.color = color;
    }
}