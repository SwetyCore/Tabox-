using System;
using System.Collections.Generic;
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
    }

    public static class NLnkFileConverter
    {
        public static List< NLnkFile> Convert( List<Lnk.LnkFile> lnkFiles)
        {
            var r = new List<NLnkFile>();
            foreach (var item in lnkFiles)
            {
                r.Add(new NLnkFile
                {
                    IconLocation=item.IconLocation,
                    RelativePath=item.RelativePath,
                    Name=item.Name,
                    SourceFile=item.SourceFile,
                    LocalPath=item.LocalPath,
                    Arguments=item.Arguments,
                });
            }
            return r;
        }
    }
}
