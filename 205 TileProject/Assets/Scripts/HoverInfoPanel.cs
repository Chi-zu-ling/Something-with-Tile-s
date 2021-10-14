using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HoverInfoPanel : MonoBehaviour
{
    public GameObject HoverPanel;
    [SerializeField] GameObject Terrain;
    [SerializeField] GameObject Points;

    // Start is called before the first frame update
    void Start(){

    }

    // Update is called once per frame
    void Update(){
 
    }

    public void showInfoTile(Tile t) {

        HoverPanel.SetActive(true);

        HoverPanel.transform.position = 
            new Vector2(
                Camera.main.ScreenToWorldPoint(Input.mousePosition).x+1,
                Camera.main.ScreenToWorldPoint(Input.mousePosition).y-0.4f);

        //HoverPanel.transform.localPosition = new Vector3(HoverPanel.transform.localPosition.x,HoverPanel.transform.localPosition.y,1);

        Debug.Log("hover "+HoverPanel.transform.localPosition.z);

        Terrain.GetComponent<TextMeshProUGUI>().text = t.type.ToString();
        Points.GetComponent<TextMeshProUGUI>().text = t.point.ToString();
       
    }
}
