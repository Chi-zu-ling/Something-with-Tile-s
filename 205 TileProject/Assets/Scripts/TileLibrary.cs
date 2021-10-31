using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileLibrary : MonoBehaviour
{

    [SerializeField] Tile tile;
    [SerializeField] Grid grid;
    [SerializeField] PreShowTile preShowTile;

    [SerializeField] Sprite Void;
    [SerializeField] Sprite Ocean;
    [SerializeField] Sprite Grassland;
    [SerializeField] Sprite Mountain;

    [SerializeField] Sprite Beach;
    [SerializeField] Sprite BeachShells;

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


    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI details;
    [SerializeField] TextMeshProUGUI pointDisplay;

    int adjRivers;

    public void updateTile(Tile t,int stage) {

        int r = 0;
        adjRivers = 0;

        //Beach-check
        if (t.type == Tile.Type.Beach) {
            bool watercheck = false;

            for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                if (t.AdjacentTiles[i].type == Tile.Type.Ocean) {
                    watercheck = true;
                }
            }

            if (watercheck == false) {
                t.type = Tile.Type.Grassland;
            }
        }

        if (t.type == Tile.Type.Grassland) {

            for (int i = 0;i < t.AdjacentTiles.Count;i++) {

                if (t.AdjacentTiles[i].type == Tile.Type.Ocean) {
                    t.type = Tile.Type.Beach;

                    t.variation = Random.Range(1,4);

                    r = Random.Range(1,5);
                    if(r > 2) {
                        Debug.Log(r + "flipped");
                        t.flipped = true;
                    }
                }
            }

        }


        //marshland Check
        for(int i = 0; i < t.AdjacentTiles.Count;i++) {
            if(t.AdjacentTiles[i].type == Tile.Type.River) {
                adjRivers++;
            }
        }

        if(adjRivers > 2 && t.type != Tile.Type.MarshLand) {
            t.type = Tile.Type.MarshLand;
        }

        t.name = t.position + ", " +t.type;
        //check if AdjacentTiles should change this tile, if yes set Type


        //Setting Sprite
        //To add Sprite, create new [SerializeField] Sprite [name];
        //in global and add the sprite to it via the Unity Editor
        //then add new Tile type in: enum Type{}
        //Lastly, create new case below.
        SetSprites(t);

        pointRules(t,stage);

    }



    //set's sprite Tiles and increases Grid Count
    public void SetSprites(Tile t) {
        switch (t.type) {

            case Tile.Type.Void:
                t.GetComponent<SpriteRenderer>().sprite = Void;
                break;

            case Tile.Type.Ocean:
                t.GetComponent<SpriteRenderer>().sprite = Ocean;
                t.tier = 1;
                break;

            case Tile.Type.Grassland:
                t.GetComponent<SpriteRenderer>().sprite = Grassland;
                t.tier = 1;
                break;

            case Tile.Type.Mountain:
                t.GetComponent<SpriteRenderer>().sprite = Mountain;
                t.tier = 1;
                break;

            case Tile.Type.Beach:

                if(t.variation == 2) {
                    t.GetComponent<SpriteRenderer>().sprite = BeachShells;
                } else {t.GetComponent<SpriteRenderer>().sprite = Beach;}

                if (t.flipped) {
                    t.GetComponent<SpriteRenderer>().flipX = true;}

                t.tier = 1;

                break;



            case Tile.Type.Meadow:
                t.GetComponent<SpriteRenderer>().sprite = Meadow;
                t.tier = 2;
                break;

            case Tile.Type.Forest:
                t.GetComponent<SpriteRenderer>().sprite = Forest;
                t.tier = 2;
                break;

            case Tile.Type.OreVein:
                t.GetComponent<SpriteRenderer>().sprite = OreVein;
                t.tier = 2;
                break;

            case Tile.Type.River:
                t.GetComponent<SpriteRenderer>().sprite = River;
                t.tier = 2;
                break;

            case Tile.Type.MarshLand:
                t.GetComponent<SpriteRenderer>().sprite = Marshland;
                t.tier = 2;
                break;


            default: {
                    Debug.Log("");
                    break;
                }
        }
    }



    public bool placementRules(Tile targetTile,Tile nextTile) {
        //only gets called by manager.targetDrop()
        //get placementRules for nextTile (the Tile You are Holding), and check if TargetTile fullfills requirements, if yes; return true; 

        switch (nextTile.type) {
            case Tile.Type.Void:
                return true;


            case Tile.Type.Ocean:
                return true;


            case Tile.Type.Grassland:
                return true;


            case Tile.Type.Mountain:
                return true;


            //Tier 2
            case Tile.Type.Meadow:
                if (targetTile.type == Tile.Type.Grassland) {
                    return true;
                } else return false;


            case Tile.Type.Forest:
                if (targetTile.type == Tile.Type.Grassland) {
                    return true;
                } else return false;


            case Tile.Type.OreVein:
                if (targetTile.type == Tile.Type.Mountain) {
                    return true;
                } else return false;


            case Tile.Type.River:

                if((targetTile.type != Tile.Type.Ocean) && (targetTile.type != Tile.Type.Void)){

                    Debug.Log(grid.rivers);

                    //first RiverTile
                    if (grid.rivers == 0) {

                        for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                            if (targetTile.AdjacentTiles[i].type == Tile.Type.Ocean) {
                                preShowTile.nextTileTypeList.Add(Tile.Type.River);
                                return true;
                            }
                        }
                        return false;
                    }

                    //not first RiverTile
                    else if(grid.rivers > 0 
                        && targetTile.type != Tile.Type.Mountain &&
                        targetTile.type != Tile.Type.OreVein) {

                        Debug.Log("not first or last Tile: " + targetTile);
                        for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                            if (targetTile.AdjacentTiles[i].type == Tile.Type.River) {
                                preShowTile.nextTileTypeList.Add(Tile.Type.River);
                                return true;
                            }
                        }
                    } 
                    
                    //final RiverTile
                    else{

                        Debug.Log("Final tile: " +  targetTile);
                        for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                            if (targetTile.AdjacentTiles[i].type == Tile.Type.River) {
                                targetTile.point += 25;
                                return true;
                            }
                        }
                    }
                }
                return false;

        }

        return true;
    }



    //Callculation of Points for this tile
    public void pointRules(Tile t,int stage) {

        #region Tier-1
        if (stage ==1) {
            switch (t.type) {

                case Tile.Type.Void:
                    break;

                case Tile.Type.Ocean:
                    t.point = 0;
                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Ocean) {
                            t.point += 1;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Beach) {
                            t.point += 3;
                        }
                    }
                    break;

                case Tile.Type.Grassland:
                    t.point = 1;
                    break;

                case Tile.Type.Mountain:
                    t.point = 0;
                    int M = 0;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Mountain) {
                            M++;
                        }
                    }

                    t.point = M;

                    break;

                case Tile.Type.Beach:
                    t.point = 5;
                    break;
            }
        }
        #endregion

        #region Tier-2
        //Tier 2
        else if (stage == 2) {
            switch (t.type) {
                case Tile.Type.OreVein:

                    t.point = (t.prevPoints*5 +3);

                    //5 times the t.points of the mointain t placed on - 4 for each mountain adjacent
                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.OreVein) {
                            t.point -= 4;
                        }
                    }

                    break;

                case Tile.Type.Forest:
                    t.point = 2;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Mountain ||
                            t.AdjacentTiles[i].type == Tile.Type.OreVein) {
                            t.point += 3;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Forest) {
                            t.point +=2;
                        }
                    }

                    break;

                case Tile.Type.Meadow:

                    t.point = 0;
                    int S = 0;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if(t.AdjacentTiles[i].type == Tile.Type.Grassland) {
                            S++;
                            t.point += 1;
                        }
                        else if (t.AdjacentTiles[i].type == Tile.Type.Meadow) {
                            S++;
                            t.point += 2;
                        }
                    }

                    t.point *= S;
                    break;

                case Tile.Type.MarshLand:
                    t.point = 0;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        t.point +=  t.AdjacentTiles[i].point;
                    }

                    t.point = ((t.point * -1)/2);

                    break;

                case Tile.Type.River:

                    t.point = t.prevPoints;

                    /*for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if(t.AdjacentTiles[i].type != Tile.Type.River) {
                            t.point += (t.AdjacentTiles[i].point * 2);
                        }
                    }*/

                    break;

            }
        }
        #endregion

        if (adjRivers > 0) {
            t.point = (t.point * (adjRivers*2)); 
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
                pointDisplay.text = " - ";
                break;

            case Tile.Type.Forest:
                details.text = "Underwoods animals love to hide in, can only be placed on Grassland";
                pointDisplay.text ="Grants 2 points. +3 for each adjacent Mountain or Orevein, +2 For each adjacent Forest";
                break;

            case Tile.Type.OreVein:
                details.text = "A Surfaced Vein of Rich Minerals and Metals, can only be placed on Mountains";
                pointDisplay.text ="Grants 3 Points. 5* original Points of Tile Placed on, -4 for each adjacent Orevein";
                break;

            case Tile.Type.River:
                if (grid.rivers > 0) {
                    details.text = "Nourishing streams of water, can be placed on anything except Void and Oceans. Will continue to grant Rivertiles until it ends on a Mountain, Orevein or deleted.";
                }else
                    details.text = "Nourishing streams of water, can be placed on anything except Void and Oceans. Must first be placed adjacent to an Ocean. Afterwards adjacent to other Rivers.";
                pointDisplay.text = "Grants a *2 Multiplier to adjacent tiles (stacks), grants a bonus if it ends on a Mountain or Orevein. Dont become too greedy";
                break;

        }
    }
}
