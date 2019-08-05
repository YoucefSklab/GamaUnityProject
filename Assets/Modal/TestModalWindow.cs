using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class TestModalWindow : MonoBehaviour
  {
	private ModalPanel ModalPanel;           //reference to the ModalPanel Class
	private DisplayManager DisplayManager;   //reference to the DisplayManager Class

	public Sprite ErrorIcon;                 //Your error icon
	public Sprite InformationIcon;           //Your information icon
	public Sprite ProgramIcon;               //Your Company Logo or Program Icon
	public Sprite WarningIcon;               //Your warning icon
	public Sprite QuestionIcon;              //Your question icon
    public GameObject UIPanel;
    public GameObject ModalWindow;

    void Awake()
	  {
		ModalPanel = ModalPanel.Instance();         //Instantiate the panel
		DisplayManager = DisplayManager.Instance(); //Instantiate the Display Manager
	  }

	//Test function:  Pop up the Modal Window with Yes, No, and Cancel buttons.
	public void TestYNC(TestModalWindow instance)
	  {
		Sprite icon = null;
        UIPanel.SetActive(true);
        //ModalWindow.SetActive(true);
        ModalPanel.MessageBox(icon, "Test 11 Yes No Cancel", "Would you like a poke in the eye?\nHow about with a sharp stick?", instance.TestYesFunction, instance.TestNoFunction, instance.TestCancelFunction, instance.TestOkFunction, false, "YesNoCancel");

    }
    
    //Test function:  Pop up the Modal Window with Yes, No, and Cancel buttons and an Icon.
    public void TestYNCI()
	  {
        string test = " test message";
		Sprite icon = ProgramIcon;
        UIPanel.SetActive(true);
        ModalPanel.MessageBox(icon, "Test 22 Yes No Cancel Icon", "Do you like this icon?", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, true, "YesNoCancel");

        GameObject.Find("Button #1").GetComponent<ResponseModalWindow>().onYes = () =>
                                        {
                                            Debug.Log("Handling Yes response... "+test);
                                        };

        GameObject.Find("Button #2").GetComponent<ResponseModalWindow>().onNo = () =>
        {
            Debug.Log("Handling No response...");
        };
        GameObject.Find("Button #3").GetComponent<ResponseModalWindow>().onCancel = () =>
            {
                Debug.Log("Handling cancel option.");
                // For example you can choose to make the message dissapear

            };
       }
    //Test function:  Pop up the Modal Window with Yes and No buttons.
    public void TestYN()
	 {
		Sprite icon = null;
        UIPanel.SetActive(true);
        ModalPanel.MessageBox(icon, "Test Yes No", "Answer 'Yes' or 'No':", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "YesNo");
	  }
	//Test function:  Pop up the Modal Window with an Ok button.
	public void TestOk()
	  {
		Sprite icon = null;
        UIPanel.SetActive(true);
        ModalPanel.MessageBox(icon, "Test Ok", "Please hit ok.", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, false, "Ok");
	  }
	//Test function:  Pop up the Modal Window with an Ok button and an Icon.
	public void TestOkIcon()
	  {
		Sprite icon = InformationIcon;
        UIPanel.SetActive(true);
        ModalPanel.MessageBox(icon, "Test OK Icon", "Press Ok.", TestYesFunction, TestNoFunction, TestCancelFunction, TestOkFunction, true, "Ok");
	  }
	//Test function:  Do something if the "Yes" button is clicked.
	void TestYesFunction()
	  {
        UIPanel.SetActive(true);
        DisplayManager.DisplayMessage("Heck yeah! Yup!");
	  }
	//Test function:  Do something if the "No" button is clicked.
	void TestNoFunction()
	  {
        UIPanel.SetActive(true);
        DisplayManager.DisplayMessage("No way, José!");
	  }
	//Test function:  Do something if the "Cancel" button is clicked.
	void TestCancelFunction()
	  {
        UIPanel.SetActive(true);
        DisplayManager.DisplayMessage("I give up!");
	  }
	//Test function:  Do something if the "Ok" button is clicked.
	void TestOkFunction()
	  {
        UIPanel.SetActive(true);
        DisplayManager.DisplayMessage("Ok!");
	  }
  }