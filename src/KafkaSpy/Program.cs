using System;
using System.IO;
using System.Linq;
using Confluent.Kafka.Admin;
using KafkaSpy.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Terminal.Gui;
using KafkaSpy.Data;

namespace KafkaSpy
{
    class Program
    {

        static void Main(string[] args)
        {
           
            try{
                //TODO: ADD M$.DependencyInjection
                
                
                var configuration = ConfigurationHelper.GetIConfigurationRoot(Directory.GetCurrentDirectory(), args);
                var kafkaConfiguration = configuration.GetKafkaConfiguration ();

                var dataContext = new DataContext(configuration.GetConnectionString("Sqlite"));

                var cluster = new KafkaClusterMetadata(kafkaConfiguration.BootstrapServers, dataContext);

                // var app = new Gui.App(cluster); //This way I can use poor man's dependency injection
                // Application.Run(app);

                var app = new Gui.Sample(cluster);

                Application.Run(app);
            }catch(Exception ex){
                Console.WriteLine($"Ala carallo {ex.ToString()}" );

            }

        }



    }
}
