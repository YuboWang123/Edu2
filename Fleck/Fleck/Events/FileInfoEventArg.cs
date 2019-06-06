using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Fleck.Events
{
    public class FileInfoEventArg : EventArgs
    {
        private readonly FileInfo _file;

        public FileInfoEventArg(FileInfo fileInfo)
        {
            _file = fileInfo;
        }

        public byte[] GetfileBytes()
        {
                if (_file.Exists)
                {
                    lock (_file)
                    {
                        using (FileStream fs = new FileStream(_file.FullName, FileMode.Open))
                        {
                            byte[] lenBytes = new byte[fs.Length];
                            fs.Read(lenBytes, 0, lenBytes.Length);
                            return lenBytes;
                        }
                    }
                }

                return null;

            
        }



    }
}
