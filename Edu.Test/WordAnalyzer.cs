using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Html;
using Aspose;

namespace Edu.Test
{
    public class WordAnalyzer
    {
        public enum TreeType
        {
            TestNo,
            TestParagraphs
        }

        private string _docPath;
        private Document _doc;

        public WordAnalyzer()
        {
            
        }

        private string[] testNo = {"一", "二"};
        public WordAnalyzer(string pth)
        {
            _docPath =pth;
             _doc=new Document(_docPath);
        }

        public List<string> GetParaText()
        {
            myVisitor visitor=new myVisitor(_doc);
            _doc.Accept(visitor);

            return null;


        }


        
    }
}
