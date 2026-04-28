using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Cutscene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(StartMainMenu());
    }


    private IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene("StartMenu");
    }
}
