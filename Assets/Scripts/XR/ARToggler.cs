using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARToggler : MonoBehaviour
{
    [SerializeField] private Camera original_main_cam;
    private bool on = false;

    public void Toggle()
    {
        if (on)
        {
            GameManager.GetInstance().HideARScene();
            original_main_cam.gameObject.SetActive(true);

            on = false;
        }
        else
        {
            original_main_cam.gameObject.SetActive(false);
            GameManager.GetInstance().ShowARScene();

            on = true;
        }
    }
    
    void Awake()
    {
        on = false;

#if !UNITY_ANDROID && !UNITY_IOS
        holder.SetActive(false);
#endif
    }

    private void Start()
    {
        original_main_cam = Camera.main;
    }

}
