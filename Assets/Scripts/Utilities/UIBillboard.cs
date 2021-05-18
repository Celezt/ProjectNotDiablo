using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

[ExecuteAlways]
public class UIBillboard : MonoBehaviour
{
	public Transform FacedObject;
	public bool ScaleDistance = true;
	[ConditionalField(nameof(ScaleDistance))]
	public float Multiplier = 1.0f;

	private Vector3 _initialScale;

	private static Camera _camera;

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

    private void Start()
    {
		_initialScale = transform.localScale;
	}

    private void LateUpdate()
    {
		if (ActiveFacedObject == null)
			return;

		if (ScaleDistance)
        {
			Transform cameraTransform = _camera.transform;
			Plane plane = new Plane(cameraTransform.forward, cameraTransform.position);
			float distance = plane.GetDistanceToPoint(transform.position);
			transform.localScale = _initialScale * distance * Multiplier;
		}

		transform.rotation = ActiveFacedObject.rotation;
    }
}
