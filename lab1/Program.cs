using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace test
{
    class Program
    {
        static float[] a;
        static float[] b;
        static int m, n;

        static void excel_write_tests()
        {
            Excel.Application ObjWorkExcel = new Excel.Application();
            Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(@"D:\учеба\parallel\zad1\pr1\pr1\bin\Debug\Копия ex3.xlsx");
            Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
            ObjWorkSheet.Range["B2"].Value = 5;
            var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

            int lastColumn = (int)lastCell.Column;
            int lastRow = (int)lastCell.Row;
            Console.WriteLine(System.Environment.ProcessorCount);

            
            int[] dlina_n = { 500, 5000, 50000 };
            int[] dlina_m = { 2, 4, 8, 10 };



            for (int k = 0; k < dlina_n.Length; k++)
            {
                for (int j = 0; j < dlina_m.Length; j++)
                {
                    n = dlina_n[k];
                    m = dlina_m[j];
                    ObjWorkSheet.Range["E" + ((10 * dlina_m.Length) * k + j * 10 + 2).ToString()].Value = " n";
                    ObjWorkSheet.Range["E" + ((10 * dlina_m.Length) * k + j * 10 + 3).ToString()].Value = " m";
                    ObjWorkSheet.Range["F" + ((10 * dlina_m.Length) * k + j * 10 + 2).ToString()].Value = n;
                    ObjWorkSheet.Range["F" + ((10 * dlina_m.Length) * k + j * 10 + 3).ToString()].Value = m;
                    for (int h = 0; h < 4; h++)
                    {
                        


                        a = new float[n];
                        b = new float[n];
                        Thread[] th = new Thread[m];
                        for (int i = 0; i < n; i++)
                        {
                            a[i] = 1;
                        }

                        Stopwatch sw = new Stopwatch();
                        TimeSpan tp;
                        int check = 0;
                        //sw.Start();
                        //_task2(new int[] { 0, n, 0 });
                        //sw.Stop();
                        //
                        //for (int i = 0; i < n; i++)
                        //{
                        //    check += b[i] == 5 ? 1 : 0;
                        //}
                        ////Console.WriteLine("task 1: check " + (check == n ? "yes" : "no"));
                        //tp = sw.Elapsed;
                        ////Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        //ObjWorkSheet.Range["E" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        //sw.Reset();
                        //sw.Start();
                        //
                        //for (int i = 0; i < m; i++)
                        //{

                        //    th[i] = new Thread(_task2);
                        //    th[i].Name = i.ToString();
                        //    th[i].Start(new int[] { (n / m) * i, n / m });
                        //    //new int[] { (n / m) * i, n / m, i }
                        //}
                        //for (int i = 0; i < m; i++)
                        //{
                        //    th[i].Join();
                        //}

                        //sw.Stop();
                        //check = 0;
                        //for (int i = 0; i < n; i++)
                        //{
                        //    check += b[i] == 5 ? 1 : 0;
                        //}
                        ////Console.WriteLine("task 2: check " + (check == n ? "yes" : "no"));
                        //tp = sw.Elapsed;
                        ////Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        //ObjWorkSheet.Range["F" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        //sw.Reset();
                        //sw.Start();

                        //_task1_4(new int[] { 0, n, 0 });
                        //sw.Stop();
                        //check = 0;
                        //for (int i = 0; i < n; i++)
                        //{
                        //    check += b[i] == 5 ? 1 : 0;
                        //}
                        ////Console.WriteLine("task 1-4: check " + (check == n ? "yes" : "no"));
                        //tp = sw.Elapsed;
                        //ObjWorkSheet.Range["G" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        ////Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);


                        //sw.Reset();
                        //sw.Start();

                        //for (int i = 0; i < m; i++)
                        //{

                        //    th[i] = new Thread(_task4);
                        //    th[i].Name = i.ToString();
                        //    th[i].Start(new int[] { (n / m) * i, n / m });
                        //    //new int[] { (n / m) * i, n / m, i }
                        //}
                        //for (int i = 0; i < m; i++)
                        //{
                        //    th[i].Join();
                        //}

                        //sw.Stop();
                        //check = 0;
                        //for (int i = 0; i < n; i++)
                        //{
                        //    check += b[i] == 5 ? 1 : 0;
                        //}
                        ////Console.WriteLine("task 4: check " + (check == n ? "yes" : "no"));
                        //tp = sw.Elapsed;
                        ////Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        //ObjWorkSheet.Range["H" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        sw.Reset();
                        sw.Start();

                        for (int i = 0; i < m; i++)
                        {

                            th[i] = new Thread(_task5);
                            th[i].Name = i.ToString();
                            th[i].Start(new int[] { (n / m) * i, n / m });
                            //new int[] { (n / m) * i, n / m, i }
                        }
                        for (int i = 0; i < m; i++)
                        {
                            th[i].Join();
                        }

                        sw.Stop();
                        
                        //Console.WriteLine("task 5: check " + (check == n ? "yes" : "no"));
                        tp = sw.Elapsed;
                        //Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        ObjWorkSheet.Range["E" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        sw.Reset();
                        sw.Start();

                        for (int i = 0; i < m; i++)
                        {

                            th[i] = new Thread(_task6);
                            th[i].Name = i.ToString();
                            th[i].Start(new int[] { (n / m) * i, n / m, i });
                            //new int[] { (n / m) * i, n / m, i }
                        }
                        for (int i = 0; i < m; i++)
                        {
                            th[i].Join();
                        }

                        sw.Stop();
                        
                        //Console.WriteLine("task 6: check " + (check == n ? "yes" : "no"));
                        tp = sw.Elapsed;
                        //Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        ObjWorkSheet.Range["F" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;

                        sw.Reset();
                        sw.Start();

                        _task1_5(new int[] { 0, n, 0 });
                        //sw.Stop();
                        tp = sw.Elapsed;
                        //Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
                        //Console.WriteLine(tp.TotalMilliseconds);
                        ObjWorkSheet.Range["G" + ((10 * dlina_m.Length) * k + j * 10 + 5 + h).ToString()].Value = tp.TotalMilliseconds;
                        
                    }
                    Console.WriteLine(((((float)dlina_m.Length * k + j) / (float)(dlina_m.Length * dlina_n.Length)) * 100f) + "%");



                }
            }






            ObjWorkBook.Close(true); //закрыть не сохраняя
            ObjWorkExcel.Quit();
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            //excel_write_tests();
            //return;
            Console.Write("N ");
            n = int.Parse(Console.ReadLine());
            Console.Write("M ");
            m = int.Parse(Console.ReadLine());
            a = new float[n];
            b = new float[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = 1;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            _task2(new int[] { 0, n, 0 });
            sw.Stop();
            int check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 1: check " + (check == n ? "yes" : "no"));
            TimeSpan tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.WriteLine(tp.TotalMilliseconds);
            
            sw.Reset();
            sw.Start();
            Thread[] th = new Thread[m];
            for (int i = 0; i < m; i++)
            {

                th[i] = new Thread(_task2);
                th[i].Name = i.ToString();
                th[i].Start(new int[] { (n / m) * i, n / m });
                //new int[] { (n / m) * i, n / m, i }
            }
            for (int i = 0; i < m; i++)
            {
                th[i].Join();
            }

            sw.Stop();
            check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 2: check " + (check == n ? "yes" : "no"));
            tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.WriteLine(tp.TotalMilliseconds);
            sw.Reset();
            sw.Start();

            _task1_4(new int[] { 0, n, 0 });
            sw.Stop();
            check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 1-4: check " + (check == n ? "yes" : "no"));
            tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.WriteLine(tp.TotalMilliseconds);

            sw.Reset();
            sw.Start();

            for (int i = 0; i < m; i++)
            {

                th[i] = new Thread(_task4);
                th[i].Name = i.ToString();
                th[i].Start(new int[] { (n / m) * i, n / m });
                //new int[] { (n / m) * i, n / m, i }
            }
            for (int i = 0; i < m; i++)
            {
                th[i].Join();
            }

            sw.Stop();
            check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 4: check " + (check == n ? "yes" : "no"));
            tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.WriteLine(tp.TotalMilliseconds);
            sw.Reset();
            sw.Start();

            for (int i = 0; i < m; i++)
            {

                th[i] = new Thread(_task5);
                th[i].Name = i.ToString();
                th[i].Start(new int[] { (n / m) * i, n / m });
                //new int[] { (n / m) * i, n / m, i }
            }
            for (int i = 0; i < m; i++)
            {
                th[i].Join();
            }

            sw.Stop();
            check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 5: check " + (check == n ? "yes" : "no"));
            tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.WriteLine(tp.TotalMilliseconds);
            sw.Reset();
            sw.Start();

            for (int i = 0; i < m; i++)
            {

                th[i] = new Thread(_task6);
                th[i].Name = i.ToString();
                th[i].Start(new int[] { (n / m) * i, n / m, i });
                //new int[] { (n / m) * i, n / m, i }
            }
            for (int i = 0; i < m; i++)
            {
                th[i].Join();
            }

            sw.Stop();
            check = 0;
            for (int i = 0; i < n; i++)
            {
                check += b[i] == 5 ? 1 : 0;
            }
            Console.WriteLine("task 6: check " + (check == n ? "yes" : "no"));
            tp = sw.Elapsed;
            Console.WriteLine("Total milliseconds: " + tp.TotalMilliseconds);
            Console.ReadKey();
        }

        static void _task2(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] + start;
            //int id = ((int[])par)[2];
            for (int i = start; i < end; i ++)
            {
                    b[i] = a[i] * 5;
            }
        }

        static void _task4(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] + start;
            //int id = ((int[])par)[2];
            for (int i = start; i < end; i++)
            {
                for (int e = 0; e < 50; e++)
                {
                    b[i] = a[i] * 5;

                }

            }
        }

        static void _task5(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] + start;
            //int id = ((int[])par)[2];
            for (int i = start; i < end; i++)
            {
                for (int e = 0; e < i; e++)
                {
                    b[i] = a[i] * 5;
                }
            }
        }

        static void _task6(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] ;
            int id = ((int[])par)[2];
            for (int i = id; i < n; i+=m)
            {
                for (int e = 0; e < i; e++)
                {
                    b[i] = a[i] * 5;
                }
            }
        }

        static void _task1_4(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] + start;
            //int id = ((int[])par)[2];
            for (int i = start; i < end; i++)
            {
                for (int e = 0; e < 50; e++)
                {
                    b[i] = a[i] * 5;

                }

            }
        }

        static void _task1_5(object par)
        {
            int start = ((int[])par)[0];
            int end = ((int[])par)[1] + start;
            //int id = ((int[])par)[2];
            for (int i = start; i < end; i++)
            {
                for (int e = 0; e < i; e++)
                {
                    b[i] = a[i] * 5;
                }
            }
        }


    }
}
