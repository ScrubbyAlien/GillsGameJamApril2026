using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
        worldState.OnEnable();
    }

    public void LoadSceneDelayed(string name, float delay)
    {
        StartCoroutine(DelayedLoadScene(name, delay));
    }

    private IEnumerator DelayedLoadScene(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadScene(name);
    }
}