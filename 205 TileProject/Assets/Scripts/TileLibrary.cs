using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileLibrary:MonoBehaviour {

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
    [SerializeField] Sprite Lumber;
    [SerializeField] Sprite Hunter;
    [SerializeField] Sprite Farm;
    [SerializeField] Sprite Forge;
    [SerializeField] Sprite Mine;
    [SerializeField] Sprite Fisher;
    [SerializeField] Sprite Trader;


    [SerializeField] TextMeshProUGUI name;
    [SerializeField] TextMeshProUGUI details;
    [SerializeField] TextMeshProUGUI pointDisplay;

    int adjRivers;

    bool mineVillage = false;
    bool lumberVillage = false;

    public void updateTile(Tile t,int stage) {

        Debug.Log("Updating Tile");

        int r = 0;
        adjRivers = 0;

        //Beach-check
        if (stage == 1) {
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
                        if (r > 2) {
                            Debug.Log(r + "flipped");
                            t.flipped = true;
                        }
                    }
                }

            }
        }


        //marshland Check
        for (int i = 0;i < t.AdjacentTiles.Count;i++) {
            if (t.AdjacentTiles[i].type == Tile.Type.River) {
                adjRivers++;
            }
        }

        if (adjRivers > 2 && t.type != Tile.Type.MarshLand && t.type != Tile.Type.Void) {
            t.type = Tile.Type.MarshLand;
        }


        //Tier 3 Tile-distribution

        if (grid.mines == 1 && !mineVillage) {
            mineVillage= true;
            Debug.Log("added mineVillage");
            preShowTile.nextTileTypeList.Add(Tile.Type.Village);
        }

        if (grid.mines == 1 && !lumberVillage) {
            lumberVillage= true;
            Debug.Log("added lumberVillage");
            preShowTile.nextTileTypeList.Add(Tile.Type.Village);
        }


        t.name = t.position + ", " +t.type;
        //check if AdjacentTiles should change this tile, if yes set Type

        SetSprites(t);

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

                if (t.variation == 2) {
                    t.GetComponent<SpriteRenderer>().sprite = BeachShells;
                } else { t.GetComponent<SpriteRenderer>().sprite = Beach; }

                if (t.flipped) {
                    t.GetComponent<SpriteRenderer>().flipX = true; }

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



            case Tile.Type.Village:
                t.GetComponent<SpriteRenderer>().sprite = Village;
                t.tier = 3;
                break;

            case Tile.Type.Lumber:
                t.GetComponent<SpriteRenderer>().sprite = Lumber;
                t.tier = 3;
                break;

            case Tile.Type.Hunter:
                t.GetComponent<SpriteRenderer>().sprite = Hunter;
                t.tier = 3;
                break;

            case Tile.Type.Farm:
                t.GetComponent<SpriteRenderer>().sprite = Farm;
                t.tier = 3;
                break;

            case Tile.Type.Mine:
                t.GetComponent<SpriteRenderer>().sprite = Mine;
                t.tier = 3;
                break;

            case Tile.Type.Forge:
                t.GetComponent<SpriteRenderer>().sprite = Forge;
                t.tier = 3;
                break;

            case Tile.Type.Fisher:
                t.GetComponent<SpriteRenderer>().sprite = Fisher;
                t.tier = 3;
                break;

            case Tile.Type.Trader:
                t.GetComponent<SpriteRenderer>().sprite = Trader;
                t.tier = 3;
                break;



            default: {
                    Debug.Log("");
                    break;
                }
        }
    }



    public void Destribute(int stage){
        if (stage == 3) {
            for (int i = 0;preShowTile.Village <= (grid.lumbers + grid.mines)/2;i++) {
                preShowTile.nextTileTypeList.Add(Tile.Type.Village);
                preShowTile.Village++;
            };

            if (grid.mines > 0) {
                for (int i = 0;preShowTile.Forge <= (grid.mines)/2;i++) {
                    preShowTile.nextTileTypeList.Add(Tile.Type.Forge);
                    preShowTile.Forge++;
                };
            }

            if(grid.forges > preShowTile.Trader &&
            grid.lumbers%2 > preShowTile.Trader &&
            grid.fishers > preShowTile.Trader &&
            grid.hunters > preShowTile.Trader &&
            grid.farmers > preShowTile.Trader) {
                preShowTile.nextTileTypeList.Add(Tile.Type.Trader);
                preShowTile.Trader++;
            }

        }

    }
     


    public bool placementRules(Tile targetTile,Tile nextTile) {
        //only gets called by manager.targetDrop()
        //get placementRules for nextTile (the Tile You are Holding), and check if TargetTile fullfills requirements, if yes; return true; 

        if (targetTile.type != Tile.Type.MarshLand) {

            #region Tier-1
            switch (nextTile.type) {
                case Tile.Type.Void:
                    return true;


                case Tile.Type.Ocean:
                    return true;


                case Tile.Type.Grassland:
                    return true;


                case Tile.Type.Mountain:
                    return true;
                #endregion

            #region Tier-2
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

            #region River
            case Tile.Type.River:

                if ((targetTile.type != Tile.Type.Ocean) && (targetTile.type != Tile.Type.Void)) {

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

                    /*if (targetTile.AdjacentTiles[i].type == Tile.Type.Ocean) {
                        return true;
                    }*/

                    //not first RiverTile
                    else if (grid.rivers > 0
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
                    else {

                        Debug.Log("Final tile: " +  targetTile);
                        for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                            if (targetTile.AdjacentTiles[i].type == Tile.Type.River) {
                                targetTile.point += 50;
                                return true;
                            }
                        }
                    }
                }
                return false;
            #endregion

            #endregion


            //Tier 3
            #region Village
            case Tile.Type.Village:
                if (!(targetTile.type == Tile.Type.Ocean ||
                    targetTile.type == Tile.Type.River ||
                    targetTile.type == Tile.Type.Void ||
                    targetTile.tier == 3)) {

                    if (grid.villages == 0) {
                        preShowTile.nextTileTypeList.Add(Tile.Type.Lumber);
                            preShowTile.Lumber++;
                        preShowTile.nextTileTypeList.Add(Tile.Type.Mine);
                            preShowTile.Lumber++;

                            for (int i = 0;i < (grid.beaches + grid.rivers)/5;i++) {
                            preShowTile.nextTileTypeList.Add(Tile.Type.Fisher);
                        }

                        for (int i = 0;i < (grid.meadows)/3;i++) {
                            preShowTile.nextTileTypeList.Add(Tile.Type.Farm);}

                        for (int i = 0;i<3;i++) {
                            preShowTile.nextTileTypeList.Add(Tile.Type.Hunter);}
                    }

                    return true;
                } else return false;
            #endregion

            #region Lumber
            case Tile.Type.Lumber:

                int adjForest = 0;

                if (!(targetTile.type == Tile.Type.Ocean ||
                    targetTile.type == Tile.Type.River ||
                    targetTile.type == Tile.Type.Void ||
                    targetTile.tier == 3)) {

                    for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                        if (targetTile.AdjacentTiles[i].type == Tile.Type.Forest) {
                            adjForest++;
                        }
                    }

                    if (adjForest >= 2) {
                        return true;
                    } else return false;

                } else return false;
            #endregion

            #region Hunter
            case Tile.Type.Hunter:
                if (!(targetTile.type == Tile.Type.Ocean ||
                    targetTile.type == Tile.Type.River ||
                    targetTile.type == Tile.Type.Void ||
                    targetTile.tier == 3)) {

                    for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                        if (targetTile.AdjacentTiles[i].tier == 3) {
                            return false;
                        }
                    }
                    return true;

                } else return false;
            #endregion

            #region Farm
            case Tile.Type.Farm:
                if (targetTile.type == Tile.Type.Meadow ||
                    targetTile.type == Tile.Type.Grassland) {

                    return true;

                } else return false;
            #endregion

            #region Mine
            case Tile.Type.Mine:
                if (targetTile.type == Tile.Type.Mountain ||
                    targetTile.type == Tile.Type.OreVein) {
                    return true;
                } else return false;
            #endregion

            #region Forge
            case Tile.Type.Forge:
                if (targetTile.type == Tile.Type.Meadow ||
                    targetTile.type == Tile.Type.Grassland ||
                    targetTile.type == Tile.Type.Mountain ||
                    targetTile.type == Tile.Type.OreVein ||
                    targetTile.type == Tile.Type.Forest ||
                    targetTile.type == Tile.Type.Beach) {

                    for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {
                        if (targetTile.AdjacentTiles[i].type == Tile.Type.Mountain ||
                                targetTile.AdjacentTiles[i].type == Tile.Type.OreVein ||
                                targetTile.AdjacentTiles[i].type == Tile.Type.Mine) {
                            return true;
                        }
                    }
                    return false;

                } else return false;
            #endregion

            #region Fisher
                case Tile.Type.Fisher:

                    bool waterlogged = false;
                    bool landed = false;

                    if (targetTile.type == Tile.Type.Meadow ||
                        targetTile.type == Tile.Type.Grassland ||
                        targetTile.type == Tile.Type.River||
                        targetTile.type == Tile.Type.Ocean ||
                        targetTile.type == Tile.Type.Forest ||
                        targetTile.type == Tile.Type.Beach) {

                        for (int i = 0;i < targetTile.AdjacentTiles.Count;i++) {

                            if (!(targetTile.AdjacentTiles[i].type == Tile.Type.Ocean ||
                                 targetTile.AdjacentTiles[i].type == Tile.Type.River)) {
                                landed = true;
                            }

                            if (targetTile.AdjacentTiles[i].type == Tile.Type.Ocean ||
                               targetTile.AdjacentTiles[i].type == Tile.Type.River) {
                                waterlogged = true;
                            } else if (!(targetTile.AdjacentTiles[i].type == Tile.Type.Ocean ||
                                 targetTile.AdjacentTiles[i].type == Tile.Type.River ||
                                 targetTile.AdjacentTiles[i].type == Tile.Type.Void)) {
                                landed = true;
                            }
                        }

                        if (waterlogged && landed) {
                            return true;
                        }

                    }
                    return false;
                #endregion

            #region Trader
                case Tile.Type.Trader:

                    if(targetTile.type == Tile.Type.Ocean ||
                        targetTile.type == Tile.Type.River) {
                        return true;
                    }
                return false;
                #endregion

            }

            return true;
        }return false;
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
                        if (t.AdjacentTiles[i].type == Tile.Type.Grassland) {
                            S++;
                            t.point += 1;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Meadow) {
                            S++;
                            t.point += 2;
                        }
                    }

                    t.point *= S;
                    break;

                case Tile.Type.MarshLand:
                    t.point = 10;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        t.point +=  t.AdjacentTiles[i].point;
                    }

                    t.point = ((t.point * -1)/2);

                    break;

                case Tile.Type.River:
                    t.point = 0;
                    break;

            }
        }
        #endregion

        #region Tier-3

        else if (stage == 3) {

            switch (t.type) {

    #region Village
                case Tile.Type.Village:
                    t.point = 0;

                    t.point += (grid.fishers*7);
                    t.point += (grid.hunters*12);
                    t.point += (grid.farmers*15);

                    Debug.Log(grid.villages);

                    if (grid.villages > 0) {
                        t.point = (t.point / grid.villages);}

                break;
                #endregion

    #region Lumber
                case Tile.Type.Lumber:

                    t.point = 5;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Forest) {
                            t.point += t.AdjacentTiles[i].prevPoints;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.River) {
                            t.point += 8;
                        }
                    }
                    break;
                #endregion

    #region Hunter
                case Tile.Type.Hunter:

                    t.point = 10;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Forest) {
                            t.point += 8;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Meadow) {
                            t.point += 7;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Grassland) {
                            t.point += 4;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Mountain) {
                            t.point += 3;
                        }
                    }

                    t.point = (int)(t.point * 1.5);
                    break;
                #endregion

    #region Farm
                case Tile.Type.Farm:
                    t.point = 8;

                    for(int i = 0;i < t.AdjacentTiles.Count;i++) {

                        if(t.AdjacentTiles[i].type == Tile.Type.Meadow) {
                            t.point += t.AdjacentTiles[i].prevPoints;
                        }else if (t.AdjacentTiles[i].type == Tile.Type.Grassland) {
                            t.point += 4;
                        } else if(t.AdjacentTiles[i].type == Tile.Type.River) {
                            t.point += 8;
                        }
                    }

                    break;
                #endregion

    #region Mine
                case Tile.Type.Mine:

                    t.point = 5;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) { 
                        if(t.AdjacentTiles[i].type == Tile.Type.OreVein) {
                            t.point += t.AdjacentTiles[i].prevPoints;
                        } else if(t.AdjacentTiles[i].type == Tile.Type.Mountain) {
                            t.point += 3;
                        }
                    }

                    break;
                #endregion

    #region Forge
                case Tile.Type.Forge:
                    t.point = 25;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Mine) {
                            t.point += t.AdjacentTiles[i].point/2;
                        }
                    }

                    break;
                #endregion

    #region Fisher
                case Tile.Type.Fisher:
                    t.point = 5;

                    for (int i = 0;i < t.AdjacentTiles.Count;i++) {
                        if (t.AdjacentTiles[i].type == Tile.Type.Ocean) {
                            t.point += 7;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.River) {
                            t.point += 6;
                        } else if (t.AdjacentTiles[i].type == Tile.Type.Beach) {
                            t.point += 5;
                        }
                    }

                    break;
                #endregion

    #region Trader
                case Tile.Type.Trader:
                    t.point = 5;

                    t.point += (grid.hunters + grid.fishers + grid.farmers) * 2;
                    t.point += (grid.lumbers) * 3;
                    t.point += (grid.mines) * 4;
                    t.point += (grid.forges) * 7;

                    if (grid.traders>0) {
                        t.point *= grid.traders;}

                break;
                #endregion

            }
        }
        #endregion


        if (adjRivers > 0) {
        t.point = (t.point * (adjRivers*2)); 
        }

        if(t.tier == 3) {
            for(int i = 0; i < t.AdjacentTiles.Count;i++) {
                if(t.AdjacentTiles[i].type == Tile.Type.Village) {
                    t.point *= 3 ;
                    break;
                }
            }
        }


    }



    public void Description(Tile t) {
        name.text = t.type.ToString();

        switch (t.type) {


            #region Tier-1
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
            #endregion




            #region Tier-2
            case Tile.Type.Meadow:
                details.text = "A patch of rich soil and diverse plantlife, can only be placed on Grassland";
                pointDisplay.text = " +1 for each adjacent Grassland, +2 for each adjacent Meadow. * adjacent Grasslands + Meadows ";
                break;

            case Tile.Type.Forest:
                details.text = "Underwoods animals love to hide in, can only be placed on Grassland";
                pointDisplay.text ="Grants 2 points. +3 for each adjacent Mountain or Orevein, +2 for each adjacent Forest";
                break;

            case Tile.Type.OreVein:
                details.text = "A surfaced vein of rich minerals and metals, can only be placed on Mountains";
                pointDisplay.text ="Grants 3 points. 5* original points of Tile placed on, -4 for each adjacent Orevein";
                break;

            case Tile.Type.River:
                if (grid.rivers > 0) {
                    details.text = "Nourishing streams of water, can be placed on anything except Void and Oceans. Will continue to grant River Tiles until it ends on a Mountain, Orevein, Deleted or adjacent to another Ocean.";
                }else
                    details.text = "Nourishing streams of water, can be placed on anything except Void and Oceans. Must first be placed adjacent to an Ocean. Afterwards adjacent to other Rivers.";
                pointDisplay.text = "Grants a *2 Multiplier to adjacent tiles (stacks), grants a bonus if it ends on a Mountain or Orevein (+50). Dont become too greedy";
                break;
            #endregion




            #region Tier-3
            case Tile.Type.Village:
                details.text = "A small Village busteling with hungry Workforce to boost nearby Tier 3 Tiles, can be placed on any Tile except water and other Tier 3 Tiles";
                pointDisplay.text = "Grants other Tier 3 Tiles a *3 multiplier. Gains +7,+12,+15 points for Fisher,Hunter,Farmer / by number of Villages on the map  ";
                break;

            case Tile.Type.Lumber:
                details.text = "Tile that produces wood for more Villages, can only be placed adjacent to two other Forests and cannot be placed on Water";
                pointDisplay.text = "Gains adjacent Forest Tile points and +8 for each River Tile";
                break;

            case Tile.Type.Hunter:
                details.text = "Tile that gathers food from the wild, cannot be placed adjacent to another Tier 3 Tile.";
                pointDisplay.text = "+3,+4,+7,+8 for each adjacent Mountain, Grassland, Meadow and Forest Tile";
                break;

            case Tile.Type.Farm:
                details.text = "A Farm which utilises the surrounding Tiles as farmland for animals and crops, can only be placed on Grassland and Meadow Tiles";
                pointDisplay.text = "Tile grants 8 base points. Gains adjacent Meadow Tile points and +4,+8 for each adjacent Grassland or River Tile";
                break;

            case Tile.Type.Mine:
                details.text = "A Mine to gather Ore and Minerals from nearby OreVeins and Mountains, can only be placed on a Mountain or Orevein Tile";
                pointDisplay.text = "Gains adjacent Orevein points. +3 for each adjacent Mountain, ";
                break;

            case Tile.Type.Forge:
                details.text = "Melts the Ore from adjacent Mines and transforms it into usefull Items, can only be placed adjacent to a Mountain and not on Water";
                pointDisplay.text = "Grants 25 base Points and gains half the poits of any adjacent Mine";
                break;

            case Tile.Type.Fisher:
                details.text = "Tile which gathers Food from the Rivers and Oceans, must be placed adjacent to River or Ocean Tiles and not on Mountain or Tier 3 Tiles";
                pointDisplay.text = "Grants 10 base Points. +5,+6,+7 for each adjacent Beach, River and Ocean Tile";
                break;

            case Tile.Type.Trader:
                details.text = "Trades Produce for Profits, can only be placed on Ocean or River Tiles";
                pointDisplay.text = "Grants 5 base Points. +2,+3,+4,+7 for each Food Source, Lumber, Mine, Forge. * Traders on the Map";
                break;
                #endregion
        }
    }
}
