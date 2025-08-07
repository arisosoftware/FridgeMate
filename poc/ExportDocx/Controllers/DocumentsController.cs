namespace FridgeMate.ExportDocx.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;        
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO.Pipelines;
using System.IO.Compression;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using FridgeMate.ExportDocx.Models;
using FridgeMate.ExportDocx.Services;



[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly WordTemplateService _wordService;
    private readonly IWebHostEnvironment _env;

    public DocumentsController(WordTemplateService wordService, IWebHostEnvironment env)
    {
        _wordService = wordService;
        _env = env;
    }

    // POST: /api/documents/generate-docx
    [HttpPost("generate-docx")]
    public IActionResult GenerateDocx([FromBody] GenerateDocxRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request body is required.");
        }

        var model = new
        {
            Name = string.IsNullOrWhiteSpace(request.Name) ? "Alice" : request.Name,
            ReportDate = (request.ReportDate ?? DateTime.Now).ToShortDateString()
        };

        var baseDir = Path.Combine(_env.ContentRootPath, "Templates");
        var mainTemplate = Path.Combine(baseDir, "main.docx");
        var sectionTemplate = Path.Combine(baseDir, "section.docx");

        if (!System.IO.File.Exists(mainTemplate))
        {
            return NotFound($"Template not found: {mainTemplate}");
        }

        if (!System.IO.File.Exists(sectionTemplate))
        {
            return NotFound($"Template not found: {sectionTemplate}");
        }

        // Write to a unique temp file to avoid collisions across concurrent requests.
        var outputPath = Path.Combine(Path.GetTempPath(), $"output-{Guid.NewGuid():N}.docx");

        try
        {
            _wordService.RenderTemplate(mainTemplate, model, outputPath);
            _wordService.InsertSubDocAtBookmark(outputPath, sectionTemplate, "SectionPlaceholder");

            var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            var downloadName = "output.docx";

            // Stream file and delete it when the response completes.
            var stream = System.IO.File.OpenRead(outputPath);
            HttpContext.Response.OnCompleted(() =>
            {
                try
                {
                    stream.Dispose();
                    System.IO.File.Delete(outputPath);
                }
                catch
                {
                    // Best-effort cleanup.
                }
                return Task.CompletedTask;
            });

            return File(stream, contentType, downloadName);
        }
        catch (Exception ex)
        {
            // Consider logging ex.
            // Attempt cleanup if something went wrong after file creation.
            try { if (System.IO.File.Exists(outputPath)) System.IO.File.Delete(outputPath); } catch { /* ignore */ }
            return Problem(title: "Failed to generate document", detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}

