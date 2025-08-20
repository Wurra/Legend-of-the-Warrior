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
    public Vector3 menuPosition;
    [Header("�¼�����")]
    public SceneLoadEventSO LoadEventSO;
    public VoidEventSO NewGameEvent;

    [Header("�㲥")]
    public VoidEventSO aftersceneLoadEvent;
    public FadeEventSO fadeEvent;
    public SceneLoadEventSO unloadedSceneEvent;

    [Header("����")]
    public GameSceneSO firstLoadScene;
    public GameSceneSO MenuScene;
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
        LoadEventSO.RaiseLoadRequestEvent(MenuScene, menuPosition, true);
        //NewGame();
    }

    private void OnEnable()
    {
        LoadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        NewGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        LoadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        NewGameEvent.OnEventRaised -= NewGame;
    }

    private void NewGame()
    {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad, firstPosition, true);
        LoadEventSO.RaiseLoadRequestEvent(sceneToLoad, firstPosition, true);
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
           //TODO;�𽥱��
           fadeEvent.FadeIn(fadeDuration);
        }
        yield return new WaitForSeconds(fadeDuration);
        //�㲥�¼�����Ѫ����ʾ
        unloadedSceneEvent.RaiseLoadRequestEvent(sceneToLoad, positionToGo, true);

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
            fadeEvent.FadeOut(fadeDuration);
        }
        isLoading= false;
        //����������ɺ󴥷��¼�
        if(currentLoadedScene.sceneType==SceneType.Location)
        {
            aftersceneLoadEvent.RaiseEvent();
        }
        
    }
}
   
   