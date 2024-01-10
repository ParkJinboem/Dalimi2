using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MaskTool : MonoBehaviour
{
	public Transform trTarget;
	public BoxCollider col;
	public GameObject prefabItem;

	private Queue<GameObject> objItems;
	private GameObject objMove;
	public GameObject toolParticle;

	private void Awake()
	{
		prefabItem.SetActive(false);
		col.enabled = false;
		objItems = new Queue<GameObject>();
	}

	public void OnPointerDown()
	{
		SoundManager.Instance.OnClickSoundEffect();
		toolParticle.SetActive(false);
		col.enabled = true;
		objItems.Clear();
		objMove = Get();
		objMove.SetActive(true);
		this.GetComponent<Image>().color = new Color(0, 0, 0, 0);
	}

	public void OnPointerUp()
	{
		//col.enabled = false;
		if (objMove != null)
		{
			if (transform == objMove.transform.parent)
			{
				Return(objMove);
			}
		}
	}

	public GameObject Get()
	{
		GameObject obj;
		if (objItems.Count > 0)
		{
			obj = objItems.Dequeue();
		}
		else
		{
			obj = Instantiate(prefabItem, transform);
		}
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		obj.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
		return obj;
	}

	public void Return(GameObject obj)
	{
		//obj.transform.parent = transform;
		obj.transform.SetParent(transform);
		objItems.Enqueue(obj);
		//obj.SetActive(false);
	}

	public void Show()
	{
		MaskRemover[] maskRemovers = trTarget.GetComponentsInChildren<MaskRemover>();
		for (int i = 0; i < maskRemovers.Length; i++)
		{
			maskRemovers[i].OnPointDown();
		}
		trTarget.gameObject.SetActive(true);
	}

	public void Hide()
	{
		trTarget.gameObject.SetActive(false);
	}

	public void ParticlePlay()
    {
		transform.parent.GetComponent<Wash>().ClearParticlePlay();
	}
}

