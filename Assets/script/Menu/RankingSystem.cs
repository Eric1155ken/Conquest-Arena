using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingSystem : MonoBehaviour
{
    string connectionString = "server=104.198.78.58;database=winningTimes;user=user1;password=fyp";

    //public GameObject winningInfoItem;
    //public GameObject contentObject;

    public TMP_Text InfoText;

    // Start is called before the first frame update
    void Start()
    {
        MySqlConnection connection = new MySqlConnection(connectionString);

        try
        {
            connection.Open();
            Debug.Log(connection.State);

            string sqlQuery = "SELECT * FROM `wins` ORDER BY `winNum` DESC;";

            MySqlCommand command = new MySqlCommand(sqlQuery, connection);
            MySqlDataReader reader = command.ExecuteReader();

            InfoText.text = "";
            while (reader.Read())
            {
                string columnName = reader.GetString(0);
                int columnValue = reader.GetInt32(1);

                //GameObject newWinningInfoItem = Instantiate(winningInfoItem, contentObject.transform);
                //newWinningInfoItem.SetActive(true);
                //newWinningInfoItem.transform.Find("Info Text").gameObject.GetComponent<TMP_Text>().text = columnName + ": " + columnValue;

                InfoText.text += columnName + ": " + columnValue + " wins" + "\n";
            }

            reader.Close();
        }
        catch (MySqlException error)
        {
            Debug.LogError(error.Message);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Lobby");
    }
}
