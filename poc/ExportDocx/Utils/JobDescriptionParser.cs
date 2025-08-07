using System.Text.RegularExpressions;

namespace FridgeMate.ExportDocx.Utils;

// This method returns a demo job post for Autodesk, including company info, job details, requirements, culture, and contact email.
// The job post is structured to provide a comprehensive overview of the position and the company, suitable for use in a job posting application.
// The data is hardcoded for demonstration purposes and can be used to test the functionality of the job posting application.
using System.Linq;
using System.Text.RegularExpressions;
using System;
using System.IO;
using System.Collections.Generic;
using FridgeMate.ExportDocx.Models;


public static class JobDescriptionParser
{
    public static JobPostDto Parse(string text)
    {
        var dto = new JobPostDto();

        dto.Company.Name = ExtractCompanyName(text);
        dto.Company.Overview = ExtractSection(text, "About the job", "Position Overview");
        dto.Company.Mission = ExtractFirstLineContaining(text, new[] { "mission", "vision" });
        dto.Company.CybersecurityFocus = ExtractFirstLineContaining(text, new[] { "cyber", "security", "platform" });

        dto.Job.Title = ExtractTitle(text);
        dto.Job.Department = ExtractFirstLineContaining(text, new[] { "team", "department", "services" });
        dto.Job.Summary = ExtractSection(text, "Position Overview", "Responsibilities");
        dto.Job.Responsibilities = ExtractBulletedList(text, "Responsibilities");

        dto.Requirements.Skills = ExtractLinesContaining(text, new[] { "skills", "programming", "java", "c++", "typescript" });
        dto.Requirements.TechnicalKnowledge = ExtractLinesContaining(text, new[] { "cloud", "api", "ci/cd", "database", "network", "incident", "design" });
        dto.Requirements.Capabilities = ExtractLinesContaining(text, new[] { "lead", "mentor", "coach", "drive", "deliver", "own", "design" });

        dto.Culture.InclusionStatement = ExtractFirstLineContaining(text, new[] { "diversity", "inclusive", "equity", "inclusion" });
        dto.Culture.Values = ExtractFirstLineContaining(text, new[] { "values", "culture", "character" });
        dto.Culture.Character = ExtractFirstLineContaining(text, new[] { "we are", "we believe", "we value", "character" });

        dto.ContactEmail = ExtractEmail(text);

        return dto;
    }

    private static string ExtractCompanyName(string text)
    {
        var match = Regex.Match(text, @"(?i)(?<=About the job\s*)\b[A-Z][a-zA-Z0-9]+\b");
        return match.Success ? match.Value : "Unknown Company";
    }

    private static string ExtractTitle(string text)
    {
        var match = Regex.Match(text, @"(?<=Job Requisition ID\s*#\s*\d+\n?)\d+,\s*(.*)");
        return match.Success ? match.Groups[1].Value.Trim() : "Unknown Title";
    }

    private static string ExtractSection(string text, string startHeader, string endHeader)
    {
        int start = text.IndexOf(startHeader, StringComparison.OrdinalIgnoreCase);
        int end = text.IndexOf(endHeader, StringComparison.OrdinalIgnoreCase);

        if (start >= 0 && end > start)
        {
            return text.Substring(start + startHeader.Length, end - start - startHeader.Length).Trim();
        }

        return "";
    }

    private static string ExtractFirstLineContaining(string text, string[] keywords)
    {
        var lines = text.Split('\n');
        foreach (var line in lines)
        {
            if (keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
                return line.Trim();
        }
        return "";
    }

    private static List<string> ExtractLinesContaining(string text, string[] keywords)
    {
        var lines = text.Split('\n');
        return lines
            .Where(line => keywords.Any(k => line.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0))
            .Select(line => line.Trim('-').Trim())
            .Distinct()
            .ToList();
    }

    private static List<string> ExtractBulletedList(string text, string header)
    {
        var list = new List<string>();
        var lines = text.Split('\n');
        bool capture = false;

        foreach (var line in lines)
        {
            if (line.StartsWith(header, StringComparison.OrdinalIgnoreCase))
                capture = true;
            else if (capture && string.IsNullOrWhiteSpace(line))
                break;
            else if (capture && (line.TrimStart().StartsWith("-") || line.TrimStart().StartsWith("•")))
                list.Add(line.Trim('-', '•', ' ').Trim());
        }

        return list;
    }

    private static string ExtractEmail(string text)
    {
        var match = Regex.Match(text, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-z]{2,}");
        return match.Success ? match.Value : "";
    }





    public static JobPostDto GetDemo1()
    {

        var jobPost = new JobPostDto
        {
            Company = new CompanyInfo
            {
                Name = "OpenText",
                Overview = "OpenText is a global leader in information management, where innovation, creativity, and collaboration are key.",
                Mission = "AI-First. Future-Driven. Human-Centered.",
                CybersecurityFocus = "Developing state-of-the-art security technologies to protect critical assets and sensitive information."
            },
            Job = new JobInfo
            {
                Title = "Lead Software Developer (Browser Security)",
                Department = "Cybersecurity",
                Summary = "Modifying the actual source code of a browser to empower penetration testers.",
                Responsibilities = new List<string>
        {
            "Work on industry-leading cybersecurity testing platform",
            "Modify actual browser source code",
            "Design and implement DAST scanner",
            "Engage in full-stack development",
            "Implement security analytics"
        }
            },
            Requirements = new CandidateProfile
            {
                Skills = new List<string>
        {
            "C++", "JavaScript", "TypeScript"
        },
                TechnicalKnowledge = new List<string>
        {
            "Browser networking internals",
            "Debugger usage on live browsers",
            "JavaScript engine internals",
            "Browser extension architecture",
            "Operating system-specific browser behaviors"
        },
                Capabilities = new List<string>
        {
            "Diagnose complex networking and browser issues",
            "Reverse engineer and modify browser behavior",
            "Build tooling for security researchers"
        }
            },
            Culture = new CultureInfo
            {
                InclusionStatement = "OpenText embraces diversity, equity, and inclusion in all aspects of our business.",
                Values = "Trust, Innovation, Collaboration, Outcome Ownership",
                Character = "More than a company — a global community of passionate contributors."
            },
            ContactEmail = "hr@opentext.com"
        };

        return jobPost;
    }


    public static JobPostDto GetDemo2()
{


    var jobPost = new JobPostDto
    {
        Company = new CompanyInfo
        {
            Name = "Autodesk",
            Overview = "Autodesk is a leader in design and make technology, delivering cloud-based data platforms for the AEC and MFG industries.",
            Mission = "Enable teams to imagine, design, and make a better world.",
            CybersecurityFocus = "Not specifically cybersecurity-focused; the role is centered around data platform engineering at exabyte scale."
        },
        Job = new JobInfo
        {
            Title = "Principal Software Developer",
            Department = "Autodesk Platform Services – Data Models Team",
            Summary = "Lead high-stakes technical initiatives, mentor engineers, and contribute to real-time cloud data platforms used across Autodesk products.",
            Responsibilities = new List<string>
        {
            "Lead team-level outcomes and critical business initiatives.",
            "Independently drive key technical and business outcomes.",
            "Collaborate with cross-functional teams to deliver large-scale solutions.",
            "Lead and participate in technical incident response and resolution.",
            "Act as product owner when required and balance technical and business priorities.",
            "Mentor and coach team members, fostering engineering excellence.",
            "Drive platform-wide improvements in quality, scalability, and engineering practices.",
            "Lead technical discussions across the organization and influence outcomes.",
            "Ensure on-time delivery of projects with unclear or evolving scopes."
        }
        },
        Requirements = new CandidateProfile
        {
            Skills = new List<string>
        {
            "Java", "Spring Boot", "AWS", "REST", "GraphQL", "Go", "TypeScript"
        },
            TechnicalKnowledge = new List<string>
        {
            "Cloud systems and web services",
            "Incident management and root cause analysis",
            "Object-oriented design and patterns",
            "Database design at cloud scale",
            "CI/CD pipelines (e.g., Jenkins)",
            "API development using REST, GraphQL, gRPC",
            "SDLC and Agile methodologies"
        },
            Capabilities = new List<string>
        {
            "Lead multi-engineer, cross-functional projects",
            "Mentor and develop talent",
            "Act as a product owner and balance diverse priorities",
            "Drive continuous improvement and engineering excellence"
        }
        },
        Culture = new CultureInfo
        {
            InclusionStatement = "Autodesk is committed to creating a diverse and inclusive workplace. We welcome people of all backgrounds to contribute to a better world.",
            Values = "Customer obsession, Innovation, Empowerment, Impact",
            Character = "We are creators who imagine better, design with intent, and make real change happen."
        },
        ContactEmail = "careers@autodesk.com"
    };
    return jobPost;
}
}
