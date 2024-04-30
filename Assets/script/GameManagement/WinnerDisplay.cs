using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class WinnerDisplay : MonoBehaviour
{
    public TMP_Text winnerText;

    private int winTeamIndex;

    string connectionString = "server=104.198.78.58;database=winningTimes;user=user1;password=fyp";
    // Start is called before the first frame update
    void Awake()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("winTeam", out object winTeamNum))
        {
            if ((int)winTeamNum == 1) 
            {
                winnerText.text = "Red Team Win!";
            }
            else if ((int)winTeamNum == 2)
            {
                winnerText.text = "Blue Team Win!";
            }

            winTeamIndex = (int)winTeamNum;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Debug.Log(connection.State);

                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    if (player.CustomProperties.TryGetValue(HashTableKey.TEAM_INDEX, out object teamIndex))
                    {
                        if (winTeamIndex == (int)teamIndex)
                        {
                            string checkQuery = "SELECT * FROM `wins` WHERE playerName = @playerName";
                            MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection);
                            checkCommand.Parameters.AddWithValue("@playerName", player.NickName);

                            MySqlDataReader reader = checkCommand.ExecuteReader();

                            if (reader.HasRows)
                            {
                                reader.Close();

                                string updateQuery = "UPDATE `wins` SET winNum = winNum + 1 WHERE playerName = @playerName";
                                MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                                updateCommand.Parameters.AddWithValue("@playerName", player.NickName);

                                updateCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                reader.Close();

                                string insertQuery = "INSERT INTO `wins` (playerName, winNum) VALUES (@playerName, @winNum)";
                                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                                insertCommand.Parameters.AddWithValue("@playerName", player.NickName);
                                insertCommand.Parameters.AddWithValue("@winNum", 1);

                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (MySqlException error)
            {
                Debug.LogError(error.Message);
            }
        }
    }

    public void loadMenuScene()
    {
        SceneManager.LoadScene("Connect");
    }
}
