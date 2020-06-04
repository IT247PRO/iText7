using System;
using System.IO;
using System.Text;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Filter;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace itextSharp7
{
    class Program
    {
        static void Main(string[] args)
        {
            ExtractPhysicalAddress();
        }
        public static void ExtractPhysicalAddress()
        {

            var di = new DirectoryInfo(@"c:\temp\ime");
            foreach (var file in di.GetFiles("*.pdf"))
            {
                PdfDocument pdfDoc = new PdfDocument(new PdfReader(file.FullName));

                Rectangle rect = new Rectangle(300, 470, 70, 150);
                TextRegionEventFilter regionFilter = new TextRegionEventFilter(rect);

                FilteredEventListener listener = new FilteredEventListener();
                
                LocationTextExtractionStrategy extractionStrategy = listener
                    .AttachEventListener(new LocationTextExtractionStrategy(), regionFilter);

                
                new PdfCanvasProcessor(listener).ProcessPageContent(pdfDoc.GetPage(2));

                
                String actualText = extractionStrategy.GetResultantText();

                pdfDoc.Close();

                Console.WriteLine(file.Name);
                Console.WriteLine(actualText);

                using (StreamWriter writer = new StreamWriter(file.FullName.Replace(".pdf", ".txt")))
                {
                    writer.Write(actualText);
                }
            }
        }       
    }
}
