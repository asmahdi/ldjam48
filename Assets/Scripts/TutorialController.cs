using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject[] tutorials;
    public AudioSource click;
    private int count;
    private void OnMouseUp()
    {
        
        if (Input.GetMouseButtonUp(0))
        {
            NextTutorial();
            click.Play();
        }
    }

    private void NextTutorial()
    {
        if (count > 6)
        {
            SceneManager.LoadScene("Menu");
            return;
        }
        

        for (int i = 0; i < tutorials.Length; i++)
        {
            tutorials[i].SetActive(false);
            
        }
        tutorials[count].SetActive(true);
        count++;

        
    }
}
