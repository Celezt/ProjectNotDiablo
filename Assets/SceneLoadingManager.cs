using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MeshCombineStudio;

public class SceneLoadingManager : MonoBehaviour
{
    MeshCombineStudio.MeshCombiner meshCombiner;
    

    public CanvasGroup canvas;
    // Start is called before the first frame update
    void Start()
    {
        GenerateDungeon();
        OptimiseRooms();
        GenerateNavmesh();
        GenerateAi();
        EndLoading();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void GenerateDungeon()
    {
        Debug.Log("Generating Dungeon");

    }
    void OptimiseRooms()
    {
        Debug.Log("Optimising Rooms");
        meshCombiner.CombineAll();
    }
    void GenerateNavmesh()
    {
        Debug.Log("Generating Navmesh");
        
    }
    void GenerateAi()
    {
        Debug.Log("Generating AI");

    }
    void EndLoading()
    {
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
