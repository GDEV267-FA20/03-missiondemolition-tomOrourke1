using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{

    static private MissionDemolition S;



    [Header("Set in Inspector")]
    public Text uiLevel;
    public Text uitShots;
    public Text uitButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";


    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        
        // Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }
        // Destroy old grojectiles if they exist

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Reset the camera
        SwitchView("Show Both");

        ProjectileeLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;


        UpdateGUI();


        mode = GameMode.playing;


    }


    void UpdateGUI()
    {
        // SHow the data in the GUITexts
        uiLevel.text = "Level: " + (level + 1) + " of " + levelMax;

        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();

        //Check for level end

        if( (mode == GameMode.playing) && Goal.goalMet)
        {
            // Change mode to stop checking for level end
            mode = GameMode.levelEnd;

            //Zoom out
            SwitchView("Show Both");

            //start the nect level in 2 secons
            Invoke("NextLevel", 2f);

        }


    }
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView( string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;

        switch (showing)
        {
            case "Show SLingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "ShowBoth";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;

        }

    }
    // static method that allows code anywhere to increment shots taken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }



}
