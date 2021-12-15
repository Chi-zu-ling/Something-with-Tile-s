using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class PreShowTile:MonoBehaviour {

    public GameObject NTPanel;
    public GameObject tiles;
    public Manager manager;
    public TileLibrary tileLibrary;

    public NextTile NextTile;

    public GameObject currentTileCluster;

    public bool isDraggable = false;
    public Vector3 mouseDragStartPos;
    public Vector3 gameObjectDragStartPos;

    public List<Tile> nextTileCluster = new List<Tile>();

    public List<Tile.Type> nextTileTypeList;

    public Tile.Type nexttileType;
    public int r;

    public int width;
    public int height;

    public float z;

    public float W;
    public float H;

    public int Village;
    public int Lumber;
    public int Forge;
    public int Mine;
    public int Trader;


    public void startUp() {

        nextTileTypeList.Add(Tile.Type.Grassland);
        nextTileTypeList.Add(Tile.Type.Grassland);
        nextTileTypeList.Add(Tile.Type.Grassland);
        nextTileTypeList.Add(Tile.Type.Mountain);
        nextTileTypeList.Add(Tile.Type.Mountain);
        nextTileTypeList.Add(Tile.Type.Ocean);
        nextTileTypeList.Add(Tile.Type.Ocean);

        while (nextTileTypeList.Count < 16) {
            populateTypes();
        }

        //Debug.Log("End of Startup: " + nextTileTypeList.Count);
        nextTile();
    }


    public void nextTile() {

        Debug.Log("NextTile");
         /*Debug.Log(nextTileTypeList.Count);
         Debug.Log(NextTile.nextTileCluster.Count);*/

        if (nextTileTypeList.Count == 0 && NextTile.nextTileCluster.Count == 0 && nextTileCluster.Count == 0) {
            manager.PointTally();} 
        
        else {

            if (nextTileTypeList.Count > 0) {
                r = Random.Range(0,nextTileTypeList.Count);
                nexttileType = nextTileTypeList[r];
            }

            if(nextTileCluster.Count != 0) addToNextTile();

            if (nextTileTypeList.Count > 0) {
                createTiles(nexttileType);
                nextTileTypeList.RemoveAt(r);
            }

            if (NextTile.nextTileCluster.Count == 0) {
                addToNextTile();
                clearCluster();
                nextTile();
            }

        }

        if (NextTile.nextTileCluster.Count > 0) {
            tileLibrary.Description(NextTile.nextTileCluster[0]);
        }
    }



    public void createTiles(Tile.Type nexttileType) {

        #region ClusterSize
        int min = 0;
        int max = 0;

        //general
        if(manager.stage == 1) { min = 3; max = 6; }
        else if(manager.stage == 2) { min = 1; max = 3; }
        else if(manager.stage == 3) { min = 1; max = 2; }

        //type Specifics
        if(nexttileType == Tile.Type.Forest) {
             min = 1; max = 4; }


        width = Random.Range(min,max);
        height = Random.Range(min,max);

        if (nexttileType == Tile.Type.River) {
            { width = 1; height = 1;}}

        #endregion



        for (int y = 0;y < height;y++) {
            for (int x = 0;x < width;x++) {



                int chance = Random.Range(0,(width*height));
                if (chance > (width*height)/4 || nexttileType == Tile.Type.River || manager.stage == 3) {

                    Vector2Int position = new Vector2Int(x,y);

                    z = y;
                    Tile nextTiles = Instantiate(tiles,new Vector3(x,y,z),tiles.transform.rotation).GetComponent<Tile>();


                    W = (width%2 == 0) ? 0.5f : 0;
                    H = (height%2 == 0) ? 0.5f : 0;

                    nextTiles.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    nextTiles.type = nexttileType;
                    nextTiles.position = position;
                    nextTiles.name = x.ToString() + ", " + y.ToString() + " - " + nextTiles.type;

                    nextTiles.transform.parent = this.gameObject.transform;

                    manager.tileLibrary.updateTile(nextTiles, manager.stage);
                    nextTileCluster.Add(nextTiles);
                }
            }
        }

        if ((nextTileCluster.Count < 8 && manager.stage == 1) || (nextTileCluster.Count == 0)) {
            //Debug.Log("Cluster was < 8 or 0");
            clearCluster();
            nextTileCluster.Clear();
            createTiles(nexttileType);
        } else {
            overSizeCheck();
        }
    }



    public void overSizeCheck() {

        Tile left = nextTileCluster[0];
        Tile right = nextTileCluster[0];
        Tile bottom = nextTileCluster[0];
        Tile top = nextTileCluster[0];

        for(int i = 0; i < nextTileCluster.Count;i++) {

            if(nextTileCluster[i].position.x < left.position.x) { left = nextTileCluster[i]; }
            if(nextTileCluster[i].position.x > right.position.x) { right = nextTileCluster[i]; }
            if(nextTileCluster[i].position.y < left.position.x) { bottom = nextTileCluster[i]; }
            if(nextTileCluster[i].position.y > left.position.x) { top = nextTileCluster[i]; }
        }

        #region Debug-null
        /*Debug.Log(left);
        Debug.Log(right);
        Debug.Log(bottom);
        Debug.Log(top);*/
        #endregion

        width = (right.position.x)-(left.position.x) + 1;
        W = (width%2 == 0) ? 0.5f : 0;

        height = (top.position.y) - (bottom.position.y) + 1;
        H = (height%2 == 0) ? 0.5f : 0;

        #region Debug-size
        /*Debug.Log(left.position.x + ",-");
        Debug.Log(right.position.x + ",-");
        Debug.Log("-,"+bottom.position.y);
        Debug.Log("-,"+top.position.y);

        Debug.Log("width: " + width);
        Debug.Log("height: " + height);*/
        #endregion

        for (int i = 0;i < nextTileCluster.Count;i++) {

            nextTileCluster[i].position.x = nextTileCluster[i].position.x-left.position.x;
            nextTileCluster[i].position.y = nextTileCluster[i].position.y-bottom.position.y;

            int x = nextTileCluster[i].position.x;
            int y = nextTileCluster[i].position.y;



            nextTileCluster[i].transform.position =
                        new Vector3(
                            (float)(NTPanel.transform.position.x+x)-(float)(width/2)+W,
                            (float)(NTPanel.transform.position.y+y)-(float)(height/2)+H,
                            nextTileCluster[i].transform.position.z);
            //Debug.Log(nextTileCluster[i]);
        }

    }



    public void Switch() {
        List<Tile> preSwitch = new List<Tile>();

        for(int i = 0; i < NextTile.nextTileCluster.Count;i++) {
            preSwitch.Add(NextTile.nextTileCluster[i]);
        }

        addToNextTile();

        for(int i = 0; i < preSwitch.Count;i++) {

            float x = NextTile.nextTileCluster[i].transform.position.x;
            float y = NextTile.nextTileCluster[i].transform.position.y;
            float z = NextTile.nextTileCluster[i].transform.position.z;

            NextTile.nextTileCluster[i].transform.position = new Vector3(x,y -= 0.5f,z += 1.5f);

            nextTileCluster.Add(NextTile.nextTileCluster[i]);

            nextTileCluster[i].GetComponent<SpriteRenderer>().sortingOrder = -1;

            NextTile.nextTileCluster[i].transform.parent = this.gameObject.transform;

        }

        for (int i = 0; i < NextTile.nextTileCluster.Count;i++) {
            for(int o = 0; o < preSwitch.Count;o++) {
                if(NextTile.nextTileCluster[i] == preSwitch[o]) {
                    NextTile.nextTileCluster.Remove(NextTile.nextTileCluster[i]);
                }
            }
        }

        overSizeCheck();

        tileLibrary.Description(NextTile.nextTileCluster[0]);

        preSwitch.Clear();
    }



    public void addToNextTile() {
        //Debug.Log("filling up Tiles from below");

        for (int i = 0;i < nextTileCluster.Count;i++) {

            float x = nextTileCluster[i].transform.position.x;
            float y = nextTileCluster[i].transform.position.y;
            float z = nextTileCluster[i].transform.position.z;

            nextTileCluster[i].transform.position = new Vector3(x,y += 0.5f,z -= 1.5f);
            NextTile.nextTileCluster.Add(nextTileCluster[i]);

            nextTileCluster[i].GetComponent<SpriteRenderer>().sortingOrder = 0;

            nextTileCluster[i].transform.parent = NextTile.gameObject.transform;
        }
       
        NextTile.width = width;
        NextTile.height = height;

        nextTileCluster.Clear();
    }



    public void populateTypes() {
        int r = Random.Range(0,4);
        if (r == 0) {
            nextTileTypeList.Add(Tile.Type.Ocean);
        } else if (r == 1) {
            nextTileTypeList.Add(Tile.Type.Mountain);
        } else
            nextTileTypeList.Add(Tile.Type.Grassland);
    }



    public void clearCluster() {
        //Debug.Log("Clearing Cluster");

        foreach (Transform child in transform) {
            if (child != transform) {
                //Debug.Log("Deleting this");
                Destroy(child.gameObject);
            }
        }
    }


    public void reset() {
        //Debug.Log("twas a Null in the Code");

        isDraggable = false;
        manager.hoverTargetParent = null;

        transform.position = NTPanel.transform.position;

        for (int i = 0;i < nextTileCluster.Count;i++) {
            nextTileCluster[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

}
