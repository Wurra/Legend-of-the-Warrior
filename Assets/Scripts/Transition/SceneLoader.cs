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
    public Transform playerTransform; // ��ұ任���
    public Vector3 firstPosition; // ��ʼλ�ñ任���
    [Header("�¼�����")]
    public SceneLoadEventSO LoadEventSO;
    public GameSceneSO firstLoadScene;

    [Header("�㲥")]
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
    //����mainmenu���ٸ�
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
           //TODO;ʵ����Ļ����Ч��
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
            //TODO;ʵ����Ļ����Ч��
        }
        isLoading= false;
        //����������ɺ󴥷��¼�
        aftersceneLoadEvent.RaiseEvent();
    }
}
   
   