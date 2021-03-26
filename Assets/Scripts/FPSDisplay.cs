using UnityEngine;
using System.Collections;
 
public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    public bool ShowFPS;

    void Awake()
    {  
        //Application.targetFrameRate = 1000;  
    }

    void Update()
    {
        if (ShowFPS == true)
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ShowFPS == true)
            {
                ShowFPS = false;
            }else{
                ShowFPS = true;
            }
        }
    }
 
    void OnGUI()
    {    
        if (ShowFPS == true)
        {
            int w = Screen.width, h = Screen.height;
            GUIStyle style = new GUIStyle();
            
            Rect rect = new Rect(0, 0, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
            
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

            if (fps < 30)
            {
                Debug.LogWarning("Fall fps!!! " + fps);
            }

            GUI.Label(rect, text, style);       
        }   
    }
}