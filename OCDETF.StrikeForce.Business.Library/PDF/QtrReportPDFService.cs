using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using OCDETF.StrikeForce.Business.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCDETF.StrikeForce.Business.Library.PDF
{
    public class QtrReportPDFService
    {
        private IQuarterReportService QtrReportService { get; set; }
        private IQuarterlyActivityService QtrActivityService { get; set; }

        IStaffingService StaffingService { get; set; }
        iText.Layout.Document PdfDocument { get; set; }

        public QtrReportPDFService(IQuarterReportService qtrReportService, IQuarterlyActivityService qtrActivityService, IStaffingService staffingService)
        {
            this.QtrReportService = qtrReportService;
            this.StaffingService = staffingService;
            this.QtrActivityService = qtrActivityService;
        }

        public void CreatePDF(string quarterlyReportID)
        {
            
            string fileName = "File-" + Guid.NewGuid().ToString().ToUpper() + ".pdf";
            PdfDocument pdf = new PdfDocument(new PdfWriter(fileName));
            
          

            PdfDocument = new iText.Layout.Document(pdf);
            pdf.SetDefaultPageSize(PageSize.A5);
            PdfDocument.SetFontSize(5);
            PdfDocument.SetTopMargin(0.5f);
            
            QuarterlyReport qtrReport = this.QtrReportService.GetReport(quarterlyReportID, string.Empty, true);

            qtrReport.OperationsBegin = string.IsNullOrEmpty(qtrReport.OperationsBegin) ? "" : qtrReport.OperationsBegin;
            qtrReport.MOUDate = string.IsNullOrEmpty(qtrReport.MOUDate) ? "" : qtrReport.MOUDate;
            qtrReport.HistoryNotes = string.IsNullOrEmpty(qtrReport.HistoryNotes) ? "" : qtrReport.HistoryNotes;
            qtrReport.Mission = string.IsNullOrEmpty(qtrReport.Mission) ? "" : qtrReport.Mission;
            qtrReport.Structure = string.IsNullOrEmpty(qtrReport.Structure) ? "" : qtrReport.Structure;


            
            Paragraph title = new Paragraph(qtrReport.Name + " (FY " + qtrReport.FiscalYear + " - Q" + qtrReport.Quarter + ")");
            title.SetFontSize(15);
            title.SetBold();
            PdfDocument.Add(title);

            LineSeparator aLine = new LineSeparator(new SolidLine());
            PdfDocument.Add(aLine);
            PdfDocument.Add(new Paragraph("\n"));



            SetHistory(qtrReport);
            SetMission(qtrReport);
            SetStructure(qtrReport);
            SetOfficeLocations(qtrReport);
            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            
            
            Table staffingTable = new Table(7);
            staffingTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            staffingTable.SetAutoLayout();
                        
            AddStaffingHeader(staffingTable);
            SetStaffingData(staffingTable, quarterlyReportID);
            PdfDocument.Add(staffingTable);

            Table requiredTable = new Table(7);
            requiredTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            requiredTable.SetAutoLayout();
            
            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            
            AddActivityHeader(requiredTable, "Required Information (OCDETF and non-OCDETF cases)");
            SetQuarterlyActivity(requiredTable, quarterlyReportID, StrikeForceTables.Required);
            PdfDocument.Add(requiredTable);


            Table ocdetfTable = new Table(7);
            ocdetfTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            ocdetfTable.SetAutoLayout();

            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            AddActivityHeader(ocdetfTable, "OCDETF cases only (data supplied by OCDETF Executive Office)*");
            SetQuarterlyActivity(ocdetfTable, quarterlyReportID, StrikeForceTables.OCDETF);
            PdfDocument.Add(ocdetfTable);

            Table seizuresTable = new Table(7);
            seizuresTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            seizuresTable.SetAutoLayout();

            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            AddActivityHeader(seizuresTable, "Seizures");
            SetQuarterlyActivity(seizuresTable, quarterlyReportID, StrikeForceTables.Seizures);
            PdfDocument.Add(seizuresTable);
            PdfDocument.Add(new Paragraph("\n"));
            PdfDocument.Add(new Paragraph("Seizure Notes:"));
            PdfDocument.Add(new Paragraph(qtrReport.SeizureNotes));

            Table specificTable = new Table(7);
            specificTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            specificTable.SetAutoLayout();

            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            AddActivityHeader(specificTable, "Specific information (OCDETF and non-OCDETF cases)");
            SetQuarterlyActivity(specificTable, quarterlyReportID, StrikeForceTables.Specific);
            PdfDocument.Add(specificTable);
            PdfDocument.Add(new Paragraph("\n"));

            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            PdfDocument.Add(new Paragraph("Challenges").SetFontSize(10));
            PdfDocument.Add(new LineSeparator(new SolidLine()));
            PdfDocument.Add(new Paragraph(qtrReport.Challenges));

            PdfDocument.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            PdfDocument.Add(new Paragraph("Heads Up").SetFontSize(10));
            PdfDocument.Add(new LineSeparator(new SolidLine()));
            PdfDocument.Add(new Paragraph(qtrReport.HeadsUp));

            PdfDocument.Close();
        }

        private void SetOfficeLocations(QuarterlyReport qtrReport)
        {
            Table officeLocationsTable = new Table(2);
            officeLocationsTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            officeLocationsTable.SetBorder(new SolidBorder(WebColors.GetRGBColor("#ffffff"), 0.0f));
            officeLocationsTable.SetAutoLayout();

            Cell c1 = new Cell();
            c1.Add(new Paragraph("Office Locations: "));
            c1.SetBorder(Border.NO_BORDER);
            c1.SetBold();
            c1.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            officeLocationsTable.AddCell(c1);

            Cell c2 = new Cell();
            c2.Add(new Paragraph(qtrReport.OfficeLocations));
            c2.SetBorder(Border.NO_BORDER);
            c2.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            officeLocationsTable.AddCell(c2);

            PdfDocument.Add(officeLocationsTable);
        }

        private void SetStructure(QuarterlyReport qtrReport)
        {
            Table structureTable = new Table(2);
            structureTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            structureTable.SetBorder(new SolidBorder(WebColors.GetRGBColor("#ffffff"), 0.0f));
            structureTable.SetAutoLayout();

            Cell c1 = new Cell();
            c1.Add(new Paragraph("Structure: "));
            c1.SetBorder(Border.NO_BORDER);
            c1.SetBold();
            c1.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            structureTable.AddCell(c1);

            Cell c2 = new Cell();
            c2.Add(new Paragraph(qtrReport.Structure));
            c2.SetBorder(Border.NO_BORDER);
            c2.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            structureTable.AddCell(c2);

            PdfDocument.Add(structureTable);
        }

        private void SetMission(QuarterlyReport qtrReport)
        {
            Table missionTable = new Table(2);
            missionTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            missionTable.SetBorder(new SolidBorder(WebColors.GetRGBColor("#ffffff"), 0.0f));
            missionTable.SetAutoLayout();

            Cell c1 = new Cell();
            c1.Add(new Paragraph("Mission Statement: "));
            c1.SetBorder(Border.NO_BORDER);
            c1.SetBold();
            c1.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            missionTable.AddCell(c1);

            Cell c2 = new Cell();
            c2.Add(new Paragraph(qtrReport.Mission));
            c2.SetBorder(Border.NO_BORDER);
            c2.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            missionTable.AddCell(c2);

            PdfDocument.Add(missionTable);
        }

        private void SetHistory(QuarterlyReport qtrReport)
        {
            Table historyTable = new Table(2);
            historyTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
            historyTable.SetBorder(new SolidBorder(WebColors.GetRGBColor("#ffffff"), 0.0f));
            historyTable.SetAutoLayout();



            historyTable.StartNewRow().SetBorder(Border.NO_BORDER);

            Cell c1 = new Cell();
            c1.Add(new Paragraph("Beginning of Operations: "));
            c1.SetBorder(Border.NO_BORDER);
            c1.SetBold();
            c1.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            historyTable.AddCell(c1);

            Cell c2 = new Cell();
            c2.Add(new Paragraph(qtrReport.OperationsBegin));
            c2.SetBorder(Border.NO_BORDER);
            c2.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            historyTable.AddCell(c2);

            Cell c3 = new Cell();
            c3.Add(new Paragraph("Date of MOU: "));
            c3.SetBorder(Border.NO_BORDER);
            c3.SetBold();
            c3.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            historyTable.AddCell(c3);

            Cell c4 = new Cell();
            c4.Add(new Paragraph(qtrReport.MOUDate));
            c4.SetBorder(Border.NO_BORDER);
            c4.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            historyTable.AddCell(c4);

            Cell c5 = new Cell();
            c5.Add(new Paragraph("Notes: "));
            c5.SetBorder(Border.NO_BORDER);
            c5.SetBold();
            c5.SetWidth(new UnitValue(UnitValue.PERCENT, 25));
            historyTable.AddCell(c5);

            Cell c6 = new Cell();
            c6.Add(new Paragraph(qtrReport.HistoryNotes));
            c6.SetBorder(Border.NO_BORDER);
            c6.SetWidth(new UnitValue(UnitValue.PERCENT, 75));
            historyTable.AddCell(c6);


            PdfDocument.Add(historyTable);
        }

        //public iText.Layout.Document Create(iText.Layout.Document document, IList<SummaryAnalysis> summary, string title, string fileName)
        //{


        //    Paragraph section = new Paragraph(title);
        //    section.SetFontSize(25);
        //    section.SetBold();
        //    document.Add(section);

        //    LineSeparator aLine = new LineSeparator(new SolidLine());
        //    document.Add(aLine);
        //    document.Add(new Paragraph("\n\n"));
        //    Table newTable = new Table(22);
        //    newTable.SetWidth(new UnitValue(UnitValue.PERCENT, 100));
        //    newTable.SetAutoLayout();

        //    int i = 0;

        //    var test = WebColors.GetRGBColor("#e0dfdc");
        //    AddHeader(newTable);
        //    foreach (SummaryAnalysis aItem in summary)
        //    {
        //        newTable.StartNewRow();

        //        ++i;

        //        Cell aCell = new Cell();
        //        aCell.Add(new Paragraph(aItem.Order.ToString()));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 2));
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        newTable.AddCell(aCell);


        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(aItem.ActivityName.ToString()));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 15));

        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Atlanta) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Atlanta))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Baltimore) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Baltimore))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Boston) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Boston))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Cleveland) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Cleveland))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Chicago) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Chicago))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Dallas) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Dallas))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Denver) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Denver))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        //-----------------

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Detroit) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Detroit))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.ElPaso) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.ElPaso))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Houston) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Houston))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.KansasCity) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.KansasCity))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.LosAngeles) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.LosAngeles))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.NewYork) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.NewYork))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        //********************

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Phoenix) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Phoenix))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Sacramento) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Sacramento))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.SanDiego) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.SanDiego))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.SanJuan) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.SanJuan))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.StLouis) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.StLouis))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Tampa) ? "" : string.Format("{0:n0}", Convert.ToInt32(aItem.Tampa))));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);

        //        //********************
        //        aCell = new Cell();
        //        if (i % 2 == 0)
        //            aCell.SetBackgroundColor(test);
        //        aCell.Add(new Paragraph(string.IsNullOrEmpty(aItem.Total.ToString()) ? "" : string.Format("{0:n0}", aItem.Total)));
        //        aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
        //        aCell.SetTextAlignment(TextAlignment.RIGHT);
        //        newTable.AddCell(aCell);
        //    }
        //    document.Add(newTable);
        //    return document;


        //}

        private void AddStaffingHeader(Table newTable)
        {
            Paragraph title = new Paragraph("Staffing");
            title.SetFontSize(10);
            title.SetBold();
            PdfDocument.Add(title);
            
            LineSeparator aLine = new LineSeparator(new SolidLine());
            PdfDocument.Add(aLine);
            PdfDocument.Add(new Paragraph("\n"));

            newTable.StartNewRow();

            Cell aCell = new Cell();
            aCell.Add(new Paragraph("#"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Agency"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("# Agents"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("# Federal Agency TFOs"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            newTable.AddCell(aCell);


            aCell = new Cell();
            aCell.Add(new Paragraph("# Analysts"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("# Other"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Total for Agency"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
            newTable.AddCell(aCell);

            //pdfDocument.Add(newTable);
        }

        private void SetStaffingData(Table newTable, string quarterlyReportID)
        {
            IList<Staffing> lstStaffing = this.StaffingService.Get(quarterlyReportID);
            var backgroundColor = WebColors.GetRGBColor("#e0dfdc");
            int i = 0;

            foreach (Staffing aItem in lstStaffing)
            {
                newTable.StartNewRow();
                i++;

                Cell aCell = new Cell();
                aCell.Add(new Paragraph(aItem.Order.ToString()));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 4));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(aItem.AgencyName));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.NumberOfAgents)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.NumberOfFederalTFOs)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.NumberOfAnalysts)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.OtherNumbers)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.Total)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 16));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);
            }            
        }


        private void AddActivityHeader(Table newTable, string aTitle)
        {
            Paragraph title = new Paragraph(aTitle);
            title.SetFontSize(10);
            title.SetBold();
            PdfDocument.Add(title);
            
            LineSeparator aLine = new LineSeparator(new SolidLine());
            PdfDocument.Add(aLine);
            PdfDocument.Add(new Paragraph("\n"));

            newTable.StartNewRow();

            Cell aCell = new Cell();
            aCell.Add(new Paragraph("#"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 3));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph(""));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 32));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("First Quarter"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Second Quarter"));
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            newTable.AddCell(aCell);


            aCell = new Cell();
            aCell.Add(new Paragraph("Third Quarter"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Fourth Quarter"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
            newTable.AddCell(aCell);

            aCell = new Cell();
            aCell.Add(new Paragraph("Total"));
            aCell.SetTextAlignment(TextAlignment.CENTER);
            aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
            newTable.AddCell(aCell);

            //pdfDocument.Add(newTable);
        }
        private void SetQuarterlyActivity(Table newTable, string quarterlyReportID, StrikeForceTables table)
        {
            IList<QuarterlyActivity> lstStaffing = this.QtrActivityService.Get(quarterlyReportID, table.ToString());
            var backgroundColor = WebColors.GetRGBColor("#e0dfdc");
            int i = 0;

            foreach (QuarterlyActivity aItem in lstStaffing)
            {
                newTable.StartNewRow();
                i++;

                Cell aCell = new Cell();
                aCell.Add(new Paragraph(aItem.Order.ToString()));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 3));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(aItem.ActivityName));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 32));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.FirstQuarter)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.SecondQuarter)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.ThirdQuarter)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.FourthQuarter)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);

                aCell = new Cell();
                aCell.Add(new Paragraph(string.Format("{0:n0}", aItem.Total)));
                aCell.SetWidth(new UnitValue(UnitValue.PERCENT, 11));
                if (i % 2 == 0)
                    aCell.SetBackgroundColor(backgroundColor);
                newTable.AddCell(aCell);
            }
            
        }
    }
}
