namespace NowinHost
{
    using System;
    using Microsoft.Owin.Hosting;
    using OwinCore;

    public class Program
    {
        public static void Main(string[] args)
        {
            var options = new StartOptions
            {
                ServerFactory = "Nowin",
                Port = 8080
            };

            using (WebApp.Start<Startup>(options))
            {
                Console.WriteLine("Running a http server on port 8080");
                Console.ReadKey();
            }
        }
    }
}
