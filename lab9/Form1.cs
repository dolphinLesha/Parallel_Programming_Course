using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Form1 : Form
    {
        OpenFileDialog filopen = new OpenFileDialog();
        DataTable iris = new DataTable("iris");
        main mf;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size = new Size(300, 100);
            //load_dataset_b.Size = new Size(150, 100);
            iris.Columns.Add("sepallength", typeof(float));
            iris.Columns.Add("sepalwidth", typeof(float));
            iris.Columns.Add("petallength", typeof(float));
            iris.Columns.Add("petalwidth", typeof(float));
            iris.Columns.Add("class", typeof(string));
        }

        private void load_dataset_b_Click(object sender, EventArgs e)
        {
            filopen.Filter = "csv files (*.csv)|*.csv";
            if (filopen.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = filopen.FileName;
            iris = new DataTable("iris");
            iris.Columns.Add("sepallength", typeof(float));
            iris.Columns.Add("sepalwidth", typeof(float));
            iris.Columns.Add("petallength", typeof(float));
            iris.Columns.Add("petalwidth", typeof(float));
            iris.Columns.Add("class", typeof(string));

            read_csv(filename);
            mf = new main(iris);
            mf.Show();

        }

        void read_csv(string filen)
        {
            //prev solution
           
            {
                using (TextFieldParser parser = new TextFieldParser(filen))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.ReadFields();
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        
                        iris.Rows.Add(new object[]{
                            float.Parse(fields[0].Replace(".",",")),
                            float.Parse(fields[1].Replace(".",",")),
                            float.Parse(fields[2].Replace(".",",")),
                            float.Parse(fields[3].Replace(".",",")),
                            fields[4]
                            });
                    }
                }
            }
            {
                //StreamReader red = new StreamReader(filen);
                //string result = await red.ReadToEndAsync();
                //result = result.Replace(",","|")
                //string[] lines = result.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);
                //foreach(var line in lines)
                //{
                //    line
                //}
            }
        }

        
    }
}
