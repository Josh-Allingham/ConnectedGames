using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Cutscene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartMainMenu());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(30);
        SceneManager.LoadScene("StartMenu");
    }
}
