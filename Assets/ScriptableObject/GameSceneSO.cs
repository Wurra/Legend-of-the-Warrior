using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
[CreateAssetMenu(menuName = "Event/GameSceneSO")]
public class GameSceneSO : ScriptableObject
{
    public SceneType sceneType; // 场景类型
    public AssetReference sceneReference; // 场景的地址引用
}
