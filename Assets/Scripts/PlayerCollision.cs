using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public bool megaJump = false;
    [SerializeField] int trampolineSpring = 0;
    [SerializeField] int honeyPot = 0;
    [SerializeField] int rubyRed = 0;
    [SerializeField] int rubyGreen = 0;
    [SerializeField] int rubyTurquoise = 0;

    [SerializeField] AudioClip rubySound;
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] AudioClip springSound;
    [SerializeField] AudioClip trampolineSound;
    [SerializeField] AudioClip honeyPotSound;
    [SerializeField] AudioClip lifeFlashSound;

    [SerializeField] Image healthImage;
    [SerializeField] TMP_Text redRubyText;
    [SerializeField] TMP_Text greenRubyText;
    [SerializeField] TMP_Text turquoiseRubyText;
    [SerializeField] TMP_Text honeyPotText;
    [SerializeField] TMP_Text fenceText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject toBeContinuedPanel;


    [SerializeField] GameObject honeyEffect;
    [SerializeField] GameObject fenceGameObject;
    [SerializeField] GameObject shieldGameObject;

    bool fire1Pressed = false;
    bool mushroomAttackOK = true;
    bool mushroomCanWalk = true;
    bool canDstroyFence = false;
    bool fenceIsDestroyed = false;
    [SerializeField] bool shieldIsGained = false;// remove [SerializeField]
    AudioSource audioSource;
    CheckpointManager checkpointManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        checkpointManager = GetComponent<CheckpointManager>();
    }

    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            fire1Pressed = true;              
        }

        if(Input.GetButtonDown("Fire2") && canDstroyFence && !fenceIsDestroyed)
        {
            ProduceParticuleFX(honeyEffect, fenceGameObject.GetComponent<Collider>(), 0.8f);
            fenceGameObject.SetActive(false);
            canDstroyFence = false;
            fenceIsDestroyed = true;
            fenceText.text = "";
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameLost();
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "to_be_continued")
        {
            PauseGame();
            Cursor.lockState = CursorLockMode.None;
            toBeContinuedPanel.SetActive(true);
        }

        if(other.gameObject.tag == "shield")
        {
            shieldGameObject.SetActive(true);
            Destroy(other.gameObject, 0.1f);
            shieldIsGained = true;
        }

        if(other.gameObject.tag == "fence" && !fenceIsDestroyed)
        {
            fenceText.text = "Press Fire2 to open the Sheep enclosure"; 
            canDstroyFence = true;         
        }

        if(other.gameObject.tag == "honey_pot")
        {
            audioSource.PlayOneShot(honeyPotSound);
            ProduceParticuleFX(honeyEffect, other, 0.8f);
            honeyPot++;
            honeyPotText.text = honeyPot.ToString();
        }    

        if(other.gameObject.tag == "spring")
        {
            audioSource.PlayOneShot(springSound);
            ProduceParticuleFX(honeyEffect, other, 0.8f);
            trampolineSpring++;
        }    

        if(other.gameObject.tag == "life")
        {
            audioSource.PlayOneShot(lifeFlashSound);
            IncreaseLife(0.1f);
            ProduceParticuleFX(honeyEffect, other, 0.8f);
        }    

        //For mega Jump
        if(other.gameObject.tag == "trampoline")
        {
            if(other.gameObject.name == "First_Jump" && trampolineSpring > 0)
            {
                audioSource.PlayOneShot(trampolineSound);
                megaJump = true;
            }

            if((other.gameObject.name == "Mushroom_Jump" || 
                other.gameObject.name == "City_Jump") && 
                trampolineSpring > 1)
            {
                audioSource.PlayOneShot(trampolineSound);
                megaJump = true;
            }
            
            if(other.gameObject.name == "Oasis_Jump" && trampolineSpring > 2)
            {
                audioSource.PlayOneShot(trampolineSound);
                megaJump = true;
            }

            if(other.gameObject.name == "Last_Jump" && trampolineSpring > 3)
            {
                audioSource.PlayOneShot(trampolineSound);
                megaJump = true;
            }

            if(other.gameObject.name == "Inter2_Jump" && trampolineSpring > 4)
            {
                audioSource.PlayOneShot(trampolineSound);
                megaJump = true;
            }
        }  

        if(other.gameObject.tag == "rubis_rouge")
        {
            audioSource.PlayOneShot(rubySound);
            rubyRed++;
            redRubyText.text = rubyRed.ToString();
            ProduceParticuleFX(honeyEffect, other, 0.8f);
        }

        if(other.gameObject.tag == "rubis_turquoise")
        {
            audioSource.PlayOneShot(rubySound);
            rubyTurquoise++;
            turquoiseRubyText.text = rubyTurquoise.ToString();
            ProduceParticuleFX(honeyEffect, other, 0.8f);
        }
        
        if(other.gameObject.tag == "rubis_vert")
        {
            audioSource.PlayOneShot(rubySound);
            rubyGreen++;
            greenRubyText.text = rubyGreen.ToString();
            ProduceParticuleFX(honeyEffect, other, 0.8f);
        }
         

        if(other.gameObject.tag == "checkpoint")
        {
            checkpointManager.UpdateCheckpointPosition(other.transform.position);
            audioSource.PlayOneShot(checkpointSound);
            other.GetComponent<JewelAnimation>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "fence" && !fenceIsDestroyed)
        {
            fenceText.text = ""; 
            canDstroyFence = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit) 
    {
        if(hit.gameObject.tag == "mushroom_attack" && mushroomAttackOK)
        {
            mushroomAttackOK = false;

            if(fire1Pressed && shieldIsGained)
            {
                Debug.Log("PlayerAttack");
                fire1Pressed = false;
                mushroomCanWalk = false;
                StartCoroutine("MushroomGetBack");
            }
            else{
                Debug.Log("MushroomAttack");
                if(!DecreaseLife(0.1f))
                {
                    mushroomAttackOK = true;
                    return;
                } 
            }       

            StartCoroutine("ResetMushroomAttack");          
        }

        if(hit.gameObject.tag == "fallinsea")
        {
            GameLost();
        }

        if(hit.gameObject.tag == "sheep")
        {
            DecreaseLife(0.001f);
        } 

        if(hit.gameObject.tag == "coconut")
        {
            if(fire1Pressed && shieldIsGained)
            {
                Debug.Log("PlayerAttack");
                fire1Pressed = false;
            }
            else
            {
                Debug.Log("CoconutAttack");
                DecreaseLife(0.003f);
            }       
        }
    }

    void ProduceParticuleFX(GameObject effect, Collider other, float time)
    {
            GameObject go = Instantiate(effect, other.transform.position, Quaternion.identity);
            Destroy(go, 0.8f);
            Destroy(other.gameObject);
    }

    IEnumerator ResetMushroomAttack()
    {
        yield return new WaitForSeconds(0.8f);
        mushroomAttackOK = true;
    }

    void IncreaseLife(float value)
    {
        if(healthImage.fillAmount < 1.0f) healthImage.fillAmount += value;
    }

    bool DecreaseLife(float value)
    {
        if(healthImage.fillAmount > 0.0f) 
        {
            healthImage.fillAmount -= value;
            return true;
        } 
        else
        {
            GameLost();
            return false;
        }           
    }

    IEnumerator RefillLifeBar()
    {
        yield return new WaitForSeconds(0.8f);
        healthImage.fillAmount = 1.0f; 
    }

    void PauseGame ()
    {
        Time.timeScale = 0;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
    }

    void GameLost()
    {
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);
        PauseGame();
    }

    public void ReplayFromLastCheckpoint()
    {
        transform.position = checkpointManager.GetCheckpointPosition();
        pausePanel.SetActive(false);
        ResumeGame();
        StartCoroutine("RefillLifeBar");       
    }

    public void ReplayFromTheBeginning()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        ResumeGame();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
        ResumeGame();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public bool GetMushroomCanWalk()
    {
        return mushroomCanWalk;
    }

    IEnumerator MushroomGetBack()
    {
        yield return new WaitForSeconds(4.0f);
        mushroomCanWalk = true;
    }
}

