using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private WorldState worldState;

    [SerializeField]
    private UnityEvent OnLoadScene;

    [SerializeField]
    private string sceneOnEscape;

    private float delay;

    public void LoadScene(string name)
    {
        if (delay > 0) LoadSceneDelayed(name, delay);
        OnLoadScene?.Invoke();
        SceneManager.LoadScene(name);
        worldState.OnEnable();
    }

    public void SetSceneLoadDelay(float delay)
    {
        this.delay = delay;
    }

    public void LoadSceneDelayed(string name, float delay)
    {
        StartCoroutine(DelayedLoadScene(name, delay));
    }

    private IEnumerator DelayedLoadScene(string name, float delay)
    {
        yield return new WaitForSeconds(delay);
        delay = 0;
        LoadScene(name);
    }

    public void QuitApplication()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (sceneOnEscape != "")
            {
                delay = 0;
                LoadScene(sceneOnEscape);
            }
        }
    }
}