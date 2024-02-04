using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using static UnityEngine.Analytics.IAnalytic;
using System.Security.Claims;
using System;
using UnityEngine.UI;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    private bool is1D = false;
    public GameObject b;
    public GameObject he;
    public GameObject mxe;
    public GameObject hve;
    public GameObject result;
    public GameObject objectToDraw;
    public GameObject startButton;
    private int lengthOfDirFile= 0, lineView = 0;
    private List<GameObject> objects_b = new List<GameObject>();
    private List<GameObject> objects_h = new List<GameObject>();
    private List<GameObject> objects_mx = new List<GameObject>();
    private List<GameObject> objects_hv = new List<GameObject>();
    private List<GameObject> objects_result = new List<GameObject>();
    private List<float> xA;
    private List<float> yA;

    public Sprite start;
    public Sprite pause;

    public Slider slider;
    public TMP_Text text;

    private bool isStarted = false;

    public void startPlaying()
    {
        isStarted = !isStarted;
        if (isStarted)
        {
            startButton.GetComponent<Image>().sprite = pause;
            StartCoroutine(startCaller());
        }
        else
        {
            startButton.GetComponent<Image>().sprite = start;
        }
    }
    IEnumerator startCaller()
    {
        while (isStarted)
        {
            nextFrame();
            yield return new WaitForSeconds(3f);
        }
    }

    void Start(){
        lengthOfDirFile = CountFilesInFolder("Assets/OutputData");
        if(Transfer.WV == "1d") is1D = true;
        if(is1D) slider.gameObject.SetActive(false);
        text.text = "5.2";
        setupFrame();
    }
    public void nextFrame()
    {
        if(lineView != lengthOfDirFile-1)
        {
            lineView++;
            if(is1D)
            {
                drawFram1D(lineView);
            }
            else
            {
                drawFram2D(lineView);
            }
                
        }
    }
    public void OnChanged()
    {
        String val = slider.value.ToString();
        text.text =  val.Substring(0,4);
        drawFram2D(lineView);
    }
    public void backFrame()
    {
        if (lineView != 0)
        {
            lineView--;
            if (is1D)
            {
                drawFram1D(lineView);
            }
            else
            {
                drawFram2D(lineView);
            }

        }
    }
    private void drawFram2D(int frame)
    {
        List<float> bat, hu, h, hv;
        string path = "Assets/OutputData/soulation_" + frame + ".csv";
        bat = IO.Read(path, 5);
        //hu = IO.Read(path, 3);
        h = IO.Read(path, 2);
        //hv = IO.Read(path, 4);
        objects_result.Clear();
        CleanChildren(result.transform);
        for (int l_iy = 0; l_iy < yA.Count; l_iy++)
        {
            /*
            GameObject object_b = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], bat[l_iy]), Quaternion.identity, b.transform);
            object_b.GetComponent<Renderer>().material.color = Color.white;
            objects_b[i].Add(object_b);

            GameObject object_h = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], h[l_iy]), Quaternion.identity, he.transform);
            object_h.GetComponent<Renderer>().material.color = Color.red;
            objects_h[i].Add(object_h);

            GameObject object_hu = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], hu[l_iy]), Quaternion.identity, mxe.transform);
            object_hu.GetComponent<Renderer>().material.color = Color.yellow;
            objects_mx[i].Add(object_hu);
            */
            Color color;
            float result_data = h[l_iy] + bat[l_iy];
            if(result_data > slider.value)
            {
                color = Color.red;
            }
            else
            {
                color = Color.blue;
            }
            GameObject object_hv = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], result_data), Quaternion.identity, result.transform);
            object_hv.GetComponent<Renderer>().material.color = color;
            objects_result.Add(object_hv);
        }
    }
    private void setupFrame()
    {
        xA = IO.Read("Assets/OutputData/soulation_0.csv", 0);
        yA = IO.Read("Assets/OutputData/soulation_0.csv", 1);
        if (is1D)
        {
            drawFram1D(0);
        }
        else
        {
            drawFram2D(0);
        }
    }
    private void drawFram1D(int frame)
    {
        List<float> bat, mx, h;
        string path = "Assets/OutputData/soulation_" + frame + ".csv";        
        bat = IO.Read(path, 4);
        mx = IO.Read(path, 3);
        h = IO.Read(path, 2);
        
        objects_b.Clear();
        objects_h.Clear();
        objects_mx.Clear();
        CleanChildren(he.transform);
        CleanChildren(mxe.transform);
        CleanChildren(b.transform);
        for (int l_ix = 0; l_ix < xA.Count; l_ix++)
        {

            GameObject object_b = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_ix], bat[l_ix]), Quaternion.identity, b.transform);
            object_b.GetComponent<Renderer>().material.color = Color.black;
            objects_b.Add(object_b);

            GameObject object_h = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_ix], h[l_ix]), Quaternion.identity, he.transform);
            object_h.GetComponent<Renderer>().material.color = Color.red;
            objects_h.Add(object_h);

            GameObject object_hu = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_ix], mx[l_ix]), Quaternion.identity, mxe.transform);
            object_hu.GetComponent<Renderer>().material.color = Color.blue;
            objects_mx.Add(object_hu);
        }
    }
    public void CleanChildren(Transform mTargetObject)
    {
        int nbChildren = mTargetObject.childCount;

        for (int i = nbChildren - 1; i >= 0; i--)
        {
            DestroyImmediate(mTargetObject.GetChild(i).gameObject);
        }
    }
    /*
    private void loadData2D() {
        int len = CountFilesInFolder("Assets/OutputData");
        string path = "Assets/OutputData/";
        List<float> xA = IO.Read(path + "soulation_0.csv", 0);
        List<float> yA = IO.Read(path + "soulation_0.csv", 1);
        List<float> bat, hu, h,hv;
        int c;
        for (int i = 0; i < len; i++)
        {
            objects_b.Add(new List<GameObject>());
            objects_h.Add(new List<GameObject>());
            objects_hv.Add(new List<GameObject>()); 
            objects_mx.Add(new List<GameObject>());
            path = "Assets/OutputData/soulation_" + i + ".csv";
            Debug.Log(path);
            bat = IO.Read(path, 5);
            hu = IO.Read(path, 3);
            h = IO.Read(path, 2);
            hv = IO.Read(path, 4);

            for (int l_iy = 0; l_iy < yA.Count; l_iy++)
            {
                /*
                GameObject object_b = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], bat[l_iy]), Quaternion.identity, b.transform);
                object_b.GetComponent<Renderer>().material.color = Color.white;
                objects_b[i].Add(object_b);

                GameObject object_h = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], h[l_iy]), Quaternion.identity, he.transform);
                object_h.GetComponent<Renderer>().material.color = Color.red;
                objects_h[i].Add(object_h);

                GameObject object_hu = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], hu[l_iy]), Quaternion.identity, mxe.transform);
                object_hu.GetComponent<Renderer>().material.color = Color.yellow;
                objects_mx[i].Add(object_hu);
                
                GameObject object_hv = Instantiate(objectToDraw, new Vector3(xA[l_iy], yA[l_iy], h[l_iy] + bat[l_iy]), Quaternion.identity, hve.transform);
                object_hv.GetComponent<Renderer>().material.color = Color.blue;
                objects_hv[i].Add(object_hv);
            }

        }
    }
    private void loadData(){
        int len = CountFilesInFolder("Assets/OutputData");
        string path = "Assets/OutputData/";
        List<float> xA = IO.Read(path+"soulation_0.csv",0);
        List<float> yA = IO.Read(path + "soulation_0.csv", 1);
        List<float> bat,mx,h;
        int c;
        for (int i = 0; i < len; i++)
        {
            objects_b.Add(new List<GameObject>());
            objects_h.Add(new List<GameObject>());
            objects_mx.Add(new List<GameObject>());
            path = "Assets/OutputData/soulation_"+i+".csv";
            Debug.Log(path);
            bat = IO.Read(path, 4);
            mx = IO.Read(path,3);
            h = IO.Read(path,2);
            c = 0;
            for (int l_iy = 0; l_iy < 1; l_iy++)
            {
                for (int l_ix = 0; l_ix < xA.Count; l_ix++)
                {
                    
                    GameObject object_b = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_iy], bat[c]), Quaternion.identity, b.transform);
                    object_b.GetComponent<Renderer>().material.color = Color.white;
                    objects_b[i].Add(object_b);

                    GameObject object_h = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_iy], h[c]), Quaternion.identity, he.transform);
                    object_h.GetComponent<Renderer>().material.color = Color.red;
                    objects_h[i].Add(object_h);

                    GameObject object_hu = Instantiate(objectToDraw, new Vector3(xA[l_ix], yA[l_iy], mx[c]), Quaternion.identity, mxe.transform);
                    object_hu.GetComponent<Renderer>().material.color = Color.blue;
                    objects_mx[i].Add(object_hu);

                    c++;
                }
            }
            
        }
    }
   */
    int CountFilesInFolder(string path)
    {
        if (Directory.Exists(path))
        {
            string[] files = Directory.GetFiles(path);
            return files.Length;
        }
        else
        {
            Debug.LogError("Directory not found: " + path);
            return 0;
        }
    }
    /*
    public void next(){
        if (lineView == objects_b.Count - 1)
        {
            return;
        }

        if (Transfer.WV == "1d")
        {
            for (int i = 0; i < objects_b[lineView].Count; i++)
            {
                objects_b[lineView][i].SetActive(false);
                objects_h[lineView][i].SetActive(false);
                objects_mx[lineView][i].SetActive(false);
            }
            lineView++;
            for (int i = 0; i < objects_b[lineView].Count; i++)
            {
                objects_b[lineView][i].SetActive(true);
                objects_h[lineView][i].SetActive(true);
                objects_mx[lineView][i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < objects_hv[lineView].Count; i++)
            {
                //objects_b[lineView][i].SetActive(false);
                //objects_h[lineView][i].SetActive(false);
                //objects_mx[lineView][i].SetActive(false);
                objects_hv[lineView][i].SetActive(false);

            }
            lineView++;
            for (int i = 0; i < objects_hv[lineView].Count; i++)
            {
                //objects_b[lineView][i].SetActive(true);
                //objects_h[lineView][i].SetActive(true);
                //objects_mx[lineView][i].SetActive(true);
                objects_hv[lineView][i].SetActive(true);

            }
        }



        

    }
    public void back(){
        if(lineView == 0){
            return;
        }
        for(int i = 0; i < objects_b[lineView].Count; i++){
            objects_b[lineView][i].SetActive(false);
            objects_h[lineView][i].SetActive(false);
            objects_mx[lineView][i].SetActive(false);
        }
        lineView--;
        for(int i = 0; i < objects_b[lineView].Count; i++){
            objects_b[lineView][i].SetActive(true);
            objects_h[lineView][i].SetActive(true);
            objects_mx[lineView][i].SetActive(true);
        }
    }
    */
}
