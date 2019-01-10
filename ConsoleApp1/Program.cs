﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;

namespace ConsoleApp1
{
    //グローバル変数用エクセルの形式(ヘッダーも読み込むのでvalueがstringでないとCSVHelperで例外エラーになるのでvalueもstring)
    public class GlobalVariableInt
    {
        public string key { get; set; }
        public string value { get; set; }
        public string variable { get; set; }
    }

    class Program
    {
        /// <summary>
        /// コマンドライン引数：[csvファイルパス][書き込むテキストファイルパス]
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //コマンドライン引数がないとエラー
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a numeric argument.");
                Console.ReadKey();
                return;
            }
            if (args[0].Length == 0 || args[1].Length == 0)
            {
                Console.WriteLine("Not Found CommandLine Args.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Read File : {0:s}",args[0]);

            //CSVを読み込む
            IEnumerable<GlobalVariableInt> records;
            List<GlobalVariableInt> records2 = new List<GlobalVariableInt>();
            using (var csv = new CsvReader(new StreamReader(args[0])))
            {
                try
                {
                    records = csv.GetRecords<GlobalVariableInt>();

                    //余計な空データを除いてListにする
                    foreach (var rec in records)
                    {
                        if (rec.key.Length > 0)
                        {
                            records2.Add(rec);
                        }
                    }

                    //Listを繋げて一つのstringにする
                    var sb = new System.Text.StringBuilder();
                    for (int i=1;i<records2.Count;i++)
                    {
                        string str = string.Format("public const string {0:s} = \"{1:s}\";", records2[i].variable, records2[i].key);

                        sb.AppendLine(str);

                        System.Console.Write(str);
                        System.Console.WriteLine();
                    };

                    //stringをファイルに書き込む
                    Console.WriteLine("Write File : {0:s}", args[1]);
                    File.WriteAllText(args[1], sb.ToString());

                } catch (CsvHelperException exc)
                {
                    System.Console.Write("***CsvHelperException:" + exc.Message);
                }
            }

 

#if DEBUG
            Console.WriteLine("続行するには何かキーを押してください．．．");
            Console.ReadKey();
#endif
        }


        
    }
}