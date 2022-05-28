using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public ProjectView(List<Lnk.LnkFile> projs)
        {
            InitializeComponent();
            vm  = new VM(NLnkFileConverter.Convert(projs));
            DataContext = vm;
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


        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = lv.SelectedValue as NLnkFile;
            try
            {
                Process.Start(selected.LocalPath, selected.Arguments);

            }
            catch (Exception)
            {

                MessageBox.Show("Error");
            }
        }
    }
}
