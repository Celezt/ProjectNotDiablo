using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[ExecuteAlways]
public class UITarget : MonoBehaviour
{
	public Transform FacedObject;
	public bool ScaleDistance = true;
	[ConditionalField(nameof(ScaleDistance))]
	public float Multiplier = 1.0f;

	private Transform ActiveFacedObject
	{
		get
		{
			if (FacedObject != null) return FacedObject;
			if (_camera != null) return _camera.transform;
			_camera = FindObjectOfType<Camera>();

			return _camera == null ? null : _camera.transform;
		}
	}
	private static Camera _camera;

	private void LateUpdate()
    {
		if (ActiveFacedObject == null)
			return;

		transform.rotation = ActiveFacedObject.rotation;

		if (ScaleDistance)
        {
			float scale = Vector3.Distance(ActiveFacedObject.position, transform.position) * 0.01f * Multiplier;
			transform.localScale = new Vector3(scale, scale, scale);
		}
    }
}
