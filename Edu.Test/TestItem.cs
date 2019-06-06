
using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Test
{
    public class BaseTest
    {
       
    }

    public class TestItem:BaseTest
    {
        public myVisitor.ItemType ItemType { get; set; } //data type
        public List<Node> StrContent { get; set; } //data with different style.
    }
}
