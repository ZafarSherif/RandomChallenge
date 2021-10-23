using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

[System.Serializable]
public class Challenge
{
    public string name;
    public string description;
    public int reward;
}

[System.Serializable]
public class Challenges
{
    public Challenge[] challenges;
}
 
public class ChallengeManager : MonoBehaviour
{
    List<Challenge> challengeList = new List<Challenge>();
    public TextAsset jsonFile;
    [SerializeField] Text challengeText;
    [SerializeField] Text timerText;
    [SerializeField] Button finishChallengeButton;
    private float timeCounter = 0;

    Challenge getChallenge()
    {
        if(challengeList.Count == 0) {
            return null;
        }
        int randChallengeIndex = Random.Range(0, challengeList.Count);
        Challenge echo = challengeList[randChallengeIndex];
        challengeList.Remove(challengeList[randChallengeIndex]);
        return echo;
    }

    void Start()
    {
        Challenges challengeJson = JsonUtility.FromJson<Challenges>(jsonFile.text);
        foreach (Challenge challenge in challengeJson.challenges)
        {
            challengeList.Add(challenge);
        }
		finishChallengeButton.onClick.AddListener(finishChallenge);

        startChallenge();
    }

    void finishChallenge()
    {
        timeCounter = 10;
        finishChallengeButton.interactable = false;
    }

    void startChallenge()
    {
        Challenge currentChallenge = getChallenge();
        if(currentChallenge == null){
            challengeText.text = "All challenges completed";
            timeCounter = -1;
            finishChallengeButton.interactable = false;
            return;
        }
        challengeText.text = currentChallenge.description;
        timeCounter = 30;
        finishChallengeButton.interactable = true;
    }

    void Update()
    {
        if (timeCounter == -1) {
            return;
        }
        if (timeCounter > 0) {
            timerText.text = (Mathf.Floor(timeCounter)).ToString();
            timeCounter = timeCounter - Time.deltaTime;
        } else {
            startChallenge();
        }
    }
}
