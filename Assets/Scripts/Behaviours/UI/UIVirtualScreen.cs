using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms.BaseAtoms;
using MyBox;

public class UIVirtualScreen : Singleton<MonoBehaviour>
{
    [SerializeField] private Vector2Variable _virualScreenSizeVariable;
    [SerializeField] private float _refreshTime = 0.5f;

    private Coroutine _coroutineUpdateVirtualScreenSize;

    private void OnEnable()
    {
        _coroutineUpdateVirtualScreenSize = StartCoroutine(UpdateVirtualScreenSize());
    }

    private void OnDisable()
    {
        StopCoroutine(_coroutineUpdateVirtualScreenSize);
    }

    private IEnumerator UpdateVirtualScreenSize()
    {
        while (true)
        {
            _virualScreenSizeVariable.Value = new Vector2(Screen.width, Screen.height);

            yield return new WaitForUnscaledSeconds(_refreshTime);
        }
    }
}
