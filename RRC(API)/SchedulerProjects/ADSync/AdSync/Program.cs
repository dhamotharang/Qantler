using System;

namespace AdSync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Connecting to AD server...");
            SyncTask syncTask = new SyncTask();

            if (syncTask.connectLadp())
            {
                Console.WriteLine("Connected to AD server.");
                Console.WriteLine("Connecting to DB...");

                if (syncTask.connectDB())
                {
                    Console.WriteLine("Connected to DB.");
                    syncTask.closeDB();
                    syncTask.syncDBwithAD();
                }
                else
                {
                    Console.WriteLine("Unable to connect to DB");
                }
            }
            else
            {
                Console.WriteLine("Unable to connect to AD server");
            }
        }
    }
}