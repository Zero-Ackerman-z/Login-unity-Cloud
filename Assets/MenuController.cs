using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void PlayGame1()
    {
        SceneHelper.LoadScene("Juego1");
    }

    public void PlayGame2()
    {
        SceneHelper.LoadScene("Juego2");
    }

    public void PlayGame3()
    {
        SceneHelper.LoadScene("Juego3");
    }

    public void PlayGame4()
    {
        SceneHelper.LoadScene("Juego4");
    }
    public void PlayGame5()
    {
        SceneHelper.LoadScene("Juego5");
    }
    public void Logout()
    {
        SceneHelper.LoadScene("Login");
    }
}
