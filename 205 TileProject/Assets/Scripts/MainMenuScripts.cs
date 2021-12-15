using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;
using System.IO;
using System;

public class MainMenuScripts : MonoBehaviour
{

    public GameObject StartMenu;
    public GameObject StartButtons;
    public GameObject Title;

    public GameObject Library;
    [SerializeField] TextMeshProUGUI libBtnText;

    //public static List<int> Scores = new List<int>();

    [Serializable]
    public class GameData{
        public List<int> SavedScores = new List<int>();}

    GameData gd = new GameData();

    static int Score1 = 0;
    static int Score2 = 0;
    static int Score3 = 0;
    static int Score4 = 0;
    static int Score5 = 0;

    [SerializeField] GameObject ScoreBoard;
    [SerializeField] TextMeshProUGUI score1;
    [SerializeField] TextMeshProUGUI score2;
    [SerializeField] TextMeshProUGUI score3;
    [SerializeField] TextMeshProUGUI score4;
    [SerializeField] TextMeshProUGUI score5;

    public GameObject Options;

    [SerializeField] GameObject[] TitleTiles;

    bool lib = false;
    bool sco = false;

    string SS = "SavedScores";

    #region sprites
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
    #endregion


    // Start is called before the first frame update
    void Start(){

        //SDT = FindObjectOfType<SceneDataTransfer>();

        FillTitle();
        OrderList();
        FillScoreBoard();
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void NavigatingLibrary() {

        if (sco) {
            ScoreBoard.SetActive(false);
            sco = false;
        }

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

    public void OpenScoreBoard() {
        //PlayerPrefs.SetInt("PlayerScore",points);

        if (lib) {
            Library.SetActive(false);
            lib = false;
        }

        if (!sco) {
            Title.SetActive(false);
            ScoreBoard.SetActive(true);

            sco = true;

            //score1.text = Scores[0].ToString();
        } 
        
        else {
            Title.SetActive(true);
            ScoreBoard.SetActive(false);

            sco = false;
        }
    }

    public void Quit() {
        Application.Quit();}


    public void FillScoreBoard() {
        Debug.Log("filling Scores");


        score1.text = gd.SavedScores[0].ToString();
        score2.text = gd.SavedScores[1].ToString();
        score3.text = gd.SavedScores[2].ToString();
        score4.text = gd.SavedScores[3].ToString();
        score5.text = gd.SavedScores[4].ToString();

    }


    void OrderList() {

        gd = Load(SS);

        if(gd != null) { 
        }
        else {
            Debug.Log("Load was Null");
            gd = new GameData();
            gd.SavedScores = new List<int>();}

        if(Manager.points > 0) {
            gd.SavedScores.Add(Manager.points);}

        Manager.points = 0;

        while (gd.SavedScores.Count < 5) {
            gd.SavedScores.Add(0);
        }

        //gd.SavedScores.Sort();
        gd.SavedScores = gd.SavedScores.OrderByDescending(x => x).ToList();

        while(gd.SavedScores.Count > 5) {
            gd.SavedScores.RemoveAt(gd.SavedScores.Count-1);
        }

        Save(gd, SS);

    }


    public void FillTitle() {
        for(int i = 0; i < TitleTiles.Length;i++) {
            int r = UnityEngine.Random.Range(0,17);
            // adding UnityEngine. infront of Random because both Unityengine and System use a Random, system is confused

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



    public static void Save(GameData SaveScores, string _fileName) {
        string filePath = Path.Combine(Application.persistentDataPath, _fileName);
        Debug.Log(Path.Combine(Application.persistentDataPath, _fileName));

        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        string savedjson = JsonUtility.ToJson(SaveScores);
        Debug.Log("savedjson" + savedjson);
        
        File.WriteAllText(filePath ,savedjson);
    }
    
    public static GameData Load(string _fileName) {
        if (_fileName != null) {
            string filePath = Path.Combine(Application.persistentDataPath, _fileName);

            if (File.Exists(filePath)) {
                return JsonUtility.FromJson<GameData>(File.ReadAllText(filePath));
            }
        }
        return null;
    }

}
