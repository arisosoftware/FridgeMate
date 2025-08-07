using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using HandlebarsDotNet;
using System.Text;

namespace FridgeMate.ExportDocx.Services;

public class WordTemplateService
{
    public void RenderTemplate(string templatePath, object model, string outputPath)
    {
        // Load DOCX as text and render Handlebars
        byte[] docBytes = File.ReadAllBytes(templatePath);
        using var ms = new MemoryStream();
        ms.Write(docBytes, 0, docBytes.Length);

        using var wordDoc = WordprocessingDocument.Open(ms, true);
        string text;

        // Step 1: Get all text in the doc
        using (var reader = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
        {
            text = reader.ReadToEnd();
        }

        // Step 2: Handlebars render
        var template = Handlebars.Compile(text);
        var result = template(model);

        // Step 3: Replace content
        using (var writer = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
        {
            writer.Write(result);
        }

        File.WriteAllBytes(outputPath, ms.ToArray());
    }

    public void InsertSubDocAtBookmark(string mainPath, string subDocPath, string bookmarkName)
    {
        using var main = WordprocessingDocument.Open(mainPath, true);
        var mainBody = main.MainDocumentPart.Document.Body;

        // Find bookmark
        var bookmark = mainBody.Descendants<BookmarkStart>()
            .FirstOrDefault(b => b.Name == bookmarkName);

        if (bookmark == null)
            throw new Exception($"Bookmark {bookmarkName} not found.");

        var parent = bookmark.Parent;

        using var subDoc = WordprocessingDocument.Open(subDocPath, false);
        var subBodyClone = subDoc.MainDocumentPart.Document.Body.CloneNode(true);

        // Insert subDoc's content
        foreach (var element in subBodyClone.Elements())
        {
            parent.InsertAfter(element.CloneNode(true), bookmark);
        }

        main.MainDocumentPart.Document.Save();
    }
}
