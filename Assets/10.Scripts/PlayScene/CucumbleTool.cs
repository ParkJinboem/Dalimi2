using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CucumbleTool : MonoBehaviour
{
	public GameObject prefabCucumber;
	public Queue<GameObject> objCucumbers;
	private GameObject objMoveCucumber;

	public List<GameObject> cucumberChild;
	public GameObject toolParticle;
	public bool showObj;
	public int clearCount = 0;
	private CucumbleChecker cucmbleChecker;

	private void Awake()
	{
		prefabCucumber.SetActive(false);
		objCucumbers = new Queue<GameObject>();
	}

    private void Start()
	{
		cucmbleChecker = FindObjectOfType<CucumbleChecker>();
        for (int i = 0; i < cucmbleChecker.transform.childCount; i++)
        {
			cucmbleChecker.transform.GetChild(i).gameObject.SetActive(true);
			cucumberChild.Add(cucmbleChecker.transform.GetChild(i).gameObject);
		}
	}

	public GameObject GetCucumber()
	{
		GameObject obj;
		if (objCucumbers.Count > 0)
		{
			obj = objCucumbers.Dequeue();
		}
		else
		{
			obj = Instantiate(prefabCucumber, transform.parent);
		}
		//Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//obj.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
		//gameObject.GetComponent<Image>().raycastTarget = false;
		gameObject.SetActive(false);
		return obj;
	}

	public void Return(GameObject obj)
	{
		obj.transform.parent = this.transform.parent;
		objCucumbers.Enqueue(obj);
		//obj.SetActive(false);
	}

	public void OnPointerDown()
	{
		toolParticle.SetActive(false);
		if (clearCount ==2)
        {
			return;
        }
		SoundManager.Instance.OnClickSoundEffect();

		BoxCollider[] tempBoxCu = cucmbleChecker.GetComponentsInChildren<BoxCollider>();
		foreach (BoxCollider box in tempBoxCu)
		{
			box.enabled = true;
		}
		showObj = false;
		objMoveCucumber = GetCucumber();
		objMoveCucumber.SetActive(true);
	}

	public void OnPointerUp()
	{
		//BoxCollider[] tempBoxCu = cucmbleChecker.GetComponentsInChildren<BoxCollider>();
		//foreach (BoxCollider box in tempBoxCu)
		//{
		//	box.enabled = false;
		//}

		//if(cucumberChild[0].transform.childCount == 1 || cucumberChild[1].transform.childCount ==1)
		if(showObj)
        {
			gameObject.SetActive(true);
			//gameObject.GetComponent<Image>().raycastTarget = true;
			if (clearCount == 2)
            {
				gameObject.SetActive(false);
			}
		}

		if (objMoveCucumber != null)
		{
			if (transform == objMoveCucumber.transform.parent)
			{
				Return(objMoveCucumber);
			}
		}
	}

	public void ParticlePlay()
	{
		transform.parent.GetComponent<Wash>().ClearParticlePlay();
	}
}
