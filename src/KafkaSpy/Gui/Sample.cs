using KafkaSpy.Gui.Controls;
using Terminal.Gui;

namespace KafkaSpy.Gui
{
    public class Sample : Toplevel
    {
        private KafkaClusterMetadata _kafkaCluster;

        public Sample(KafkaClusterMetadata kafkaCluster) : base()
        {
            _kafkaCluster = kafkaCluster;

            Application.Init();

            // Creates the top-level window to show
            var win = new Window("KafkaSpy")
            {
                X = 0,
                Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                CanFocus=false
                
            };
            this.Add(win);

            // Creates a menubar, the item "New" has a help menu.
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_File", new MenuItem [] {
                    new MenuItem ("_Quit", "", () => { this.Running = false; })
                }),
                new MenuBarItem ("_Edit", new MenuItem [] {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
                })
            });
            this.Add(menu);

            var frmCluster =new FrmCluster(_kafkaCluster);
            var frmTopics = new FrmTopics(_kafkaCluster);
            
            frmTopics.Y =Pos.Bottom(frmCluster);

            var frmTopicDetails = new FrmTopicDetails(frmTopics.GetSelectedTopic().Name, null);
            frmTopicDetails.Y = Pos.Bottom(frmCluster);
            frmTopicDetails.X = Pos.Right(frmTopics);

            // frmCluster.OnEnter +=(x,y)=>{
            //     SetFocus(frmTopics.TopicListView);    
            // };

            frmTopics.SelectedChanged += (source, topic)=> {frmTopicDetails.SetTopic(topic);};
             
    

            win.Add(frmCluster, frmTopics, frmTopicDetails);

            frmTopicDetails.SetTopic(frmTopics.GetSelectedTopic());
            SetFocus(frmTopics.TopicListView);

            


        }
    }
}