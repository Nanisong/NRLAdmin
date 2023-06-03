using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace NRLAdmin.Models
{
    public class Ladder
    {
        public List<string> TeamNameList { set; get; } = new List<string>();
        public List<string> PointsList { set; get; } = new List<string>();
        public List<string> WinsList { set; get; } = new List<string>();
        public List<string> DrawsList { set; get; } = new List<string>();
        public List<string> LostsList { set; get; } = new List<string>();
        public List<string> PlayedList { set; get; } = new List<string>();

        public void GetLadderInfo()
        {
            SqlConnectionStringBuilder sConnB = new SqlConnectionStringBuilder()
            {
                DataSource = "FINISHHOMEWORK",
                InitialCatalog = "NRL",
                UserID = "root",
                Password = "root"
            };
            SqlConnection conn = new SqlConnection(sConnB.ConnectionString);

            try
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();

                    cmd.CommandText = @"SELECT 
							GT.HomeTeamID AS TeamID
							, GT.TeamName AS TeamName
							, SUM(GT.WPH + GT.WPA + GT.DPH + GT.DPA) AS POINTS
							, SUM(GT.WCH + GT.WCA) AS WINS
							, SUM(GT.WPH + GT.WPA) AS WINPOINTS
							, SUM(GT.DCH + GT.DCA) AS DRAWS
							, SUM(GT.DPH + GT.DPA) AS DRAWPOINS
							, SUM(GT.LCH + GT.LCA) AS LOSTS
							, SUM(GT.WCH + GT.WCA + GT.DCH + GT.DCA + GT.LCH + GT.LCA) AS PLAYED
							FROM 
								(SELECT 
								ISNULL(GWH.WINPOINT, 0) AS WPH
								, GWH.HomeTeamID
								, GWH.TeamName 
								, ISNULL(GWA.WINPOINTAWAY,0) AS WPA
								, ISNULL(GWH.WINCOUNTHOME, 0) AS WCH
								, ISNULL(GWA.WINCOUNTAWAY, 0) AS WCA
								, ISNULL(GDH.DRAWCOUNTHOME, 0) AS DCH
								, ISNULL(GDH.DRAWPOINTHOME, 0) AS DPH
								, ISNULL(GDA.DRAWCOUNTAWAY, 0) AS DCA
								, ISNULL(GDA.DRAWPOINTAWAY, 0) AS DPA
								, ISNULL(GLH.LOSTCOUNTHOME, 0) AS LCH
								, ISNULL(GLA.LOSTCOUNTAWAY, 0) AS LCA

								FROM
									(SELECT Teams.TeamID  AS HomeTeamID, Teams.TeamName AS TeamName, ISNULL(GTID.WINCOUNTHOME,0) 
										AS WINCOUNTHOME, ISNULL(GTID.WINPOINT,0) AS WINPOINT  from Teams 
								LEFT JOIN 
										(SELECT 
										HomeTeamID, COUNT(HomeTeamID) AS WINCOUNTHOME, COUNT(HomeTeamID)*2 AS WINPOINT 
										FROM GAMES WHERE HomeTeamScore > AwayTeamScore GROUP BY HomeTeamID) 
										AS GTID
										ON GTID.HomeTeamID = Teams.TeamID) 
									AS GWH
								LEFT JOIN
									(SELECT 
									AwayTeamID, COUNT(AwayTeamID) AS WINCOUNTAWAY, COUNT(AwayTeamID)*2 AS WINPOINTAWAY
									FROM GAMES WHERE AwayTeamScore > HomeTeamScore GROUP BY AwayTeamID
									) AS GWA
								ON GWH.HomeTeamID = GWA.AwayTeamID
								LEFT JOIN
									(SELECT 
									HomeTeamID, COUNT(HomeTeamID) AS DRAWCOUNTHOME, COUNT(HomeTeamID)*1 AS DRAWPOINTHOME
									FROM GAMES WHERE AwayTeamScore = HomeTeamScore GROUP BY HomeTeamID
									) AS GDH
								ON GWH.HomeTeamID = GDH.HomeTeamID
								LEFT JOIN
									(SELECT 
									AwayTeamID, COUNT(AwayTeamID) AS DRAWCOUNTAWAY, COUNT(AwayTeamID)*1 AS DRAWPOINTAWAY
									FROM GAMES WHERE AwayTeamScore = HomeTeamScore GROUP BY AwayTeamID
									) AS GDA
								ON GWH.HomeTeamID = GDA.AwayTeamID
								LEFT JOIN
									(SELECT 
									HomeTeamID, COUNT(HomeTeamID) AS LOSTCOUNTHOME
									FROM GAMES WHERE HomeTeamScore < AwayTeamScore GROUP BY HomeTeamID
									) AS GLH
								ON GWH.HomeTeamID = GLH.HomeTeamID
								LEFT JOIN
									(SELECT 
									AwayTeamID, COUNT(AwayTeamID) AS LOSTCOUNTAWAY
									FROM GAMES WHERE HomeTeamScore > AwayTeamScore GROUP BY AwayTeamID
									) AS GLA
								ON GWH.HomeTeamID = GLA.AwayTeamID
								) AS GT
							GROUP BY GT.HomeTeamID, GT.TeamName
							ORDER BY POINTS DESC";
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            TeamNameList.Add(string.Format("{0}", reader["TeamName"]));
                            PointsList.Add(string.Format("{0}", reader["POINTS"]));
                            WinsList.Add(string.Format("{0}", reader["WINS"]));
                            DrawsList.Add(string.Format("{0}", reader["DRAWS"]));
                            LostsList.Add(string.Format("{0}", reader["LOSTS"]));
                            PlayedList.Add(string.Format("{0}", reader["PLAYED"]));
                        }
                    }
                    conn.Close();
                }
                catch
                {

                }
            }
        }
    //}
}