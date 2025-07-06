using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    public List<GameObject> content;
    public float timeBetweenPattern;

    private void Start()
    {
        StartCoroutine("TestFunc");
    }
    private IEnumerator TestFunc()
    {
        for(int i = 0; i < content.Count; i++)
        {
            content[i].SetActive(true);
            yield return new WaitForSeconds(timeBetweenPattern);
            content[i].SetActive(false);
        }
    }
}
