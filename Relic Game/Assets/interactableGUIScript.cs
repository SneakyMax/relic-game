using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class interactableGUIScript : MonoBehaviour {
	public Image selectImage;

	public void Start()
	{
		if(selectImage == null)
		{
			selectImage = GetComponentInChildren<Image>();
		}
		selectImage.enabled = false;
	}

	public void OnTriggerStay(Collider collider)
	{
		if(collider.gameObject.CompareTag("Player"))
			PlayerIsInImageDisplayRange();
	}

	public void OnTriggerExit(Collider collider)
	{
		if(collider.gameObject.CompareTag("Player"))
			PlayerIsOutOfDisplayRange();
	}

	private void PlayerIsInImageDisplayRange()
	{
		selectImage.enabled = true;
	}

	private void PlayerIsOutOfDisplayRange()
	{
		selectImage.enabled = false;
	}
}
