namespace FridgeMate.ExportDocx.Models;

public class JobPostDto
{
    public CompanyInfo Company { get; set; }
    public JobInfo Job { get; set; }
    public CandidateProfile Requirements { get; set; }
    public CultureInfo Culture { get; set; }
    public string ContactEmail { get; set; }
}

public class CompanyInfo
{
    public string Name { get; set; }
    public string Overview { get; set; }
    public string Mission { get; set; }
    public string CybersecurityFocus { get; set; }
}

public class JobInfo
{
    public string Title { get; set; }
    public string Department { get; set; }
    public string Summary { get; set; }
    public List<string> Responsibilities { get; set; }
}

public class CandidateProfile
{
    public List<string> Skills { get; set; }
    public List<string> TechnicalKnowledge { get; set; }
    public List<string> Capabilities { get; set; }
}

public class CultureInfo
{
    public string InclusionStatement { get; set; }
    public string Values { get; set; }
    public string Character { get; set; }
}