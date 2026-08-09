using AIEnterpriseCommandCenter.Application.Common;
using AIEnterpriseCommandCenter.Application.DTOs.Asset;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Repositories;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin,IT Admin")]
    public class AssetController : Controller
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public AssetController(IAssetRepository assetRepository, IEmployeeRepository employeeRepository)
        {
            _assetRepository = assetRepository;
            _employeeRepository = employeeRepository;
        }

        // Asset List
        public async Task<IActionResult> Index(string? searchTerm, int page = 1)
        {
            const int pageSize = 5;

            ViewBag.Dashboard = await _assetRepository.GetDashboardAsync();

            ViewBag.SearchTerm = searchTerm;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchResults = await _assetRepository.SearchAsync(searchTerm);

                var res = new PagedResult<AssetDto>
                {
                    Items = searchResults.ToList(),
                    PageNumber = 1,
                    PageSize = searchResults.Count(),
                    TotalRecords = searchResults.Count()
                };

                return View(res);
            }

            var model = await _assetRepository.GetPagedAsync(page, pageSize);

            return View(model);
        }
        // Create Page
        public IActionResult Create()
        {
            return View();
        }

        // Save Asset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAssetDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _assetRepository.AddAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assign(int id)
        {
            ViewBag.AssetId = id;

            ViewBag.Employees = await _employeeRepository.GetAllEmployeesAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Assign(int assetId, int employeeId)
        {
            await _assetRepository.AssignAssetAsync(assetId, employeeId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Return(int id)
        {
            await _assetRepository.ReturnAssetAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);

            if (asset == null)
                return NotFound();

            return View(asset);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);

            if (asset == null)
                return NotFound();

            var dto = new UpdateAssetDto
            {
                Id = asset.Id,
                AssetCode = asset.AssetCode,
                AssetName = asset.AssetName,
                Category = asset.Category,
                Brand = asset.Brand,
                Model = asset.Model,
                PurchaseDate = asset.PurchaseDate,
                PurchasePrice = asset.PurchasePrice,
                Status = asset.Status,
                EmployeeId = asset.EmployeeId
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateAssetDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            await _assetRepository.UpdateAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _assetRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var assets = await _assetRepository.ExportAsync();

            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Assets");

            worksheet.Cell(1, 1).Value = "Asset Code";
            worksheet.Cell(1, 2).Value = "Asset Name";
            worksheet.Cell(1, 3).Value = "Category";
            worksheet.Cell(1, 4).Value = "Brand";
            worksheet.Cell(1, 5).Value = "Status";
            worksheet.Cell(1, 6).Value = "Employee";

            int row = 2;

            foreach (var asset in assets)
            {
                worksheet.Cell(row, 1).Value = asset.AssetCode;
                worksheet.Cell(row, 2).Value = asset.AssetName;
                worksheet.Cell(row, 3).Value = asset.Category;
                worksheet.Cell(row, 4).Value = asset.Brand;
                worksheet.Cell(row, 5).Value = asset.Status;
                worksheet.Cell(row, 6).Value = asset.EmployeeName;

                row++;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Assets_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
        }
    }
}
