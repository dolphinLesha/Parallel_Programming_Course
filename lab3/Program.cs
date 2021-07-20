using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zad3
{
    class Program
    {

        static string buffer ="lul";
        static bool bEmpty = true;
        static int iEmpty = 0;
        static int wr_in = 0;
        static int re_in = 0;
        static bool finish_writers = false;

        static AutoResetEvent ev_empty;
        static AutoResetEvent ev_full;

        static Semaphore wr_sem = new Semaphore(1, 1);
        static Semaphore re_sem = new Semaphore(1, 1);

        static int len_of_elements=5;
        static int ammount_mes = 1000000;

        static string[,] mes;



        static void Main(string[] args)
        {
            Console.WriteLine("thread num = ");
            int m = int.Parse(Console.ReadLine());

            Stopwatch sw = new Stopwatch();
            Thread[] writers = new Thread[m];
            Thread[] readers = new Thread[m];
            mes = new string[m, ammount_mes];
            for(int i=0;i<m;i++)
            {
                for (int e = 0; e < ammount_mes; e++)
                {
                    mes[i, e] =  "".PadLeft(len_of_elements, 'D') + i + " " + e;
                }
            }
            count_thread = new int[m];
            ev_empty = new AutoResetEvent(true);
            ev_full = new AutoResetEvent(false);
            ev_empty.Set();
            sw.Start();
            //simple
            //for (int i = 0; i < m; i++)
            //{
            //    writers[i] = new Thread(writer_simple);
            //    readers[i] = new Thread(reader_simple);
            //    writers[i].Start(new int[] { i });
            //    readers[i].Start(new int[] { i });
            //}


            //lock
            //for (int i = 0; i < m; i++)
            //{
            //    writers[i] = new Thread(writer_lock);
            //    readers[i] = new Thread(reader_lock);
            //    writers[i].Start(new int[] { i });
            //    readers[i].Start(new int[] { i });
            //}

            //signal
            for (int i = 0; i < m; i++)
            {
                writers[i] = new Thread(writer_signal);
                readers[i] = new Thread(reader_signal);
                writers[i].Start(new int[] { i });
                readers[i].Start(new int[] { i });
            }

            //semaphore
            //for (int i = 0; i < m; i++)
            //{
            //    writers[i] = new Thread(writer_sema);
            //    readers[i] = new Thread(reader_sema);
            //    writers[i].Start(new int[] { i });
            //    readers[i].Start(new int[] { i });
            //}

            //interlocked
            //for (int i = 0; i < m; i++)
            //{
            //    writers[i] = new Thread(writer_Inter);
            //    readers[i] = new Thread(reader_Inter);
            //    writers[i].Start(new int[] { i });
            //    readers[i].Start(new int[] { i });
            //}

            for (int i = 0;i<m;i++)
            {
                writers[i].Join();
                //readers[i].Join();
            }
            //Console.WriteLine("finish is true");
            
            finish_writers = true;
            ev_full.Set();
            //finish = true;
            for (int i = 0; i < m; i++)
            {
                readers[i].Join();
            }
            sw.Stop();
            Console.WriteLine("time: " + sw.Elapsed.TotalMilliseconds);
            for(int i=0;i<m;i++)
            {
                Console.WriteLine(100 * ((float)count_thread[i] / (float)counter));
            }
            Console.WriteLine(counter);


            Console.ReadKey();
        }

        

        static void writer_simple(object par)
        {
            int index = ((int[])par)[0];
            int current = 0;
            while(current<ammount_mes)
            {
                if(bEmpty)
                {
                    buffer = mes[index,current++];
                    bEmpty = false;
                }
            }
        }

        static void reader_simple(object par)
        {
            int index = ((int[])par)[0];
            List<string> messages = new List<string>();
            while (!finish_writers)
            {
                if (!bEmpty)
                {
                    messages.Add(buffer);
                    bEmpty = true;
                }
            }
            lock ("w")
            {
                counter += messages.Count;
            }
            count_thread[index] = messages.Count;
            //for (int i=0;i<messages.Count;i++)
            //{
            //    Console.WriteLine(index + " read " + messages[i]);
            //}
        }

        static void writer_lock(object par)
        {
            int index = ((int[])par)[0];
            int current = 0;
            while (current < ammount_mes)
            {
                lock("writer")
                {
                    if (bEmpty)
                    {
                        buffer = mes[index,current++];
                        bEmpty = false;
                    }
                }
                
            }
        }
        static int counter=0;
        static int[] count_thread;

        static void reader_lock(object par)
        {
            int index = ((int[])par)[0];
            List<string> messages = new List<string>();
            while (!finish_writers)
            {
                if (!bEmpty)
                {
                    lock("reader")
                    {
                        if (!bEmpty)
                        {
                            messages.Add(buffer);
                            bEmpty = true;

                        }
                    }
                    
                }
            }
            lock ("w")
            {
                counter += messages.Count;
            }
            count_thread[index] = messages.Count;
            //for (int i = 0; i < messages.Count; i++)
            //{
            //    Console.WriteLine(index + " read " + messages[i]);
            //}

        }

        static void writer_signal(object par)
        {
            int index = ((int[])par)[0];
            int current = 0;
            while (current < ammount_mes)
            {
                ev_empty.WaitOne();
                buffer = current.ToString();
                current++;
                // bEmpty = false;

                ev_full.Set();

            }
            //Console.WriteLine("writer " + index + " end");
        }
        

        static void reader_signal(object par)
        {
            int index = ((int[])par)[0];
            List<string> messages = new List<string>();
            while (true)
            {
                ev_full.WaitOne();
                if (!finish_writers)
                {
                    messages.Add(buffer);
                   // Console.WriteLine(buffer);
                    
                }
                else
                {
                    //Console.WriteLine("r" + index);
                    
                    counter += messages.Count;
                    
                    count_thread[index] = messages.Count;
                    ev_full.Set();
                    //Console.WriteLine("reader " + index + " end"); 
                    break;
                }
                ev_empty.Set();
                
            }
            //for (int i = 0; i < messages.Count; i++)
            //{
            //    Console.WriteLine(index + " read " + messages[i]);
            //}

        }

        static void writer_sema(object par)
        {
            int index = ((int[])par)[0];
            
            int current = 0;
            while (current < ammount_mes)
            {
                wr_sem.WaitOne();
                    if (bEmpty)
                    {
                        buffer = mes[index,current++];
                        bEmpty = false;
                    }
                wr_sem.Release();

            }
        }
        

        static void reader_sema(object par)
        {
            int index = ((int[])par)[0];
            List<string> messages = new List<string>();
            while (!finish_writers)
            {
                
                    re_sem.WaitOne();
                        if (!bEmpty)
                        {
                            messages.Add(buffer);
                            bEmpty = true;

                        }
                    re_sem.Release();

                
            }
            lock ("w")
            {
                counter += messages.Count;
            }
            count_thread[index] = messages.Count;
            //for (int i = 0; i < messages.Count; i++)
            //{
            //    Console.WriteLine(index + " read " + messages[i]);
            //}

        }

        static void writer_Inter(object par)
        {
            int index = ((int[])par)[0];
            int current = 0;
            while (current < ammount_mes)
            {
                if(0==Interlocked.Exchange(ref wr_in,1))
                {
                    if (bEmpty)
                    {
                        buffer = mes[index,current++];
                        bEmpty = false;
                    }
                    Interlocked.Exchange(ref wr_in,0);
                }
                    
                

            }
        }
        

        static void reader_Inter(object par)
        {
            int index = ((int[])par)[0];
            List<string> messages = new List<string>();
            while (!finish_writers)
            {

                if (!bEmpty)
                {
                    if(0==Interlocked.Exchange(ref re_in,1))
                    {
                        if (!bEmpty)
                        {
                            messages.Add(buffer);
                            bEmpty = true;

                        }
                        Interlocked.Exchange(ref re_in, 0);
                    }

                }
            }
            lock ("w")
            {
                counter += messages.Count;
            }
            count_thread[index] = messages.Count;
            //for (int i = 0; i < messages.Count; i++)
            //{
            //    Console.WriteLine(index + " read " + messages[i]);
            //}

        }


    }
}
