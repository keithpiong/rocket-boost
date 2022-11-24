using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float thrustPower;
    [SerializeField] float rotationSpeed;
    AudioSource audioSource;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        OutOfBounds();
    }

    void ProcessThrust()
    {
        // when rocket press SPACE
        if (Input.GetKey(KeyCode.Space))
        {
            _startThrusting();
        } else {
           _stopThrusting();
        }
    }

    void ProcessRotation()
    {   
        if (Input.GetKey(KeyCode.A))  // when rocket press left 
        {
            _rotateLeft();
        } else if (Input.GetKey(KeyCode.D)) // when rocket press right
        {
            _rotateRight();
        } else {
           _stopRotating();
        }
    }

    void _startThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustPower * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }

    void _stopThrusting()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void _rotateLeft()
    {
        Debug.Log("Rotate Left");
        ApplyRotation(rotationSpeed);
        if (!rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Play();
        }
    }

    void _rotateRight()
    {
        Debug.Log("Rotate Right" );
        ApplyRotation(-rotationSpeed);
        if (!leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Play();
        }
    }

    void _stopRotating()
    {
        rightThrustParticles.Stop();
        leftThrustParticles.Stop();
    }

    private void ApplyRotation(float rotationSpeed)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(rotationSpeed * Vector3.forward * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }

    private void OutOfBounds()
    {
        if (transform.position.y < -10f) {
            Debug.Log("OUT OF BOUNDS");
            StartCrashSequence();
        }
    }

    void StartCrashSequence()
    {
        this.enabled = false;
        Invoke("ReloadLevel", 2f); //invoke method is to call a function with time delay
    }

    void ReloadLevel()
    {
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevel);
    }
}
