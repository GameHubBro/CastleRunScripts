using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;


public class Test : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Text inputText1;
    [SerializeField] private Text inputText2;

    

    public void ButtonToReadText2()
    {
        //text.text = "";
        //string[] array = new string[2];
        //if (File.Exists(@"C:\Users\Staza\ProfilUnityProjects\Animation\HumanoidAnimation\fileToSeek.txt"))
        //{
        //    array = File.ReadAllLines(@"C:\Users\Staza\ProfilUnityProjects\Animation\HumanoidAnimation\fileToSeek.txt");
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (i % 2 == 0)
        //        {
        //            text.text += array[i];
        //        }
        //        else if (i % 2 != 0)
        //        {
        //            text.text += "    " + array[i] + "\n";
        //        }
        //    }
        //}
        //else
        //{
        //    text.text = "No scores yet";
        //}

        text.text = "";
        string[] array = new string[2];
        string[,] bigArray = new string[6, 2];

        

        if (File.Exists(@"C:\Users\Staza\ProfilUnityProjects\Animation\HumanoidAnimation\fileToSeek.txt"))
        {
            array = File.ReadAllLines(@"C:\Users\Staza\ProfilUnityProjects\Animation\HumanoidAnimation\fileToSeek.txt");
            bigArray[0, 0] = array[0];
            bigArray[0, 1] = array[1];
            bigArray[1, 0] = array[2];
            bigArray[1, 1] = array[3];
            bigArray[2, 0] = array[4];
            bigArray[2, 1] = array[5];
            bigArray[3, 0] = array[6];
            bigArray[3, 1] = array[7];
            bigArray[4, 0] = array[8];
            bigArray[4, 1] = array[9];
            bigArray[5, 0] = array[10];
            bigArray[5, 1] = array[11];

            bigArray = Sorting(bigArray);

            for (int i = 0; i < 6-1; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    text.text += bigArray[i, k];
                }
            }          
        }
        else
        {
            text.text = "No scores yet";
        }
    }

    private string[,] Sorting(string [,] arr)
    {
        string temp = null;
        string temp2 = null;
        for (int i = 5; i > 0; i--)
        {           
                if (Convert.ToInt32(arr[i, 1]) > Convert.ToInt32(arr[i - 1, 1]))
                {
                    temp = arr[i, 1];
                    arr[i, 1] = arr[i - 1, 1];
                    arr[i - 1, 1] = temp;

                    temp2 = arr[i, 0];
                    arr[i, 0] = arr[i - 1, 0];
                    arr[i - 1, 0] = temp2;
                }
        }
        return arr;
    }
    

    public void ButtonToWriteText()
    {
        string[] textToWrite = new string[] { inputText1.text, inputText2.text };
        File.AppendAllLines(@"C:\Users\Staza\ProfilUnityProjects\Animation\HumanoidAnimation\fileToSeek.txt", textToWrite);   
    }

    public void SetRecord()
    {
        int score = Convert.ToInt32(inputText2.text);
        string inputName = inputText1.text;

        HighscoreTable.instance.AddHighscoreEntry(score, inputName);
    }

    public void DeleteHistory()
    {
        HighscoreTable.instance.DeleteHighscoreHistory();
    }
}
