using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace Total_Commander.Model
{
    class IO_Object
    {
        public Icon Icon { get; set; }

        public string Path { get; set; }

        public dynamic Object { get; set; }

        public IO_Object(string path)
        {
            this.Path = path;
            if (File.Exists(path))
            {
                this.Object = new FileInfo(path);
            }
            else if (Directory.Exists(path))
            {
                this.Object = new DirectoryInfo(path);
            }
            else
            {
                this.Object = null;
            }
        }

        public override string ToString()
        {
            return $"[{this.Object.Name}]";
        }
    }
}
