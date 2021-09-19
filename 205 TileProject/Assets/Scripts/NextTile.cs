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

    public GameObject currentTileCluster;

    public bool isDraggable = false;
    public Vector3 mouseDragStartPos;
    public Vector3 gameObjectDragStartPos;

    public List<Tile> nextTileCluster = new List<Tile>();

    public List<Tile.Type> nextTileTypeList;

    public int width;
    public int height;

    public float z;

    public float W;
    public float H;

    public void startUp(){

        currentTileCluster.GetComponent<BoxCollider2D>().size = new Vector2(15,15);
        //Debug.Log("End of Startup: " + nextTileTypeList.Count);
    }

    public void readjust() {

        for (int i = 0;i < nextTileCluster.Count; i++) {

            W = (width%2 == 0) ? 0.5f : 0f;
            H = (height%2 == 0) ? 0.5f : 0;

            nextTileCluster[i].transform.position =
                new Vector3(
                    (float)(NTPanel.transform.position.x+nextTileCluster[i].position.x)-(float)(width/2)+W,
                    (float)(NTPanel.transform.position.y+nextTileCluster[i].position.y)-(float)(height/2)+H,
                    z);
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
