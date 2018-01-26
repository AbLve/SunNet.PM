using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EvoPdf;

namespace SF.Framework.PDF
{
   public class PdfProvider
    {
       private PdfConverter pdfConverter = null;
       public PdfProvider(PdfConverter converter)
       {
           pdfConverter = converter;
       }
       public PdfProvider()
       {
           pdfConverter = GetPdfConverter();
       }
       private PdfConverter GetPdfConverter()
       {
           PdfConverter pdfConverter = new PdfConverter();
           pdfConverter.HtmlViewerWidth = 1024;
           pdfConverter.HtmlViewerHeight = 0;
           pdfConverter.PdfDocumentOptions.ShowHeader = false;
           pdfConverter.PdfDocumentOptions.ShowFooter = false;
           pdfConverter.PdfDocumentOptions.LeftMargin = 10;
           pdfConverter.PdfDocumentOptions.RightMargin = 10;
           pdfConverter.PdfDocumentOptions.TopMargin = 10;
           pdfConverter.PdfDocumentOptions.BottomMargin = 10;
           pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
           return pdfConverter;
       }
       /// <summary>
       /// Generate PDF file with the whole html content
       /// </summary>
       /// <param name="contentHtml">internal content</param>
       /// <param name="fileName">export file name</param>
       public void GeneratePDF(string contentHtml,string fileName)
       {   
           byte[] pdfBytes = pdfConverter.GetPdfBytesFromHtmlString(contentHtml);
           ExportPdfFile(pdfBytes, fileName);
       }

       private void ExportPdfFile(byte[] pdfBytes,string fileName)
       {
           pdfConverter.LicenseKey = SF.Framework.SFConfig.EVOPDFKEY; 
           System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
           response.Clear();
           response.AddHeader("Content-Type", "application/pdf");
           response.AddHeader("Content-Disposition", String.Format("attachment; filename=" + fileName + ".pdf; size={0}", pdfBytes.Length.ToString()));
           response.BinaryWrite(pdfBytes);
           response.End();
       }
      
    }
}
