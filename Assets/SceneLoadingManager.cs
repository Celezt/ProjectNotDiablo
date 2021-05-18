using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshCombineStudio;
using UnityEngine.UI;

public class SceneLoadingManager : MonoBehaviour
{
    public MeshCombineStudio.MeshCombiner meshCombiner;

    public CanvasGroup canvas;
    public Slider progressBar;
    int loadingProgress = 0;
    bool loading = true;

    bool dungeonLoaded;
    bool roomsOptimised;
    bool navmeshGenerated;
    bool aiGenerated;
    bool processActive;

    // Start is called before the first frame update
    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (loading == true)
        {
            progressBar.value += Mathf.Clamp01(loadingProgress * 0.1f);
        }
        if (meshCombiner.combined)
        {
            roomsOptimised = true;
            processActive = false;
            Debug.Log("Combine Complete");
        }
        if (dungeonLoaded == false)
        {
            GenerateDungeon();
            dungeonLoaded = true;
        }
        if (roomsOptimised == false && dungeonLoaded == true && processActive == false)
        {
            OptimiseRooms();
            processActive = true;
        }
        if (navmeshGenerated == false && roomsOptimised == true)
        {
            GenerateDungeon();
            navmeshGenerated = true;
        }
        if (navmeshGenerated == true && aiGenerated == false)
        {
            GenerateDungeon();
            aiGenerated = true;
        }
        if (aiGenerated == true)
        {
            EndLoading();
        }
    }

    
    void GenerateDungeon()
    {
        loadingProgress = 20;
        Debug.Log("Generating Dungeon");
        
    }
    void OptimiseRooms()
    {
        loadingProgress = 40;
        Debug.Log("Optimising Rooms");
        
        meshCombiner.CombineAll();
    }
    void GenerateNavmesh()
    {
        loadingProgress = 60;
        Debug.Log("Generating Navmesh");
        
    }
    void GenerateAi()
    {
        loadingProgress = 80;
        Debug.Log("Generating AI");

    }
    void EndLoading()
    {
        loadingProgress = 100;
        Debug.Log("Ending Loading screen");
        StartCoroutine(FadeLoadingScreen(2));
    }

    IEnumerator FadeLoadingScreen(float duration)
    {
        float startValue = canvas.alpha;
        float time = 0;

        while (time < duration)
        {
            canvas.alpha = Mathf.Lerp(startValue, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvas.alpha = 0;
        canvas.gameObject.SetActive(false);
    }
}
