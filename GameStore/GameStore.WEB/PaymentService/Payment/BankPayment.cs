using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameStore.Domain.Entities;
using GameStore.WEB.Services.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GameStore.WEB.Services.Payment
{
    public class BankPayment : IPayment
    {
        public Order _order { private get; set; }

        public BankPayment(Order order)
        {
            _order = order;
        }

        public object MakePayment()
        {
            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            doc.SetMargins(10, 10, 10, 10);
            //Create PDF Table with 4 columns  
            PdfPTable tableLayout = new PdfPTable(4);
            doc.SetMargins(10, 10, 10, 10);
            //Create PDF Table  

            PdfWriter.GetInstance(doc, workStream).CloseStream = false;
            doc.Open();

            //Add Content to PDF   
            doc.Add(AddContentToPDF(tableLayout));

            // Closing the document  
            doc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return workStream;
        }

        protected PdfPTable AddContentToPDF(PdfPTable tableLayout)
        {
            float[] headers = { 50, 24, 45, 35}; //Header Widths  
            tableLayout.SetWidths(headers); //Set the pdf headers  
            tableLayout.WidthPercentage = 100; //Set the PDF File width percentage  
            tableLayout.HeaderRows = 1;

            List<OrderDetail> orderDetails = _order.OrderDetails.ToList();

            tableLayout.AddCell(new PdfPCell(new Phrase("Invoice", new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_CENTER
            });

             tableLayout.AddCell(new PdfPCell(new Phrase("Customer Id: " + _order.CustomerId.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT
            });

             tableLayout.AddCell(new PdfPCell(new Phrase("Order Id: " + _order.Id.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT
            });

             tableLayout.AddCell(new PdfPCell(new Phrase("Order date: " + _order.OrderDate.ToString(), new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT
            });

            //Add header  
            AddCellToHeader(tableLayout, "Game name");
            AddCellToHeader(tableLayout, "Price");
            AddCellToHeader(tableLayout, "Quantity");
            AddCellToHeader(tableLayout, "Sum");

            //Add body  
            foreach (var detail in orderDetails)
            {
                AddCellToBody(tableLayout, detail.Game.Name);
                AddCellToBody(tableLayout, detail.Game.Price.ToString());
                AddCellToBody(tableLayout, detail.Quantity.ToString());
                AddCellToBody(tableLayout, detail.Price.ToString());
            }

              tableLayout.AddCell(new PdfPCell(new Phrase("Total sum: " + _order.OrderDetails.Select(x => x.Price).Sum().ToString(), new Font(Font.FontFamily.HELVETICA, 8, 1, new BaseColor(0, 0, 0)))) {
                Colspan = 12, Border = 0, PaddingBottom = 5, HorizontalAlignment = Element.ALIGN_LEFT
            });

            return tableLayout;
        }

        private void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.YELLOW)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(128, 0, 0)
            });
        }

        private void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.BLACK)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT, Padding = 5, BackgroundColor = new BaseColor(255, 255, 255)
            });
        }
    }
}