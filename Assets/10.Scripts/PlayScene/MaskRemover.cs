using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskRemover : MonoBehaviour
{
	public MaskTool maskTool;

	private RaycastHit hit;
	private bool isAttach;
	public GameObject popUpExit;

	private void Awake()
	{
		hit = new RaycastHit();
	}

	void Update()
	{
		if (isAttach || popUpExit.activeSelf)
		{
			return;
		}

		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);

		Ray ray = new Ray(transform.position, Vector3.forward);
		Physics.Raycast(ray, out hit);
		if (hit.collider != null)
		{
			if (hit.collider.transform.childCount == 1 && hit.collider.transform == maskTool.trTarget)
			{
				SoundManager.Instance.OnPlayOneShot("ve_02");
				isAttach = true;
				//transform.parent = hit.collider.transform;
				transform.SetParent(hit.collider.transform);
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				gameObject.GetComponent<RectTransform>().localScale = new Vector2(1,1);
				gameObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
				maskTool.col.enabled = false;
				maskTool.trTarget.transform.GetChild(0).gameObject.SetActive(false);
				maskTool.ParticlePlay();
				PlayManager.Instance.UpdateGameStepInWash();
				PlayManager.Instance.ShowNextButton(true);
				maskTool.gameObject.SetActive(false);
			}
		}
	}

	public void OnPointDown()
	{
		Return();
	}

	private void Return()
	{
		maskTool.Return(gameObject);
		isAttach = false;
	}
}
