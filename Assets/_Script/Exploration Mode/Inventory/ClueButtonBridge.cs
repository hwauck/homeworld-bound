using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClueButtonBridge : MonoBehaviour
{
	public Image bigImage;  // Set by CluePopulator on creation.
	public Sprite clueSprite;// As above.

	void Start()
	{
        Debug.Log("Calling Start() method of ClueButtonBridge!");
		GetComponent<Button>().onClick.AddListener(() =>    // Adds an event to the button
		{
			bigImage.sprite = clueSprite;
			bigImage.color = new Color(1f, 1f, 1f, 1f);
            Debug.Log("Clicked clue button!");
		});
	}

}
