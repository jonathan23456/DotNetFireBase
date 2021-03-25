using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;



namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        DataTable dt = new DataTable();

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "",
            BasePath = ""
        };

        IFirebaseClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);

            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Address");
            dt.Columns.Add("Age");
            dt.Columns.Add("CorrelationID");
            dt.Columns.Add("Created At");

            dataGridView1.DataSource = dt;


        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var data = new Data
            {
                Id = textBox1.Text,
                Name = textBox2.Text,
                Address = textBox3.Text,
                Age = textBox4.Text
            };



            SetResponse response = await client.SetTaskAsync("Information/" + textBox1.Text, data);
            Data result = response.ResultAs<Data>();

            MessageBox.Show("Data Inserted " + result.Id);

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.GetTaskAsync("Information/" + textBox1.Text);

            Data obj = response.ResultAs<Data>();

            textBox1.Text = obj.Id;
            textBox2.Text = obj.Name;
            textBox3.Text = obj.Address;
            textBox4.Text = obj.Age;

            MessageBox.Show("Data Retrieved successfully");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Export();
        }

        private async void Export()
        {
            FirebaseResponse response = await client.GetTaskAsync("Information/");
            List<Data> obj = response.ResultAs<List<Data>>();
            if(obj!=null)
            {
                obj.RemoveAt(0);

                var source = new BindingSource();                
                source.DataSource = obj;
                dataGridView1.DataSource = source;                
            }

        }
    }
}
