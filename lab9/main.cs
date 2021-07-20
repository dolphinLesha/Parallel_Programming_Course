using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class main : Form
    {
        DataTable iris;
        float maxw = 0, maxh = 0;
        float[] pedw, pedh;
        SolidBrush[] bruses;
        int am_data = 0;
        public main(DataTable dt)
        {
            
            InitializeComponent();
            
            iris = dt;
            pedw = new float[iris.Rows.Count];
            pedh = new float[iris.Rows.Count];
            am_data = iris.Rows.Count;
            am_data_input.Text = am_data.ToString();
            am_data_sl.Value = 100;
            int k = 0;
            bruses = new SolidBrush[iris.Rows.Count];
            Random ran = new Random();
            
            foreach (DataRow row in iris.Rows)
            {
                bruses[k] = new SolidBrush(Color.FromArgb(
                ran.Next(100, 256), ran.Next(100, 256), ran.Next(100, 256)));
                pedw[k] = (float)row[3];
                pedh[k++] = (float)row[2];

            }
            find_max();
        }

        void find_max()
        {
            
            
            foreach(DataRow row in iris.Rows)
            {
                if (maxw < (float)row[3])
                    maxw = (float)row[3];
                if (maxh < (float)row[2])
                    maxh = (float)row[2];
            }
        }

        private void main_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
            var pw = pictureBox1.Width;
            var ph = pictureBox1.Height;
            var cw = pw / maxw;
            var ch = ph / maxh;
            var r = 7;
            Brush b=Brushes.Black;

            foreach (DataRow row in iris.Rows)
            {
                switch ((string)row[4])
                {
                    case "Iris-setosa":
                        b = Brushes.Coral;
                        break;
                    case "Iris-versicolor":
                        b = Brushes.HotPink;
                        break;
                    case "Iris-virginica":
                        b = Brushes.BurlyWood;
                        break;
                }
                e.Graphics.FillEllipse(b, (float)row[3] * cw - r, ph-((float)row[2] * ch - r),
                    r, r);
            }



        }

        double[,] l_matrix;

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            var pw = pictureBox1.Width;
            var ph = pictureBox1.Height;
            var cw = pw / maxw;
            var ch = ph / maxh;
            var r = 7;

            try
            {
                for (int i = 0; i < classes.Count; i++)
                {

                    for (int y = 0; y < classes[i].Count; y++)
                    {

                        e.Graphics.FillEllipse(bruses[i], pedw[classes[i][y]] * cw - r,
                            ph - (pedh[classes[i][y]] * ch - r),
                        r, r);
                    }

                }
            }
            catch { }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            par_b.Visible = false;
            cancel_par.Visible = false;
            posl_b.Visible = false;
            cancel_p.Visible = true;
            pedw = new float[am_data];
            pedh = new float[am_data];
            int k = 0;
            List<float> pw = new List<float>();
            List<float> ph = new List<float>();
            List<float> pwi = new List<float>();
            List<float> phi = new List<float>();
            foreach (DataRow row in iris.Rows)
            {
                pw.Add((float)row[3]);
                ph.Add((float)row[2]);
                //pedw[k] = (float)row[3];
                //pedh[k++] = (float)row[2];
                //if (k >= am_data)
                //    break;
            }
            Random r = new Random();
            for(int i = 0; i < am_data; i++)
            {
                int a = r.Next(0, pw.Count);
                pwi.Add(pw[a]);
                phi.Add(ph[a]);
                pw.RemoveAt(a);
                ph.RemoveAt(a);
            }
            pedh = phi.ToArray();
            pedw = pwi.ToArray();
            posl_alg();
        }
        CancellationTokenSource cts_posl = new CancellationTokenSource();
        CancellationTokenSource cts_par = new CancellationTokenSource();
        async void posl_alg()
        {
            cts_posl = new CancellationTokenSource();
            var progress = new Progress<int>(update_progress);
            double res = await posledov(progress);
            label1.Text = String.Format("время работы: {0}", res);
        }

        void update_progress(int val)
        {
            progressBar1.Value = 100-val;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void am_data_sl_Scroll(object sender, EventArgs e)
        {
            am_data = (int)((float)iris.Rows.Count * ((float)am_data_sl.Value / 100f));
            am_data_input.Text = am_data.ToString();
            am_data_t.Text = am_data_sl.Value.ToString() + "%";
        }

        private void am_data_input_TextChanged(object sender, EventArgs e)
        {
            int am;
            if (!int.TryParse(am_data_input.Text, out am))
                am = iris.Rows.Count;
            if(am>iris.Rows.Count)
                am= iris.Rows.Count;
            if (am < iris.Rows.Count * 0.1f)
                am = (int)(iris.Rows.Count * 0.1f);
            am_data = am;
            am_data_input.Text = am_data.ToString();
            am_data_sl.Value = (int)((am_data / (float)iris.Rows.Count) *100);
            am_data_t.Text = am_data_sl.Value.ToString()+"%";
        }

        private void cancel_p_Click(object sender, EventArgs e)
        {
            cts_posl.Cancel();
            cancel_p.Visible = false;
            posl_b.Visible = true;
            par_b.Visible = true;
        }

        List<List<int>> classes = new List<List<int>>();

        private void par_b_Click(object sender, EventArgs e)
        {
            par_b.Visible = false;
            cancel_par.Visible = false;
            posl_b.Visible = false;
            cancel_par.Visible = true;
            pedw = new float[am_data];
            pedh = new float[am_data];
            int k = 0;
            List<float> pw = new List<float>();
            List<float> ph = new List<float>();
            List<float> pwi = new List<float>();
            List<float> phi = new List<float>();
            foreach (DataRow row in iris.Rows)
            {
                pw.Add((float)row[3]);
                ph.Add((float)row[2]);
                //pedw[k] = (float)row[3];
                //pedh[k++] = (float)row[2];
                //if (k >= am_data)
                //    break;
            }
            Random r = new Random();
            for (int i = 0; i < am_data; i++)
            {
                int a = r.Next(0, pw.Count);
                pwi.Add(pw[a]);
                phi.Add(ph[a]);
                pw.RemoveAt(a);
                ph.RemoveAt(a);
            }
            pedh = phi.ToArray();
            pedw = pwi.ToArray();
            par_alg();
        }

        private void cancel_par_Click(object sender, EventArgs e)
        {
            cts_par.Cancel();
            par_b.Visible = true;
            posl_b.Visible = true;
            cancel_par.Visible = false;
        }

        async void par_alg()
        {
            cts_par = new CancellationTokenSource();
            var progress = new Progress<int>(update_progress);
            double res = await paralel(progress);
            label1.Text = String.Format("время работы: {0}", res);
        }

        Task<double> posledov(IProgress<int> progress)
        {
            classes = new List<List<int>>();

            return Task<double>.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                double total_time = 0;
                l_matrix = new double[pedh.Length, pedh.Length];
                int need_classes = 3;
                if (!int.TryParse(am_cl_input.Text, out need_classes)) need_classes = 3;
                int cur_classes = pedw.Length;
                int rclasses = cur_classes - need_classes;
                sw.Start();
                for (int i = 0; i < pedh.Length; i++)
                {
                    List<int> t = new List<int>();
                    t.Add(i);
                    classes.Add(t);
                    for (int e = 0; e < pedw.Length; e++)
                    {
                        l_matrix[i, e] = Math.Sqrt(Math.Pow(pedw[i] - pedw[e], 2) +
                            Math.Pow(pedh[i] - pedh[e], 2));
                        if (i == e)
                            l_matrix[i, e] = -1;
                    }
                }
                while (cur_classes > need_classes)
                {
                    double max_g = 10000;
                    double max_l = 0;
                    int maxg_i1 = 0, maxg_i2 = 0;
                    for (int i = 0; i < classes.Count; i++)
                    {
                        for (int w = i + 1; w < classes.Count; w++)
                        {
                            for (int e = 0; e < classes[i].Count; e++)
                            {
                                for (int y = 0; y < classes[w].Count; y++)
                                {
                                    if (max_l < l_matrix[classes[i][e], classes[w][y]] &&
                                        l_matrix[classes[i][e], classes[w][y]] != -1)
                                    {
                                        max_l = l_matrix[classes[i][e], classes[w][y]];
                                    }
                                }
                            }
                            if (max_g > max_l)
                            {
                                max_g = max_l;
                                maxg_i1 = i;
                                maxg_i2 = w;
                            }
                            max_l = 0;
                        }
                    }
                    for (int i = 0; i < classes[maxg_i2].Count; i++)
                    {
                        classes[maxg_i1].Add(classes[maxg_i2][i]);
                    }
                    classes.RemoveAt(maxg_i2);
                    cur_classes--;
                    progress.Report((int)(((cur_classes-need_classes)/ (float)rclasses) * 100));
                    total_time += sw.Elapsed.TotalSeconds;
                    //pictureBox2.Invalidate();
                    if (cts_posl.Token.IsCancellationRequested)
                        return total_time;
                    //Thread.Sleep(50);
                    sw.Restart();
                }
                total_time += sw.Elapsed.TotalSeconds;
                sw.Stop();
                pictureBox2.Invalidate();
                return total_time;
            }
            );
            
        }

        Task<double> paralel(IProgress<int> progress)
        {
            classes = new List<List<int>>();

            return Task<double>.Run(() =>
            {
                Stopwatch sw = new Stopwatch();
                double total_time = 0;
                l_matrix = new double[pedh.Length, pedh.Length];
                int need_classes = 3;
                if (!int.TryParse(am_cl_input.Text, out need_classes)) need_classes = 3;
                int cur_classes = pedw.Length;
                int rclasses = cur_classes - need_classes;
                sw.Start();
                ParallelOptions options = new ParallelOptions()
                {
                    //CancellationToken = cts.Token,
                    MaxDegreeOfParallelism = 4
                };
                Parallel.For(0, pedh.Length, options, i =>
                {
                    for (int e = 0; e < pedw.Length; e++)
                    {
                        l_matrix[i, e] = Math.Sqrt(Math.Pow(pedw[i] - pedw[e], 2) +
                            Math.Pow(pedh[i] - pedh[e], 2));
                        //if (i == e)
                        //    l_matrix[i, e] = -1;
                    }
                });
                for (int i = 0; i < pedh.Length; i++)
                {
                    List<int> t = new List<int>();
                    t.Add(i);
                    classes.Add(t);
                }
                while (cur_classes > need_classes)
                {
                    double max_g = 10000;
                    ConcurrentDictionary<int, (double,int,int)> dic = new ConcurrentDictionary<int, (double, int, int)>();
                    
                    int maxg_i1 = 0, maxg_i2 = 0;
                    
                    Parallel.For(0, classes.Count, options, i => 
                    {
                        int id = Thread.CurrentThread.ManagedThreadId;
                        if (!dic.ContainsKey(id))
                        {
                            bool b = dic.TryAdd(id, (10000, 0, 0));
                            while(!b)
                                b = dic.TryAdd(id, (10000, 0, 0));

                        }
                        double max_l = 0;
                        for (int w = i + 1; w < classes.Count; w++)
                        {
                            for (int e = 0; e < classes[i].Count; e++)
                            {
                                for (int y = 0; y < classes[w].Count; y++)
                                {
                                    if (max_l < l_matrix[classes[i][e], classes[w][y]] &&
                                        l_matrix[classes[i][e], classes[w][y]] != -1)
                                    {
                                        max_l = l_matrix[classes[i][e], classes[w][y]];
                                    }
                                }
                            }
                            if (dic[id].Item1 > max_l)
                            {
                                dic[id] = (max_l,i,w);
                                
                            }
                            max_l = 0;
                        }
                    });
                    
                    foreach(var pair in dic)
                    {
                        if(max_g>pair.Value.Item1)
                        {
                            max_g = pair.Value.Item1;
                            maxg_i1 = pair.Value.Item2;
                            maxg_i2 = pair.Value.Item3;
                        }
                    }
                    for (int i = 0; i < classes[maxg_i2].Count; i++)
                    {
                        classes[maxg_i1].Add(classes[maxg_i2][i]);
                    }
                    classes.RemoveAt(maxg_i2);
                    cur_classes--;
                    progress.Report((int)(((cur_classes - need_classes) / (float)rclasses) * 100));
                    total_time += sw.Elapsed.TotalSeconds;
                    //pictureBox2.Invalidate();
                    if (cts_par.Token.IsCancellationRequested)
                        return total_time;
                    //Thread.Sleep(50);
                    sw.Restart();
                }
                total_time += sw.Elapsed.TotalSeconds;
                sw.Stop();
                pictureBox2.Invalidate();
                return total_time;
            }
            );

        }
    }
}
