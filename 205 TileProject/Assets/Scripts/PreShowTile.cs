using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class PreShowTile:MonoBehaviour {

    public GameObject NTPanel;
    public GameObject tiles;
    public Manager manager;

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

        if (nextTileTypeList.Count == 0 && nextTileCluster.Count == 0) {
            manager.NextStage();
        } else {

            if (nextTileTypeList.Count > 0) {
                Random.Range(0,nextTileTypeList.Count);
                nexttileType = nextTileTypeList[r];
            }

            addToNextTile();

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
        NextTile.Description(NextTile.nextTileCluster[0]);
    }



    public void createTiles(Tile.Type nexttileType) {

        Debug.Log("Creating new Tiles");

        width = Random.Range(3,6);
        height = Random.Range(3,6);

        //Debug.Log("W: " + width);
        //Debug.Log("H: " + height);


        for (int y = 0;y < height;y++) {
            for (int x = 0;x < width;x++) {



                int chance = Random.Range(0,(width*height));
                if (chance > (width*height)/4) {

                    Vector2Int position = new Vector2Int(x,y);

                    z = (float)(height-y)/10;
                    //Debug.Log(z);
                    Tile nextTiles = Instantiate(tiles,new Vector3(x,y,z),tiles.transform.rotation).GetComponent<Tile>();


                    W = (width%2 == 0) ? 0.5f : 0;
                    H = (height%2 == 0) ? 0.5f : 0;

                    nextTiles.GetComponent<SpriteRenderer>().sortingOrder = -1;

                    /*nextTiles.GetComponent<RectTransform>().localPosition = new Vector3(
                        nextTiles.GetComponent<RectTransform>().localPosition.x,
                        nextTiles.GetComponent<RectTransform>().localPosition.y,0);*/

                    //Debug.Log(nextTiles.transform.position.z);

                    nextTiles.type = nexttileType;
                    nextTiles.position = position;
                    nextTiles.name = x.ToString() + ", " + y.ToString() + " - " + nextTiles.type;
                    //Debug.Log(nextTiles);
                    //nextTileCluster.Add(nextTiles);
                    nextTiles.transform.parent = this.gameObject.transform;

                    nextTiles.updateTile();
                    nextTileCluster.Add(nextTiles);
                }
            }
        }

        if (nextTileCluster.Count < 8) {
            Debug.Log("Cluster was < 8");
            clearCluster();
            nextTileCluster.Clear();
            createTiles(nexttileType);
        } else {
            overSizeCheck();
        }
    }

    public void overSizeCheck() {
        Tile widthCheck = null;
        Tile heightCheck = null;

        for (int i = 0;i < nextTileCluster.Count;i++) {
            heightCheck = nextTileCluster.FirstOrDefault(x => x.position.y == 0);
            widthCheck = nextTileCluster.FirstOrDefault(x => x.position.x == 0);
        }

        if (!widthCheck) {
            //Debug.Log("there was no 0 x");
            width = width-1;
            W = (width%2 == 0) ? 0.5f : 0;
        }
        if (!heightCheck) {
            //Debug.Log("there was no 0 y");
            height = height-1;
            H = (height%2 == 0) ? 0.5f : 0;
        }

        widthCheck = null;
        heightCheck = null;

        for (int i = 0;i < nextTileCluster.Count;i++) {
            heightCheck = nextTileCluster.FirstOrDefault(x => x.position.y == height-1);
            widthCheck = nextTileCluster.FirstOrDefault(x => x.position.x == width-1);
        }
        if (!widthCheck) {
            //Debug.Log("there was no max x");
            width = width-1;
            W = (width%2 == 0) ? 0.5f : 0;
        }
        if (!heightCheck) {
            //Debug.Log("there was no max y");
            height = height-1;
            H = (height%2 == 0) ? 0.5f : 0;
        }

        //Debug.Log("Readjusted Sizes: W" + width + ", H" + height);
        for (int i = 0;i < nextTileCluster.Count;i++) {


            int x = nextTileCluster[i].position.x;
            int y = nextTileCluster[i].position.y;

            nextTileCluster[i].transform.position =
                        new Vector3(
                            (float)(NTPanel.transform.position.x+x)-(float)(width/2)+W,
                            (float)(NTPanel.transform.position.y+y)-(float)(height/2)+H,
                            nextTileCluster[i].transform.position.z);
        }

    }

    public void addToNextTile() {
        Debug.Log("filling up Tiles from below");

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
        int r = Random.Range(0,3);
        if (r == 0) {
            nextTileTypeList.Add(Tile.Type.Ocean);
        } else if (r == 1) {
            nextTileTypeList.Add(Tile.Type.Mountain);
        } else
            nextTileTypeList.Add(Tile.Type.Grassland);
    }


    public void clearCluster() {
        Debug.Log("Clearing Cluster");

        foreach (Transform child in transform) {
            if (child != transform) {
                //Debug.Log("Deleting this");
                Destroy(child.gameObject);
            }
        }
    }


    public void reset() {
        Debug.Log("twas a Null in the Code");

        isDraggable = false;
        manager.hoverTargetParent = null;

        transform.position = NTPanel.transform.position;

        for (int i = 0;i < nextTileCluster.Count;i++) {
            nextTileCluster[i].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

}
