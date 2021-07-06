using Gameplay;
using System;
using UnityEngine;

public class TilesController : MonoBehaviour
{
    private const int TilesNumber = 6;

    [SerializeField] Tile[] _tiles = new Tile[TilesNumber];

    Tile _currentLeftTile;
    Tile _currentRightTile;
    Tile _currentTargetTile;

    public void Awake()
    {
        foreach (Tile tile in _tiles)
        {
            tile.RegisterTilesController(this);
        }
        AppManager.TutorialManager.RegisterTilesController(this);
    }

    private void GetAdjecentTiles(Tile targetTile, out Tile _currentLeftTile, out Tile _currentRightTile)
    {
        _currentTargetTile = targetTile;
        _currentLeftTile = Array.IndexOf(_tiles, targetTile) == 0 ? null : _tiles[Array.IndexOf(_tiles, targetTile) - 1];
        _currentRightTile = Array.IndexOf(_tiles, targetTile) == TilesNumber - 1 ? null : _tiles[Array.IndexOf(_tiles, targetTile) + 1];
    }

    public void ResetHighlights(bool forcedReset = false)
    {
        if (!AppManager.TutorialManager.TileHighlightIsBlocked || forcedReset)
        {
            _currentLeftTile?.ResetTutorialHighlight();
            _currentRightTile?.ResetTutorialHighlight();
            _currentTargetTile?.ResetTutorialHighlight();
        }
    }

    public void SetupAdjacentTilesHighlight(Tile targetTile)
    {
        if (!AppManager.TutorialManager.TileHighlightIsBlocked)
        {
            ResetHighlights();
            GetAdjecentTiles(targetTile, out _currentLeftTile, out _currentRightTile);
            _currentLeftTile?.HighlightAsAdjacent();
            _currentRightTile?.HighlightAsAdjacent();
            targetTile?.HighlightAsTarget();
        }
    }

    public void OnDisable()
    {
        ResetHighlights(true);
    }
}