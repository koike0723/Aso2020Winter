using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSControll : MonoBehaviour
{

    [SerializeField]
    int frame_par_second = 30;

    [SerializeField]
    Text text;
    
    // 変数
    int frameCount;
    float prevTime;
    float fps;


    private void Awake()
    {
        Application.targetFrameRate = frame_par_second;
    }

    // Start is called before the first frame update
    void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        DispFPSLog();
    }

    private void DispFPSLog()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;
        if(text != null)
        {
            if (time >= 0.5f)
            {
                fps = frameCount / time;
                text.text = "fps: " + fps.ToString("f0");
                //Debug.Log(fps);

                frameCount = 0;
                prevTime = Time.realtimeSinceStartup;
            }
        }
    }
}
