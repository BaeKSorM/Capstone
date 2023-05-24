using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private void FindObjectsWithTagInChildren(Transform parent, string tag)
    {
        int childCount = parent.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = parent.GetChild(i);

            // 태그 검사
            if (child.CompareTag(tag))
            {
                // Ground 태그를 가진 오브젝트를 찾았을 때 수행할 동작
                Debug.Log("Found object with Ground tag: " + child.name);
            }

            // 자식들에 대해 재귀적으로 검색
            FindObjectsWithTagInChildren(child, tag);
        }
    }

    private void Start()
    {
        string tagToSearch = "BottomGround";
        Transform parent = transform; // 탐색을 시작할 부모 오브젝트의 Transform을 설정

        FindObjectsWithTagInChildren(parent, tagToSearch);
    }
}
