using System;
using System.Collections.Generic;
using System.Drawing;

namespace Calculator
{
    public class Trix
    {
        public class InferenceResult
        {
            public String Reason { get; set; }
            public String Caption { get; set; }
            public Color Colour { get; set; }
            public int Number { get; set; }
        }

        public struct CalcParameters
        {
            public int SplitHorisontal { get; set; }
            public int SplitVertical { get; set; }
            public Size FormSize { get; set; }
            public Point FormPosition { get; set; }
            public List<int> ColumnsGridAutoWidth { get; set; }
            public List<int> ColumnsGridManualWidth { get; set; }
            public List<int> ColumnsGridRightWidth { get; set; }
            public String FolderDocuments { get; set; }
            public String FolderTemplates { get; set; }
            public String FolderIncoming { get; set; }
            public String FolderOutcoming { get; set; }
            public String FolderMaterials { get; set; }
            public int CommonMargin { get; set; }
            public int ZerosWithinInternalCode { get; set; }
            public bool UseWord2003 { get; set; }
        }
    }
}
