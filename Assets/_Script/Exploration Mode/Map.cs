using System.Collections;
using UnityEngine;
using UnityEngine.Events;

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
        rrRect.anchoredPosition = new Vector3(-420.5f, -187.5f, 0);

        Highlighter.Highlight(this.gameObject);
        yield return new WaitForSeconds(4f);
        Highlighter.Unhighlight(this.gameObject);

        // zoom Rotations Remaining Panel to upper right
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
}
