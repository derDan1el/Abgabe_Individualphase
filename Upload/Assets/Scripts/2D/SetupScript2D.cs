using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetupScript2D : MonoBehaviour
{
    public GameObject loadingScreen;
    public TMP_Dropdown dropdown;
    public GameObject[] listOfChoices;
    public TMP_InputField[] Inputs;
    public Toggle toggle;
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

        string selectedOption = change.options[change.value].text;

        if (selectedOption == "Dambreak")
        {
            Debug.Log("Dambreak");
        }
      
    }

    public void startSimulation()
    {
        foreach (GameObject choice in listOfChoices)
        {
            if (choice.activeSelf)
            {
                TMP_InputField field = choice.GetComponentInChildren<TMP_InputField>();
                if (field.text.Length < 0)
                {
                    Debug.Log("Please fill all the fields");
                    return;
                }
            }

        }
        moveAndStart();
    }

    private void moveAndStart()
    {
        IWavePropagation propagation2D = new WavePropagation2d(uint.Parse(Inputs[0].text), uint.Parse(Inputs[1].text), toggle.isOn);
        ISetup setup = new DamBreak2D();
        uint x = uint.Parse(Inputs[0].text);
        uint y = uint.Parse(Inputs[1].text);
        float dx = float.Parse(Inputs[2].text);
        float l_dxy = dx / x;
        float hMax = float.MinValue;
        float endTime = float.Parse(Inputs[3].text);
        for (uint l_cy = 0; l_cy < y; l_cy++)
        {
            for (uint l_cx = 0; l_cx < x; l_cx++)
            {
                float l_y = l_cy * l_dxy + float.Parse(Inputs[5].text);
                float l_x = l_cx * l_dxy + float.Parse(Inputs[4].text);
                float l_h = setup.GetHeight(l_x, l_y);
                hMax = Math.Max(l_h, hMax);
                float l_hu = setup.GetMomentumX(l_x, l_y);
                float l_hv = setup.GetMomentumY(l_x, l_y);
                float l_bv = setup.GetBathymetry(l_x, l_y);
                propagation2D.setHeight(l_cx,
                              l_cy,
                              l_h);
                propagation2D.setMomentumX(l_cx,
                                        l_cy,
                                        l_hu);
                propagation2D.setMomentumY(l_cx,
                        l_cy,
                        l_hv);
                propagation2D.setBathymetry(l_cx,
                                        l_cy,
                                        l_bv);
            }
        }
        float l_speedMax = (float)Math.Sqrt(9.81 * hMax);
        float l_dt = 0.50f * l_dxy / l_speedMax;
        string path = "Assets/OutputData";
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Directory.CreateDirectory(path);
        Debug.Log(endTime);
        WriteMe(propagation2D, endTime, l_dt, l_dxy, float.Parse(Inputs[4].text), float.Parse(Inputs[5].text), x,y);
        Transfer.WV = "2d";
        Transfer.x = x;
        Transfer.y = y;
        SceneManager.LoadScene("Simulation");
    }

    public void WriteMe(IWavePropagation propagation2D, float endTime, float dt, float dxy, float domain_start_x,float domainstart_y, uint nx, uint ny)
    {

        float simulationTime = 0;
        uint step = 0, timeStep = 0;
        float scalling = dt / dxy;
        String path;
        while (simulationTime < endTime)
        {
            if (step % 25 == 0)
            {
                path = "Assets/OutputData/soulation_" + timeStep + ".csv";
                using StreamWriter l_file = new StreamWriter(path);
                IO.Write2d(dxy, nx, ny, propagation2D.GetStride(), domain_start_x,domainstart_y, propagation2D.getHeightValues(), propagation2D.getMomentumXValues(), propagation2D.getMomentumYValues(), propagation2D.getBathymetryValues(), l_file);
                timeStep++;
            }
            propagation2D.timeStep(scalling);
            step++;
            simulationTime += dt;
        }
    }
}
