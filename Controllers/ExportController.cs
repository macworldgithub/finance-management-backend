// Controllers/ExportController.cs
using Microsoft.AspNetCore.Mvc;
using finance_management_backend.Services;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly AssessmentOfAdequacyService _service;
    private readonly ChartPdfService _chartPdfService;

    public ExportController(AssessmentOfAdequacyService service, ChartPdfService chartPdfService)
    {
        _service = service;
        _chartPdfService = chartPdfService;
    }

    // GET api/export/assessment/pdf
    [HttpGet("assessment/pdf")]
    public async Task<IActionResult> ExportAssessmentPdf([FromQuery] string? search = null)
    {
        // get all items or filtered list (you can adjust paging as required)
        var paged = await _service.GetAllAsync(page: 1, search: search, pageSize: 10000);
        var items = paged.Items;

        var pdfBytes = await _chartPdfService.GeneratePdfAsync(items);
        return File(pdfBytes, "application/pdf", "AssessmentOfAdequacy.pdf");
    }
}
