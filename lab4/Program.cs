using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace zad4
{
    class Program
    {
        static Dictionary<char, int>[] symbols;
        static Dictionary<string, int>[] words;
        static Dictionary<string, int>[] bigramms;

        static Dictionary<char, int> all_symbols;
        static Dictionary<string, int> all_words;
        static Dictionary<string, int> all_bigramms;

        static ConcurrentDictionary<string, int> conc_words;
        static ConcurrentDictionary<string, int> conc_bigrams;
        static ConcurrentDictionary<char, int> conc_symbols;
        static int ammount_streams=0;
        static int ammount_files=0;

        static BlockingCollection<string> buferr = new BlockingCollection<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("потоки:");
            //ammount_streams = int.Parse(Console.ReadLine());
            Console.WriteLine("файлы:");
            // ammount_files = int.Parse(Console.ReadLine());
            ammount_streams = 8;
            ammount_files = 20;
            Stopwatch sw = new Stopwatch();

            conc_words = new ConcurrentDictionary<string, int>();
            conc_symbols = new ConcurrentDictionary<char, int>();
            conc_bigrams = new ConcurrentDictionary<string, int>();

            Thread[] threads = new Thread[ammount_streams];
            sw.Start();
            //1
            //symbols = new Dictionary<char, int>[ammount_streams];
            //words = new Dictionary<string, int>[ammount_streams];
            //bigramms = new Dictionary<string, int>[ammount_streams];
            //for (int i = 0; i < ammount_streams; i++)
            //{
            //    threads[i] = new Thread(reader_async);
            //    int diap = ammount_files / ammount_streams;

            //    threads[i].Start(new int[] { i * diap, i * diap + diap, i });

            //}

            //for (int i = 0; i < ammount_streams; i++)
            //{
            //    threads[i].Join();
            //}

            //all_words = new Dictionary<string, int>();
            //all_symbols = new Dictionary<char, int>();
            //all_bigramms = new Dictionary<string, int>();

            //for (int i = 0; i < ammount_streams; i++)
            //{
            //    foreach (KeyValuePair<string, int> kvp in words[i])
            //    {
            //        if (all_words.ContainsKey(kvp.Key))
            //        {
            //            all_words[kvp.Key] += kvp.Value;
            //        }
            //        else
            //        {
            //            all_words.Add(kvp.Key, kvp.Value);
            //        }
            //    }
            //    foreach (KeyValuePair<char, int> kvp in symbols[i])
            //    {
            //        if (all_symbols.ContainsKey(kvp.Key))
            //        {
            //            all_symbols[kvp.Key] += kvp.Value;
            //        }
            //        else
            //        {
            //            all_symbols.Add(kvp.Key, kvp.Value);
            //        }
            //    }
            //    foreach (KeyValuePair<string, int> kvp in bigramms[i])
            //    {
            //        if (all_bigramms.ContainsKey(kvp.Key))
            //        {
            //            all_bigramms[kvp.Key] += kvp.Value;
            //        }
            //        else
            //        {
            //            all_bigramms.Add(kvp.Key, kvp.Value);
            //        }
            //    }
            //}

            //concurent
            //for (int i = 0; i < ammount_streams; i++)
            //{
            //    threads[i] = new Thread(safely_reader_async);
            //    int diap = ammount_files / ammount_streams;

            //    threads[i].Start(new int[] { i * diap, i * diap + diap, i });

            //}
            //for (int i = 0; i < ammount_streams; i++)
            //{
            //    threads[i].Join();
            //}

            //3

            //Thread[] obrab = new Thread[10];
            //int amm_read = 10;
            //for (int i = 0; i < amm_read; i++)
            //{
            //    threads[i] = new Thread(reader_2);
            //    int diap = ammount_files / amm_read;

            //    threads[i].Start(new int[] { i * diap, i * diap + diap, i });

            //}
            //for (int i = 0; i < obrab.Length; i++)
            //{
            //    obrab[i] = new Thread(obrabotka);
            //    int diap = ammount_files / ammount_streams;

            //    obrab[i].Start(new int[] { i * diap, i * diap + diap, i });
            //}
            //for (int i = 0; i < amm_read; i++)
            //{
            //    threads[i].Join();
            //}
            //buferr.CompleteAdding();
            //for (int i = 0; i < obrab.Length; i++)
            //{
            //    obrab[i].Join();
            //}


            //posl
            symbols = new Dictionary<char, int>[1];
            words = new Dictionary<string, int>[1];
            bigramms = new Dictionary<string, int>[1];
            posledov(new int[] { 0, ammount_files, 0 });
            sw.Stop();
            Console.WriteLine(sw.Elapsed.TotalMilliseconds);
            Console.ReadKey();
        }


        static void reader_async(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1];
            int index = ((int[])par)[2];
            words[index] = new Dictionary<string, int>();
            symbols[index] = new Dictionary<char, int>();
            bigramms[index] = new Dictionary<string, int>();
            string bigr = "";
            for (int i = start;i<end;i++)
            {
                StreamReader re = new StreamReader("file" + i.ToString() + ".txt");
                while(!re.EndOfStream)
                {
                    string line = re.ReadLine();
                    {
                        line = line.Replace(".", " ");
                        line = line.Replace(",", " ");
                        line = line.Replace("!", " ");
                        line = line.Replace("?", " ");
                        line = line.Replace(":", " ");
                        line = line.Replace(";", " ");
                        line = line.Replace("-", " ");
                        line = line.Replace("'", " ");
                        line = line.Replace("\n", " ");
                        line = line.Replace("\"", " ");
                        line = line.Replace("1", " ");
                        line = line.Replace("2", " ");
                        line = line.Replace("3", " ");
                        line = line.Replace("4", " ");
                        line = line.Replace("5", " ");
                        line = line.Replace("6", " ");
                        line = line.Replace("7", " ");
                        line = line.Replace("8", " ");
                        line = line.Replace("9", " ");
                        line = line.Replace("0", " ");
                        line = line.Replace("%", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.ToLower();
                    }
                    string[] words2 = line.Split(' ');
                    for(int e=0;e<words2.Length;e++)
                    {
                        if(!words[index].ContainsKey(words2[e]))
                        {
                            words[index].Add(words2[e], 1);
                        }
                        else
                        {
                            words[index][words2[e]]++;
                        }
                        
                        for (int r=0;r< words2[e].Length; r++)
                        {
                            
                            if (!symbols[index].ContainsKey(words2[e][r]))
                            {
                                symbols[index].Add(words2[e][r], 1);
                            }
                            else
                            {
                                symbols[index][words2[e][r]]++;
                            }
                            if(r< words2[e].Length-1)
                            {
                                bigr = words2[e][r].ToString() + words2[e][r + 1].ToString();

                                if (!bigramms[index].ContainsKey(bigr))
                                {
                                    bigramms[index].Add(bigr, 1);
                                }
                                else
                                {
                                    bigramms[index][bigr]++;
                                }
                            }
                        }
                    }
                }
            }
        }


        static void safely_reader_async(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1];
            int index = ((int[])par)[2];
            
            string bigr = "";
            for (int i = start; i < end; i++)
            {
                StreamReader re = new StreamReader("file" + i.ToString() + ".txt");
                while (!re.EndOfStream)
                {
                    string line = re.ReadLine();
                    {
                        line = line.Replace(".", " ");
                        line = line.Replace(",", " ");
                        line = line.Replace("!", " ");
                        line = line.Replace("?", " ");
                        line = line.Replace(":", " ");
                        line = line.Replace(";", " ");
                        line = line.Replace("-", " ");
                        line = line.Replace("'", " ");
                        line = line.Replace("\n", " ");
                        line = line.Replace("\"", " ");
                        line = line.Replace("1", " ");
                        line = line.Replace("2", " ");
                        line = line.Replace("3", " ");
                        line = line.Replace("4", " ");
                        line = line.Replace("5", " ");
                        line = line.Replace("6", " ");
                        line = line.Replace("7", " ");
                        line = line.Replace("8", " ");
                        line = line.Replace("9", " ");
                        line = line.Replace("0", " ");
                        line = line.Replace("%", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.ToLower();
                    }
                    string[] words2 = line.Split(' ');
                    for (int e = 0; e < words2.Length; e++)
                    {
                        conc_words.AddOrUpdate(words2[e], 1, (k, old) => old + 1);

                        for (int r = 0; r < words2[e].Length; r++)
                        {

                            conc_symbols.AddOrUpdate(words2[e][r], 1, (k, old) => old + 1);
                            if (r < words2[e].Length - 1)
                            {
                                bigr = words2[e][r].ToString() + words2[e][r + 1].ToString();

                                conc_bigrams.AddOrUpdate(bigr, 1, (k, old) => old + 1);
                            }
                        }
                    }
                }
            }
        }


        static void reader_2(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1];
            int index = ((int[])par)[2];
            for (int i = start; i < end; i++)
            {
                StreamReader re = new StreamReader("file" + i.ToString() + ".txt");
                while (!re.EndOfStream)
                {
                    string line = re.ReadLine();
                    {
                        line = line.Replace(".", " ");
                        line = line.Replace(",", " ");
                        line = line.Replace("!", " ");
                        line = line.Replace("?", " ");
                        line = line.Replace(":", " ");
                        line = line.Replace(";", " ");
                        line = line.Replace("-", " ");
                        line = line.Replace("'", " ");
                        line = line.Replace("\n", " ");
                        line = line.Replace("\"", " ");
                        line = line.Replace("1", " ");
                        line = line.Replace("2", " ");
                        line = line.Replace("3", " ");
                        line = line.Replace("4", " ");
                        line = line.Replace("5", " ");
                        line = line.Replace("6", " ");
                        line = line.Replace("7", " ");
                        line = line.Replace("8", " ");
                        line = line.Replace("9", " ");
                        line = line.Replace("0", " ");
                        line = line.Replace("%", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.ToLower();
                    }
                    bool b = buferr.TryAdd(line);
                   
                    
                }
            }
        }

        static void obrabotka(object par)
        {
            string line;
            string bigr;
           while (!buferr.IsCompleted)
            {
                bool b = buferr.TryTake(out line);
                
                if(b)
                    
                    {
                        string[] words2 = line.Split(' ');
                        for (int e = 0; e < words2.Length; e++)
                        {
                            conc_words.AddOrUpdate(words2[e], 1, (k, old) => old + 1);

                            for (int r = 0; r < words2[e].Length; r++)
                            {

                                conc_symbols.AddOrUpdate(words2[e][r], 1, (k, old) => old + 1);
                                if (r < words2[e].Length - 1)
                                {
                                    bigr = words2[e][r].ToString() + words2[e][r + 1].ToString();

                                    conc_bigrams.AddOrUpdate(bigr, 1, (k, old) => old + 1);
                                }
                            }
                        }
                    }
                


            }
        }

        static void posledov(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1];
            int index = ((int[])par)[2];
            words[index] = new Dictionary<string, int>();
            symbols[index] = new Dictionary<char, int>();
            bigramms[index] = new Dictionary<string, int>();
            string bigr = "";
            for (int i = start; i < end; i++)
            {
                StreamReader re = new StreamReader("file" + i.ToString() + ".txt");
                while (!re.EndOfStream)
                {
                    string line = re.ReadLine();
                    {
                        line = line.Replace(".", " ");
                        line = line.Replace(",", " ");
                        line = line.Replace("!", " ");
                        line = line.Replace("?", " ");
                        line = line.Replace(":", " ");
                        line = line.Replace(";", " ");
                        line = line.Replace("-", " ");
                        line = line.Replace("'", " ");
                        line = line.Replace("\n", " ");
                        line = line.Replace("\"", " ");
                        line = line.Replace("1", " ");
                        line = line.Replace("2", " ");
                        line = line.Replace("3", " ");
                        line = line.Replace("4", " ");
                        line = line.Replace("5", " ");
                        line = line.Replace("6", " ");
                        line = line.Replace("7", " ");
                        line = line.Replace("8", " ");
                        line = line.Replace("9", " ");
                        line = line.Replace("0", " ");
                        line = line.Replace("%", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.Replace("  ", " ");
                        line = line.ToLower();
                    }
                    string[] words2 = line.Split(' ');
                    for (int e = 0; e < words2.Length; e++)
                    {
                        if (!words[index].ContainsKey(words2[e]))
                        {
                            words[index].Add(words2[e], 1);
                        }
                        else
                        {
                            words[index][words2[e]]++;
                        }

                        for (int r = 0; r < words2[e].Length; r++)
                        {

                            if (!symbols[index].ContainsKey(words2[e][r]))
                            {
                                symbols[index].Add(words2[e][r], 1);
                            }
                            else
                            {
                                symbols[index][words2[e][r]]++;
                            }
                            if (r < words2[e].Length - 1)
                            {
                                bigr = words2[e][r].ToString() + words2[e][r + 1].ToString();

                                if (!bigramms[index].ContainsKey(bigr))
                                {
                                    bigramms[index].Add(bigr, 1);
                                }
                                else
                                {
                                    bigramms[index][bigr]++;
                                }
                            }
                        }
                    }
                }
            }
        }

    }
}
