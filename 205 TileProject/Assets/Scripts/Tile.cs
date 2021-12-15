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

    [SerializeField] private TMP_Text tilePointsText;

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

    public bool triggered = false;

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
        Lumber,
        Hunter,
        Farm,
        Forge,
        Mine,
        Fisher,
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

    public IEnumerator Triggered() {
        if (triggered == false) {

            triggered = true;

            float x = this.transform.position.x;
            float y = this.position.y;
            float z = this.transform.position.z;

            if(this.type != Tile.Type.Void){
                this.transform.position = new Vector3(x,y += 0.15f,z);}


            yield return new WaitForSeconds(0.1f);


            StartCoroutine(manager.ShowTilePoints(this, this.tilePointsText));
           

            for (int i = 0;i < this.AdjacentTiles.Count;i++) {
                StartCoroutine(this.AdjacentTiles[i].Triggered());}


            yield return new WaitForSeconds(0.3f);

            triggered = false;

            if (this.type != Tile.Type.Void) {
                this.transform.position = new Vector3(x,y -= 0.15f,z);}


            if(this == grid.grid[grid.grid.Count-1]) {
                yield return new WaitForSeconds(0.1f);
                //manager.pointDisplay.text = points.ToString();
                manager.NextStage();
            }
        }
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
