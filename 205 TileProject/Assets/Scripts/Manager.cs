using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class Manager : MonoBehaviour
{

    public Grid grid;
    public NextTile nextTile;
    public PreShowTile preShowTile;
    public Tile tile;
    public HoverInfoPanel hoverInfoPanel;
    public TileLibrary tileLibrary;

    public int points = 0;
    int round = 0;
    public int stage = 1;

    public GameObject hoverTargetParent;

    [SerializeField] TextMeshProUGUI pointDisplay;

    // Start is called before the first frame update
    void Start()
    {
        grid.startUp();

        for (int i = 0;i < grid.grid.Count;i++) {
            tileLibrary.updateTile(grid.grid[i],stage);
        }

        preShowTile.startUp();
    }



    public void Round() {

        nextTile.clearCluster();

        grid.clearCount();

        for(int i=0; i < grid.grid.Count;i++) {
            tileLibrary.updateTile(grid.grid[i],stage);

            switch (grid.grid[i].type) {

                //tier 1
                case Tile.Type.Void:
                    grid.Void += 1;
                    break;

                case Tile.Type.Mountain:
                    grid.mountains += 1;
                    break;

                case Tile.Type.Ocean:
                    grid.oceans += 1;
                    break;

                case Tile.Type.Grassland:
                    grid.grasslands += 1;
                    break;

                case Tile.Type.Beach:
                    grid.beaches += 1;
                    break;

                //tier 2
                case Tile.Type.Forest:
                    grid.forests += 1;
                    break;

                case Tile.Type.Meadow:
                    grid.meadows += 1;
                    break;

                case Tile.Type.OreVein:
                    grid.oreveins += 1;
                    break;

                case Tile.Type.River:
                    grid.rivers += 1;
                    break;
            }
        }
        //grid.Count();

        grid.adjacentTiles();

        preShowTile.nextTile();
        round++;
    }



    public void NextStage() {
        stage++;

        if (stage == 2) {
            Debug.Log("Start Stage 2");


            if(grid.Void == 0) {
                points += 100;
            }

            //Mountain Related
            for(int i = 0; i < (grid.mountains / 7);i++) {
                preShowTile.nextTileTypeList.Add(Tile.Type.OreVein);
            }

            //GrassLand Related
            for (int i = 0;i < (grid.grasslands / 5);i++) {

                int r = Random.Range(0,3);

                if(r == 1) { preShowTile.nextTileTypeList.Add(Tile.Type.Meadow);}
                else preShowTile.nextTileTypeList.Add(Tile.Type.Forest);
            }

            //Ocean Related
            if(grid.oceans > 0) {
                preShowTile.nextTileTypeList.Add(Tile.Type.River);
            }

            Round();
        } 
        
        else if (stage == 3) {

        } 
        
        else Debug.Log("End of Game");

    }



    public void PointTally() {

        for(int i = 0;i < grid.grid.Count; i++) {

            //tile.pointRules(grid.grid[i]);
            points += grid.grid[i].point;
            grid.grid[i].prevPoints += grid.grid[i].point;
            grid.grid[i].point = 0;
            //Debug.Log(points);
        }

        pointDisplay.text = points.ToString();
    }



    public void DeleteTile() {
        nextTile.nextTileCluster.Clear();
        Round();
    }



    public void SwitchTiles() {
        preShowTile.Switch();
    }

    /*
     //------------------------------------------------------
     //====================[  All the draggables  ]===========================
     //---------------------------------------------------------------
    */

    #region Hovers

    public void mouseHover(Tile t) {

        // if nexttile is currently not picked up, if hovering over nextTile, set it as parent
        if (!nextTile.isDraggable) {

            if (t.transform.parent.gameObject.name == "NextTile") {
                hoverTargetParent = t.transform.parent.gameObject;
            } else hoverTargetParent = null;
        }

        if (!nextTile.isDraggable && t.transform.parent.gameObject.name == "Grid") {
          hoverInfoPanel.showInfoTile(t);
        }
    }

    public void MouseTarget() {

        //if (!nextTile.isDraggable) {
            if (hoverTargetParent.name == "NextTile") {
                nextTile.isDraggable = true;
                hoverTargetParent = nextTile.gameObject;
            }
        //}
    }


    public void mouseDrag() {

        if (nextTile.isDraggable == true) {
            for (int i = 0;i < nextTile.nextTileCluster.Count;i++) {
                nextTile.nextTileCluster[i].GetComponent<BoxCollider2D>().enabled = false;
            }
            nextTile.GetComponent<BoxCollider2D>().enabled = false;
            
            nextTile.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y-0.5f);

            nextTile.GetComponent<RectTransform>().localPosition = new Vector3(
                nextTile.GetComponent<RectTransform>().localPosition.x,
                nextTile.GetComponent<RectTransform>().localPosition.y , 0);

            //Debug.Log(nextTile.transform.position);
            //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
    }


    public void targetDrop() {

        if (nextTile.isDraggable) {

            //when uneven +1 else 0.5f (to get the right grid.Tile)
            float W = (nextTile.width%2 != 0) ? 0.5f : 1;
            float H = (nextTile.height%2 != 0) ? 0.5f : 1;

            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x+W;
            float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y+H;

            //get the Tile the Mouse is Hovering over, if there is one
            Tile hoverGridTile;
            hoverGridTile = grid.grid.FirstOrDefault(x => x.position.x == (int)(mouseX) && x.position.y == (int)(mouseY));

            /*Debug.Log("MouseX: "+mouseX);
            Debug.Log("MouseY: "+mouseY);
            Debug.Log("HovergridTile: " + hoverGridTile);*/

            //add nullchecks inside FirstorDefault Code
            Tile targetTile;

            //adjust the Tile targeted to the tile corresponding to the lowest left tile of the nextTileCluster
            if (hoverGridTile) {
                targetTile = grid.grid.FirstOrDefault(
                    searchedTile => searchedTile.position.x == (int)(hoverGridTile.position.x-(float)(nextTile.width/2)) &&
                    searchedTile.position.y == (int)(hoverGridTile.position.y-(float)((nextTile.height/2)))
                    );

                //if targetTile is not null; find all the other tiles of the nextTileCluster and check if they are not null.
                if (targetTile) {

                    Tile nullcheckTile = null;
                    bool canPlace = true; ;

                    for (int i = 0;i < nextTile.nextTileCluster.Count;i++) {

                        nullcheckTile = grid.grid.FirstOrDefault(
                        searchedTile => searchedTile.position.x == (targetTile.position.x + nextTile.nextTileCluster[i].position.x) &&
                        searchedTile.position.y == (targetTile.position.y + nextTile.nextTileCluster[i].position.y)
                        );


                        if (!nullcheckTile) {
                            Debug.Log("one of the Tiles does not exist, Abort");
                            break;}

                        canPlace = tileLibrary.placementRules(nullcheckTile,nextTile.nextTileCluster[i]);

                        if (!canPlace) {
                            Debug.Log("Cannot place Tile according to TilePlacement Rules");
                            break;}
                    }


                    //if no tiles where null

                    //bool canPlace = true;
                    //canPlace = nextTile.nextTileCluster[i].placementRules(grid.grid[o],nextTile.nextTileCluster[i]);

                    //Setting GridTiles.type to corresponding NextTiles.Type
                    if (nullcheckTile && canPlace) {
                        for (int i = 0;i < nextTile.nextTileCluster.Count;i++) {
                            for (int o = 0;o < grid.grid.Count();o++) {

                                if (nextTile.nextTileCluster[i].position+targetTile.position == grid.grid[o].position) {
                                    grid.grid[o].type = nextTile.nextTileCluster[i].type;
                                    tileLibrary.updateTile(grid.grid[o],stage);
                                }
                            }
                        }

                        hoverTargetParent = null;
                        nextTile.isDraggable = false;

                        nextTile.transform.position = nextTile.NTPanel.transform.position;
                        nextTile.nextTileCluster.Clear();

                        Round();
                    } else nextTile.reset();

                } else nextTile.reset();

            } else nextTile.reset();

        }
    }

    public void hoverEnter(Tile t) {
        if (!nextTile.isDraggable && t.transform.parent.gameObject.name == "Grid") {
            float x = t.transform.position.x;
            float y = t.transform.position.y;
            float z = t.transform.position.z;

            t.transform.position = new Vector3(x,y += 0.2f,z);
            t.GetComponent<BoxCollider2D>().offset = new Vector2(0,-0.2f);
            t.GetComponent<BoxCollider2D>().size = new Vector2(1,1.2f);
            t.hover = true;
        }
    }

    public void hoverExit(Tile t) {
        if (t.hover) {
            hoverInfoPanel.HoverPanel.SetActive(false);
            float x = t.transform.position.x;
            float y = t.transform.position.y;
            float z = t.transform.position.z;

            t.transform.position = new Vector3(x,y -= 0.2f,z);
            t.GetComponent<BoxCollider2D>().offset = new Vector2(0,0f);
            t.GetComponent<BoxCollider2D>().size = new Vector2(1,1f);
            t.hover = false;
        }
    }
    #endregion

}
