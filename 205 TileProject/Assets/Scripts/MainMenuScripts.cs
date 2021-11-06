using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScripts : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject StartButtons;
    public GameObject Title;

    public GameObject Library;
    [SerializeField] TextMeshProUGUI libBtnText;

    public GameObject Options;

    [SerializeField] GameObject[] TitleTiles;

    bool lib = false;

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



    // Start is called before the first frame update
    void Start(){
        FillTitle();
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void NavigatingLibrary() {
        if (!lib) {
            Title.SetActive(false);
            Library.SetActive(true);

            libBtnText.text = "Close Library";
            lib = true;
        } 

        else {
            Library.SetActive(false);
            Title.SetActive(true);
            libBtnText.text = "Open Library";
            lib = false;
        }
    }

    public void FillTitle() {
        for(int i = 0; i < TitleTiles.Length;i++) {
            int r = Random.Range(0,17);

            if(r == 0) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Grassland;} 
            else if(r == 1) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Mountain;} 
            else if (r == 2) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Ocean;} 


            else if (r == 3) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Forest;} 
            else if (r == 4) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Meadow;} 
            else if (r == 5) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = OreVein;} 
            else if (r == 6) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = River;} 
            

            else if (r == 7) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Beach;} 

            else if (r == 8) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Marshland;} 
            
            else if (r == 9) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Village;} 
            else if (r == 10) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Lumber;} 
            else if (r == 11) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Hunter;} 
            else if (r == 12) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Mine;} 
            else if (r == 13) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Forge;} 
            else if (r == 14) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Fisher;} 
            else if (r == 15) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Farm;}
            else if (r == 16) {
                TitleTiles[i].GetComponent<SpriteRenderer>().sprite = Trader;}
        }
    }


}
