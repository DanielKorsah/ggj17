using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BWLevels { None = -1, Welcome, LevelSelect, Game, Pause, Victory, Persistent };

public class SceneController : MonoBehaviour
{
    #region singleton
    static SceneController instance = null;
    static public SceneController Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        instance = this;
    }
    #endregion

    readonly public List<string> levels = new List<string> { "Welcome", "", "BitWorldLevels", "PauseMenu", "Completed", "Persistent" };

    private List<BWLevels> majorLevels = new List<BWLevels> { BWLevels.Welcome, BWLevels.LevelSelect, BWLevels.Game, BWLevels.Victory };

    public BWLevels currentLevel = BWLevels.None;
    private List<BWLevels> addedLevels = new List<BWLevels>();
    private List<Scene> scenes = new List<Scene>();

    private List<BWLevels> loadQueue = new List<BWLevels>();
    private List<BWLevels> unloadQueue = new List<BWLevels>();

    // Start is called before the first frame update
    void Start()
    {
        // load welcome screen
        if (currentLevel == BWLevels.None)
        {
            ChangeLevel(BWLevels.Welcome);
        }
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }

    // Swap major level by unloading current one, and loading a new one
    public void ChangeLevel(BWLevels level)
    {
        // If level currently loaded, close it
        if (currentLevel != BWLevels.None)
        {
            UnloadLevel(currentLevel);
        }
        // load new
        SceneManager.LoadSceneAsync(levels[(int)level], LoadSceneMode.Additive);
        currentLevel = level;
    }

    // Load new level (change if major, additive if not)
    public void AddLevel(BWLevels level)
    {
        if (majorLevels.Contains(level))
        {
            ChangeLevel(level);
        }
        else
        {
            // add level
            SceneManager.LoadSceneAsync(levels[(int)level], LoadSceneMode.Additive);
            //StartCoroutine(ActivateNewScene(level));
            addedLevels.Add(level);
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
        scenes.Add(scene);
    }
    private void OnSceneUnload(Scene scene)
    {
        if (scenes.Count > 0)
        {
            SceneManager.SetActiveScene(scenes[scenes.Count - 1]);
        }
    }

    //private IEnumerator ActivateNewScene(BWLevels level)
    //{
    //    Scene scene = SceneManager.GetSceneByName(levels[(int)level]);
    //    while (true) {
    //        if(SceneManager.scene)
    //        if (SceneManager.SetActiveScene(scene))
    //        {
    //            yield break;
    //        }
    //        yield return null;
    //    }
    //}

    // Unload a level
    public void UnloadLevel(BWLevels level)
    {
        // If unloading current major level, first close additive levels
        if (level == currentLevel)
        {
            // close all added levels and current level
            for (int i = scenes.Count - 1; i >= 0; --i)
            {
                if (scenes[i].name == levels[(int)BWLevels.Persistent])
                {
                    break;
                }
                SceneManager.UnloadSceneAsync(scenes[i]);
                scenes.RemoveAt(i);
            }
        }
        else
        {
            addedLevels.Remove(level);
            // close requested level
            for (int i = 1; i < scenes.Count; ++i)
            {
                if (scenes[i].name == levels[(int)level])
                {
                    SceneManager.UnloadSceneAsync(scenes[i]);
                    scenes.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void QueueLoad(BWLevels level)
    {
        loadQueue.Add(level);
    }

    public void QueueUnload(BWLevels level)
    {
        unloadQueue.Add(level);
    }

    public void ExecuteQueues()
    {
        for (int i = unloadQueue.Count - 1; i >= 0; --i)
        {
            UnloadLevel(unloadQueue[i]);
        }
        unloadQueue.Clear();

        for (int i = loadQueue.Count - 1; i >= 0; --i)
        {
            AddLevel(loadQueue[i]);
        }
        loadQueue.Clear();
    }
}
