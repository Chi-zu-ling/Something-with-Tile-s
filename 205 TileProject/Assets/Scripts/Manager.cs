using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Manager : MonoBehaviour
{

    public Grid grid;
    public NextTile nextTile;
    public Tile tile;

    public int points = 0;
    int round = 0;

    public GameObject hoverTargetParent;

    // Start is called before the first frame update
    void Start()
    {
        grid.startUp();

        for (int i = 0;i < grid.grid.Count;i++) {
            grid.grid[i].updateTile();
        }

        nextTile.startUp();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(hoverTargetParent);
    }

    public void Round() {

        nextTile.clearCluster();

        for(int i=0; i < grid.grid.Count;i++) {
            grid.grid[i].updateTile();}

        grid.adjacentTiles();

        nextTile.nextTile();
        round++;
        //Debug.Log(round);
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
          
            nextTile.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y,-1);
        }
    }


    public void targetDrop() {

        if (nextTile.isDraggable) {

            //when uneven +1 else 0.5f (to get the right grid.Tile)
            float W = (nextTile.W == 0.5) ? 1 : 0.5f;
            float H = (nextTile.H == 0.5) ? 1 : 0.5f;

            float mouseX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x+W;
            float mouseY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y+H;

            //get the Tile the Mouse is Hovering over, if there is one
            Tile hoverGridTile;
            hoverGridTile = grid.grid.FirstOrDefault(x => x.position.x == (int)(mouseX) && x.position.y == (int)(mouseY));
            //hoverGridTile = grid.grid.FirstOrDefault(x => x.position.x == (int)(mouseX) && x.position.y == (int)(mouseY));

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

                        canPlace = nextTile.nextTileCluster[i].placementRules(nullcheckTile,nextTile.nextTileCluster[i]);

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
                                    grid.grid[o].updateTile();}
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
    #endregion

}
