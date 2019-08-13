using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManu : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
   }

   public void FirstLevel()
   {
        SceneManager.LoadScene(1);
   }


   public void SecondLevel()
   {
        SceneManager.LoadScene(2); 
   }


   public void ExitGame()
   {
        Debug.Log("Quit the game...");
        Application.Quit();
   }
}
