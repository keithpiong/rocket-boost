
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float nextLevelDelay;
     AudioSource audioSource;
    [SerializeField] AudioClip crashAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    bool isTransitioning;
    bool isDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isTransitioning = false;
    }

    void Update() {
        CheatInputsListener();
    }

    private void OnCollisionEnter(Collision other) {
        if (isTransitioning || isDisabled) {
            return;
        }
        switch(other.gameObject.tag)
        {
            case "Friendly":
                DebugCollision("Friendlies");
                break;
            case "Finish":
                DebugCollision("Finish!!!");
                StartSuccessSequence();
                break;
            case "Fuel":
                DebugCollision("Collected Fuel");
                break;
            default:
                DebugCollision("Hit the Obstacle");
                StartCrashSequence();
                break;
        }
    }

    void DebugCollision(string log)
    {
        Debug.Log(log);
    }

    void ReloadLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }

    void LoadNextLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;
        if (nextLevel == SceneManager.sceneCountInBuildSettings)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        Invoke("LoadNextLevel", nextLevelDelay); //invoke method is to call a function with time delay
        
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
        Invoke("ReloadLevel", nextLevelDelay); //invoke method is to call a function with time delay
    }


     // Cheat and Debug
    void CheatInputsListener() {
        switch (Input.inputString) 
        {
            case "l":
                LoadNextLevel();
                break;
            case "c":
                _toggleCollisionMode();
                break;
            default:
                break;
        }
    }

    void _toggleCollisionMode()
    {
        isDisabled = !isDisabled;
        Debug.Log("Collsion Mode Is " + isDisabled);
    }

}
