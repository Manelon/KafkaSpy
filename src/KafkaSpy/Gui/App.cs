using NStack;
using Terminal.Gui;

namespace KafkaSpy.Gui
{
    public class App : Toplevel
    {
        private KafkaClusterMetadata _kafkaCluster;
        public App(KafkaClusterMetadata kafkaCluster) : base()
        {
            _kafkaCluster = kafkaCluster;
            Application.Init();
            Init();
        }

        public override bool ProcessHotKey(KeyEvent kb)
        {
            switch (kb.Key)
            {
                case Key.F10:
                    Running = false;
                    return true;
            }
            return base.ProcessKey(kb);
        }
        private void Init()
        {
            // Creates the top-level window to show
            var app = new Window("KafkaSpy")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                CanFocus = false
            };


            var exit = new Label("[F10 Exit (or ESC plus 0)]")
            {
                X = 0,
                Y = Pos.Bottom(app) - 4
            };

            app.Add(exit);
            
            var clusterWindow = new Window("Cluster")
            {
                Height = Dim.Sized(3),
                Width = Dim.Fill(),
                CanFocus=false
            };

            var Boostrap = new Label($"BOOSTRAP SERVERS: ${_kafkaCluster.GetBootstrapServers()}");
            clusterWindow.Add(Boostrap);
            app.Add(exit);
            app.Add(clusterWindow);

            var wdnTopics = new Window("Topics"){
                Height = Dim.Fill()-1, // Leave 1 row for the botton comands
                Width = Dim.Percent(50),

                X=0,
                Y=Pos.Bottom(clusterWindow)
            };

            var lstTopics = new ListView(_kafkaCluster.GetTopics()){
                Height = Dim.Fill(), 
                Width = Dim.Percent(50),
                CanFocus=true,
                AllowsMarking=true,
                AllowsMultipleSelection=false
            };

            wdnTopics.Add(lstTopics);

            app.Add(wdnTopics);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_File", new MenuItem [] {
                new MenuItem ("_Quit", "", () => { this.Running = false; })
                })
            });

            Add(menu, app);
            SetFocus(lstTopics);
        }
    }
}