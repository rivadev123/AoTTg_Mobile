using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using ControlFreak2;

public class Settings : MonoBehaviour
{
    public float MoveSpeed;

    public InputField xSens;
    public InputField ySens;

    public RectTransform SelectionUI;

    public RectTransform[] UI;

    public RectTransform JoystickMove;
    public RectTransform Jump;

    public RectTransform Attack;
 
    public RectTransform HookR;
    public RectTransform HookL;
    public RectTransform DoubleHook;
    public RectTransform Special;
    public RectTransform Reload;

    public bool Load;
    public bool Save;

    public bool Menu;

    public RectTransform currentObject;
    private string HUDLayoutString;


    /// <summary>
    /// ///////////
    /// </summary>
    public CanvasScaler InGameCanvas;
    public GameObject UICF2;
    public InitialPosPlaceholder[] r;
    // Start is called before the first frame update
    private void Awake()
    {

        if (Menu) //If menu
        {
            if (PlayerPrefs.GetString("DefaultHUD") == "")
            {
                for (int i = 0; i < UI.Length; i++)
                {
                    HUDLayoutString += UI[i].anchoredPosition.x;
                    HUDLayoutString += ",";

                    HUDLayoutString += UI[i].anchoredPosition.y;
                    HUDLayoutString += ",";

                    HUDLayoutString += UI[i].localScale.x;
                    HUDLayoutString += ",";
                }

                PlayerPrefs.SetString("DefaultHUD", HUDLayoutString);
                PlayerPrefs.SetString("HUDLayout", HUDLayoutString);

            }

        }
        else //if not
        {
           // ChangeCanvasScale();
        }
        LoadHUD();
        Invoke("CallHUDINI", 0.03f);
    }
    private void Start()
    {
    
            if (PlayerPrefs.GetFloat("SensX",0f) == 0f)
            {
                PlayerPrefs.SetFloat("SensX", 2.0f);
            }

            if (PlayerPrefs.GetFloat("SensY",0f) == 0f)
            {
                PlayerPrefs.SetFloat("SensY", 2.0f);
            }

    }

    void ChangeCanvasScale()
    {
        InGameCanvas.referenceResolution = new Vector2(Screen.width, Screen.height);
    }
    private void CallHUDINI()
    {
        if (!Menu)
        {
            Debug.Log(FindObjectOfType<TouchButton>().GetWorldPos());


            r = UICF2.GetComponentsInChildren<InitialPosPlaceholder>();
           

                foreach (InitialPosPlaceholder r2 in r)
                {
                    if (r2.gameObject.name.Contains("Jump"))
                    {
                        r2.transform.position = Jump.transform.position;
                    }
                    if (r2.gameObject.name.Contains("Attack"))
                    {
                        r2.transform.position = Attack.transform.position;
                    }
                    if (r2.gameObject.name.Contains("HookR"))
                    {
                        r2.transform.position = HookR.transform.position;
                    }
                    if (r2.gameObject.name.Contains("HookL"))
                    {
                        r2.transform.position = HookL.transform.position;
                    }
                    if (r2.gameObject.name.Contains("DoubleHook"))
                    {
                        r2.transform.position = DoubleHook.transform.position;
                    }
                    if (r2.gameObject.name.Contains("Special"))
                    {
                        r2.transform.position = Special.transform.position;
                    }
                    if (r2.gameObject.name.Contains("Reload"))
                    {
                        r2.transform.position = Reload.transform.position;
                    }
                
            }
        }
    }
    public void LoadSens()
    {
        xSens.text = PlayerPrefs.GetFloat("SensX").ToString();
        ySens.text = PlayerPrefs.GetFloat("SensY").ToString();
    }
    public void ChangeSens()
    {
        

        float px = 0;
        float py = 0;
 

        float.TryParse(xSens.text, out px);
        float.TryParse(ySens.text, out py);
      

        PlayerPrefs.SetFloat("SensX",px);
        PlayerPrefs.SetFloat("SensY", py);

    }
    void Update()
    {

        //Move
   

        Vector2 input;
        input.x = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
        input.y = ControlFreak2.CF2Input.GetAxisRaw("Vertical");


        if(currentObject != null)
        {
            currentObject.anchoredPosition += input * MoveSpeed;
            SelectionUI.transform.position = currentObject.transform.position;

            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.R))
            {
                Vector3 v = new Vector3(0.2f, 0.2f, 0.2f);
                currentObject.localScale += v;
            }

            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.E))
            {
                Vector3 v = new Vector3(0.2f, 0.2f, 0.2f);
                currentObject.localScale -= v;
            }
        }

    }

    // Update is called once per frame
    public void SaveHUD()
    {
        HUDLayoutString = "";
        //J = joystick  A attack HL hookL

        for (int i = 0; i < UI.Length; i++)
        {
            Vector3 pos = UI[i].anchoredPosition;
       

            HUDLayoutString += pos.x;
            HUDLayoutString += ",";

            HUDLayoutString += pos.y;
            HUDLayoutString += ",";

            HUDLayoutString += UI[i].localScale.x;
            HUDLayoutString += ",";
        }

        PlayerPrefs.SetString("HUDLayout",HUDLayoutString);
    }
    public void LoadHUD()
    {
        HUDLayoutString = PlayerPrefs.GetString("HUDLayout");

        for (int i = 0; i < UI.Length; i++)
        {
            float px = 0;
            float py = 0;
            float s = 0;

            float.TryParse(HUDLayoutString.Split(',')[0 + (i * 3)], out px);
            float.TryParse(HUDLayoutString.Split(',')[1 + (i * 3)], out py);
            float.TryParse(HUDLayoutString.Split(',')[2 + (i * 3)], out s);

            //  if (UI[i].GetComponent<TouchButton>())
            //  {
            //      UI[i].GetComponent<TouchButton>().SetWorldPos(new Vector2(px, py));
            //  }


            UI[i].anchoredPosition = new Vector2(px, py);

            UI[i].localScale = new Vector2(s, s);
        }
    }
    public void ResetHud()
    {
        HUDLayoutString = PlayerPrefs.GetString("DefaultHUD");

        for (int i = 0; i < UI.Length; i++)
        {
            float px = 0;
            float py = 0;
            float s = 0;

            float.TryParse(HUDLayoutString.Split(',')[0 + (i * 3)], out px);
            float.TryParse(HUDLayoutString.Split(',')[1 + (i * 3)], out py);
            float.TryParse(HUDLayoutString.Split(',')[2 + (i * 3)], out s);

            UI[i].anchoredPosition = new Vector2(px, py);
            UI[i].localScale = new Vector2(s, s);
        }
    }



    public void PressedJoystick()
    {
        currentObject = JoystickMove;
    }
    public void PressedReload()
    {
        currentObject = Reload;
    }
    public void PressedHookL()
    {
        currentObject = HookL;
    }
    public void PressedHookR()
    {
        currentObject = HookR;
    }
    public void PressedHookDouble()
    {
        currentObject = DoubleHook;
    }
    public void PressedAttack()
    {
        currentObject = Attack;
    }
    public void PressedSpecial()
    {
        currentObject = Special;
    }
    public void PressedJump()
    {
        currentObject = Jump;
    }
}
