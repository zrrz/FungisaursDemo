using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryScreenManager : MonoBehaviour {

    int currentPage = 0;

    [SerializeField]
    GameObject nextButton;
    [SerializeField]
    GameObject prevButton;
    [SerializeField]
    Text skipButtonText;

    [SerializeField]
    GameObject[] pages;
    [SerializeField]
    string[] text;

    [SerializeField]
    float wordsPerSecond = 20f;

	void Awake () {
        text = new string[pages.Length];
        for (int i = 0; i < pages.Length; i++) {
            text[i] = pages[i].GetComponentInChildren<Text>().text;
            pages[i].GetComponentInChildren<Text>().text = "";
        }
	}

	//void Update () {

	//}

	private void Update()
	{
        if(Input.GetButtonDown("Fire1")) {
            Debug.LogError("Fire1");
            if(scrollingText) {
                Debug.LogError("scrolling");
                scrollingText = false;
                Text pageText = pages[currentPage].GetComponentInChildren<Text>();
                pageText.text = text[currentPage];
            }
        }
	}

	public void SetCurrentPage(int pageIndex) {
        if(pageIndex < 0 || pageIndex > pages.Length - 1) {
            Debug.LogError("PageIndex: " + pageIndex + " out of range");
            return;
        }
        if (currentPage == pageIndex && pages[currentPage].activeSelf)
            return;
        if (pages[currentPage].activeSelf)
            pages[currentPage].SetActive(false);

        if(pageIndex == 0) {
            prevButton.SetActive(false);
        } else {
            prevButton.SetActive(true);
        }

        if (pageIndex == pages.Length - 1)
        {
            nextButton.SetActive(false);
            skipButtonText.text = "Continue";
        }
        else
        {
            nextButton.SetActive(true);
            skipButtonText.text = "Skip";
        }

        pages[pageIndex].SetActive(true);
        currentPage = pageIndex;
        StartCoroutine(DisplayText(currentPage));
    }

    bool scrollingText = false;

    IEnumerator DisplayText(int pageIndex) {
        int size = text[pageIndex].Length;
        int index = 0;
        Text pageText = pages[pageIndex].GetComponentInChildren<Text>();
        pageText.text = "";
        scrollingText = true;
        while(pageIndex == currentPage && index < size && scrollingText) {
            pageText.text += text[currentPage][index];
            index++;
            yield return new WaitForSeconds(1f/wordsPerSecond);
        }
        scrollingText = false;
    }

    public void NextPage() {
        SetCurrentPage(currentPage + 1);
    }

    public void PreviousPage() {
        SetCurrentPage(currentPage - 1);
    }
}
