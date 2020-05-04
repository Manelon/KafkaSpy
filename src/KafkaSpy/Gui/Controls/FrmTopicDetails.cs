using System;
using System.Threading.Tasks;
using Confluent.Kafka;
using KafkaSpy.Commands;
using KafkaSpy.Data;
using Terminal.Gui;

namespace KafkaSpy.Gui.Controls
{
    public class FrmTopicDetails : FrameView
    {
        Label lblPartitions = new Label(0, 0, "Partitions");
        Label lblContentType = new Label("ContentType");
        public Button btnCount = new Button("Count");
        Label lblProgressSteps = new Label("Progress Steps");
        TextField txtProgressSteps = new TextField("10000");

        Label lblProgressCount = new Label("");

        ClientConfig _kafkaClientConfig;

        public FrmTopicDetails(string topicName, ClientConfig kafkaClientConfig) : base(topicName)
        {

            _kafkaClientConfig = kafkaClientConfig;
            Width = Dim.Fill();
            Height = 7; //I don't know why, but the frameView needs an extra row

            Add(lblPartitions);
            lblContentType.X = Pos.Right(lblPartitions);
            lblContentType.Y = Pos.Y(lblPartitions);
            Add(lblContentType);


            btnCount.Y = Pos.Bottom(lblPartitions);
            btnCount.CanFocus = true;
            btnCount.Clicked += btnCount_onClick;
            Add(btnCount);

            lblProgressSteps.Y = Pos.Y(btnCount);
            lblProgressSteps.X = Pos.Right(btnCount);
            Add(lblProgressSteps);

            txtProgressSteps.Y = Pos.Y(btnCount);
            txtProgressSteps.X = Pos.Right(lblProgressSteps);
            txtProgressSteps.Width = Dim.Fill();
            //txtProgressSteps.CanFocus=true;
            Add(txtProgressSteps);

            lblProgressCount.Y = Pos.Bottom(btnCount);
            lblProgressCount.Width = Dim.Fill();

            Add(lblProgressCount);

        }

        public void SetTopic(Topic topic)
        {
            this.Title = topic.Name;
            lblPartitions.Text = "Partitions " + topic.Partitions;
            lblContentType.Text = "Content Type " + topic.ContentType;

        }

        public async void btnCount_onClick()
        {

            try{
                Remove(btnCount);
                lblProgressCount.Text = "";
                var progress = new Progress<CountTopicMessajesRestult>(UpdateProgress);
            if (int.TryParse(txtProgressSteps.Text.ToString(), out int steps))
            {
                lblProgressCount.Text = "Running";
                //var result = await KafkaConsumer.CountTopicMessajesAsync(_kafkaClientConfig, $"Count_{this.Title}", this.Title.ToString(), steps, progress);
                var result =  await Task.Run( ()=> KafkaConsumer.CountTopicMessajes (_kafkaClientConfig, $"Count_{this.Title}", this.Title.ToString(), steps, progress));
                lblProgressCount.Text = result.ToString();
            }
            
            }catch(Exception ex){
                lblProgressCount.Text = ex.Message;
            }
            finally{
                Add(btnCount);
            }

            

        }

        private void UpdateProgress(CountTopicMessajesRestult progress)
        {
                //Application.MainLoop.Invoke (()=>lblProgressCount.Text = progress.ToString());
                lblProgressCount.Text = progress.ToString();
                Application.Refresh(); //This should not be necesary. But for the moment, it 
        }

    }
}