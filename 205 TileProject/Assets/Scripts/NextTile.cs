using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

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

    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI details;
    [SerializeField] TextMeshProUGUI pointDisplay;

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

    public void Description(Tile t) {
        name.text = t.type.ToString();

        switch (t.type) {
            case Tile.Type.Void:
                details.text = " ... ";
                break;

            case Tile.Type.Ocean:
                details.text = "A big Area of Water, can be placed on Void and any Tier 1 Tile";
                pointDisplay.text ="Grants 0 Points. + 1 for each adjacent Ocean, +3 for each adjacent Beach";
                break;

            case Tile.Type.Grassland:
                details.text = "Greenery waiting to develop, can be placed on Void and any Tier 1 Tile";
                pointDisplay.text ="Grants 1 Point. no Modifiers";
                break;

            case Tile.Type.Mountain:
                details.text = "Rockformations potentially holding valuable minerals, can be placed on Void and any Tier 1 Tile";
                pointDisplay.text ="Grants 0 points. +1 for each adjacent Mountain";
                break;


            case Tile.Type.Meadow:
                details.text = "A Patch of Rich soil and diverse Plantlife, can only be placed on Grassland";
                break;

            case Tile.Type.Forest:
                details.text = "Underwoods animals love to hide in, can only be placed on Grassland";
                pointDisplay.text ="Grants 2 points. +3 for each adjacent Mountain or Orevein, +2 For each adjacent Forest";
                break;

            case Tile.Type.OreVein:
                details.text = "A Surfaced Vein of Rich Minerals and Metals, can only be placed on MountainTiles";
                pointDisplay.text ="Grants 3 Points. 5* original Points of Tile Placed on, -4 for each adjacent Orevein";
                break;

            case Tile.Type.River:
                details.text = "";
                break;



        }
    }

}
