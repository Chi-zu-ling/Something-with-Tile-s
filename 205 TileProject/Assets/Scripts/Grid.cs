using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Grid:MonoBehaviour {

    public GameObject tiles;

    public int gridWidth = 12;
    public int gridHeight = 12;

    float width;
    float height;

    public int Void;
    public int mountains;
    public int oceans;
    public int grasslands;
    public int beaches;

    public int forests;
    public int oreveins;
    public int meadows;
    public int rivers;

    public int villages;
    public int lumbers;
    public int hunters;
    public int farmers;
    public int mines;
    public int forges;
    public int fishers;
    public int traders;

    public List<Tile> grid = new List<Tile>();

    // Start is called before the first frame update
    public void startUp() {

        //this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,0);

        spawnGrid();
        adjacentTiles();
        centerCamera();
    }

    public void clearCount() {
        Void = 0;
        mountains = 0;
        oceans = 0;
        grasslands = 0;
        beaches = 0;

        forests = 0;
        oreveins = 0;
        meadows = 0;
        rivers = 0;

        villages = 0;
        lumbers = 0;
        hunters = 0;
        farmers = 0;
        mines = 0;
        forges = 0;
        fishers = 0;
        traders = 0;
    }

    /*public void Count(){
        Debug.Log("Void: " + Void);
        Debug.Log("Mountains: " + mountains);
        Debug.Log("Oceans: " + oceans);
        Debug.Log("Grassland: " + grasslands);

    }*/


    //... spawns grid, only call once at start of game
    public void spawnGrid() {
        for (int y = 0;y < gridHeight;y++) {

            for (int x = 0;x < gridWidth;x++) {

                //float z = ((height-(height-y))/10);
                float z = (float)-(height-y)/10;
                Vector2Int Position = new Vector2Int(x,y);


                //instantiate (prefab, vector3, euler.rotation, transform)) if you add the ,transform
                // it automaticaly makes this object a child of the gameobject
                Tile gridTile = Instantiate(tiles,new Vector3(x,y,z),tiles.transform.rotation).GetComponent<Tile>();

                gridTile.position = Position;
                gridTile.name = x.ToString() + ", " + y.ToString();

                gridTile.type = Tile.Type.Void;

                //makes all of these the children of the Grid Gameobject
                gridTile.transform.parent = this.gameObject.transform;

                //gridTile.GetComponent<BoxCollider2D>().transform.position = new Vector3(gridTile.transform.position.x,gridTile.transform.position.y,-1);
                grid.Add(gridTile);
            }
        }
    }


    //checks a tile, then scanns for the tiles adjacent to it and adds them to the adjacentTile list for later use.
    public void adjacentTiles() { 
        for(int i = 0; i < grid.Count;i++) {

            grid[i].AdjacentTiles.Clear();

            for(int o = 0; o < grid.Count;o++) {
                if(
                    grid[o].position.x == grid[i].position.x+1 && grid[o].position.y == grid[i].position.y ||
                    grid[o].position.x == grid[i].position.x-1 && grid[o].position.y == grid[i].position.y ||
                    grid[o].position.x == grid[i].position.x && grid[o].position.y == grid[i].position.y+1 ||
                    grid[o].position.x == grid[i].position.x && grid[o].position.y == grid[i].position.y-1


                    ) {
                    grid[i].AdjacentTiles.Add(grid[o]);
                }
            }
        }
    }


    //center's camera on Grid (scale is off, UI needs adjustments, need somw way to make UI pixel perfect size as cameraview, and make it snapp to camera)
    public void centerCamera(){
        Camera.main.transform.position = new Vector3((gridWidth/2)-0.5f+4,(gridHeight/2)-0.5f,Camera.main.transform.position.z);
    }
}
