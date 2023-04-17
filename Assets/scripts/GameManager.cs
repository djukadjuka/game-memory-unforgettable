using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Main Logic Items
    /// If this much time passes then you get worst coefficient when multiplying points (2 minutes)
    public const float ForgettableTime = 60*2;
    /// Currently revealed cards (for finding pairs)
    public int RevealedCards { get; set; }
    /// Amount of target pairs to find to win the game
    public int TargetPairs { get; set; }
    /// Currently found pairs in game
    public int FoundPairs { get; set; }

    public int NumberOfTurns { get; set; }
    #endregion

    #region Points System
    /// Total points the player has aquired
    private float _totalPoints;
    public float TotalPoints
    {
        get
        {
            return (float)Math.Round(_totalPoints, 2);
        }
        set
        {
            _totalPoints = value;
        }
    }
    public const float PointsPerPair = 10.0f;

    #endregion

    #region Cards
    [Header("Cards")]
    public CardBehavior card1;
    public CardBehavior card2;
    public GameObject CardHolder;

    #endregion

    #region Music and Sound Effects
    public enum SoundEffect
    {
        WIN, REVEAL, FOUND_PAIR
    }

    [Header("Music and Sound Effects")]
    [SerializeField]
    public AudioSource SoundEffectsAudioSource;

    [SerializeField]
    public AudioClip RevealCardAudioClip;
    [SerializeField]
    public AudioClip WinAudioClip;
    [SerializeField]
    public AudioClip FindPairAudioClip;

    [SerializeField]
    public AudioSource BackgroundMusicAudioSource;
    [SerializeField]
    public AudioClip BackgroundSongAudioClip;
    #endregion

    #region UI
    [Header("UI Stuff")]
    [SerializeField]
    public TextMeshProUGUI WinScreenText; // For writing congrats
    [SerializeField]
    public TextMeshProUGUI elapsedTimeTMP; // For increasing and writing elapsed time in UI
    [SerializeField]
    public TextMeshProUGUI NumberOfTurnsTMP; // For increasing and writing number of turns in UI
    [SerializeField]
    public TextMeshProUGUI NumberOfPointsTMP; // Man guess..
    [SerializeField]
    public GameObject WinScreen; // where to place all win stuff
    [SerializeField]
    public float elapsedTimeFloat = 0; // How much time has passed since restart
    [SerializeField]
    public GameObject GameRunningUIItems;

    #endregion

    #region Materials
    [Header("Materials")]
    [SerializeField]
    public List<Material> Materials = new List<Material>(); // All card faces
    private List<int> SelectedMaterials = new List<int>(); // All selected card faces (used in cards)

    #endregion

    #region All Functions

    // Start is called before the first frame update
    void Start()
    {
        BackgroundMusicAudioSource.clip = BackgroundSongAudioClip;
        BackgroundMusicAudioSource.Play();

        // Hide winscreen on start
        WinScreen.SetActive(false);

        // Housekeeping, initialize items
        RevealedCards = 0;
        FoundPairs = 0;
        TotalPoints = 0;
        
        // Fetch all cards in game
        List<GameObject> Cards = new List<GameObject>();
        Cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));

        // Initialize how many is needed to win - in case of reducing number of cards for debugging
        TargetPairs = Cards.Count / 2;

        while (Cards.Count != 0)
        {
            // Fetch random index from materials that isn't already selected
            int matIdx;
            while (true)
            {
                matIdx = UnityEngine.Random.Range(0, Materials.Count);
                if (SelectedMaterials.Contains(matIdx)) continue;

                SelectedMaterials.Add(matIdx);
                break;
            }

            // Fetch random card from available cards
            int card1idx = UnityEngine.Random.Range(0, Cards.Count);
            // Set its card-face components material to the random material selected from Materials
            Cards[card1idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            // Remove card from collection, as to not fetch it again
            Cards.Remove(Cards[card1idx]);

            // Repeat procedure - makes two random cards have same face
            int card2idx = UnityEngine.Random.Range(0, Cards.Count);
            Cards[card2idx].transform.Find("card-face").gameObject.GetComponent<Renderer>().material = Materials[matIdx];
            Cards.Remove(Cards[card2idx]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increase elapsed time if all target pairs found
        if (this.TargetPairs > this.FoundPairs)
        {
            elapsedTimeFloat += Time.deltaTime;
            elapsedTimeTMP.SetText($"{(int)elapsedTimeFloat}");
        }
    }
    
    /// <summary>
    /// Calls hide on all cardbehavior references, sets them to null and resets number of revealed cards
    /// </summary>
    public void HideCards()
    {
        card1.Hide();
        card2.Hide();

        RevealedCards = 0;

        card1 = null;
        card2 = null;
    }

    public bool RevealCard(CardBehavior c)
    {
        // Check if two or more cards are already revealed, fail at revealing
        if (RevealedCards >= 2)
        {
            return false;
        }

        // Check which card is not revealed and set it
        if (card1 == null)
        {
            card1 = c;
        }
        else
        {
            // Increase number of turns when second card is revealed
            NumberOfTurns++;
            NumberOfTurnsTMP.SetText($"{NumberOfTurns}");
            card2 = c;
        }

        // Increase number of revealed cards
        RevealedCards++;

        // If two cards are revealed - process if faces are the same
        if (RevealedCards == 2)
        {
            // Check material names for revealed cards
            if (card1.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name ==
            card2.transform.Find("card-face").gameObject.GetComponent<Renderer>().material.name)
            {
                // Increase number of pairs and number of points
                FoundPairs++;
                PlaySoundEffect(SoundEffect.FOUND_PAIR);
                AddPoints();

                // Reset items
                RevealedCards = 0;
                card1 = null;
                card2 = null;

                // Check if found pairs is the same as target, win if true
                if (FoundPairs == TargetPairs)
                {
                    Invoke("Win", 1);
                }
            }
            else
            {
                // Invoke - call a procedure after enough time has passed (1sec) without blocking game
                PlaySoundEffect(SoundEffect.REVEAL);
                Invoke("HideCards", 1);
            }
        }
        else
        {
            // Just play audio clip if its the first card
            PlaySoundEffect(SoundEffect.REVEAL);
        }

        return true;
    }

    public void Win()
    {
        // Show the win screen and hide all cards by hiding the card holder
        PlaySoundEffect(SoundEffect.WIN);
        WinScreen.SetActive(true);
        CardHolder.SetActive(false);
        GameRunningUIItems.SetActive(false);

        float w1 = 12, w2 = 14, w3 = 16;
        float useTime = (float) Math.Round(elapsedTimeFloat);
        string congratsText = $"Congratulations! You got {TotalPoints} points!\nIt {(useTime < w1? "only ": "")}took you {useTime} seconds to finish.";
        string congratsTextComment = "";
        if(elapsedTimeFloat <= w1)
        {
            congratsTextComment += "\nWhat an unforgettable score!";
        }else if (elapsedTimeFloat <= w2)
        {
            congratsTextComment += "\nNot bad at all!";
        }else if(elapsedTimeFloat <= w3)
        {
            congratsTextComment += "\nBut I think you can do better!";
        }
        else
        {
            congratsTextComment += "\nPerhaps you should seek professional help.";
        }

        // Write congradulatory text
        this.WinScreenText.SetText($"{congratsText + congratsTextComment}");
    }


    public void Restart()
    {
        // Reloading current scene effectively resets game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AddPoints()
    {
        TotalPoints += PointsPerPair * GetTimePointCoeff();
        this.NumberOfPointsTMP.SetText($"{TotalPoints}");
    }

    public float GetTimePointCoeff()
    {
        if(elapsedTimeFloat >= ForgettableTime)
        {
            return 0.5f;
        }

        float low1 = ForgettableTime; // if you dont win in 5 minutes seek professional help
        float high1 = 0.0f; // Impossible to finish in 0 seconds but it will still be 0.9 or something if finished in 10 seconds or so
        float low2 = 0.1f; // worst points and worst coeff for longer time
        float high2 = 1.0f; // Max points and best coeff for shorter time

        return low2 + (elapsedTimeFloat - low1) * (high2 - low2) / (high1 - low1);
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        switch (soundEffect)
        {
            case SoundEffect.WIN:
                SoundEffectsAudioSource.PlayOneShot(WinAudioClip);
                break;
            case SoundEffect.REVEAL:
                SoundEffectsAudioSource.PlayOneShot(RevealCardAudioClip);
                break;
            case SoundEffect.FOUND_PAIR:
                SoundEffectsAudioSource.PlayOneShot(FindPairAudioClip);
                break;
            default:
                break;
        }
    }

    #endregion
}
