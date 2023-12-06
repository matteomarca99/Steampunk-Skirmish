using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Arrow : MonoBehaviour
{
	public Transform Origin { get { return origin; } set { origin = value; } }

	[SerializeField] private float baseHeight;
	[SerializeField] private RectTransform baseRect;
	[SerializeField] private Transform origin;
	[SerializeField] private bool startsActive;

	[SerializeField] private Color outlineColor;
	[SerializeField] private Color centerColor;

	[SerializeField] private List<Image> outlines;
	[SerializeField] private List<Image> centers;

	private RectTransform myRect;
	private Canvas canvas;
	private Camera mainCamera;
	private bool isActive;

	private void Awake()
	{
		ChangeColors(outlines, outlineColor);
		ChangeColors(centers, centerColor);

		myRect = (RectTransform)transform;
		canvas = GetComponentInParent<Canvas>();
		mainCamera = Camera.main;
		SetActive(startsActive);
	}

	private void Update()
	{
		if (!isActive)
			return;
		Setup();
	}

	private void Setup()
	{
		if (origin == null || canvas == null)
			return;

		Vector2 originPosOnScreen = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, origin.position);
		myRect.anchoredPosition = originPosOnScreen - new Vector2(Screen.width, Screen.height) / 2f;

		Vector2 differenceToMouse = Input.mousePosition - (Vector3)originPosOnScreen;
		differenceToMouse.Scale(new Vector2(1f / myRect.localScale.x, 1f / myRect.localScale.y));
		transform.up = differenceToMouse;
		baseRect.anchorMax = new Vector2(baseRect.anchorMax.x, differenceToMouse.magnitude / canvas.scaleFactor / baseHeight);
	}

	private void SetActive(bool b)
	{
		isActive = b;
		if (b)
			Setup();
		baseRect.gameObject.SetActive(b);
	}

	public void Activate() => SetActive(true);
	public void Deactivate() => SetActive(false);
	public void SetupAndActivate(Transform origin)
	{
		Origin = origin;
		Activate();
	}

	void ChangeColors(List<Image> images, Color color)
	{
		foreach (Image image in images)
		{
			image.color = color;
		}
	}
}
