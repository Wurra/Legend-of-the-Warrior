using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public Transform playerTransform; // 玩家变换组件
    public Vector3 firstPosition; // 初始位置变换组件
    [Header("事件监听")]
    public SceneLoadEventSO LoadEventSO;
    public GameSceneSO firstLoadScene;

    [Header("广播")]
    public VoidEventSO aftersceneLoadEvent;
    private GameSceneSO currentLoadedScene;
    private GameSceneSO sceneToLoad;
    public Vector3 positionToGo;
    private bool fadeScreen;
    private bool isLoading;
    public float fadeDuration;
    private void Awake()
    {
        //Addressables.LoadSceneAsync(firstLoadScene.sceneReference, LoadSceneMode.Additive);
        //currentLoadedScene = firstLoadScene;
        //currentLoadedScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive);
    }
    //做完mainmenu后再改
    private void Start()
    {
        NewGame();
    }

    private void OnEnable()
    {
        LoadEventSO.LoadRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        LoadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        OnLoadRequestEvent(sceneToLoad, firstPosition, true);
    }
    private void OnLoadRequestEvent(GameSceneSO locationToLoad, Vector3 posToGO, bool FadeScreen)
    {
        if (isLoading)
        {
            return;
        }
        isLoading=true;
        sceneToLoad = locationToLoad;
        positionToGo = posToGO;
        this.fadeScreen = FadeScreen;
        if (currentLoadedScene != null) 
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScreen)
        {
           //TODO;实现屏幕淡出效果
        }
        yield return new WaitForSeconds(fadeDuration);
  
       currentLoadedScene.sceneReference.UnLoadScene();
       playerTransform.gameObject.SetActive(false);
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        var loadingOption=sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnOnLoadCompleted;
    }

    private void OnOnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadedScene = sceneToLoad;
        playerTransform.position = positionToGo;
        playerTransform.gameObject.SetActive(true);
        if (fadeScreen)
        {
            //TODO;实现屏幕淡入效果
        }
        isLoading= false;
        //场景加载完成后触发事件
        aftersceneLoadEvent.RaiseEvent();
    }
}
   
   