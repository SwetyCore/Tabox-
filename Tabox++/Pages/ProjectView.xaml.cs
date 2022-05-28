using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tabox__.Pages
{
    /// <summary>
    /// ProjectView.xaml 的交互逻辑
    /// </summary>
    public partial class ProjectView : Page
    {
        public VM vm;
        public List<Lnk.LnkFile> Projs;
        public ProjectView(List<Lnk.LnkFile> projs)
        {
            InitializeComponent();
            vm  = new VM(NLnkFileConverter.Convert(projs));
            DataContext = vm;
            Projs = projs;
        }

        

        public class VM:ObservableObject
        {
            public VM(List<NLnkFile> projs)
            {
                Projects = projs;
            }

            


            private List<NLnkFile> _projects;

            public List<NLnkFile> Projects
            {
                get { return _projects; }
                set { SetProperty(ref _projects, value); }
            }

        }

        public static void runcmd(string cmd)
        {

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";         //确定程序名
            p.StartInfo.Arguments = "/c " + cmd;   //确定程式命令行
            p.StartInfo.UseShellExecute = false;      //Shell的使用
            p.StartInfo.CreateNoWindow = true;        //设置置不显示示窗口
            p.Start();

        }
        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = lv.SelectedValue as NLnkFile;
            var selectedIndex = lv.SelectedIndex;
            try
            {
                //if ()
                //Process.Start(selected.LocalPath, selected.Arguments == null ? "" : selected.Arguments);
                File.WriteAllBytes("tmp.lnk", Projs[selectedIndex].RawBytes);
                runcmd($"start tmp.lnk");
                //runcmd($"start {selected.SourceFile}");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
