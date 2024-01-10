using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumbleRemover : MonoBehaviour
{
	public Transform trTarget;
	public CucumbleTool cucumbleTool;
	private bool isAttach;
	public GameObject popUpExit;

	public void OnPointDown()
	{
		Attach();
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
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit);
		if (hit.collider != null)
		{
			if (hit.collider.gameObject.tag == "Cucumble" &&
				hit.collider.gameObject.transform.parent == trTarget &&
				hit.collider.gameObject.GetComponentInChildren<CucumbleRemover>() == null)
			{
				isAttach = true;
				cucumbleTool.showObj = true;

				//gameObject.GetComponent<RectTransform>().parent = hit.collider.gameObject.GetComponent<RectTransform>();
				gameObject.GetComponent<RectTransform>().SetParent(hit.collider.gameObject.GetComponent<RectTransform>());
				gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
				gameObject.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
				transform.parent.GetChild(0).gameObject.SetActive(false);
				gameObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
				cucumbleTool.clearCount++;
				cucumbleTool.OnPointerUp();
				if (cucumbleTool.clearCount == 2)
                {
					SoundManager.Instance.OnPlayOneShot("ve_02");
					cucumbleTool.showObj = false;
					cucumbleTool.clearCount = 0;
					cucumbleTool.ParticlePlay();
					PlayManager.Instance.UpdateGameStepInWash();
					PlayManager.Instance.ShowNextButton(true);
				}
			}
		}
		
	}

	public void OnPointUp()
	{
		cucumbleTool.OnPointerUp();
	}

	public void Clear()
	{
		Attach();
	}

	private void Attach()
	{
		isAttach = false;
	}
}
