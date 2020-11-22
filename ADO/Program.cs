using System;

namespace ADO
{
    class Program
    {
        static void Main(string[] args)
        {
            //MODALITA' CONNECTION:
            ConnectedMode.Connected();
            //ConnectedMode.ConnectedWithParameter();
            //ConnectedMode.ConnectedStoreProcedure();
            //ConnectedMode.ConnectedScalar();

            //MODALITA' DISCONNECTION:
            DisconnectedMode.Disconnected();
            ConnectedMode.Connected();
        }
    }
}
