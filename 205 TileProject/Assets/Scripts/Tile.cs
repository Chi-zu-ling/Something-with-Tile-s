using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Tile:MonoBehaviour {
    Grid grid;
    NextTile nextTile;
    Manager manager;



    public int pixelWidth = 1;
    public int pixelHeight = 1;
    float width;
    float height;

    public bool hover = false;

    public int point;
    public int prevPoints = 0;

    public int tier = 0;
    public int variation;

    public bool flipped = false;

    public Vector2Int position;
    public List<Tile> AdjacentTiles;

    public List<Type> prevType;

    public enum Type {
        Void,
        Ocean,
        Grassland,
        Mountain,

        Beach,

        OreVein,
        Forest,
        Meadow,
        River,

        MarshLand,

        Village,
        LumberHouse,
        HunterHut,
        Farm,
        Forge,
        Mine,
        FisherHut,
        Trader
    }

    public Type type;

    // Start is called before the first frame update
    void Start() {
        grid = FindObjectOfType<Grid>();
        nextTile = FindObjectOfType<NextTile>();
        manager = FindObjectOfType<Manager>();

        float width = pixelWidth;
        float height = pixelHeight;
    }

    #region MouseFunctions

    private void OnMouseOver() {
        manager.mouseHover(this);
    }

    private void OnMouseDown() {
        manager.MouseTarget();
    }

    private void OnMouseDrag() {
        manager.mouseDrag();
    }

    private void OnMouseUp() {
        manager.targetDrop();
    }

    private void OnMouseEnter() {
        manager.hoverEnter(this);
    }

    private void OnMouseExit() {
        manager.hoverExit(this);
    }

    #endregion
}
