using Terminal.Gui;

namespace KafkaSpy.Gui.Controls
{
    public class FrmCluster:FrameView
    {
         KafkaClusterMetadata _kafkaCluster;
         Label _lblBootstraps;
        public FrmCluster(KafkaClusterMetadata kafkaCluster):base("Kafka Cluster"){
            _kafkaCluster = kafkaCluster;

            X = 0;
            Y = 0;
            Height = 4;
            Width = Dim.Fill();
            CanFocus = false;

            _lblBootstraps = new Label($"Bootstraps Server {_kafkaCluster.GetBootstrapServers()}"){
                Width=Dim.Fill(),
                Height=1,
                CanFocus =false
            };

            Add(_lblBootstraps);

        }
    }
}