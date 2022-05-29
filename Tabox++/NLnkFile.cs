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
        public static string GetName(LnkFile  file)
        {

            string r = "";
            var extra = file.ExtraBlocks;
            try
            {
                foreach (var item in extra)
                {
                    try
                    {

                        r = new Dictionary<string, string>
                            ((new List<ExtensionBlocks.PropertySheet>
                            (((Lnk.ExtraData.PropertyStoreDataBlock)
                            item)
                            .PropertyStore.Sheets)[0]).PropertyNames).FirstOrDefault()
                            .Value;
                        if (r == null)
                        {
                            r = new Dictionary<string, string>
                            ((new List<ExtensionBlocks.PropertySheet>
                            (((Lnk.ExtraData.PropertyStoreDataBlock)
                            item)
                            .PropertyStore.Sheets)[1]).PropertyNames).FirstOrDefault()
                            .Value;
                        }

                        break;
                    }
                    catch
                    {

                    }


                }

            }
            catch {
                return GetName(Decode(file.LocalPath));
            }
            //return extra;
            return r;
        }
        public static string GetIcon(LnkFile file)
        {
            string r = "";
            var extra = file.ExtraBlocks;
            try
            {

                r = new Dictionary<string, string>
                    ((new List<ExtensionBlocks.PropertySheet>
                    (((Lnk.ExtraData.PropertyStoreDataBlock)
                    (new List<Lnk.ExtraData.ExtraDataBase>(file.ExtraBlocks)[0]))
                    .PropertyStore.Sheets)[0]).PropertyNames).FirstOrDefault()
                    .Value;

            }
            catch { }
            //return extra;
            return r;
        }

        public static string Decode(string str)
        {

            Encoding ec = Encoding.GetEncoding("iso-8859-1");
            Encoding gbk = Encoding.GetEncoding("GBK");
            try
            {
                return gbk.GetString(ec.GetBytes(str));
            }
            catch
            {
                return str;
            }
        }

        public static List< NLnkFile> Convert( List<Lnk.LnkFile> lnkFiles)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            //byte[] btArr = ec.GetBytes(str);
            var r = new List<NLnkFile>();
            foreach (var item in lnkFiles)
            {
                if (item.LocalPath != null)
                {
                    var path = Decode(item.LocalPath);
                    if (!File.Exists(path))
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
                        IconLocation = item.IconLocation==null?GetIcon(item):item.IconLocation,
                        RelativePath = item.RelativePath,
                        Name = item.Name==null? GetName(item):item.Name,
                        SourceFile = item.SourceFile,
                        //LocalPath=Encoding.GetEncoding("gb2312") item.LocalPath,
                        LocalPath = Decode(item.LocalPath),
                        Arguments = item.Arguments,

                        lnkFile=item

                    });
                }
                catch(Exception ex)
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
