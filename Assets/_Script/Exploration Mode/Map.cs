using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public UnityEvent finishedMovingMap;

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        
    }

    public void doIntroMap()
    {
        StartCoroutine(introMap());
    }

    IEnumerator introMap()
    {
        // move to center of screen
        RectTransform rrRect = GetComponent<RectTransform>();
        rrRect.anchoredPosition = new Vector3(-382.99f, -150f, 0);
        show();

        yield return new WaitForSeconds(1f);

        // zoom Map to upper right
        Vector3 startPosition = rrRect.anchoredPosition;
        Vector3 endPosition = new Vector3(0, 0, 0);
        float lerpTime = 0.5f;
        float currentLerpTime = 0f;

        while (Vector3.Distance(rrRect.anchoredPosition, endPosition) > 2)
        {
            Debug.Log(rrRect.anchoredPosition);
            rrRect.anchoredPosition = Vector3.Lerp(startPosition, endPosition, currentLerpTime / lerpTime);
            currentLerpTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        rrRect.anchoredPosition = endPosition;
        finishedMovingMap.Invoke();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void show()
    {
        gameObject.GetComponent<Image>().enabled = true;
    }

    public void hide()
    {
        gameObject.GetComponent<Image>().enabled = false;
    }
}
