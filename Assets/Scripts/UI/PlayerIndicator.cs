using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 1.0f, 0); // offset of this indicator to player
    private Player _player;
    private SpriteRenderer _spriteRenderer;

    private bool bIsReady = false;

    [SerializeField] private Sprite _indicator;
    [SerializeField] private Sprite _readyIcon;

    public void Init(Player _myPlayer, Color color)
    {
        _player = _myPlayer;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = color;

        _player.EventCenter.Subscribe(PlayerEvent.OnPlayerReady, OnPlayerReady);
    }
    private void OnDestroy()
    {
        _player.EventCenter.Unsubscribe(PlayerEvent.OnPlayerReady, OnPlayerReady);
    }

    void Update()
    {
        if (_player == null) return;

        transform.position = _player.transform.position + Vector3.up * offset.y;
    }
    private void OnPlayerReady()
    {
        Debug.Log("Indicator On player Ready");
        bIsReady = !bIsReady;
        _spriteRenderer.sprite = bIsReady? _indicator : _readyIcon;
        //We have to cancle ready once game round start
    }
}