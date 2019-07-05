using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UA : MonoBehaviour
{

   

    public string ua_name;
    public int ua_code;
    public int population;
    public string classe_densite;
    public int cout_expro;
    public string fullNameOfUAname;


    private CanvasGroup cg;

    Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        cg = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void showTip()
    {
        Debug.Log("Test is Test");
    }

    void OnMouseDown()
    {
       
        Debug.Log("This on mouse down -> "+ gameObject.name);
    }

    void OnMouseOver()
    {
        GameObject.Find("Tips").GetComponent<RectTransform>().transform.position = worldToUISpace(GameObject.Find("Canvas").GetComponent<Canvas>(), Input.mousePosition);
        SetInfo();
        cg.interactable = true;
        cg.alpha = 1;
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        Vector3 screenPos = worldPos;
        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);

        return parentCanvas.transform.TransformPoint(movePos);
    }

    void OnMouseExit()
    {
        ResetSetInfo();
        cg.interactable = false;
        cg.alpha = 0;
    }

    void SetInfo()
    {
        GameObject.Find("name").GetComponent<Text>().text = "Name: "+ ua_name;
        GameObject.Find("code").GetComponent<Text>().text = "Code: "+ ua_code;
        GameObject.Find("population").GetComponent<Text>().text = "Population: "+ population;
        GameObject.Find("classe_densite").GetComponent<Text>().text = "Classe densité: "+classe_densite;
        GameObject.Find("cout_expro").GetComponent<Text>().text = "Cout expro: "+cout_expro;
        GameObject.Find("full_name").GetComponent<Text>().text = "Full name: "+fullNameOfUAname;
     
    }


    void ResetSetInfo()
    {
        GameObject.Find("name").GetComponent<Text>().text = "Name: " ;
        GameObject.Find("code").GetComponent<Text>().text = "Code: " ;
        GameObject.Find("population").GetComponent<Text>().text = "Population: " ;
        GameObject.Find("classe_densite").GetComponent<Text>().text = "Classe densité: " ;
        GameObject.Find("cout_expro").GetComponent<Text>().text = "Cout expro: " ;
        GameObject.Find("full_name").GetComponent<Text>().text = "Full name: " ;

    }

    public Vector3 GetNewPosition()
    {
        return new Vector3(-2218, -963f, 100f);
    }

}
