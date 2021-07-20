using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab1
{ 

    class task_info
    {
        public double ms_s;
        public double ms_e;
        public string file_name = "";
        public int thrId;
        public int tskId;
        public task_info(double s, double e, string file, int th_id, int ta_id)
        {
            ms_s = s;
            ms_e = e;
            file_name = file;
            thrId = th_id;
            tskId = ta_id;
        }
    }

    
    class main_process
    {

        static string[] path_into;
        static string[] path_out;

        static int amm_files = 72;

        static BlockingCollection<task_info> collection = new BlockingCollection<task_info>();



        static void Main(string[] args)
        {
           
            create_paths();
            posl(amm_files/8);
            //parFor(amm_files, 5);

        }

        static void create_paths()
        {
            path_into = new string[amm_files];
            path_out = new string[amm_files];
            string cur = Directory.GetCurrentDirectory();
            for (int i = 0;i<amm_files;i++)
            {
                path_into[i] = cur + "/picts/pict" + (i+1).ToString() + ".jpg";
                path_out[i] = cur + "/picts_r/pict" + (i + 1).ToString() + ".jpg";
            }
        }
        static Stopwatch pro_sw = new Stopwatch();
        public static void proc_image(string img_path, string img_save)
        {
            
            Bitmap pict = new Bitmap(img_path);
            
            double ms_s = pro_sw.Elapsed.TotalSeconds;
            


            for (int i = 0; i < pict.Height; i++)
            {
                for (int e = 0; e < pict.Width; e++)
                {
                    var pix = pict.GetPixel(e, i);
                    var c = 0.2989f * pix.R + 0.5870f * pix.G + 0.1140f * pix.B;
                    if (c < 255 / 3.9f)
                        c = 0;
                    else if (c < 255 / 1.8f)
                        c = 255 / 4;
                    else if (c < 255 / 4 * 3.5f)
                        c = 255 / 4 * 3;
                    else
                        c = 255;

                    pict.SetPixel(e, i, Color.FromArgb((int)c, (int)c, (int)c));
                }
            }
            
            double ms_e = pro_sw.Elapsed.TotalSeconds;
            int thr_id = Thread.CurrentThread.ManagedThreadId;
            if(Task.CurrentId!=null)
            {
                collection.TryAdd(new task_info(ms_s, ms_e, Path.GetFileNameWithoutExtension(img_path), thr_id, Task.CurrentId.Value));
            }
            
            pict.Save(img_save);

        }

        public static void proc_image(string img_path)
        {

            Bitmap pict = new Bitmap(img_path);

            double ms_s = pro_sw.Elapsed.TotalSeconds;



            for (int i = 0; i < pict.Height; i++)
            {
                for (int e = 0; e < pict.Width; e++)
                {
                    var pix = pict.GetPixel(e, i);
                    var c = 0.2989f * pix.R + 0.5870f * pix.G + 0.1140f * pix.B;
                    if (c < 255 / 4)
                        c = 0;
                    else if (c < 255 / 2)
                        c = 255 / 4;
                    else if (c < 255 / 4 * 3)
                        c = 255 / 4 * 3;
                    else
                        c = 255;

                    pict.SetPixel(e, i, Color.FromArgb((int)c, (int)c, (int)c));
                }
            }

            double ms_e = pro_sw.Elapsed.TotalSeconds;
            int thr_id = Thread.CurrentThread.ManagedThreadId;
            if (Task.CurrentId != null)
            {
                collection.TryAdd(new task_info(ms_s, ms_e, Path.GetFileNameWithoutExtension(img_path), thr_id, Task.CurrentId.Value));
            }
            string img_save = "picts_r/" + Path.GetFileNameWithoutExtension(img_path)+".jpg";
            pict.Save(img_save);

        }

        static int i_global;
        static int posl_cancel = 0;

        static void posl(int amm)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            double ms_s = sw.Elapsed.TotalSeconds;
            Thread thr_progress = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.WriteLine(((int)(((float)i_global/(float)amm)*100f)).ToString() + "%");
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            //Interlocked.Increment(ref posl_cancel);
                            posl_cancel = 1;
                            break;
                        }
                    }
                }
            });
            thr_progress.Start();
            for (i_global=0; i_global < amm; i_global++)
            {
                proc_image(path_into[i_global], path_out[i_global]);
                if (posl_cancel == 1)
                {
                    Console.WriteLine("stop");
                    break;
                }
                    
            }
            
            sw.Stop();
            double ms_e = sw.Elapsed.TotalSeconds;
            log("posled", amm.ToString(), "0", "0", (ms_e - ms_s).ToString(),(posl_cancel==1 ? "yes" : "no"));
            //thr_progress.Abort();
        }

        static void log(params string[] type)
        {
            StreamReader re = new StreamReader("id_val.txt");
            int num = int.Parse(re.ReadLine());
            re.Close();
            StreamWriter wr2 = new StreamWriter("id_val.txt");
            wr2.WriteLine(num + 1);
            wr2.Close();
            StreamWriter wr = new StreamWriter("log/log" + num + ".txt");
            wr.WriteLine("exp: " + type[0]);
            wr.WriteLine("amm files: " + type[1]);
            wr.WriteLine("amm threads: " + type[2]);
            wr.WriteLine("main thread: " + type[3]);
            wr.WriteLine("total time: " + type[4]);
            wr.WriteLine("cancel: " + type[5]);
            task_info[] temp = collection.ToArray();
            wr.Write("task start: ");
            wr.Write("task end:   " );
            wr.Write("file:   " );
            wr.Write("task: " );
            wr.WriteLine("thread: " );
            for (int i=0;i< temp.Length;i++)
            {
                wr.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t", temp[i].ms_s.ToString(), temp[i].ms_e.ToString(), temp[i].file_name.ToString(),
                    temp[i].tskId.ToString(), temp[i].thrId.ToString()));
                //wr.Write(temp[i].ms_s.ToString().PadRight(12));
                //wr.Write(temp[i].ms_e.ToString().PadRight(12));
                //wr.Write(temp[i].file_name.ToString().PadRight(8));
                //wr.Write(temp[i].tskId.ToString().PadRight(6));
                //wr.WriteLine(temp[i].thrId.ToString().PadRight(8));
            }
            wr.Close();
        }

        static void parFor(int amm, int thr_amm)
        {
            //Console.WriteLine("ParallelStd: " + num + " thrcount: " + thrCount);

            Stopwatch sw = new Stopwatch();
            
            sw.Start();
            double ms_s = sw.Elapsed.TotalSeconds;
            CancellationTokenSource cts = new CancellationTokenSource();
            Thread thr_cancel = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            cts.Cancel();
                            break;
                        }
                    }
                }
            });
            thr_cancel.Start();
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                //MaxDegreeOfParallelism = thr_amm 
            };
            pro_sw.Start();
            try
            {
                Parallel.For(0, amm, options, i => { proc_image(path_into[i],path_out[i]); });
            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("stop");
            }
            pro_sw.Stop();
            double ms_e = sw.Elapsed.TotalSeconds;

            log("parfornon", amm.ToString(), thr_amm.ToString(), Thread.CurrentThread.ManagedThreadId.ToString(), (ms_e - ms_s).ToString(), "no");
            Console.WriteLine("vse");

        }

        static void parStat(int amm,int thr_amm)
        {
            //Console.WriteLine("ParallelStd: " + num + " thrcount: " + thrCount);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            double ms_s = sw.Elapsed.TotalSeconds;
            CancellationTokenSource cts = new CancellationTokenSource();
            Thread thr_cancel = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            cts.Cancel();
                            break;
                        }
                    }
                }
            });
            thr_cancel.Start();
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = thr_amm
            };
            pro_sw.Start();
            try
            {
                Parallel.ForEach(Partitioner.Create(0, amm), options, range => { 
                    for(int i=range.Item1; i<range.Item2;i++)
                    {
                        proc_image(path_into[i], path_out[i]);
                    }
                    });

            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("stop");
            }
            pro_sw.Stop();
            double ms_e = sw.Elapsed.TotalSeconds;

            log("parstatno", amm.ToString(), thr_amm.ToString(), Thread.CurrentThread.ManagedThreadId.ToString(), (ms_e - ms_s).ToString(), "no");
            Console.WriteLine("vse");

        }
        static void parDyn(int amm, int thr_amm)
        {
            //Console.WriteLine("ParallelStd: " + num + " thrcount: " + thrCount);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            double ms_s = sw.Elapsed.TotalSeconds;
            CancellationTokenSource cts = new CancellationTokenSource();
            Thread thr_cancel = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(100);
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Escape)
                        {
                            cts.Cancel();
                            break;
                        }
                    }
                }
            });
            thr_cancel.Start();
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = thr_amm
            };
            pro_sw.Start();
            try
            {
                Parallel.ForEach(Partitioner.Create(path_into,true), options, (into)=> {
                    proc_image(into);
                });

            }
            catch (OperationCanceledException o)
            {
                Console.WriteLine("stop");
            }
            pro_sw.Stop();
            double ms_e = sw.Elapsed.TotalSeconds;

            log("pardyn", amm.ToString(), thr_amm.ToString(), Thread.CurrentThread.ManagedThreadId.ToString(), (ms_e - ms_s).ToString(), "no");
            Console.WriteLine("vse");

        }
    }
}
