using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour
{
    Grid grid;
    NextTile nextTile;
    Manager manager;

    [SerializeField] Sprite Void;
    [SerializeField] Sprite Ocean;
    [SerializeField] Sprite Grassland;
    [SerializeField] Sprite Mountain;

    public int pixelWidth = 1;
    public int pixelHeight = 1;
    float width;
    float height;

    int point;


    //public string Type;
    public Vector2Int position;
    public List<Tile> AdjacentTiles;

    public List<Type> prevType;

    public enum Type{
        Void,
        Ocean,
        Grassland,
        Mountain
    }

    public Type type;

    // Start is called before the first frame update
    void Start(){
        grid = FindObjectOfType<Grid>();
        nextTile = FindObjectOfType<NextTile>();
        manager = FindObjectOfType<Manager>();

        float width = pixelWidth;
        float height = pixelHeight;
    }

    public void updateTile() {
        this.name = this.position + ", " +this.type;

        //check if AdjacentTiles should change this tile, if yes set Type


        //Setting Sprite
        //To add Sprite, create new [SerializeField] Sprite [name];
        //in global and add the sprite to it via the Unity Editor
        //then add new Tile type in: enum Type{}
        //Lastly, create new case below.
        switch (type){

            case Type.Void:
                this.GetComponent<SpriteRenderer>().sprite = Void;
                break;

            case Type.Ocean:

                break;
            case Type.Grassland:

                break;
            case Type.Mountain:
                this.GetComponent<SpriteRenderer>().sprite = Mountain;
                break;

            

            default: {
                    Debug.Log("");
            break;
            }
        }

        pointRules();

    }

    public bool placementRules(Tile targetTile, Tile nextTile) {
        //only gets called by manager.targetDrop()
        //get placementRules for nextTile, and check if TargetTile fullfills requirements, if yes; return true; 

        switch (nextTile.type) {
            case Type.Void:

                return true;
                

            case Type.Ocean:

                return true;
                

            case Type.Grassland:

                return true;
                

            case Type.Mountain:

                return true;
                

        }

        return true;
    }

    //Callculation of Points for this tile
    public void pointRules() {
        switch (type) {
            case Type.Void:
                break;

            case Type.Ocean:
                break;

            case Type.Grassland:
                break;

            case Type.Mountain:

                for(int i=0;i < AdjacentTiles.Count;i++) {
                    if(AdjacentTiles[i].type == Tile.Type.Mountain) {
                        manager.points += 2;
                    }
                }

                break;
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
    #endregion
}
