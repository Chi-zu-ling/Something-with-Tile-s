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

    [SerializeField] Sprite Void;
    [SerializeField] Sprite Ocean;
    [SerializeField] Sprite Grassland;
    [SerializeField] Sprite Mountain;
    [SerializeField] Sprite Beach;

    [SerializeField] Sprite OreVein;
    [SerializeField] Sprite Meadow;
    [SerializeField] Sprite River;
    [SerializeField] Sprite Forest;
    [SerializeField] Sprite Marshland;

    [SerializeField] Sprite Village;
    [SerializeField] Sprite LumberHouse;
    [SerializeField] Sprite HunterHut;
    [SerializeField] Sprite Farm;
    [SerializeField] Sprite Forge;
    [SerializeField] Sprite Mine;
    [SerializeField] Sprite FisherHut;
    [SerializeField] Sprite Trader;

    public int pixelWidth = 1;
    public int pixelHeight = 1;
    float width;
    float height;

    public bool hover = false;

    public int point;
    public int prevPoints = 0;

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



    public void updateTile(int stage) {

        //Beach-check
        if (this.type == Tile.Type.Beach) {
            bool watercheck = false;

            for (int i = 0;i < this.AdjacentTiles.Count;i++) {
                if (this.AdjacentTiles[i].type == Tile.Type.Ocean) {
                    watercheck = true;
                }
            }

            if (watercheck == false) {
                this.type = Tile.Type.Grassland;
            }
        }

        if (this.type == Tile.Type.Grassland) {
            for (int i = 0;i < this.AdjacentTiles.Count;i++) {
                if (this.AdjacentTiles[i].type == Tile.Type.Ocean) {
                    this.type = Tile.Type.Beach;
                }
            }
        }

        this.name = this.position + ", " +this.type;

        //check if AdjacentTiles should change this tile, if yes set Type


        //Setting Sprite
        //To add Sprite, create new [SerializeField] Sprite [name];
        //in global and add the sprite to it via the Unity Editor
        //then add new Tile type in: enum Type{}
        //Lastly, create new case below.
        SetSprites();

        pointRules(this,stage);

    }




    public void SetSprites() {
        switch (type) {

            case Type.Void:
                this.GetComponent<SpriteRenderer>().sprite = Void;
                break;

            case Type.Ocean:
                this.GetComponent<SpriteRenderer>().sprite = Ocean;
                break;

            case Type.Grassland:
                this.GetComponent<SpriteRenderer>().sprite = Grassland;
                break;

            case Type.Mountain:
                this.GetComponent<SpriteRenderer>().sprite = Mountain;
                break;

            case Type.Beach:
                this.GetComponent<SpriteRenderer>().sprite = Beach;
                break;



            case Type.Meadow:
                this.GetComponent<SpriteRenderer>().sprite = Meadow;
                break;

            case Type.Forest:
                this.GetComponent<SpriteRenderer>().sprite = Forest;
                break;

            case Type.OreVein:
                this.GetComponent<SpriteRenderer>().sprite = OreVein;
                break;

            case Type.River:
                this.GetComponent<SpriteRenderer>().sprite = River;
                break;

            /*case Type.MarshLand:
                this.GetComponent<SpriteRenderer>().sprite = ;
                break;*/


            default: {
                    Debug.Log("");
                    break;
                }
        }
    }

    public bool placementRules(Tile targetTile,Tile nextTile) {
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


            //Tier 2
            case Type.Meadow:
                if (targetTile.type == Type.Grassland) {
                    return true;
                } else return false;

            case Type.Forest:
                if (targetTile.type == Type.Grassland) {
                    return true;
                } else return false;

            case Type.OreVein:
                if (targetTile.type == Type.Mountain) {
                    return true;
                } else return false;

        }

        return true;
    }

    //Callculation of Points for this tile
    public void pointRules(Tile tile,int stage) {

        #region Tier-1
        if (stage ==1) {
            switch (type) {

                case Type.Void:
                    break;

                case Type.Ocean:
                    point = 0;
                    for (int i = 0;i < tile.AdjacentTiles.Count;i++) {
                        if (tile.AdjacentTiles[i].type == Tile.Type.Ocean) {
                            point += 1;
                        } else if (tile.AdjacentTiles[i].type == Tile.Type.Beach) {
                            point += 3;
                        }
                    }
                    break;

                case Type.Grassland:
                    point = 1;
                    break;

                case Type.Mountain:
                    point = 0;
                    int M = 0;

                    for (int i = 0;i < tile.AdjacentTiles.Count;i++) {
                        if (tile.AdjacentTiles[i].type == Tile.Type.Mountain) {
                            M++;
                        }
                    }

                    point = M;

                    break;

                case Type.Beach:
                    point = 5;
                    break;
            }
        }
        #endregion

        #region Tier-2
        //Tier 2
        else if (stage == 2) {
            switch (type) {
                case Type.OreVein:

                    point = (prevPoints*5 +3);

                    //5 times the points of the mointain tile placed on - 4 for each mountain adjacent
                    for (int i = 0;i < tile.AdjacentTiles.Count;i++) {
                        if (tile.AdjacentTiles[i].type == Tile.Type.OreVein) {
                            point -= 4;
                        }
                    }

                    break;

                case Type.Forest:
                    point = 2;

                    for (int i = 0;i < tile.AdjacentTiles.Count;i++) {
                        if (tile.AdjacentTiles[i].type == Tile.Type.Mountain ||
                            tile.AdjacentTiles[i].type == Tile.Type.OreVein) {
                            point += 3;
                        } else if (tile.AdjacentTiles[i].type == Tile.Type.Forest) {
                            point +=2;
                        }
                    }

                    break;
            }
        }
    }
    #endregion

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
        /*if (this.transform.parent.gameObject.name == "Grid") {
            float x = this.transform.position.x;
            float y = this.transform.position.y;
            float z = this.transform.position.z;

            this.transform.position = new Vector3(x,y += 0.2f,z);
        }*/
        manager.hoverEnter(this);
    }

    private void OnMouseExit() {
        /*if (this.transform.parent.gameObject.name == "Grid") {
            manager.hoverInfoPanel.HoverPanel.SetActive(false);
            float x = this.transform.position.x;
            float y = this.transform.position.y;
            float z = this.transform.position.z;

            this.transform.position = new Vector3(x,y -= 0.2f, z);
        }*/
        manager.hoverExit(this);
    }

    #endregion
}
