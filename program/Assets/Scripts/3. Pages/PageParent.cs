using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageParent : MonoBehaviour {
    private PageHandler[] pages;

    private void Awake() {
        pages = transform.GetComponentsInChildren<PageHandler>(true);
        foreach (var page in pages) {
            if (page != null) {
                page.Hide();
                PageManager.Add(page.name, page);
            }
        }
    }

    private void OnDestroy() {
        foreach (var page in pages) {
            PageManager.Remove(page.name);
        }
    }
}