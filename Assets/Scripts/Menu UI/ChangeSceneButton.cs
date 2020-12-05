using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField] private string scene_name;

    public void ChangeScene()
    {
        GameManager.GetInstance().ChangeScene(scene_name);
    }
}
