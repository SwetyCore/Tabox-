using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
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
using JumpList;
using System.Collections.ObjectModel;
using System.IO;
using Lnk;
using Tabox__.Pages;

namespace Tabox__
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VM vm = new VM();

        public static Dictionary<string, string> map = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        public static void LoadMap()
        {
            var lines = File.ReadAllLines(@"Assets/appidMap.txt");
            foreach (var item in lines)
            {
                var m = item.Split('|');
                map[m[0]] = m[1];
            }
        }

        public static List<LnkFile> GetLnks(List<JumpList.Custom.Entry> entries)
        {
            var result = new List<LnkFile>();

            foreach (var item in entries)
            {
                result=result.Union(item.LnkFiles).ToList<LnkFile>();
            }

            return result;
        }

        public static List<LnkFile> GetLnks(List<JumpList.Automatic.AutoDestList> entries)
        {
            var result = new List<LnkFile>();

            foreach (var item in entries)
            {
                //result = result.Union(item.LnkFiles).ToList<LnkFile>();
                result.Add(item.Lnk);
            }

            return result;
        }

        public class VM : ObservableObject
        {
            public VM()
            {
                LoadJumpList();
            }



            public class ProjectGroup
            {
                public string IconPath { get; set; }
                public string AppName { get; set; }

                public List<LnkFile> LnkFiles { get; set; }
            }

            public string TryGetName(string appid)
            {
                if (MainWindow.map.ContainsKey(appid))
                {
                    return map[appid];
                }
                return appid;
            }


            public void LoadJumpList()
            {
                LoadMap();

                string History = System.Environment.GetFolderPath(Environment.SpecialFolder.Recent);

                string CustomDestinationsPath = Path.Combine(History, "CustomDestinations");
                string AutomaticDestinationsPath = Path.Combine(History, "AutomaticDestinations");
                DirectoryInfo cdpdi = new DirectoryInfo(CustomDestinationsPath);
                DirectoryInfo adpdi = new DirectoryInfo(AutomaticDestinationsPath);

                ProjectGroups = new ObservableCollection<ProjectGroup>();



                foreach (var item in cdpdi.GetFiles())
                {



                    Console.WriteLine(item.FullName);
                    try
                    {

                        var r = JumpList.JumpList.LoadCustomJumplist(item.FullName);


                        //仅包含命名的
                        //if (!MainWindow.map.ContainsKey(r.AppId.AppId))
                        //{
                        //    continue;
                        //}


                        Console.WriteLine(r);

                        ProjectGroups.Add(new ProjectGroup { AppName = TryGetName( r.AppId.AppId) ,LnkFiles= GetLnks( r.Entries)});

                    }
                    catch (Exception)
                    {

                    }
                }
#if !DEBUG
                foreach (var item in adpdi.GetFiles())
                {
                    Console.WriteLine(item.FullName);
                    try
                    {
                        
                        var r = JumpList.JumpList.LoadAutoJumplist(item.FullName);
                        //仅包含命名的
                        //if (!MainWindow.map.ContainsKey(r.AppId.AppId))
                        //{
                        //    continue;
                        //}



                        Console.WriteLine(r);

                        ProjectGroups.Add(new ProjectGroup { AppName =TryGetName( r.AppId.AppId), LnkFiles = GetLnks(r.DestListEntries) });

                    }
                    catch (Exception)
                    {

                    }
                }
#endif

            }

            private ObservableCollection<ProjectGroup> _projectGroups;

            public ObservableCollection< ProjectGroup> ProjectGroups
            {
                get { return _projectGroups; }
                set { SetProperty(ref _projectGroups, value); }
            }




        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = sender as ListView;
            frame.Navigate(new ProjectView(lv.SelectedValue as List<LnkFile>));
        }
    }
}
