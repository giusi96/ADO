using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ADO
{
    public class ConnectedMode
    {
        const string connectionString = @"Persist Security Info = False; Integrated Security =true; Initial Catalog=CinemaDb; Server = WINAPPM2JMGI3WG\SQLEXPRESS; ";
        //1: salva la password: nel nostro caso è false perchè ci stiamo collegando con il nostro account windows
        //2: accesso tramite window autentication
        //3: nome del Database
        //4: nome del server

        public static void Connected()
        {
            //Creare una connessione

            //Metodo 1
            //SqlConnection connection = new SqlConnection();
            //connection.ConnectionString = connectionString;

            //Metodo 2
            //SqlConnection connection = new SqlConnection(connectionString);

            //Metodo 3
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Aprire la connessione
                connection.Open();

                //Creare un command
                SqlCommand command = new SqlCommand();
                command.Connection = connection; //si deve riferire a questa connetion con quella connetionString
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText = "SELECT * FROM Movies";

                //Creare parametri:per ora non ce ne sono

                //Eseguire command->DataReader
                SqlDataReader reader = command.ExecuteReader();

                //Leggere i dati:riga per riga dalla tabella
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}", reader["ID"], reader["Titolo"], reader["Genere"], reader["Durata"]);
                }

                //Chiudere la connessione
                reader.Close();
                connection.Close();
            }


        }

        public static void ConnectedWithParameter()
        {
            //Creare e usare connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //inserimento del valoe del parametro da riga di comando: voglio fare una select sul genre in base a quale dò
                Console.WriteLine("Genere del film: ");
                string Genere;
                Genere = Console.ReadLine();

                //aprire connessione
                connection.Open();

                //creare il command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.Text;
                command.CommandText= "SELECT * FROM Movies WHERE Genere= @Genere";

                //creazione parametro
                SqlParameter genereParam = new SqlParameter();
                genereParam.ParameterName = "@Genere";
                genereParam.Value = Genere;
                command.Parameters.Add(genereParam); //aggiunta del paramtro in command
                //altro modo: le 4 righe precedenti si possono fare così
                //command.Parameters.AddWithValue("@Genere", Genere);

                //Eseguire il command
                SqlDataReader reader = command.ExecuteReader();

                //Lettura dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2}", reader["ID"], reader["Titolo"], reader["Genere"]);
                }

                //chiusura connessione
                reader.Close();
                connection.Close();
            }
        }

        public static void ConnectedStoreProcedure()
        {
            //Creare e usare connessione
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //aprire connessione
                connection.Open();

                //creare il command
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = "tpGetActorsByCachetRange";

                //creare parametri
                command.Parameters.AddWithValue("@mincachet", 5000);
                command.Parameters.AddWithValue("@maxcachet", 9000);

                //creare valore di ritorno - la stp ha un valore di output
                SqlParameter returnValue = new SqlParameter();
                returnValue.ParameterName = "@returncount";
                returnValue.SqlDbType = System.Data.SqlDbType.Int; //non necessario, a meno di nvarchar

                returnValue.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(returnValue);

                //eseguire il command
                SqlDataReader reader = command.ExecuteReader();

                //leggere i dati
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}", reader["ID"], reader["FirstName"], reader["Lastname"], reader["Cachet"]);
                }

                reader.Close();

                ////se non vogliamo vedere la tabella:
                //command.ExecuteNonQuery(); //lo uso quando non mi aspetto una tabella, quindi per esempio ho un insert, un update

                Console.WriteLine("#Actors: {0}", command.Parameters["@returncount"].Value); //per visualizzare il parametro di output

                //chiusura connessione
                connection.Close();
            }
        }

        public static void ConnectedScalar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //aprire connessione
                connection.Open();

                //creare il command
                SqlCommand scalarCommand = new SqlCommand();
                scalarCommand.Connection = connection;
                scalarCommand.CommandType = System.Data.CommandType.Text;
                scalarCommand.CommandText = "Select count(*) from Movies";

                //eseguire il command
                int count = (int)scalarCommand.ExecuteScalar();

                //visualizza
                Console.WriteLine("Conteggio dei film: {0}", count);

                //chiusura connessione
                connection.Close();


            }
        }
    }
}
