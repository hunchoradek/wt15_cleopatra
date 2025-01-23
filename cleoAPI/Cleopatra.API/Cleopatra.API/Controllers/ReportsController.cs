using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Cleopatra.Infrastructure;
using Cleopatra.Domain;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly SalonContext _context;

    public ReportsController(SalonContext context)
    {
        _context = context;
    }

    // GET: /api/reports/bookings
    [HttpGet("bookings")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GenerateBookingReport([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        var bookings = await _context.Appointments
            .Include(a => a.Client)
            .Include(a => a.Employee)
            .Where(a => a.appointment_date >= from && a.appointment_date <= to)
            .ToListAsync();

        if (!bookings.Any())
        {
            return NotFound("No bookings found for the selected period.");
        }

        // Set the QuestPDF license type
        QuestPDF.Settings.License = LicenseType.Community;

        // Create a memory stream
        var stream = new MemoryStream();

        try
        {
            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header()
                        .Text("Booking Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Booking ID");
                                header.Cell().Element(CellStyle).Text("Client");
                                header.Cell().Element(CellStyle).Text("Employee");
                                header.Cell().Element(CellStyle).Text("Service");
                                header.Cell().Element(CellStyle).Text("Date & Time");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var booking in bookings)
                            {
                                table.Cell().Element(CellStyle).Text(booking.appointment_id.ToString());
                                table.Cell().Element(CellStyle).Text(booking.Client?.name ?? "Unknown");
                                table.Cell().Element(CellStyle).Text(booking.Employee?.name ?? "Unknown");
                                table.Cell().Element(CellStyle).Text(booking.service);
                                table.Cell().Element(CellStyle).Text($"{booking.appointment_date:yyyy-MM-dd} {booking.start_time} - {booking.end_time}");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            }
                        });
                });
            });

            // Render the document to the memory stream
            document.GeneratePdf(stream);

            // Set the stream position to the beginning before returning
            stream.Position = 0;

            // Save the report record in the database
            var report = new Report
            {
                type = "Booking",
                created_at = DateTime.UtcNow
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Return the stream as a PDF file
            return File(stream, "application/pdf", "BookingReport.pdf");
        }
        catch (Exception ex)
        {
            // In case of an error, close the stream and return an error code
            stream.Dispose();
            return StatusCode(500, $"An error occurred while generating the report: {ex.Message}");
        }
    }

    // GET: /api/reports/clients
    [HttpGet("clients")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GenerateClientReport()
    {
        var clients = await _context.Clients.ToListAsync();

        if (!clients.Any())
        {
            return NotFound("No clients found.");
        }

        // Set the QuestPDF license type
        QuestPDF.Settings.License = LicenseType.Community;

        // Create a memory stream
        var stream = new MemoryStream();

        try
        {
            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header()
                        .Text("Client Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Client ID");
                                header.Cell().Element(CellStyle).Text("Name");
                                header.Cell().Element(CellStyle).Text("Phone Number");
                                header.Cell().Element(CellStyle).Text("Email");
                                header.Cell().Element(CellStyle).Text("Notes");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var client in clients)
                            {
                                table.Cell().Element(CellStyle).Text(client.client_id.ToString());
                                table.Cell().Element(CellStyle).Text(client.name);
                                table.Cell().Element(CellStyle).Text(client.phone_number ?? "N/A");
                                table.Cell().Element(CellStyle).Text(client.email ?? "N/A");
                                table.Cell().Element(CellStyle).Text(client.notes ?? "N/A");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            }
                        });
                });
            });

            // Render the document to the memory stream
            document.GeneratePdf(stream);

            // Set the stream position to the beginning before returning
            stream.Position = 0;

            // Save the report record in the database
            var report = new Report
            {
                type = "Client",
                created_at = DateTime.UtcNow
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Return the stream as a PDF file
            return File(stream, "application/pdf", "ClientReport.pdf");
        }
        catch (Exception ex)
        {
            // In case of an error, close the stream and return an error code
            stream.Dispose();
            return StatusCode(500, $"An error occurred while generating the report: {ex.Message}");
        }
    }

    // GET: /api/reports/resources
    [HttpGet("resources")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GenerateResourceReport()
    {
        var resources = await _context.Resources.ToListAsync();

        if (!resources.Any())
        {
            return NotFound("No resources found.");
        }

        // Set the QuestPDF license type
        QuestPDF.Settings.License = LicenseType.Community;

        // Create a memory stream
        var stream = new MemoryStream();

        try
        {
            // Generate the PDF using QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header()
                        .Text("Resource Report")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Resource ID");
                                header.Cell().Element(CellStyle).Text("Name");
                                header.Cell().Element(CellStyle).Text("Quantity");
                                header.Cell().Element(CellStyle).Text("Unit");
                                header.Cell().Element(CellStyle).Text("Reorder Level");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                }
                            });

                            foreach (var resource in resources)
                            {
                                table.Cell().Element(CellStyle).Text(resource.resource_id.ToString());
                                table.Cell().Element(CellStyle).Text(resource.name);
                                table.Cell().Element(CellStyle).Text(resource.quantity.ToString());
                                table.Cell().Element(CellStyle).Text(resource.unit ?? "N/A");
                                table.Cell().Element(CellStyle).Text(resource.reorder_level.ToString());

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                }
                            }
                        });
                });
            });

            // Render the document to the memory stream
            document.GeneratePdf(stream);

            // Set the stream position to the beginning before returning
            stream.Position = 0;

            // Save the report record in the database
            var report = new Report
            {
                type = "Resource",
                created_at = DateTime.UtcNow
            };
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // Return the stream as a PDF file
            return File(stream, "application/pdf", "ResourceReport.pdf");
        }
        catch (Exception ex)
        {
            // In case of an error, close the stream and return an error code
            stream.Dispose();
            return StatusCode(500, $"An error occurred while generating the report: {ex.Message}");
        }
    }
    [HttpGet("all")]
    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> GetAllReports()
    {
        var reports = await _context.Reports.ToListAsync();
        return Ok(reports);
    }
}
