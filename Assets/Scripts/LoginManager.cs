using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public InputField username;
    public InputField sex;
    public InputField country;

    public void SetLoginValues()
    {
        PlayerPrefs.SetString("playerId", username.text);
        PlayerPrefs.SetString("playerSex", sex.text);
        PlayerPrefs.SetString("playerCountry", country.text);

        SceneManager.LoadScene("ExampleScene", LoadSceneMode.Single);
    }
}
