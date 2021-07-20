using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.DesignerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lab2
{
    internal class Program
    {
        private static Dictionary<string, int> dic_words = new Dictionary<string, int>();
        private static Dictionary<int, string> dic_lenwords = new Dictionary<int, string>();
        private static ConcurrentDictionary<string, int> c_dic_words = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<int,Dictionary<int, string>> c_dic_lenwords = new ConcurrentDictionary<int, Dictionary<int, string>>();
        private static List<string>[] m_l_words;

        private static DataTable dt_words = new DataTable();

        private static int maxam = 5;

        public struct chast_word
        {
            public string posled;
            public string linq;
            public string plinq;
            public string parfor;
        }
        
        
        
        
        public static void Main(string[] args)
        {
            Console.WriteLine("posled=================");
            posled();
            Console.WriteLine("link=================");
            linqsub();
            Console.WriteLine("linkpar=================");
            linqparsub();
            Console.WriteLine("parfor=================");
            parfor();

            Console.ReadKey();
        }

        private static int max_len_word = 0;
        static (int,string) max_count_word = (0,"");
        private static ConcurrentDictionary<int,int> c_max_len_word = new ConcurrentDictionary<int, int>();
        static (int,string) c_max_count_word = (0,"");
        static void posled()
        {
            string filen = "files/file";
            double total_time_task1=0;
            double total_time_task2=0;
            string[] file_names=new string[maxam];
            Stopwatch sw = new Stopwatch();
            for (int i = 0; i < maxam; i++)
            {
                file_names[i] = filen + i.ToString() + ".txt";
            }
            sw.Start();
            for (int i = 0; i < maxam; i++)
            {
                total_time_task1+= read_file_task1(filen+i.ToString()+".txt");
            }

            total_time_task1 = sw.Elapsed.TotalSeconds;
            sw.Restart();
            for (int i = 0; i < maxam; i++)
            {
                total_time_task2+=read_file_task2(filen+i.ToString()+".txt");
            }
            sw.Stop();
            total_time_task2 = sw.Elapsed.TotalSeconds;
            Console.WriteLine("chast word: " + max_count_word.Item2.ToString() +
                              "\nwith " + max_count_word.Item1.ToString() + " iterations");
            Console.WriteLine("total time task1: " + Math.Round(total_time_task1,3).ToString() + " sec");
            string[] words_len = dic_lenwords[max_len_word].Split(',');
            Console.WriteLine("first 10 total length words with length: " + max_len_word.ToString());
            int dd = (words_len.Length < 9) ? words_len.Length : 10;
            for(int i=0;i<dd;i++)
            {
                Console.Write(words_len[i] + ", ");
            }
            Console.WriteLine("\ntotal time task2: " + Math.Round(total_time_task2,3).ToString() + " sec");
            var result3 = read_file_task3(file_names);
            Console.WriteLine("duplicate files task: {0} \nwith {1} duplicates",result3.Item1,
                result3.Item2.ToString());
            Console.WriteLine("total time task3: " + Math.Round(result3.Item3,3).ToString() + " sec");
        }

        static void linqsub()
        {
            string filen = "files/file";
            string[] file_names=new string[maxam];
            for (int i = 0; i < maxam; i++)
            {
                file_names[i] = filen + i.ToString() + ".txt";
            }

            var result = read_file_linq1(file_names);
            Console.WriteLine("chast word linq: {0} \nwith {1} iterations",result.Item1,
                result.Item2.ToString());
            Console.WriteLine("total time linq1: " + Math.Round(result.Item3,3).ToString() + " sec");
            var result2 = read_file_linq2(file_names);
            
            
            int dd = (result2.Item2.Count < 9) ? result2.Item2.Count : 10;
            Console.WriteLine("first " + dd.ToString() + " total length words with length: " + result2.Item1.ToString());
            for(int i=0;i<dd;i++)
            {
                Console.Write(result2.Item2[i] + (i==dd-1 ? "" : ", "));
            }
            Console.WriteLine("\ntotal time linq2: " + Math.Round(result2.Item3,3).ToString() + " sec");

            var result3 = read_file_linq3(file_names);
            Console.WriteLine("duplicate files linq: {0} \nwith {1} duplicates",result3.Item2,
                result3.Item1.ToString());
            Console.WriteLine("total time linq3: " + Math.Round(result3.Item3,3).ToString() + " sec");
            
        }
        
        static void linqparsub()
        {
            string filen = "files/file";
            string[] file_names=new string[maxam];
            for (int i = 0; i < maxam; i++)
            {
                file_names[i] = filen + i.ToString() + ".txt";
            }

            var result = read_file_linqpar1(file_names);
            Console.WriteLine("chast word linq: {0} \nwith {1} iterations",result.Item1,
                result.Item2.ToString());
            Console.WriteLine("total time linq1par: " + Math.Round(result.Item3,3).ToString() + " sec");
            var result2 = read_file_linqpar2(file_names);
            
            
            int dd = (result2.Item2.Count < 9) ? result2.Item2.Count : 10;
            Console.WriteLine("first " + dd.ToString() + " total length words with length: " + result2.Item1.ToString());
            for(int i=0;i<dd;i++)
            {
                Console.Write(result2.Item2[i] + (i==dd-1 ? "" : ", "));
            }
            Console.WriteLine("\ntotal time linq2par: " + Math.Round(result2.Item3,3).ToString() + " sec");

            var result3 = read_file_linqpar3(file_names);
            Console.WriteLine("duplicate files linqpar: {0} \nwith {1} duplicates",result3.Item2,
                result3.Item1.ToString());
            Console.WriteLine("total time linq3par: " + Math.Round(result3.Item3,3).ToString() + " sec");
            
        }
        
        static void parfor()
        {
            string filen = "files/file";
            double total_time_task1=0;
            double total_time_task2=0;
            string[] file_names=new string[maxam];
            Stopwatch sw = new Stopwatch();
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4 
            };
            for (int i = 0; i < maxam; i++)
            {
                file_names[i] = filen + i.ToString() + ".txt";
            }
            sw.Start();
            Parallel.ForEach(Partitioner.Create(0, maxam), options, range => { 
                for(int i=range.Item1; i<range.Item2;i++)
                {
                    read_file_parfor1(filen+i.ToString()+".txt");
                }
            });
            foreach (var pair in c_dic_words)
            {
                if (pair.Value > c_max_count_word.Item1)
                {
                    c_max_count_word.Item1 = pair.Value;
                    c_max_count_word.Item2 = pair.Key;
                }
            }
            total_time_task1 = sw.Elapsed.TotalSeconds;
            int partit = 4;
            
            sw.Restart();
            Parallel.ForEach(Partitioner.Create(0, maxam,partit), options, range => { 
                for(int i=range.Item1; i<range.Item2;i++)
                {
                    read_file_parfor2(filen+i.ToString()+".txt");
                }
            });
            int mm = 0;
            string words = "";
            foreach (var pair in c_max_len_word)
            {
                if (pair.Value > mm)
                    mm = pair.Value;
            }
            foreach (var dic in c_dic_lenwords)
            {
                if (dic.Value.ContainsKey(mm))
                    words += dic.Value[mm];
            }
            
            sw.Stop();
            
            total_time_task2 = sw.Elapsed.TotalSeconds;
            Console.WriteLine("chast word: " + c_max_count_word.Item2.ToString() +
                              "\nwith " + c_max_count_word.Item1.ToString() + " iterations");
            Console.WriteLine("total time parfor1: " + Math.Round(total_time_task1,3).ToString() + " sec");
            string[] words_len = words.Split(',');
            Console.WriteLine("first 10 total length words with length: " + max_len_word.ToString());
            int dd = (words_len.Length < 9) ? words_len.Length : 10;
            for(int i=0;i<dd;i++)
            {
                Console.Write(words_len[i] + ", ");
            }
            Console.WriteLine("\ntotal time parfor2: " + Math.Round(total_time_task2,3).ToString() + " sec");
            var result3 = read_file_parfor3(file_names);
            Console.WriteLine("duplicate files parfor: {0} \nwith {1} duplicates",result3.Item1,
                result3.Item2.ToString());
            Console.WriteLine("total time parfor3: " + Math.Round(result3.Item3,3).ToString() + " sec");
        }

        static double read_file_task1(string fname)
        {
            Stopwatch sw = new Stopwatch();
            StreamReader re = new StreamReader(fname);
            string except = "1234567890!\"/*';.,??()_-";
            
            string line;
            string[] words;
            sw.Start();
            while (!re.EndOfStream)
            {
                line = re.ReadLine();
                
                // foreach (char simb in except)
                // {
                //     line = line.Replace(simb.ToString(),"");
                // }
                // line = line.Replace("-"," ");
                // line = line.Replace("  "," ");
                // if(line=="")
                //     continue;
                // words = line.Split(' ');
                words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0',';'
                },StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    //task1
                    // if(word=="")
                    //     continue;
                    if (dic_words.ContainsKey(word))
                        dic_words[word]++;
                    else
                        dic_words.Add(word, 1);
                    if (max_count_word.Item1 < dic_words[word])
                    {
                        max_count_word.Item1 = dic_words[word];
                        max_count_word.Item2 = word;
                    }
                        
                }
            }
            
            
            re.Close();
            sw.Stop();
            // Console.WriteLine("file " + fname + " expired");
            // Console.WriteLine("task1 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return sw.Elapsed.TotalSeconds;
        }
        
        static double read_file_parfor1(string fname)
        {
            Stopwatch sw = new Stopwatch();
            StreamReader re = new StreamReader(fname);
            string except = "1234567890!\"/*';.,??()_-";
            
            string line;
            string[] words;
            sw.Start();
            while (!re.EndOfStream)
            {
                line = re.ReadLine();
                
                // foreach (char simb in except)
                // {
                //     line = line.Replace(simb.ToString(),"");
                // }
                // line = line.Replace("-"," ");
                // line = line.Replace("  "," ");
                // if(line=="")
                //     continue;
                // words = line.Split(' ');
                words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0',';'
                },StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    //task1
                    // if(word=="")
                    //     continue;
                    c_dic_words.AddOrUpdate(word, 1, (k, v) => v + 1);
                    
                    // if(c_max_count_word.Item1)
                    // if (c_dic_words.ContainsKey(word))
                    //     dic_words[word]++;
                    // else
                    //     dic_words.Add(word, 1);
                    // if (max_count_word.Item1 < dic_words[word])
                    // {
                    //     max_count_word.Item1 = dic_words[word];
                    //     max_count_word.Item2 = word;
                    // }
                        
                }
            }
            
            
            re.Close();
            sw.Stop();
            return sw.Elapsed.TotalSeconds;
        }
        
        static double read_file_task2(string fname)
        {
            Stopwatch sw = new Stopwatch();
            StreamReader re = new StreamReader(fname);
            string except = "1234567890!\"/*'.,??()_-";
            
            string line;
            string[] words;
            sw.Start();
            while (!re.EndOfStream)
            {
                line = re.ReadLine();
                // if(line=="")
                //     continue;
                // foreach (char simb in except)
                // {
                //     line = line.Replace(simb.ToString(),"");
                // }
                // line = line.Replace("-"," ");
                // line = line.Replace("  "," ");
                // line = line.Trim();
                // words = line.Split(' ');
                words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0',';'
                },StringSplitOptions.RemoveEmptyEntries);
                int l=0;
                
                foreach (string word in words)
                {
                    //task2
                    l = word.Length;
                    if (l < max_len_word)
                    {
                        continue;
                    }
                    else if(l>max_len_word)
                    {
                        max_len_word = l;
                        dic_lenwords.Add(l,word);
                    }
                    else
                    {
                        dic_lenwords[l] += "," + word;
                    }
                }
            }
            
            
            re.Close();
            sw.Stop();
            // Console.WriteLine("file " + fname + " expired");
            // Console.WriteLine("task2 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return sw.Elapsed.TotalSeconds;
        }
        
        static void read_file_parfor2(string fname)
        {
            Stopwatch sw = new Stopwatch();
            StreamReader re = new StreamReader(fname);
            string except = "1234567890!\"/*'.,??()_-";
            int ind = Thread.CurrentThread.ManagedThreadId;
            string line;
            string[] words;
            if (!c_dic_lenwords.ContainsKey(ind))
            {
                var b = c_dic_lenwords.TryAdd(ind,new Dictionary<int, string>());
                while (!b)
                {
                    b = c_dic_lenwords.TryAdd(ind,new Dictionary<int, string>());
                }
            }
            if (!c_max_len_word.ContainsKey(ind))
            {
                var b = c_max_len_word.TryAdd(ind,0);
                while (!b)
                {
                    b = c_max_len_word.TryAdd(ind,0);
                }
            }
                
            sw.Start();
            while (!re.EndOfStream)
            {
                line = re.ReadLine();
                // if(line=="")
                //     continue;
                // foreach (char simb in except)
                // {
                //     line = line.Replace(simb.ToString(),"");
                // }
                // line = line.Replace("-"," ");
                // line = line.Replace("  "," ");
                // line = line.Trim();
                // words = line.Split(' ');
                words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0',';'
                },StringSplitOptions.RemoveEmptyEntries);
                int l=0;
                
                foreach (string word in words)
                {
                    //task2
                    l = word.Length;
                    if (l < c_max_len_word[ind])
                    {
                        continue;
                    }
                    else if(l>c_max_len_word[ind] || !c_dic_lenwords[ind].ContainsKey(l))
                    {
                        c_max_len_word[ind] = l;
                        c_dic_lenwords[ind].Add(l,word);
                    }
                    else
                    {
                        c_dic_lenwords[ind][l] += "," + word;
                        
                    }
                }
            }
            re.Close();
            sw.Stop();
            
        }
        
        static (string,int,double) read_file_task3(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            
            StreamReader re = new StreamReader(fnames[0]);
            string except = "1234567890!\"/*'.,?()_-:;\n";
            
            string line;
            string[] words;
            sw.Start();
            
            List<Dictionary<string, int>> word_am = new List<Dictionary<string, int>>();
            Dictionary<string, int> temp = new Dictionary<string, int>();
            Dictionary<string, int> pcount = new Dictionary<string, int>();
            while (!re.EndOfStream)
            {
                line = re.ReadLine();
                // if(line=="")
                //     continue;
                // foreach (char simb in except)
                // {
                //     line = line.Replace(simb.ToString(),"");
                // }
                // line = line.Replace("-"," ");
                // line = line.Replace("  "," ");
                // line = line.Replace("  "," ");
                // line = line.Trim();
                // words = line.Split(' ');
                words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0',';'
                },StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var word in words)
                {
                    if(word=="" || word==" ")
                        continue;
                    if (temp.ContainsKey(word))
                        temp[word]++;
                    else
                        temp.Add(word, 1);
                }
                
                
            }

            word_am.Add(temp);
            re.Close();
            int max = 0;
            string pair_name="";
            for (int i = 1; i < fnames.Length; i++)
            {
                re = new StreamReader(fnames[i]);
                temp = new Dictionary<string, int>();
                while (!re.EndOfStream)
                {
                    line = re.ReadLine();
                    // if(line=="")
                    //     continue;
                    // foreach (char simb in except)
                    // {
                    //     line = line.Replace(simb.ToString(),"");
                    // }
                    // line = line.Replace("-"," ");
                    // line = line.Replace("  "," ");
                    // line = line.Trim();
                    // words = line.Split(' ');
                    words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                        '\t', '/', '\'', '(', ')', '\r', '\n',
                        '1', '2', '3', '4', '5', '6', '7',
                        '8', '9', '0',';'
                    },StringSplitOptions.RemoveEmptyEntries);
                    
                    foreach (var word in words)
                    {
                        if(word=="" || word==" ")
                            continue;
                        if (temp.ContainsKey(word))
                            temp[word]++;
                        else
                            temp.Add(word, 1);
                    }
                
                
                }
                word_am.Add(temp);
                for (int e = 0; e < i; e++)
                {
                    int local_count = 0;
                    foreach (var dic in temp)
                    {
                        if (word_am[e].ContainsKey(dic.Key))
                        {
                            // int razn = word_am[e][dic.Key] - dic.Value;
                            // local_count += (razn >= 0 ? dic.Value : word_am[e][dic.Key]);
                            local_count++;
                        }
                    }
                    pcount.Add(fnames[e]+","+fnames[i],local_count);
                    if (local_count >= max)
                    {
                        max = local_count;
                        pair_name = fnames[e] + "," + fnames[i];
                    }
                }
                re.Close();
            }

            
            
            
            sw.Stop();
            // Console.WriteLine("files task3" + " expired");
            // Console.WriteLine("task3 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return (pair_name,max,sw.Elapsed.TotalSeconds);
        }
        
        static (string,int,double) read_file_parfor3(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            
            
            string except = "1234567890!\"/*'.,?()_-:;\n";
            
            string line;
            string[] words;
            sw.Start();

            // Dictionary<string, int>[] word_am = new Dictionary<string, int>[fnames.Length];
            BlockingCollection<Dictionary<string, int>> word_am = new BlockingCollection<Dictionary<string, int>>();
            // List<Dictionary<string, int>> word_am = new List<Dictionary<string, int>>();
            
            Dictionary<string, int> pcount = new Dictionary<string, int>();
            
            ParallelOptions options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = 4 
            };
            
            
            Parallel.ForEach(Partitioner.Create(0, fnames.Length), options, range => { 
                for(int i=range.Item1; i<range.Item2;i++)
                {
                    Dictionary<string, int> temp = new Dictionary<string, int>();
                    StreamReader re = new StreamReader(fnames[i]);
                    while (!re.EndOfStream)
                    {
                        line = re.ReadLine();
                        // if(line=="")
                        //     continue;
                        // foreach (char simb in except)
                        // {
                        //     line = line.Replace(simb.ToString(),"");
                        // }
                        // line = line.Replace("-"," ");
                        // line = line.Replace("  "," ");
                        // line = line.Replace("  "," ");
                        // line = line.Trim();
                        // words = line.Split(' ');
                        words = line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                            '\t', '/', '\'', '(', ')', '\r', '\n',
                            '1', '2', '3', '4', '5', '6', '7',
                            '8', '9', '0',';'
                        },StringSplitOptions.RemoveEmptyEntries);
                
                        foreach (var word in words)
                        {
                            if(word=="" || word==" ")
                                continue;
                            if (temp.ContainsKey(word))
                                temp[word]++;
                            else
                                temp.Add(word, 1);
                        }
                    }
                    re.Close();
                    var b = word_am.TryAdd(temp,0);
                    // word_am[i] = temp;
                }
            });

            Dictionary<string, int>[] word_am2 = word_am.ToArray();

            
            
            int max = 0;
            string pair_name="";
            Parallel.For(1, fnames.Length, options, i =>
            {
                for (int e = 0; e < i; e++)
                {
                    int local_count = 0;
                    
                    foreach (var pair in word_am2[i])
                    {
                        if (word_am2[e].ContainsKey(pair.Key))
                        {
                            // int razn = word_am[e][dic.Key] - dic.Value;
                            // local_count += (razn >= 0 ? dic.Value : word_am[e][dic.Key]);
                            local_count++;
                        }
                    }
                    // pcount.Add(fnames[e]+","+fnames[i],local_count);
                    lock ("max")
                    {
                        if (local_count >= max)
                        {
                            max = local_count;
                            pair_name = fnames[e] + "," + fnames[i];
                        }
                    }
                    
                }
            });
            

            
            
            
            sw.Stop();
            // Console.WriteLine("files task3" + " expired");
            // Console.WriteLine("task3 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return (pair_name,max,sw.Elapsed.TotalSeconds);
        }

        static (string,int,double) read_file_linq1(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            // var all_words = fnames.SelectMany(
            //     path => File.ReadLines(path).SelectMany(
            //         line => line.Split(' ')));
            
            var all_words = fnames.SelectMany(
                path => File.ReadLines(path).SelectMany(
                    line => line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                        '\t', '/', '\'', '(', ')', '\r', '\n',
                        '1', '2', '3', '4', '5', '6', '7',
                        '8', '9', '0',';'
                    },StringSplitOptions.RemoveEmptyEntries)));

            var clear_words = all_words;
            
            // var clear_words = all_words.Select(word =>
            //     word.Replace(".", "").
            //         Replace(",","").
            //         Replace("0","").
            //         Replace("1","").
            //         Replace("2","").
            //         Replace("3","").
            //         Replace("4","").
            //         Replace("5","").
            //         Replace("6","").
            //         Replace("7","").
            //         Replace("8","").
            //         Replace("9","").
            //         Replace("(","").
            //         Replace(")","").
            //         Replace("-","").
            //         Replace("?","").
            //         Replace("_","").
            //         Replace(";","")).Where(word=>word!="");
            
            var count_words = clear_words.Aggregate(
                new Dictionary<string, int>(),
                (dic, w) =>
                {
                    if (dic.ContainsKey(w))
                        dic[w]++;
                    else
                        dic.Add(w, 1);
                    return dic;
                });
            var order_pairs = count_words.OrderBy(
                pair => -pair.Value);
            KeyValuePair<string, int> tep = order_pairs.First();
            
            sw.Stop();
            // Console.WriteLine("files " + " expired");
            // Console.WriteLine("link1 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return (tep.Key,tep.Value,sw.Elapsed.TotalSeconds);
        }
        static (int,List<string>,double) read_file_linq2(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // var all_words = fnames.SelectMany(
            //     path => File.ReadLines(path).SelectMany(
            //         line => line.Split(' ')));
            
            var all_words = fnames.SelectMany(
                path => File.ReadLines(path).SelectMany(
                    line => line.Split(new char[]{' ', ',', '.', '?','?', '!', '-', '"', ':',
                        '\t', '/', '\'', '(', ')', '\r', '\n',
                        '1', '2', '3', '4', '5', '6', '7',
                        '8', '9', '0',';','*'
                    },StringSplitOptions.RemoveEmptyEntries)));

            var clear_words = all_words.Where(p=>p.Length>9);
            
            // var clear_words = all_words.Select(word =>
            //     word.Replace(".", "").
            //         Replace(",","").
            //         Replace("0","").
            //         Replace("1","").
            //         Replace("2","").
            //         Replace("3","").
            //         Replace("4","").
            //         Replace("5","").
            //         Replace("6","").
            //         Replace("7","").
            //         Replace("8","").
            //         Replace("9","").
            //         Replace("(","").
            //         Replace(")","").
            //         Replace("-","").
            //         Replace("?","").
            //         Replace("_","").
            //         Replace(";","")).Where(word=>word!="");
            
            // var grou = clear_words.Select(p => new {word = p, len = p.Length}).
            //         GroupBy(p=>p.len);
            //
            // foreach (var elem in grou)
            // {
            //     foreach (var pair in elem)
            //     {
            //         Console.WriteLine(pair.len.ToString() + pair.word);
            //     }
            // }
            var maxi = clear_words.Select(p => p.Length).Max();
            var max_words = clear_words.Where(p => p.Length == maxi).ToList();
            
            var max_words = clear_words.OrderBy(p => -p.Length).Take(10).ToList();
            var maxi = 0;
            try
            {
                maxi = max_words[0].Length;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            sw.Stop();
            // Console.WriteLine("files " + " expired");
            // Console.WriteLine("link2 ellapsed time: " + sw.Elapsed.TotalSeconds.ToString());
            return (maxi,max_words,sw.Elapsed.TotalSeconds);
        }
        
        static (int,string,double) read_file_linq3(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            // var files = fnames.SelectMany(path => File.ReadLines(path).Where(line=>line!="").Select(p=>new {p,path}));
            // //
            // //
            // var clearlines = files.Select(lines => new {line= lines.p.Replace(".", "").
            //     Replace(",","").Replace(" - "," ").
            //     Replace("0","").Replace("1","").
            //     Replace("2","").Replace("3","").
            //     Replace("4","").Replace("5","").
            //     Replace("6","").Replace("7","").
            //     Replace("8","").Replace("9","").
            //     Replace("(","").Replace(")","").
            //     Replace("'","").Replace(":","").Replace("?","").
            //     Replace(";","").Replace("  "," ").Replace("  "," "), lines.path});
            // var words = clearlines.SelectMany(ws => ws.line.Split(' ').
            //     Select(wd=>new{wd,ws.path}));
            // words = words.Where(wrd => wrd.wd != "" && wrd.wd!=" ");
            // var jn = words.Join(words, e => e.wd, o => o.wd,
            //     (e, o) => new
            //     {
            //         word = e.wd,
            //         path1 = e.path,
            //         path2 = o.path
            //     }).Where(p=>p.path1!=p.path2);
            // var test = jn.Where(p => p.word == "WORLD");
            // int k = 0;
            // jn = jn.Distinct();
            // var pjn = jn.Select(p => p.path1 + " " + p.path2);
            // // foreach (var elem in pjn)
            // // {
            // //     Console.WriteLine(elem);
            // //     k++;
            // //     if(k>10)
            // //         break;
            // // }
            // // int max = 0;
            // // Console.WriteLine(pjn.Count());
            // var count_paths = pjn.Aggregate(
            //     new Dictionary<string, int>(),
            //     (dic, w) =>
            //     {
            //         if (dic.ContainsKey(w))
            //             dic[w]++;
            //         else
            //             dic.Add(w, 1);
            //         return dic;
            //     });
            // var mx = count_paths.Max(pair => pair.Value);
            // // var order_pairs = count_paths.OrderBy(
            // //     pair => -pair.Value);
            // // KeyValuePair<string, int> tep = order_pairs.First();
            // KeyValuePair<string, int> tep = count_paths.Where(p => p.Value == mx).First();
            List<IEnumerable<string>> wordsf = new List<IEnumerable<string>>();
            Dictionary<string, int> pairs = new Dictionary<string, int>();
            
            
            
            for (int i = 0; i < fnames.Length; i++)
            {
                wordsf.Add(File.ReadLines(fnames[i]).SelectMany(line => line.Split(new char[]
                {
                    ' ', ',', '.', '?', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0', ';', '*'
                },StringSplitOptions.RemoveEmptyEntries)));
                for (int e = 0; e < i; e++)
                {
                    var conc = wordsf[e].Intersect(wordsf[i]);
                    // var conc = wordsf[e].Except(wordsf[i]);
                    // conc = wordsf[e].Except(conc);
                    pairs.Add(fnames[e]+","+fnames[i],conc.Count());
                    
                }
            }
            var ord = pairs.OrderBy(pair => -pair.Value);
            KeyValuePair<string, int> tep = ord.First();

            sw.Stop();
            return (tep.Value,tep.Key,sw.Elapsed.TotalSeconds);
        }
        
        static (string,int,double) read_file_linqpar1(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var all_words = fnames.AsParallel().WithDegreeOfParallelism(4).SelectMany(
                path => File.ReadLines(path).SelectMany(
                    line => line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                        '\t', '/', '\'', '(', ')', '\r', '\n',
                        '1', '2', '3', '4', '5', '6', '7',
                        '8', '9', '0',';'
                    },StringSplitOptions.RemoveEmptyEntries)));

            var clear_words = all_words;
            
            var count_words = clear_words.Aggregate(
                new Dictionary<string, int>(),
                (dic, w) =>
                {
                    if (dic.ContainsKey(w))
                        dic[w]++;
                    else
                        dic.Add(w, 1);
                    return dic;
                });
            var order_pairs = count_words.OrderBy(
                pair => -pair.Value);
            KeyValuePair<string, int> tep = order_pairs.First();
            
            sw.Stop();
            return (tep.Key,tep.Value,sw.Elapsed.TotalSeconds);
        }
        static (int,List<string>,double) read_file_linqpar2(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var all_words = fnames.AsParallel().WithDegreeOfParallelism(4).SelectMany(
                path => File.ReadLines(path).SelectMany(
                    line => line.Split(new char[]{' ', ',', '.', '?', '!', '-', '"', ':',
                        '\t', '/', '\'', '(', ')', '\r', '\n',
                        '1', '2', '3', '4', '5', '6', '7',
                        '8', '9', '0',';'
                    },StringSplitOptions.RemoveEmptyEntries)));

            var clear_words = all_words.Where(p=>p.Length>9);
            var grou = clear_words.Select(p => new {word = p, len = p.Length}).
                    GroupBy(p=>p.len);
            var maxi = clear_words.Select(p => p.Length).Max();
            var max_words = clear_words.Where(p => p.Length == maxi).ToList();
            
            sw.Stop();
            return (maxi,max_words,sw.Elapsed.TotalSeconds);
        }
        
        static (int,string,double) read_file_linqpar3(string[] fnames)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            List<IEnumerable<string>> wordsf = new List<IEnumerable<string>>();
            Dictionary<string, int> pairs = new Dictionary<string, int>();
            
            
            
            for (int i = 0; i < fnames.Length; i++)
            {
                wordsf.Add(File.ReadLines(fnames[i]).AsParallel().WithDegreeOfParallelism(4).SelectMany(line => line.Split(new char[]
                {
                    ' ', ',', '.', '?', '?', '!', '-', '"', ':',
                    '\t', '/', '\'', '(', ')', '\r', '\n',
                    '1', '2', '3', '4', '5', '6', '7',
                    '8', '9', '0', ';', '*'
                },StringSplitOptions.RemoveEmptyEntries)));
                for (int e = 0; e < i; e++)
                {
                    var conc = wordsf[e].Intersect(wordsf[i]);
                    // var conc = wordsf[e].Except(wordsf[i]);
                    // conc = wordsf[e].Except(conc);
                    pairs.Add(fnames[e]+","+fnames[i],conc.Count());
                    
                }
            }
            var ord = pairs.OrderBy(pair => -pair.Value);
            KeyValuePair<string, int> tep = ord.First();

            sw.Stop();
            
            return (tep.Value,tep.Key,sw.Elapsed.TotalSeconds);
        }
    }
    
    
}