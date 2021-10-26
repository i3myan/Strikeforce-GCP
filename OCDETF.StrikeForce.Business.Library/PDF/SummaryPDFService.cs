using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf.Colorspace;
using iText.Layout.Element;
using iText.Layout.Properties;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.PDF
{
    public class SummaryPDFService
    {
        private IQtrActivityAnalysisService analysis { get; set; }
        public SummaryPDFService(IQtrActivityAnalysisService analysis) { this.analysis = analysis; }

        public iText.Layout.Document Create(iText.Layout.Document document, IList<SummaryAnalysis> summary, string title, string fileName)
        {            
            Paragraph section = new Paragraph(title);
            section.SetFontSize(25);
            section.SetBold();            
            document.Add(section);

            LineSeparator aLine = new LineSeparator(new SolidLine());
            document.Add(aLine);
            document.Add(new Paragraph("\n\n"));
            Table newTable = new Table(22);
            
            newTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            newTable.SetAutoLayout();

            int i = 0;

            var test = WebColors.GetRGBColor("#e0dfdc");
            AddHeader(newTable);
            foreach (SummaryAnalysis aItem in summary)
            {
                newTable.StartNewRow();

                ++i;

                Cell aCell = new Cell();
                
                aCell.Add(new Paragraph(aItem.Order.ToString()));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 2));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                newTable.AddCell(aCell);
                

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                
                aCell.Add(new Paragraph(aItem.ActivityName.ToString()));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 15));
                aCell.SetMargin(500);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Atlanta) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Atlanta))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Baltimore) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Baltimore))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Boston) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Boston))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Cleveland) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Cleveland))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Chicago) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Chicago))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);
                
                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Dallas) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Dallas))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Denver) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Denver))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                //-----------------

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Detroit) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Detroit))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.ElPaso) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.ElPaso))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Houston) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Houston))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.KansasCity) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.KansasCity))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.LosAngeles) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.LosAngeles))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.NewYork) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.NewYork))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                //********************

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Phoenix) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Phoenix))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Sacramento) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Sacramento))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.SanDiego) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.SanDiego))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.SanJuan) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.SanJuan))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.StLouis) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.StLouis))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Tampa) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Tampa))));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);

                //********************
                aCell = new Cell();
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(test);
                aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Total.ToString()) ? "" : string.Format("{0:n0}", aItem.Total)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                aCell.SetTextAlignment(TextAlignment.RIGHT);
                newTable.AddCell(aCell);               
            }
            document.Add(newTable);
            return document;

            
        }

        public byte[] CreateSummaryAnalysis(int fy, int[] Quarters)
        {
            string fileName = "File-" + Guid.NewGuid().ToString().ToUpper() + ".pdf";
            PdfDocument pdf = new PdfDocument(new PdfWriter(fileName));                        

            iText.Layout.Document document = new iText.Layout.Document(pdf);
            pdf.SetDefaultPageSize(PageSize.A2.Rotate());
            
            var seizures = this.analysis.Total(StrikeForceTables.Seizures.ToString(), fy, Quarters);
            this.Create(document, seizures, "Seizures - " + fy.ToString(), string.Empty);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            var specific = this.analysis.Total(StrikeForceTables.Specific.ToString(), fy, Quarters);
            this.Create(document, specific, "Specific information (OCDETF and non-OCDETF cases)  - " + fy.ToString(), string.Empty);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            var required = this.analysis.Total(StrikeForceTables.Required.ToString(), fy, Quarters);
            this.Create(document, required, "Required Information (OCDETF and non-OCDETF cases)  - " + fy.ToString(), string.Empty);

            document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            var staffing = this.analysis.Total(StrikeForceTables.Staffing.ToString(), fy, Quarters);
            this.Create(document, required, "Staffings - " + fy.ToString(), string.Empty);
            
            document.Close();

            var result = File.ReadAllBytes(fileName);
            File.Delete(fileName);
            return result;
        }

        private void AddHeader(Table newTable)
        {
            newTable.StartNewRow();

            Cell aCell = new Cell();
            aCell.Add(new Paragraph("#"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 2));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Activity Name"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 15));

            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Atlanta"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Baltimore"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Boston"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Cleveland"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Chicago"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            
            aCell = new Cell();
            aCell.Add(new Paragraph("Dallas"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Denver"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            //-----------------

            aCell = new Cell();
            aCell.Add(new Paragraph("Detroit"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("El Paso"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Houston"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Kansas City"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Los Angeles"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("New York"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            //********************

            aCell = new Cell();
            aCell.Add(new Paragraph("Phoenix"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Sacramento"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("San Diego"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("San Juan"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("St. Louis"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Tampa"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            //********************
            aCell = new Cell();
            aCell.Add(new Paragraph("Total"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);
        }
    }
}
