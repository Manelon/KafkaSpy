using System.Linq;
using KafkaSpy.Data;
using Terminal.Gui;

namespace KafkaSpy.Gui.Controls
{
    public class FrmTopics: FrameView
    {
        KafkaClusterMetadata _kafkaCluster;
        public event SelectedChangedEventHandler SelectedChanged;
        public delegate void SelectedChangedEventHandler(object sender, Topic e);
        public ListView TopicListView {get;private set;}
        public FrmTopics(KafkaClusterMetadata kafkaCluster):base("Topics"){
            _kafkaCluster=kafkaCluster;
            Width = Dim.Percent(50);
            Height = Dim.Fill();

            TopicListView = new ListView(_kafkaCluster.GetTopics().Select(x=>x.Name).ToList() ){
                Height = Dim.Fill(), 
                Width = Dim.Fill(),
                CanFocus=true,
                AllowsMultipleSelection=false
            };
            Add(TopicListView);

            TopicListView.SelectedChanged += ()=>{
                SelectedChanged?.Invoke(this, _kafkaCluster.GetTopics()[TopicListView.SelectedItem]);
            };

        }

        public Topic GetSelectedTopic(){
            return _kafkaCluster.GetTopics()[TopicListView.SelectedItem];
        }
    }
}