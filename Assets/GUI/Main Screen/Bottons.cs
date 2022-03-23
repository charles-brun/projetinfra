using UnityEngine;
using UnityEngine.SceneManagement;
//using kcp2k;

public class Bottons : MonoBehaviour
{


    public GameObject SecondBG;
    float[] BackGroundPos = { 0, -162, -324};
    int counter = -1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Vertical"))
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                counter++;
                if (counter >= 3)
                {
                    counter = 0;
                }
            } else
            {
                counter--;
                if (counter < 0)
                {
                    counter = 2;
                }
            }
            Input.ResetInputAxes();
        }


        if (Input.GetKeyDown(KeyCode.Return) && counter == 0)
        {
            PlayClicked();
        }

        if (counter >= 0)
        {
            SecondBG.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, Screen.height / (1080 / BackGroundPos[counter]), 0);            
        }

    }

    public void PlayClicked()
    {
        //Debug.Log("Play Clické");
        SceneManager.LoadScene("MatchMakingScreen");

    }

    public void OptionsClicked()
    {
        Debug.Log("Options Clické");
        //SceneManager.LoadScene("Game");
    }

    public void TestClicked()
    {
        Debug.Log("Test Clické");
        //SceneManager.LoadScene("Game");

    }
}
