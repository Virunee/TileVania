using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float waitTime = 3f;
    [SerializeField] float levelExitSloMoFactor = 0.2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(WaitAndLoadNextLevel());
    }

    IEnumerator WaitAndLoadNextLevel()
    {
        Time.timeScale = levelExitSloMoFactor;
        yield return new WaitForSecondsRealtime(waitTime);
        Time.timeScale = 1f;
        var currentSceneIdx = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIdx + 1);
    }
}
