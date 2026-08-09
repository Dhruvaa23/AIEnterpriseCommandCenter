using AIEnterpriseCommandCenter.Application.Interfaces;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Helpers;


namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin,IT Admin,HR Manager")]

    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        // ==========================================================
        // Reports Dashboard
        // ==========================================================

        public IActionResult Index()
        {
            return View();
        }

        // ==========================================================
        // Employee Report
        // ==========================================================

        public async Task<IActionResult> EmployeeReport()
        {
            var employees = await _reportRepository.GetEmployeeReportAsync();

            return View(employees);
        }

        // ==========================================================
        // Asset Report
        // ==========================================================

        public async Task<IActionResult> AssetReport()
        {
            var assets = await _reportRepository.GetAssetReportAsync();

            return View(assets);
        }

        // ==========================================================
        // Project Report
        // ==========================================================

        public async Task<IActionResult> ProjectReport()
        {
            var projects = await _reportRepository.GetProjectReportAsync();

            return View(projects);
        }

        // ==========================================================
        // Ticket Report
        // ==========================================================

        public async Task<IActionResult> TicketReport()
        {
            var tickets = await _reportRepository.GetTicketReportAsync();

            return View(tickets);
        }

        // ==========================================================
        // Excel Export (Coming Next)
        // ==========================================================

        public async Task<IActionResult> ExportEmployeeExcel()
        {
            var employees = await _reportRepository.ExportEmployeesAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Employees");

            // Header
            worksheet.Cell(1, 1).Value = "Employee Code";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Department";
            worksheet.Cell(1, 5).Value = "Designation";
            worksheet.Cell(1, 6).Value = "Phone";
            worksheet.Cell(1, 7).Value = "Joining Date";
            worksheet.Cell(1, 8).Value = "Salary";
            worksheet.Cell(1, 9).Value = "Status";

            var header = worksheet.Range("A1:I1");

            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = emp.EmployeeCode;
                worksheet.Cell(row, 2).Value = emp.FullName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.Department;
                worksheet.Cell(row, 5).Value = emp.Designation;
                worksheet.Cell(row, 6).Value = emp.PhoneNumber;
                worksheet.Cell(row, 7).Value = emp.JoiningDate.ToString("dd-MM-yyyy");
                worksheet.Cell(row, 8).Value = emp.Salary;
                worksheet.Cell(row, 9).Value = emp.IsActive ? "Active" : "Inactive";

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Employee_Report_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> ExportAssetExcel()
        {
            var assets = await _reportRepository.ExportAssetsAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Assets");

            worksheet.Cell(1, 1).Value = "Asset Code";
            worksheet.Cell(1, 2).Value = "Asset Name";
            worksheet.Cell(1, 3).Value = "Category";
            worksheet.Cell(1, 4).Value = "Brand";
            worksheet.Cell(1, 5).Value = "Model";
            worksheet.Cell(1, 6).Value = "Status";
            worksheet.Cell(1, 7).Value = "Assigned To";
            worksheet.Cell(1, 8).Value = "Purchase Date";
            worksheet.Cell(1, 9).Value = "Purchase Price";

            var header = worksheet.Range("A1:I1");

            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Alignment.Horizontal =
                XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var asset in assets)
            {
                worksheet.Cell(row, 1).Value = asset.AssetCode;
                worksheet.Cell(row, 2).Value = asset.AssetName;
                worksheet.Cell(row, 3).Value = asset.Category;
                worksheet.Cell(row, 4).Value = asset.Brand;
                worksheet.Cell(row, 5).Value = asset.Model;
                worksheet.Cell(row, 6).Value = asset.Status;
                worksheet.Cell(row, 7).Value = asset.EmployeeName;
                worksheet.Cell(row, 8).Value = asset.PurchaseDate.ToString("dd-MM-yyyy");
                worksheet.Cell(row, 9).Value = asset.PurchasePrice;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Asset_Report_{DateTime.Now:yyyyMMdd}.xlsx");
        }


        public async Task<IActionResult> ExportProjectExcel()
        {
            var projects = await _reportRepository.ExportProjectsAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Projects");

            worksheet.Cell(1, 1).Value = "Project Code";
            worksheet.Cell(1, 2).Value = "Project Name";
            worksheet.Cell(1, 3).Value = "Manager";
            worksheet.Cell(1, 4).Value = "Priority";
            worksheet.Cell(1, 5).Value = "Status";
            worksheet.Cell(1, 6).Value = "Progress";
            worksheet.Cell(1, 7).Value = "Start Date";
            worksheet.Cell(1, 8).Value = "End Date";
            worksheet.Cell(1, 9).Value = "Team Members";

            var header = worksheet.Range("A1:I1");

            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var item in projects)
            {
                worksheet.Cell(row, 1).Value = item.ProjectCode;
                worksheet.Cell(row, 2).Value = item.ProjectName;
                worksheet.Cell(row, 3).Value = item.ManagerName;
                worksheet.Cell(row, 4).Value = item.Priority.ToString();
                worksheet.Cell(row, 5).Value = item.Status.ToString();
                worksheet.Cell(row, 6).Value = item.Progress + "%";
                worksheet.Cell(row, 7).Value = item.StartDate.ToString("dd-MM-yyyy");
                worksheet.Cell(row, 8).Value = item.EndDate.ToString("dd-MM-yyyy");
                worksheet.Cell(row, 9).Value = item.TeamMembers;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Project_Report_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> ExportTicketExcel()
        {
            var tickets = await _reportRepository.ExportTicketsAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Tickets");

            worksheet.Cell(1, 1).Value = "Ticket No";
            worksheet.Cell(1, 2).Value = "Title";
            worksheet.Cell(1, 3).Value = "Employee";
            worksheet.Cell(1, 4).Value = "Category";
            worksheet.Cell(1, 5).Value = "Priority";
            worksheet.Cell(1, 6).Value = "Status";
            worksheet.Cell(1, 7).Value = "Assigned To";
            worksheet.Cell(1, 8).Value = "Created On";

            var header = worksheet.Range("A1:H1");

            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.DarkBlue;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            int row = 2;

            foreach (var item in tickets)
            {
                worksheet.Cell(row, 1).Value = item.TicketNumber;
                worksheet.Cell(row, 2).Value = item.Title;
                worksheet.Cell(row, 3).Value = item.EmployeeName;
                worksheet.Cell(row, 4).Value = item.Category.ToString();
                worksheet.Cell(row, 5).Value = item.Priority.ToString();
                worksheet.Cell(row, 6).Value = item.Status.ToString();
                worksheet.Cell(row, 7).Value = item.AssignedTo ?? "-";
                worksheet.Cell(row, 8).Value = item.CreatedOn.ToString("dd-MM-yyyy");

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Ticket_Report_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        // ==========================================================
        // PDF Export 
        // ==========================================================

        public async Task<IActionResult> ExportEmployeePdf()
        {
            var employees = await _reportRepository.ExportEmployeesAsync();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    // Header
                    page.Header().Column(column =>
                    {
                        column.Item().Text("AI ENTERPRISE COMMAND CENTER")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Text("Employee Report")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy hh:mm tt}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    // Table
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn(2);
                            columns.ConstantColumn(70);
                        });

                        // Header
                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Code").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Name").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Department").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Designation").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Status").FontColor(Colors.White).Bold();
                        });

                        foreach (var emp in employees)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(emp.EmployeeCode);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(emp.FullName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(emp.Department);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(emp.Designation);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(emp.IsActive ? "Active" : "Inactive");
                        }
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("AI Enterprise Command Center | Page ");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "Employee_Report.pdf");
        }

        public async Task<IActionResult> ExportAssetPdf()
        {
            var assets = await _reportRepository.ExportAssetsAsync();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(column =>
                    {
                        column.Item().Text("AI ENTERPRISE COMMAND CENTER")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Text("Asset Report")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy hh:mm tt}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn(2);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Code").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Asset").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Category").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Status").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Assigned To").FontColor(Colors.White).Bold();
                        });

                        foreach (var asset in assets)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asset.AssetCode);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asset.AssetName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asset.Category);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asset.Status);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(asset.EmployeeName ?? "-");
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("AI Enterprise Command Center | Page ");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "Asset_Report.pdf");
        }

        public async Task<IActionResult> ExportProjectPdf()
        {
            var projects = await _reportRepository.ExportProjectsAsync();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(column =>
                    {
                        column.Item().Text("AI ENTERPRISE COMMAND CENTER")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Text("Project Report")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy hh:mm tt}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(70);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.ConstantColumn(60);
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Code").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Project").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Manager").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Status").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Progress").FontColor(Colors.White).Bold();
                        });

                        foreach (var project in projects)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(project.ProjectCode);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(project.ProjectName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(project.ManagerName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(project.Status.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(project.Progress + "%");
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("AI Enterprise Command Center | Page ");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "Project_Report.pdf");
        }

        public async Task<IActionResult> ExportTicketPdf()
        {
            var tickets = await _reportRepository.ExportTicketsAsync();

            var pdf = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);

                    page.Header().Column(column =>
                    {
                        column.Item().Text("AI ENTERPRISE COMMAND CENTER")
                            .FontSize(22)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        column.Item().Text("Ticket Report")
                            .FontSize(16)
                            .SemiBold();

                        column.Item().Text($"Generated: {DateTime.Now:dd MMM yyyy hh:mm tt}")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);

                        column.Item().PaddingTop(10).LineHorizontal(1);
                    });

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);
                            columns.RelativeColumn(2);
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Ticket").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Title").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Employee").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Priority").FontColor(Colors.White).Bold();
                            header.Cell().Background(Colors.Blue.Darken2).Padding(5).Text("Status").FontColor(Colors.White).Bold();
                        });

                        foreach (var ticket in tickets)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(ticket.TicketNumber);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(ticket.Title);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(ticket.EmployeeName);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(ticket.Priority.ToString());
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(ticket.Status.ToString());
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("AI Enterprise Command Center | Page ");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", "Ticket_Report.pdf");
        }


    }
}