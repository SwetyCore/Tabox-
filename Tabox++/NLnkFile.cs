using Lnk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabox__
{
    public class NLnkFile
    {
        public string IconLocation { get; set; }
        public string Arguments { get; set;}
        public string WorkingDirectory { get; set;}
        public string RelativePath { get; set;}
        public string Name { get; set;}
        public string SourceFile { get; set;}
        public string LocalPath { get; set;}
        public string CommonPath { get; set;}
        public DateTimeOffset? SourceAccessed { get; set;}
        public DateTimeOffset? SourceModified { get; set;}
        public DateTimeOffset? SourceCreated { get; set;}
        public LnkFile lnkFile { get; set; }
    }

    public static class NLnkFileConverter
    {
        public static string GetName(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine("是文件");
                return Path.GetFileName(path);
            }
            else if (Directory.Exists(path))
            {
                Console.WriteLine("是文件夹");
                return Path.GetDirectoryName(path);

            }
            else
            {
                Console.WriteLine("都不是");
            }
            return "";
        }

        public static List< NLnkFile> Convert( List<Lnk.LnkFile> lnkFiles)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            Encoding ec = Encoding.GetEncoding("iso-8859-1");
            Encoding gbk = Encoding.GetEncoding("GBK");
            //byte[] btArr = ec.GetBytes(str);
            var r = new List<NLnkFile>();
            foreach (var item in lnkFiles)
            {
                if (item.LocalPath != null)
                {
                    if (!File.Exists(item.LocalPath))
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
                try
                {

                    r.Add(new NLnkFile
                    {
                        IconLocation = item.IconLocation,
                        RelativePath = item.RelativePath,
                        Name = item.Name==null? GetName(item.LocalPath):item.Name,
                        SourceFile = item.SourceFile,
                        //LocalPath=Encoding.GetEncoding("gb2312") item.LocalPath,
                        LocalPath = gbk.GetString(ec.GetBytes(item.LocalPath)),
                        Arguments = item.Arguments,

                        lnkFile=item

                    });
                }
                catch
                {
                    r.Add(new NLnkFile
                    {
                        IconLocation = item.IconLocation,
                        Name = item.Name==null? GetName(item.LocalPath):item.Name,
                        RelativePath = item.RelativePath,
                        SourceFile = item.SourceFile,
                        LocalPath = item.LocalPath,
                        //LocalPath = Encoding.Default.GetString(ec.GetBytes(item.LocalPath)),
                        Arguments = item.Arguments,
                        lnkFile = item

                    });
                }
            }
            return r;
        }
    }
}
