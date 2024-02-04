using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Globalization;
public class SetupScript1D : MonoBehaviour
{
    public GameObject loadingScreen;
    public TMP_Dropdown dropdown;
    public GameObject[] listOfChoices;
    public TMP_InputField[] Inputs;

    void Start()
    {
        setUpListner();
    
    }

    private void setUpListner()
    {
        dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(dropdown);
        });
        
    }

    private void DropdownValueChanged(TMP_Dropdown change)
    {
        foreach(GameObject choice in listOfChoices)
        {
            choice.SetActive(false);
        }
        string selectedOption = change.options[change.value].text;

        if (selectedOption == "Dambreak")
        {
            Debug.Log("Dambreak");
            listOfChoices[0].SetActive(true);
            listOfChoices[1].SetActive(true);
            listOfChoices[2].SetActive(true);
        }
        else if (selectedOption == "Rare Rare")
        {
            Debug.Log("Rare Rare");
            listOfChoices[0].SetActive(true);
            listOfChoices[2].SetActive(true);
            listOfChoices[3].SetActive(true);
        }
        else if (selectedOption == "Shock Shock")
        {
            Debug.Log("Shock Shock");
            listOfChoices[0].SetActive(true);
            listOfChoices[2].SetActive(true);
            listOfChoices[3].SetActive(true);
        }
        else if (selectedOption == "Subcritical")
        {
            Debug.Log("Subcritical");
        }
        else if(selectedOption == "Supercritical")
        {
            Debug.Log("Supercritical");
        }
        else if(selectedOption == "Tsunami Event")
        {
            Debug.Log("Tsunami Event");
        }
    }

    public void startSimulation()
    {
        foreach(GameObject choice in listOfChoices)
        {
            if(choice.activeSelf){
                TMP_InputField field = choice.GetComponentInChildren<TMP_InputField>();
                if(field.text.Length < 0)
                {
                    Debug.Log("Please fill all the fields");
                    return;
                }
            }
            
        }
        moveAndStart();
    }

    private void moveAndStart(){
        string selectedOption = dropdown.options[dropdown.value].text;
        IWavePropagation propagation1D = null;
        propagation1D= new WavePropagation1d(uint.Parse(Inputs[0].text),false);
        ISetup setup = null;
        float location,hl,hr,hu;

        if (selectedOption == "Dambreak")
        {
            location = float.Parse(listOfChoices[0].GetComponentInChildren<TMP_InputField>().text);
            hr = float.Parse(listOfChoices[1].GetComponentInChildren<TMP_InputField>().text);
            hl = float.Parse(listOfChoices[2].GetComponentInChildren<TMP_InputField>().text);
            setup = new DamBreak1d(hl,hr,location);
        }
        else if (selectedOption == "Rare Rare")
        {
            location = float.Parse(listOfChoices[0].GetComponentInChildren<TMP_InputField>().text);
            hu = float.Parse(listOfChoices[3].GetComponentInChildren<TMP_InputField>().text);
            hl = float.Parse(listOfChoices[2].GetComponentInChildren<TMP_InputField>().text);
            setup = new RareRare(hl,hu,location);
        }
        else if (selectedOption == "Shock Shock")
        {
            location = float.Parse(listOfChoices[0].GetComponentInChildren<TMP_InputField>().text);
            hu = float.Parse(listOfChoices[3].GetComponentInChildren<TMP_InputField>().text);
            hl = float.Parse(listOfChoices[2].GetComponentInChildren<TMP_InputField>().text);
            setup = new ShockShock(hl,hu,location);
        }
        else if (selectedOption == "Subcritical")
        {
            setup = new SubcriticalFlow();
        }
        else if(selectedOption == "Supercritical")
        {
            setup = new SupercriticalFlow();
        }
        else if(selectedOption == "Tsunami Event")
        {
            location = float.Parse(listOfChoices[0].GetComponentInChildren<TMP_InputField>().text);
            hr = float.Parse(listOfChoices[1].GetComponentInChildren<TMP_InputField>().text);
            hl = float.Parse(listOfChoices[2].GetComponentInChildren<TMP_InputField>().text);
            setup = new TsunamiEvent1d();
        }
        uint x = uint.Parse(Inputs[0].text);
        float dx = float.Parse(Inputs[1].text);
        float l_dxy = dx/  x ;
        float hMax = float.MinValue;
        float endTime = float.Parse(Inputs[2].text);
        for( uint l_cy = 0; l_cy < 1; l_cy++ ){
            for( uint l_cx = 0; l_cx < x; l_cx++ ){
                float l_y = l_cy * l_dxy ;
                float l_x = l_cx * l_dxy ;
                float l_h = setup.GetHeight( l_x,l_y );
                hMax = Math.Max( l_h, hMax);
                float l_hu = setup.GetMomentumX( l_x,l_y );
                float l_hv = setup.GetMomentumY( l_x,l_y );
                float l_bv = setup.GetBathymetry(l_x,l_y );
                propagation1D.setHeight( l_cx,
                              l_cy,
                              l_h );
                propagation1D.setMomentumX( l_cx,
                                        l_cy,
                                        l_hu );
                propagation1D.setBathymetry( l_cx,
                                        l_cy,
                                        l_bv);
            }
        }
        float l_speedMax = (float)Math.Sqrt(9.81 * hMax);
         

        float l_dt = 0.50f * l_dxy / l_speedMax;
        string path = "Assets/OutputData";
        if(Directory.Exists(path)){
            Directory.Delete(path,true);
        }
        Directory.CreateDirectory(path);
        WriteMe(propagation1D,endTime,l_dt,l_dxy,0,x,1);
        Transfer.WV = "1d";

        SceneManager.LoadScene("Simulation");
    }
    
    public void WriteMe(IWavePropagation propagation1D,float endTime, float dt,float dxy,float domain_start,uint nx,uint ny){
        
        float simulationTime = 0;
        uint step = 0,timeStep = 0;
        float scalling = dt / dxy;
        String path;
        while(simulationTime < endTime){
            if(step % 25 == 0){
                path = "Assets/OutputData/soulation_"+timeStep+".csv";
                using StreamWriter l_file = new StreamWriter(path);
                IO.Write1d(dxy, nx, ny, propagation1D.GetStride(), domain_start, propagation1D.getHeightValues(), propagation1D.getMomentumXValues(), propagation1D.getBathymetryValues(), l_file);
                timeStep++;
            }
            propagation1D.timeStep(scalling);
            step++;
            simulationTime += dt;
        }
    }


}



