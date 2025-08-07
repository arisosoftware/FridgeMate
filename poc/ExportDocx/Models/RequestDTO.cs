
using System;
using System.Text.Json.Serialization;
using DocumentFormat.OpenXml.Packaging;


namespace FridgeMate.ExportDocx.Models;

// Simple request DTO. Place in a shared folder if you prefer.
public class GenerateDocxRequest
{
    public string? Name { get; set; }
    public DateTime? ReportDate { get; set; }
}