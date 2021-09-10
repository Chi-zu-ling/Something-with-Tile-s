using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class NextTile : MonoBehaviour
{
    public GameObject NTPanel;
    public GameObject tiles;
    public Manager manager;

    public bool isDraggable = false;
    public Vector3 mouseDragStartPos;
    public Vector3 gameObjectDragStartPos;

    public List<Tile> nextTileCluster = new List<Tile>();

    public List<Tile.Type> nextTileType;

    public int width;
    public int height;

    public float z;

    public float W;
    public float H;

    public void startUp(){

        Debug.Log(NTPanel.transform.position.x);
        Debug.Log(NTPanel.transform.position.y);
        //this.transform.position = new Vector3(NTPanel.transform.position.x, NTPanel.transform.position.y, -3);

        Debug.Log(this);
        Debug.Log(this.GetComponent<BoxCollider2D>());

        this.GetComponent<BoxCollider2D>().size = new Vector2(15,15);
        Debug.Log(this.GetComponent<BoxCollider2D>().size);
        nextTile();
    }

    public void nextTile() {

        nextTileCluster.Clear();

        for (int i = nextTileCluster.Count;i > 0;i--) {
            //NTPanel.Destroy(NTPanel.GetChild(i-1));
        }

        width = Random.Range(3,6);
        height = Random.Range(3,6);

        //Debug.Log("W: " + width);
        //Debug.Log("H: " + height);
        
        for(int y = 0; y < height;y++) {
            for(int x = 0; x < width;x++) {

                int chance = Random.Range(0,(width*height));
                if(chance > (width*height)/4) {
                    
                    Vector2Int position = new Vector2Int(x,y);
                    
                    z = (float)-1-(height-y)/10;
                    //Debug.Log(z);
                    Tile nextTiles = Instantiate(tiles,new Vector3(x,y,z),tiles.transform.rotation).GetComponent<Tile>();


                    W = (width%2 == 0) ? 0.5f : 0;
                    H = (height%2 == 0) ? 0.5f : 0;

                    nextTiles.transform.position = 
                        new Vector3(
                            (float)(NTPanel.transform.position.x+position.x)-(float)(width/2)+W,
                            (float)(NTPanel.transform.position.y+position.y)-(float)(height/2)+H,
                            z);
                  
                    Debug.Log(nextTiles.transform.position.z);

                    nextTiles.type = Tile.Type.Mountain;
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

        if(nextTileCluster.Count < 8) { redoCluster(); }
        isDraggable = false;

        overSizeCheck();
    }

    public void overSizeCheck() { 
        Tile widthCheck = null;
        Tile heightCheck = null;

        for (int i = 0; i < nextTileCluster.Count;i++) {
            heightCheck = nextTileCluster.FirstOrDefault(x => x.position.y == 0);
            widthCheck = nextTileCluster.FirstOrDefault(x => x.position.x == 0);
        }

        if (!widthCheck){
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
        for(int i = 0;i < nextTileCluster.Count;i++) {


            int x = nextTileCluster[i].position.x;
            int y = nextTileCluster[i].position.y;

            nextTileCluster[i].transform.position =
                        new Vector3(
                            (float)(NTPanel.transform.position.x+x)-(float)(width/2)+W,
                            (float)(NTPanel.transform.position.y+y)-(float)(height/2)+H,
                            nextTileCluster[i].transform.position.z);
        }
    
    }

    public void clearCluster() {

        foreach (Transform child in transform) {
            if (child != transform) {
                //Debug.Log("Deleting this");
                Destroy(child.gameObject);
            }
        }
    }

    public void redoCluster() {

        foreach (Transform child in transform) {
            if (child != transform) {
                //Debug.Log("Deleting this");
                Destroy(child.gameObject);
            }
        }

        nextTile();
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

    private void Update() {
        
    }

}
