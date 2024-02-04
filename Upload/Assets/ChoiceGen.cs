using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChoiceGen : MonoBehaviour
{
    public GameObject window;

    public void generate()
    {
        window.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void Simulate()
    {
        SceneManager.LoadScene("Simulation");
    }

}
