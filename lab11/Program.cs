
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;

namespace lab5core
{
    class Program
    {
        static int num_iterations = 2200;
        static int num_population = 1000;
        static int num_arguments = 50;
        static double[,] arg;
        static double[][] arg2;

        static float porog_s = 0.2f;
        static float p_mut_c = 0.15f;

        static float p_select = 0.9f;
        static float p_cross = 0.5f;
        static float p_mutation = 0.05f;

        static void Main(string[] args)
        {

            work2(args);

            Console.ReadKey();
        }

        static void work1()
        {
            Stopwatch sw = new Stopwatch();
            arg = new double[num_population, num_arguments];
            genereate_population(arg);
            double[] res = new double[0];
            StreamWriter wr = new StreamWriter("results.txt");
            StreamWriter wr2 = new StreamWriter("results2.txt");
            StreamWriter wr3 = new StreamWriter("results3.txt");
            sw.Start();
            for (int i = 0; i < num_iterations; i++)
            {
                res = sphere(arg);
                sw.Stop();
                // Console.WriteLine(res.AsParallel().Min());
                if (i < num_iterations / 3)
                    wr.WriteLine(string.Format("{0}", Math.Round(res.AsParallel().Min(), 3)));
                else if (i >= num_iterations / 3 && i < num_iterations / (2f / 3f))
                    wr2.WriteLine(string.Format("{0}", Math.Round(res.AsParallel().Min(), 3)));
                else
                    wr3.WriteLine(string.Format("{0}", Math.Round(res.AsParallel().Min(), 3)));
                sw.Start();
                selective_2(res, arg);
                crossover_2(arg);
                mutation(arg);
            }
            res = sphere(arg);
            sw.Stop();
            wr3.WriteLine(Math.Round(res.AsParallel().Min(), 3));
            wr3.WriteLine(sw.Elapsed.TotalSeconds);
            Console.WriteLine("все!");
            Console.WriteLine("время: " + sw.Elapsed.TotalSeconds);
        }

        static void work2(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            MPI.Environment.Run(ref args, comm =>

             {

                 int switch_n = 500;
                 sw.Start();
                 arg2 = new double[num_population][];
                 for (int i = 0; i < num_population; i++)
                 {
                     arg2[i] = new double[num_arguments];
                 }
                 Random r = new Random();
                 double[] res = new double[0];
                 genereate_population(arg2);
                 StreamWriter wr = new StreamWriter("results1_" + comm.Rank + ".txt");
                 
                 //num_iterations /= MPI.Communicator.world.Size;
                 
                 Console.WriteLine(comm.Rank + "started");
                 for (int i = 0; i < num_iterations; i++)
                 {
                     res = sphere(arg2);
                     sw.Stop();
                     // Console.WriteLine(res.AsParallel().Min());
                     wr.WriteLine(string.Format("{0}", Math.Round(res.AsParallel().Min(), 3)));
                     
                     sw.Start();
                     selective_2(res, arg2);
                     crossover_2(arg2);
                     mutation(arg2);
                     //Console.WriteLine(comm.Rank + " " + i + "get " + res.AsParallel().Min());
                     if ((i + 1) % switch_n == 0)
                     {
                         //Console.WriteLine(comm.Rank + " " + i + "pribil ");
                         
                         List<double[]> to_moon = arg2.ToList();
                         double[][] to_moon_ar = new double[10][];
                         
                         for (int e = 0; e < 10; e++)
                         {
                             
                             double mn = sphere(to_moon[0]);

                             int inddd = 0;
                             for (int t = 1; t < to_moon.Count; t++)
                             {
                                 double ress = sphere(to_moon[t]);

                                 if (mn > ress)
                                 {
                                     mn = ress;
                                     inddd = t;
                                 }
                             }
                             to_moon_ar[e] = to_moon[inddd];
                             to_moon.RemoveAt(inddd);
                         }
                         //Console.WriteLine(comm.Rank + " " + i + "pribil3 ");
                         double[][][] to_earth = new double[comm.Size][][];

                         if (comm.Rank != 0)
                         {
                             RequestList requestList = new RequestList();
                             //Console.WriteLine(comm.Rank + " " + i + "send ");
                             
                             //Console.WriteLine(to_moon_ar[0][0]);
                             comm.Send(to_moon_ar, 0, 0);
                             //Console.WriteLine("_______________________________________________________" + comm.Rank);
                             
                             comm.Receive<double[]>(0, 0, ref to_moon_ar);
                             //Console.WriteLine(comm.Rank + " " + i + "wait ");
                             
                             //Console.WriteLine(comm.Rank + " " + i + " end wait __");
                             //requestList.Add(comm.ImmediateReceive<double[]>(0, 0, to_moon_ar));
                             for (int t = 0; t < to_moon_ar.Length; t++)
                             {
                                 to_moon.Add(to_moon_ar[t]);
                             }

                         }
                         else
                         {
                             RequestList requestList = new RequestList();
                             Request[] requests = new Request[10];
                             to_earth[0] = to_moon_ar;
                             for (int e = 1; e < comm.Size; e++)
                             {
                                
                                // Console.WriteLine(comm.Rank + " " + e + " recieve________________________________________");
                                 //requests[e] = comm.ImmediateReceive<double[]>(e, 0, to_earth[e]);
                                 comm.Receive<double[]>(e, 0, ref to_earth[e]);
                                 //requestList.Add(req);
                                 //Console.WriteLine(req.ToString());
                                 //var req = comm.ImmediateReceive<double[]>(e, 0, to_earth[e]);
                                 //req.Wait();
                                 //Console.WriteLine(comm.Rank + " " + e + " recieve " + st.ToString());

                                 //comm.Receive<double[]>(e, 0, ref to_earth[e]);
                             }
                             
                             //requestList.WaitAll();
                             //Console.WriteLine(comm.Rank + " " + " all recieve ");
                             int shufle_time = 100;
                             for (int e = 0; e < shufle_time; e++)
                             {
                                 int k1 = r.Next(0, to_earth.Length);
                                 int k2 = r.Next(0, to_earth.Length);
                                 int k3 = r.Next(0, to_earth[k1].Length);
                                 int k4 = r.Next(0, to_earth[k2].Length);
                                 double[] temp = to_earth[k1][k3];
                                 to_earth[k1][k3] = to_earth[k2][k4];
                                 to_earth[k2][k4] = temp;
                             }
                             for (int e = 1; e < comm.Size; e++)
                             {
                                 comm.Send<double[]>(to_earth[e], e, 0);
                             }
                             to_moon_ar = to_earth[0];
                             for (int t = 0; t < to_moon_ar.Length; t++)
                             {
                                 to_moon.Add(to_moon_ar[t]);
                             }
                         }
                         arg2 = to_moon.ToArray();
                     }
                 }
                 double[][][] to_earth2 = new double[comm.Size][][];
                 if (comm.Rank != 0)
                 {
                     comm.Send(arg2, 0, 0);
                 }
                 else
                 {
                     to_earth2[0] = arg2;
                     for (int e = 1; e < comm.Size; e++)
                     {
                         comm.Receive(e, 0, ref to_earth2[e]);
                     }
                     double[] ress = sphere(to_earth2[0]);
                     double min = ress.Min();
                     for (int t = 1; t < to_earth2.Length; t++)
                     {
                         ress = sphere(to_earth2[t]);
                         double tmin = ress.Min();
                         if (tmin < min)
                             min = tmin;
                     }
                     sw.Stop();
                     wr.WriteLine(string.Format("{0}", Math.Round(min, 3)));
                     wr.WriteLine(string.Format("{0}", sw.Elapsed.TotalSeconds));
                     Console.WriteLine("все!");
                     Console.WriteLine("время: " + sw.Elapsed.TotalSeconds);
                     

                 }
                 wr.Close();
             }
            );
             

                
                

            




            
        }

        static void genereate_population(double[,] arg)
        {
            Random r = new Random();
            for (int i = 0; i < arg.GetLength(0); i++)
            {
                for (int e = 0; e < arg.GetLength(1); e++)
                {
                    arg[i, e] = r.NextDouble() * 100.0f - 50.0f;
                }
            }
        }

        static void genereate_population(double[][] arg)
        {
            Random r = new Random();
            for (int i = 0; i < arg.Length; i++)
            {
                for (int e = 0; e < arg[i].Length; e++)
                {
                    arg[i][e] = r.NextDouble() * 100.0f - 50.0f;
                }
            }
        }


        static double[] sphere(double[,] arg)
        {
            double[] result = new double[num_population];

            //for (int i = 0; i < arg.GetLength(0); i++)
            //{
            //    for (int e = 0; e < arg.GetLength(1); e++)
            //    {
            //        result[i] += Math.Pow(arg[i, e] - 1, 2);
            //    }
            //}
            Parallel.ForEach(Partitioner.Create(0, arg.GetLength(0)), item =>
            {
                int i1 = ((Tuple<int, int>)item).Item1;
                int i2 = ((Tuple<int, int>)item).Item2;
                for (int i = i1; i < i2; i++)
                {
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        result[i] += Math.Pow(arg[i, e] - 1, 2);
                    }
                }
            });
            return result;
        }

        static double[] sphere(double[][] arg)
        {
            double[] result = new double[num_population];

            //for (int i = 0; i < arg.GetLength(0); i++)
            //{
            //    for (int e = 0; e < arg.GetLength(1); e++)
            //    {
            //        result[i] += Math.Pow(arg[i, e] - 1, 2);
            //    }
            //}
            Parallel.ForEach(Partitioner.Create(0, arg.Length), item =>
            {
                int i1 = ((Tuple<int, int>)item).Item1;
                int i2 = ((Tuple<int, int>)item).Item2;
                for (int i = i1; i < i2; i++)
                {
                    for (int e = 0; e < arg[i].Length; e++)
                    {
                        result[i] += Math.Pow(arg[i][e] - 1, 2);
                    }
                }
            });
            return result;
        }

        static double sphere(double[] arg)
        {
            double result = 0;



            for (int i = 0; i < arg.Length; i++)
            {
                
                    result += Math.Pow(arg[i] - 1, 2);
                
            }

            return result;
        }


        static List<int> selective(double[] res, float porog = -1)
        {
            if (porog == -1)
                porog = porog_s;
            List<int> changed_index = new List<int>();
            Random r = new Random();
            int am_select = (int)(num_arguments * p_select);
            for (int i = 0; i < am_select; i++)
            {
                int ia = r.Next(0, res.Length);
                int ib = r.Next(0, res.Length);
                if (res[ia] < res[ib] && r.NextDouble() <= porog ||
                    res[ia] > res[ib] && r.NextDouble() > porog)
                {
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        arg[ib, e] = arg[ia, e];
                    }
                    changed_index.Add(ib);
                }
                else
                {
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        arg[ia, e] = arg[ib, e];
                    }
                    changed_index.Add(ia);
                }
            }
            return changed_index.Distinct().ToList();
        }

        static void selective_2(double[] res, double[,] arg, float porog = -1)
        {
            if (porog == -1)
                porog = porog_s;

            Random r = new Random();
            int am_select = (int)(num_population * p_select);
            for (int i = 0; i < am_select; i++)
            {
                int ia = r.Next(0, res.Length);
                int ib = r.Next(0, res.Length);
                if (res[ia] < res[ib] && r.NextDouble() > porog ||
                    res[ia] > res[ib] && r.NextDouble() < porog)
                {
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        arg[ib, e] = arg[ia, e];
                    }

                }
                else
                {
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        arg[ia, e] = arg[ib, e];
                    }
                }
            }
        }

        static void selective_2(double[] res, double[][] arg, float porog = -1)
        {
            if (porog == -1)
                porog = porog_s;

            Random r = new Random();
            int am_select = (int)(num_population * p_select);
            for (int i = 0; i < am_select; i++)
            {
                int ia = r.Next(0, res.Length);
                int ib = r.Next(0, res.Length);
                if (res[ia] < res[ib] && r.NextDouble() > porog ||
                    res[ia] > res[ib] && r.NextDouble() < porog)
                {
                    Array.Copy(arg[ia], arg[ib],arg[ia].Length);
                    //arg[ib] = arg[ia];

                }
                else
                {
                    Array.Copy(arg[ib], arg[ia], arg[ia].Length);
                    //arg[ia] = arg[ib];
                }
            }
        }

        static void crossover(List<int> skr, double[,] arg)
        {
            Random r = new Random();
            int k;
            int am_cros = (int)(num_population * p_cross);
            int am_c = am_cros - skr.Count;
            for (int i = 0; i < skr.Count; i++)
            {
                int ind = r.Next(0, num_population);
                while (skr.Contains(ind))
                { ind = r.Next(0, num_population); }
                k = r.Next(0, arg.GetLength(1));
                for (int e = 0; e < k; e++)
                {
                    arg[ind, e] = arg[skr[i], e];
                }
                for (int e = k; e < arg.GetLength(1); e++)
                {
                    arg[skr[i], e] = arg[ind, e];
                }
                skr.RemoveAt(i);
                i = 0;
            }
            for (int i = 0; i < am_c; i++)
            {
                int ind = r.Next(0, num_population);
                int ind2 = r.Next(0, num_population);
                k = r.Next(0, arg.GetLength(1));
                for (int e = 0; e < k; e++)
                {
                    arg[ind, e] = arg[ind2, e];
                }
                for (int e = k; e < arg.GetLength(1); e++)
                {
                    arg[ind2, e] = arg[ind, e];
                }

            }
        }

        static void crossover_2(double[,] arg)
        {
            Random r = new Random();
            int k;
            int am_cros = (int)(num_population * p_cross);

            for (int i = 0; i < am_cros; i++)
            {
                int ind = r.Next(0, num_population);
                int ind2 = r.Next(0, num_population);
                k = r.Next(0, arg.GetLength(1));
                for (int e = 0; e < arg.GetLength(1); e++)
                {
                    if (r.NextDouble() <= 0.45f)
                        arg[ind, e] = arg[ind2, e];
                }
                //for (int e = 0; e < k; e++)
                //{
                //    arg[ind, e] = arg[ind2, e];
                //}
                //for (int e = k; e < arg.GetLength(1); e++)
                //{
                //    arg[ind2, e] = arg[ind, e];
                //}
            }
        }

        static void crossover_2(double[][] arg)
        {
            Random r = new Random();
            int k;
            int am_cros = (int)(num_population * p_cross);

            for (int i = 0; i < am_cros; i++)
            {
                int ind = r.Next(0, num_population);
                int ind2 = r.Next(0, num_population);
                k = r.Next(0, arg.Length);
                for (int e = 0; e < arg[ind].Length; e++)
                {
                    if (r.NextDouble() <= 0.45f)
                        arg[ind][e] = arg[ind2][e];
                }

            }
        }

        static void mutation(double[,] arg, float p_mut = -1)
        {
            Random r = new Random();
            if (p_mut == -1)
                p_mut = p_mut_c;
            int am_mut = (int)(p_mutation * num_population);
            //for (int i = 0; i < am_mut; i++)
            //{
            //    int k = r.Next(0, num_population);
            //    for (int e = 0; e < arg.GetLength(1); e++)
            //    {
            //        if (r.NextDouble() < p_mut)
            //            arg[k, e] = arg[k, e] + r.NextDouble() * 5f - 2.5f;
            //    }
            //}
            Parallel.ForEach(Partitioner.Create(0, am_mut), item =>
            {
                int i1 = ((Tuple<int, int>)item).Item1;
                int i2 = ((Tuple<int, int>)item).Item2;
                for (int i = i1; i < i2; i++)
                {
                    int k = r.Next(0, num_population);
                    for (int e = 0; e < arg.GetLength(1); e++)
                    {
                        if (r.NextDouble() < p_mut)
                            arg[k, e] = arg[k, e] + r.NextDouble() * 0.1f - 0.05f;
                    }
                }
            });
        }

        static void mutation(double[][] arg, float p_mut = -1)
        {
            Random r = new Random();
            if (p_mut == -1)
                p_mut = p_mut_c;
            int am_mut = (int)(p_mutation * num_population);
            //for (int i = 0; i < am_mut; i++)
            //{
            //    int k = r.Next(0, num_population);
            //    for (int e = 0; e < arg.GetLength(1); e++)
            //    {
            //        if (r.NextDouble() < p_mut)
            //            arg[k, e] = arg[k, e] + r.NextDouble() * 5f - 2.5f;
            //    }
            //}
            Parallel.ForEach(Partitioner.Create(0, am_mut), item =>
            {
                int i1 = ((Tuple<int, int>)item).Item1;
                int i2 = ((Tuple<int, int>)item).Item2;
                for (int i = i1; i < i2; i++)
                {
                    int k = r.Next(0, num_population);
                    for (int e = 0; e < arg[i].Length; e++)
                    {
                        if (r.NextDouble() < p_mut)
                            arg[k][e] = arg[k][e] + r.NextDouble() * 0.1f - 0.05f;
                    }
                }
            });
        }

    }
}

