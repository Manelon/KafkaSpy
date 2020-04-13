using System;
using System.IO;
using System.Linq;
using Confluent.Kafka.Admin;
using KafkaSpy.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Terminal.Gui;

namespace KafkaSpy
{
    class Program
    {

        static void Main(string[] args)
        {
           
            try{
                var configuration = ConfigurationHelper.GetKafkaConfiguration(Directory.GetCurrentDirectory(), args);
                var cluster = new KafkaClusterMetadata(configuration.BootstrapServers);

                var app = new Gui.App(cluster); //This way I can use poor man's dependency injection
                Application.Run(app);
            }catch(Exception ex){
                Console.WriteLine($"Ala carallo {ex.ToString()}" );

            }

        }



    }
}
