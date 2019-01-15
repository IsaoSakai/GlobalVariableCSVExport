using System;
using System.Windows.Forms;
using System.IO;

namespace GlobalVariableCSVExport
{
    public partial class Form1 : Form
    {
        public  Form1()
        {
            InitializeComponent();
        }
        
        //これがないとエラー
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            System.IO.DirectoryInfo hDirectionInfo = System.IO.Directory.GetParent(@"//VariableText.txt");
            MessageBox.Show(hDirectionInfo.ToString());
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        //Form1をLoadし描写、button1を描写とイベントセット
        private void Form1_Load(object sender, EventArgs e)
        {

            this.Text = ProductName;
            button1.Text = "フォルダーを選択してください。";
            button1.Click += Button1_Click;
        }

        //button1イベント内容
        private void Button1_Click(object sender, EventArgs e)
        {
            ShowFolderDialog();
        }

        public static string[] GetDirectories(string path, string searchPattern) {

            string[] dirs = Directory.GetDirectories(path, searchPattern);
            foreach( string  dir in dirs){
                Console.WriteLine(dir);
            }
            return null;
        }

        //ツールのメイン機能部分
        private void ShowFolderDialog()
        {
            //FolderBrowserDialogの初期化
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //-------------------------変数-----------------------------//
            //----------------------------------------------------------//

            //書き込みたいクラスへのパス
            string path = "";
            //クラスファイル名
            string get_file = "";
            //get_fileの中身を格納する配列
            string[] file_Contents = new string[1024];
            //指定クラスの存在の有無のbool値 true = あった false = 無かった
            bool check_cs = false;
            //指定文の存在の有無のbool値 true = あった false =　無かった
            bool check_line = false;

            //----------------------------------------------------------//
            //----------------------ここまで----------------------------//
            

            //処理開始
            fbd.Description = "フォルダーを選択してください。";

            //ファイル検索のスタートをdesktopに固定、変更可
            fbd.RootFolder = Environment.SpecialFolder.Desktop;

            //ファイルの追加の有無 true = 追加可能 false = 追加不能
            fbd.ShowNewFolderButton = true;

            //fbdのShowdialogの成功判定bool値 上記Mainに[STAThread]が無いとここで例外処理で止まる
            DialogResult result = fbd.ShowDialog();

            //ShowdialogのOKボタンが押されるまでここでストップ
            if (result == DialogResult.OK)
            {
                //okボタンを押されたファイルのパス取得
                path = Directory.GetParent("GlobalVariableController.cs").ToString();
                MessageBox.Show(path);
                MessageBox.Show(path + "が選択されました", "検査結果", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //pathファイル直下のすべてのファイル名格納
                string[] files = System.IO.Directory.GetFiles(path,"*",System.IO.SearchOption.AllDirectories);
                MessageBox.Show("File内検索開始", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //ここで変更をかけたいクラスを検索
                for ( int i = 0; i < files.Length; i++)
                {
                    ProgressBar progressbar = new ProgressBar();
                    progressbar.Show();
                    progressbar.Minimum = 0;
                    progressbar.Maximum = files.Length;
                    progressbar.Value = i;
                    //検索がヒットしたらget_fileに格納
                    if ( files[i] == path + "\\GlobalVariableController.cs")
                    {
                        get_file = files[i];
                        MessageBox.Show( get_file, "取得結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MessageBox.Show( get_file + "内部検索", "検索", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        check_cs = true;

                        //file_Contens内にget_fileの中身を格納
                        file_Contents = File.ReadAllLines(get_file);
                        break;
                    }
                }

                //filesを初期化しメモリーの開放
                files = null;

                //file_contents内の変更をかけたい関数を検索
                for ( int i = 0; i < file_Contents.Length; i++)
                {
                    //検索がヒット
                    if( file_Contents[i] == "public class GlobalVariableController")
                    {
                        MessageBox.Show( file_Contents[i], "取得結果", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        check_line = true;
                        break;
                    }
                }

                //ファイル検索、関数検索でどちらもヒットしなかった場合
                if ( check_cs != true || check_line != true)
                {
                    //この後ファイルのパス検索まで戻る
                    MessageBox.Show("対象のクラスがありません", "取得結果", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            GetDirectories(path, get_file);

            MessageBox.Show("グローバル変数更新完了", "作業完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
