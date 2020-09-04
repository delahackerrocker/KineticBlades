using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum LevelToLoad
{
    MainMenu,
    MissionDemo,
    TeamArenaDemo,
    StarFighter_01,
    StarFighter_02,
    DressingRoom
}

public class ButtonPushClick : MonoBehaviour
{
    public LevelToLoad levelToLoad = LevelToLoad.MainMenu;

    public float MinLocalY = 0.25f;
    public float MaxLocalY = 0.55f;
  
    public bool isBeingTouched = false;
    public bool isClicked = false;

    public bool soundPlayed = false;

    public Material greenMat;

    public GameObject timeCountDownCanvas;
    public TextMeshProUGUI timeText;

    public float smooth = 0.1f;

    protected Vector3 defaultPosition;

    public bool nowTesting = false;

    void Start()
    {
        // Start with button up top / popped up
        defaultPosition = transform.localPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);

        //timeCountDownCanvas.SetActive(false);

    }

    private void Update()
    {
        Vector3 buttonDownPosition = new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z);
        Vector3 buttonUpPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
        if (!isClicked)
        {
            if (!isBeingTouched && (transform.localPosition.y > MaxLocalY  || transform.localPosition.y < MaxLocalY))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, buttonUpPosition, Time.deltaTime * smooth);
            }

            if (transform.localPosition.y < MinLocalY)
            {
                isClicked = true;               
                transform.localPosition = buttonDownPosition;
                OnButtonDown();
            }
        }
      
    }


    void OnButtonDown()
    {
        GetComponent<MeshRenderer>().material = greenMat;
        GetComponent<Collider>().isTrigger = true;

        AudioController.Play("3DButtonClickSound", transform, 1);

        //Start the game
        if (nowTesting)
        {
            // DO NOTHING
        }
        else
        {
            StartCoroutine(StartGame(3));
        }
    }


    IEnumerator StartGame(float countDownValue)
    {
        timeText.text = countDownValue.ToString();
        timeCountDownCanvas.SetActive(true);

        
        while (countDownValue > 0)
        {

            yield return new WaitForSeconds(1.0f);
            countDownValue -= 1;

            timeText.text = countDownValue.ToString();

        }
        //Load Scene

        if (levelToLoad == LevelToLoad.MainMenu)
        {
            SceneLoader.instance.LoadScene("MainMenu");
        }
        else if (levelToLoad == LevelToLoad.TeamArenaDemo)
        {
            SceneLoader.instance.LoadScene("TeamLevel_Demo");
        }
        else if (levelToLoad == LevelToLoad.MissionDemo)
        {
            SceneLoader.instance.LoadScene("SpaceShip_Demo_v2");
        }
        else if (levelToLoad == LevelToLoad.StarFighter_01)
        {
            SceneLoader.instance.LoadScene("StarFighter_Demo_01");
        }
        else if (levelToLoad == LevelToLoad.StarFighter_02)
        {
            SceneLoader.instance.LoadScene("StarFighter_Demo_02");
        }
        else if (levelToLoad == LevelToLoad.DressingRoom)
        {
            SceneLoader.instance.LoadScene("DressingRoom");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isClicked)
        {
            ////Playing Sound

            if (!soundPlayed)
            {
                AudioController.Play("3DButtonClickSound", transform, 1);

                VibrationManager.instance.VibrateController(.1f, .1f, .1f, OVRInput.Controller.LTouch);
                VibrationManager.instance.VibrateController(.1f, .1f, .1f, OVRInput.Controller.RTouch);

                soundPlayed = true;
            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = true;
        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.tag != "BackButton")
        {
            isBeingTouched = false;
            transform.localPosition = defaultPosition;
        }
    }
}