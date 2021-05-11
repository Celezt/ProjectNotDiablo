using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadScene : MonoBehaviour
{
    public Slider progressBar;
    AsyncOperation loadingOperation;
    bool loading;
    // Start is called before the first frame update
    void Start()
    {
        LoadSelectedScene(2);
    }

    public void LoadSelectedScene(int sceneIndex)
    {
        loading = true;
        loadingOperation = SceneManager.LoadSceneAsync(sceneIndex);  
    }

    // Update is called once per frame
    void Update()
    {
        if (loading == true)
        {
            progressBar.value = Mathf.Clamp01(loadingOperation.progress / 0.9f);
        }
    }
}
