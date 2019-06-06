using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.Math;
using Aspose.Words.Tables;



namespace Edu.Test
{
    /// <summary>
    /// extract word
    /// </summary>
    public class myVisitor:DocumentVisitor
    {
        private readonly Document _doc;
    
        private string[] itemNo = { "一","二","三" }; //test number.
        private string[] splitors = {"、", ":", "："," "};
        private List<Node> _NodeCollection;
        private List<TestItem> _TestList;
        private TestItem _test;
        private bool _extractGo;
        private string BigTitle;
       

        public List<TestItem> TestList => _TestList;
        public List<Node> NodeCollection => _NodeCollection;

        public enum ItemType
        {
             QstRequirement,
             QstTitle,
             QstContent,
             QstAnalyze,
             QstAnswer
        }

        public myVisitor(Document doc)
        {
            _doc = doc;
            _NodeCollection=new List<Node>();
            _extractGo = false;
            _TestList=new List<TestItem>();
        }

        public List<Node> GetNodeList()
        {
            return NodeCollection;
        }

        public override VisitorAction VisitRun(Run run)
        {
            bool isConent =IsSignCurrent(run);
            bool isRqir = IsTitleCurrent(run);

           

            if (isConent)
            {
                _test.ItemType = GetNodeType(run.Text);
               
            }

            if (isRqir)
            {
                _test.ItemType = ItemType.QstRequirement;
                
            }

          

           return VisitorAction.Continue;
        }

 

        public override VisitorAction VisitParagraphEnd(Paragraph paragraph)
        {
            if (!_extractGo)
            {
                NodeCollection.Add(new Run(_doc, "\r\n"));
            }
            else
            {
                
                if (IsSignNext(paragraph) || IsTitleNext(paragraph) || IsSignForNewItem(paragraph)) //if next paragr is sign or big title.
                {
                    _test.StrContent = NodeCollection; //current testitem node list.

                    if (IsSignNext(paragraph))
                    {

                    }

                    if (IsTitleNext(paragraph))  //add test item big Title.
                    {


                    }


                }
            }


            return VisitorAction.Continue;
        }



        #region Judge Node
        /// <summary>
        /// is new test item. start with 1./2. /3.....
        /// </summary>
        /// <param name="run"></param>
        /// <returns></returns>
        private bool IsSignForNewItem(Paragraph nd)
        {
            Run r =(Run) nd.GetChild(NodeType.Run, 0, false);
            if (r != null)
            {
                if (r.Text.Length <= 3 && int.TryParse(r.Text,out int s))
                {
                    return true;
                }
                else
                {
                    _extractGo = true;
                    return false;
                }
            }

            return false;
            throw new NotImplementedException();
        }
        private bool IsTitleCurrent(Node run)
        {
            var s= splitors.Contains(run.GetText().Substring(0, 1));
             _extractGo =s? true:false;
             return s;
        }

        private bool IsSignCurrent(Node run)
        {
            var s= run.GetText().StartsWith("【");
            _extractGo = s ? true : false;
            return s;
        }

        private bool IsSignNext(Node nd)
        {
            var s=nd.NextSibling.GetText().StartsWith("【");
            _extractGo =s? false:true;
            return s;
        }

        private bool IsTitleNext(Node nd)
        {
            var s= splitors.Contains(nd.NextSibling.GetText().Substring(0, 1));
            _extractGo = s ? false : true;
            return s;
        } 
        #endregion

        private ItemType GetNodeType(string text)
        {
            throw new NotImplementedException();
        }

        public override VisitorAction VisitTableStart(Table table)
        {
            if (!_extractGo)
            {
                NodeCollection.Add(new Run(_doc, "\r\n"));
            }
            else
            {
                NodeCollection.Add(table);
            }
            return VisitorAction.Continue;
        }

        public override VisitorAction VisitShapeStart(Shape shape)
        {
            if (_extractGo)
            {
                NodeCollection.Add(shape);
            }

            return VisitorAction.Continue;
        }
    
        public override VisitorAction VisitOfficeMathStart(OfficeMath officeMath)
        {
            if (_extractGo)
            {
                NodeCollection.Add(officeMath);
            }
          
            return VisitorAction.Continue;
        }

 
        public override VisitorAction VisitParagraphStart(Paragraph paragraph)
        {
            if (_extractGo)
            {
                NodeCollection.Add(paragraph);
            }
            return VisitorAction.Continue;
        }

       


  


    }
}
