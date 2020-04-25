using System;
using KafkaSpy.Data;
using Terminal.Gui;

namespace KafkaSpy.Gui.Controls
{
    public class FrmTopicDetails : FrameView
    {
        Label lblPartitions = new Label(0,0,"Partitions");
        Label lblContentType = new Label("ContentType");
        public Button btnConsume = new Button("Consume");
        
        public FrmTopicDetails(string Title, Action onConsumeClik) : base(Title){

            Width=Dim.Fill();
            Height=6; //I don't know why, but the frameView needs an extra row

            Add(lblPartitions);
            lblContentType.Y = Pos.Bottom(lblPartitions);
            Add(lblContentType);
            
            btnConsume.Clicked += onConsumeClik;
            btnConsume.Width=Dim.Fill(1);
            btnConsume.Y = Pos.Bottom(lblContentType);
            btnConsume.CanFocus = true;
            Add(btnConsume);
            
            
        }

        public void SetTopic(Topic topic){
            this.Title = topic.Name;
            lblPartitions.Text = "Partitions " + topic.Partitions;
            lblContentType.Text = "Content Type " + topic;
            //btnConsume.Text = "ME CAGO EN ";
        }

    }
}