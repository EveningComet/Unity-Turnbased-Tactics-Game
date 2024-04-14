using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.ReadWrite;
using UnityEngine.Tilemaps;
using TurnbasedGame.Tiles;
using TurnbasedGame.StateMachine;
using TurnbasedGame.Turns;
using TurnbasedGame.VictoryControl;
using TurnbasedGame.Levels;

/// <summary>
/// Controls the battle map.
/// </summary>
public class BattleMapController : StateMachine<BattleState>
{
    #region Fields
    [Header("Prefabs & References")]
    /// <summary>
    /// Stores the game map. It will be used as a convenient way to tell the data map what a tile is.
    /// </summary>
    [SerializeField] private Tilemap unityTileMap = null;
    public Tilemap UnityTileMap
    {
        get { return unityTileMap; }
    }

    [SerializeField] private Camera theCamera = null;
    public Camera TheCamera { get { return theCamera; } }

    /// <summary>
    /// The color to highlight tiles when a character is looking for a target for an ability.
    /// </summary>
    [SerializeField] private Color tileAbilityHighlightColor = Color.blue;
    [SerializeField] private Color tileMovementHighlightColor = Color.black;

    [SerializeField] private SelectionController selectionController = null;
    public SelectionController SelectionController { get { return selectionController; } }

    [SerializeField] private GameObject testPlayerUnitPrefab = null;
    public GameObject TestPlayerUnitPrefab { get { return testPlayerUnitPrefab; } }
    [SerializeField] private GameObject testEnemyUnitPrefab = null;
    public GameObject TestEnemyUnitPrefab { get { return testEnemyUnitPrefab; } }

    [SerializeField] private TurnController turnController = null;
    public TurnController TurnController { get { return turnController; } }

    [SerializeField] private GameObject tooltipCanvasPrefab = null;
    public GameObject TooltipCanvasPrefab { get { return tooltipCanvasPrefab; } }

    [Header("JSON Files")]
    [SerializeField] private TextAsset aiActionSetJsonFile = null;
    public TextAsset AIActionSetJsonFile { get { return aiActionSetJsonFile; } }
    [SerializeField] private TextAsset levelJsonFile = null;
    
    private LevelData currentLevel;
    public LevelData CurrentLevel { get { return currentLevel; } }

    public VictoryController VictoryController { get; private set; }

    private GameTileMapData gameTileMapData;
    #endregion

    private void OnEnable()
    {
        TurnbasedGame.Combat.CombatController.SetBMC(this);
    }

    private void OnDisable()
    {
        TurnbasedGame.Combat.CombatController.RemoveBMC();
    }

    // Start is called before the first frame update
    void Start()
    {
        VictoryController = new VictoryController();
        currentLevel = ReadLevel(levelJsonFile);

        // Setup the map
        ChangeToState(new InitializeBattle(this));
    }

    /// <summary>
    /// Refreshes the bounds of the Unity Tilemap. This is needed because Unity's Tilemap does not
    /// automatically refresh its bounds when tiles are deleted.
    /// </summary>
    public void CompressUnityTileMapBounds()
    {
        unityTileMap.CompressBounds();
    }

    public void SetGameTileMapData(GameTileMapData newGTMD)
    {
        gameTileMapData = newGTMD;
    }

    public GameTileMapData GetTileMapData()
    {
        return gameTileMapData;
    }

    /// <summary>
    /// End the battle.
    /// </summary>
    public void EndBattle()
    {
        ChangeToState( new EndBattleState(this) );
    }

    #region Unity Tilemap Related
    /// <summary>
    /// Given the passed world space (as a Unity Vector3Int), find the specified tile.
    /// </summary>
    public GameTile GetGameTileAtWorldPosition(Vector3Int coordinate)
    {
        return gameTileMapData.GetTile(coordinate.x, coordinate.y);
    }

    /// <summary>
    /// Given the passed coordinates, have Unity's Tilemap get the world space center position of a tile. This is useful for stuff like
    /// making a unit go to the center of a tile.
    /// </summary>
    public Vector3 GetTileWorldSpaceCenter(int x, int y, int z = 0)
    {
        var positionToReturn = unityTileMap.GetCellCenterWorld(new Vector3Int(x, y, z));
        return positionToReturn;
    }

    /// <summary>
    /// Highlight the passed tiles on the Unity Tilemap.
    /// </summary>
    /// <param name="tilesToHighlight">Tiles to be highlighted.</param>
    /// <param name="movementRelated">Is this for movement? If true, use the highlight color related to movement.</param>
    public void HighlightTiles(List<GameTile> tilesToHighlight, bool movementRelated)
    {
        if (movementRelated == true)
            HighlightTiles(tilesToHighlight, tileMovementHighlightColor);
        else
            HighlightTiles(tilesToHighlight, tileAbilityHighlightColor);
    }

    /// <summary>
    /// Highlight the passed tiles on the Unity Tilemap, based on the passed color.
    /// </summary>
    /// <param name="tilesToHighlight">Tiles to be highlighted or unhiglighted.</param>
    /// <param name="highlightColor">Desired highlight color.</param>
    public void HighlightTiles(List<GameTile> tilesToHighlight, Color highlightColor)
    {
        int numTiles = tilesToHighlight.Count;
        for (int i = 0; i < numTiles; i++)
        {
            GameTile tileToHighlight = tilesToHighlight[i];
            Vector3Int highlightPos = new Vector3Int(tileToHighlight.Q, tileToHighlight.R, 0);
            unityTileMap.SetColor(highlightPos, highlightColor);
        }
    }

    /// <summary>
    /// Unhighlight the passed tiles on the Unity Tilemap.
    /// </summary>
    /// <param name="tilesToUnhighlight">The tiles to unhighlight.</param>
    public void UnhighlightTiles(List<GameTile> tilesToUnhighlight)
    {
        HighlightTiles(tilesToUnhighlight, Color.white);
    }
    #endregion

    #region Reading/Writing External Files
    public TurnbasedGame.AI.AIActionSet ReadAIActionSet(TextAsset jsonFileWithAIActionSet)
    {
        return JsonReadWriter.ReadAIActionSetJsonFile(jsonFileWithAIActionSet);
    }

    public LevelData ReadLevel(TextAsset jsonFileWithLevel)
    {
        return JsonReadWriter.ReadLevel(jsonFileWithLevel);
    }
    #endregion

    #region BattleMapController State Machine Methods
    public override void ChangeToState(State newState)
    {
        base.ChangeToState(newState);

        currentState.DoWork();
    }
    #endregion
}
