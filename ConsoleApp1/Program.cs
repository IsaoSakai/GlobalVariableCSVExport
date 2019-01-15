using System;
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

            GlobalVariableInt p = new GlobalVariableInt(); 
            args = new string[2];
            int write_num = 0;
            int remove_font = 0;
            string[] file_Contents = new string[1024];
            string csv_path = "";
            string txt_path = "";
            string txt_name = "VariableKeyText.txt";
            string class_path= "" ;
            string class_name = "GlobalVariableController.cs";
            string check_class = "public class GlobalVariableController";
            string[] write_txt = new string[64];
            string[] write_key = new string[64];
            List<string> class_content = new List<string>();


            //ここにFileのパスを記載
            FileInfo fInfo_csv = new FileInfo( "D:/diabetes-exercise/Source/Assets/Boom/GameData/xlsm/GlovalVariableInt.csv");
            string hDirectoryInfo = fInfo_csv.DirectoryName;
            Console.WriteLine(hDirectoryInfo);
            csv_path = fInfo_csv.FullName;
            Console.WriteLine( FileAccess.Read.Equals(csv_path));

            //ここにFileのパスを記載
            FileInfo fInfo_txt = new FileInfo("D:/diabetes-exercise/Source/Assets/Boom/GameData/xlsm");
            string txtdirectoryInfo = fInfo_txt.DirectoryName;
            Console.WriteLine(txtdirectoryInfo);
            txt_path = fInfo_txt.ToString();

            //ここにFileのパスを記載
            FileInfo fInfo_class = new FileInfo("D:diabetes-exercise/Source/Assets/Boom/Scripts/System/");
            string classdirectoryInfo = fInfo_class.DirectoryName;
            class_path = classdirectoryInfo;

            args[0] = csv_path;
            args[1] = txt_path;

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
                        write_txt[i] = records2[i].variable;
                        write_key[i] = records2[i].key;
                        sb.AppendLine(str);
                        write_num = records2.Count;
                        //Debug
                        System.Console.Write(str);
                        System.Console.WriteLine();

                    };


                    file_Contents = File.ReadAllLines( class_path +"\\"+ class_name);
                    for( int i = 0; i < file_Contents.Length; i++)
                    {
                        if(file_Contents[i].Contains("//"))
                        {
                            file_Contents[i] = null;
                        }
                        if( file_Contents[i] == check_class)
                        {
                            for (int j = 0; j < write_num - 1; j++)
                            {
                                if( write_key[j] == null)
                                {
                                    j++;
                                }
                                file_Contents[i + 2 + j] = "    public const string " + write_txt[j] + " = "   + "\"" + write_key[j] +"\"" + ";";
                            }
                        }

                    }


                    //Debug
                    for( int i = 0; i < file_Contents.Length; i++)
                    {
                        Console.WriteLine(file_Contents[i]);
                        File.WriteAllLines(class_path + "\\" + class_name, file_Contents);
                    }
                    //stringをファイルに書き込む
                    Console.WriteLine("Write File : {0:s}", args[1] + "\\" + txt_name);
                    File.WriteAllText(args[1] + "\\" + txt_name, sb.ToString());
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
