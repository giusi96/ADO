using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class DisconnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security =true; Initial Catalog=CinemaDb; Server = WINAPPM2JMGI3WG\SQLEXPRESS; ";
        
        public static void Disconnected()
        {
            using(SqlConnection connection =new SqlConnection(connectionString))
            {
                //Costruzione Adapter
                SqlDataAdapter adapter = new SqlDataAdapter();

                //costruzioni comandi da associare all'adapter
                SqlCommand selectCommand = new SqlCommand();
                selectCommand.Connection = connection;
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "Select * from Movies";

                SqlCommand insertCommand = new SqlCommand();
                insertCommand.Connection = connection;
                insertCommand.CommandType = System.Data.CommandType.Text;
                insertCommand.CommandText = "insert into Movies values(@Titolo, @Genere, @Durata)";

                //costruzione parametri dell'insert
                insertCommand.Parameters.Add("@Titolo", System.Data.SqlDbType.NVarChar, 255, "Titolo");
                insertCommand.Parameters.Add("@Genere", System.Data.SqlDbType.NVarChar, 255, "Genere");
                insertCommand.Parameters.Add("@Durata", System.Data.SqlDbType.Int,500, "Durata");

                //... delete e update

                //associare i comandi all'adapter
                adapter.SelectCommand = selectCommand;
                adapter.InsertCommand = insertCommand;

                //creazione Dataset
                DataSet dataset = new DataSet();

                try
                {
                    connection.Open();
                    adapter.Fill(dataset, "Movies"); //mettiamo tutto quello che c'è in movies in dataset

                    foreach (DataRow row in dataset.Tables["Movies"].Rows)
                    {
                        Console.WriteLine("Row: {0}", row["Titolo"]);
                    }

                    //creazione Record
                    DataRow movie = dataset.Tables["Movies"].NewRow();
                    movie["Titolo"] = "V per vendetta";
                    movie["Genere"] = "Azione";
                    movie["Durata"] = 125;


                    //aggiungo alla tabella Movies del dataset generale
                    dataset.Tables["Movies"].Rows.Add(movie);

                    //update del database
                    adapter.Update(dataset, "Movies");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    connection.Close();
                }

            }
        }



    }
}
