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
        public static IO_Object Inner_Directory { get; set; } = new IO_Object() { Object = new DirectoryInfo("..") };

        public string Path { get; set; }

        public dynamic Object { get; set; }

        public IO_Object()
        {

        }

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
            if(this.Object.Name == "bin")
            {
                return $"[{this.Object.ToString()}]";
            }
            else
            {
                return $"[{this.Object.Name}]";
            }
        }
    }
}
