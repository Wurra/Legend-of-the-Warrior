using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;
    /// <summary>
    /// 场景加载请求事件
    /// </summary>
    /// <param name="locationToLoad">要加载的场景</param>
    /// <param name="posToGo">Player的目标坐标</param>
    /// <param name="fadeScreen">是否渐入渐出</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToLoad,Vector3 posToGo, bool fadeScreen)
    {
        if (LoadRequestEvent != null)
        {
            LoadRequestEvent.Invoke(locationToLoad, posToGo, fadeScreen);
        }
    }
}
