using SelectPdf;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Reflection;

namespace HtmlToPdf.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HtmlToPdfController : ControllerBase
    {
        string htmlBase = @"<!DOCTYPE html><html><head><meta name=""viewport"" content=""width=device-width"" /><title>Index</title></head><body><img src=""@Src"" style=""width: 100px; height: auto;""><table style=""width:100%""><tr><td style=""width:20%""></td><td style=""width:60%"" align=""center"" valign=""top""><table><tr><td align=""center""><h2 style=""margin:0px"">@Negocio</h2></td></tr></table></td></tr><tr><td colspan=""3"" height=""20""></td></tr><tr><td colspan=""2""><table style=""width:100%""><tr><td colspan=""1"" style=""width:20%"">Cliente:</td><td colspan=""3"" style=""width:80%;border-bottom:1px solid black"">@CLIENTE</td></tr></table><table style=""width:100%""><tr><td style=""width:20%"">Documento:</td><td style=""width:30%;border-bottom:1px solid black"">Recibo de pago</td><td align=""right"" style=""width:10%"">Fecha:</td><td style=""width:40%;border-bottom:1px solid black"">@FECHA</td></tr></table></td><td></td></tr><tr><td colspan=""3"" height=""30""></td></tr><tr><td colspan=""3""><table class=""border"" style=""width:100%;""><thead><tr style=""background-color:#D8D8D8""><th>Producto</th><th>Precio</th><th>Cantidad</th><th>Precio x Cantidad</th></tr></thead><tbody>@FILAS</tbody></table></td></tr></table><div style=""text-align: right;""><h3 style=""background-color:#D8D8D8; display: inline-block; margin: 0;"">Total: $ @Total</h3></div><h2 align=""center"">Gracias por comprar en @Negocio! vuelva pronto.</h2></body></html>";
        //string htmlSamuray = @"<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""><title>Timesheet</title><style>body{font-family:Arial,sans-serif;margin:20px;}.container{width:100%;max-width:800px;margin:0 auto;border:1px solid #000;padding:20px;}.header,.footer{text-align:center;}.header img{width:100px;height:auto;}.header,.footer,.details,.timesheet,.signature{margin-bottom:20px;}.details,.timesheet{width:100%;}.details td,.timesheet td,.timesheet th{border:1px solid #000;padding:5px;text-align:center;}.timesheet th{background-color:#D8D8D8;}.no-border{border:none;}.signature td{border:none;padding:10px;}.total-row td{border-top:2px solid #000;font-weight:bold;}</style></head><body><div class=""container""><div class=""header""><img src=""logo.png"" alt=""ABM Logo""><p>TEL: (905) 948-1400<br>FAX: (905) 948-1300<br>EMAIL: info@aragonbuilding.com<br>TOLL FREE: 1-866-9ARAGON</p><p>* PLEASE FAX OR EMAIL YOUR TIME SHEET EVERY SECOND MONDAY *</p></div><table class=""details""><tr><td class=""no-border"">NAME: <strong>Alex Perera</strong></td><td class=""no-border"">EMPLOYEE No: ____________</td></tr></table><table class=""timesheet""><thead><tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Location</th><th>Total Hrs</th></tr></thead><tbody><tr><td>@d1</td><td>@ti1</td><td>@to1</td><td>@l</td><td>@h1</td></tr><tr><td>@d2</td><td>@ti2</td><td>@to2</td><td>@l</td><td>@h2</td></tr><tr><td>@d3</td><td>@ti3</td><td>@to3</td><td>@l</td><td>@h3</td></tr><tr><td>@d4</td><td>@ti4</td><td>@to4</td><td>@l</td><td>@h4</td></tr><tr><td>@d5</td><td>@ti5</td><td>@to5</td><td>@l</td><td>@h5</td></tr><tr><td>@d6</td><td>@ti6</td><td>@to6</td><td>@l</td><td>@h6</td></tr><tr><td>@d7</td><td>@ti7</td><td>@to8</td><td>@l</td><td>@h7</td></tr><tr><td>@d8</td><td>@ti8</td><td>@to8</td><td>@l</td><td>@h8</td></tr><tr><td>@d9</td><td>@ti9</td><td>@to9</td><td>@l</td><td>@h9</td></tr><tr><td> @d10 </td><td> @ti10 </td><td>@to10</td><td>@l</td><td>@h10</td></tr><tr><td>@d11</td><td>@ti11</td><td>@to11</td><td>@l</td><td>@h11</td></tr><tr><td>@d12</td><td>@ti12</td><td>@to12</td><td>@l</td><td>@h12</td></tr><tr><td>@d13</td><td>@ti13</td><td>@to13</td><td>@l</td><td>@h13</td></tr><tr><td>@d14</td><td>@ti14</td><td>@to14</td><td>@l</td><td>@h14</td></tr><tr class=""total-row""><td colspan=""4"">TOTAL</td><td>@th</td></tr></tbody></table><table class=""signature""><tr><td>SIGNATURE: @firma </td><td>SUPERVISOR: <strong>MOE</strong></td></tr></table></div></body></html>";
        
        [HttpPost("cliente")]
        public async Task<IActionResult> ConvertHtmlToPdf([FromBody] HtmlModel model)
        {

            string html = htmlBase;
            //html = html.Replace("@Src", src);
            html = html.Replace("@CLIENTE", model.Cliente);
            html = html.Replace("@Negocio", model.Negocio);
            html = html.Replace("@FILAS", model.Filas);
            html = html.Replace("@FECHA", DateTime.Now.ToString("dd/MM/yyyy"));
            html = html.Replace("@Total", model.Total);
            string nombrePdf = "Recibo" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".pdf";
            SelectPdf.HtmlToPdf oHtmlToPdf = new SelectPdf.HtmlToPdf();

            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            return File(
                pdf,
                "application/pdf",
                nombrePdf
            );
        }

        [HttpPost("samuray")]
        public async Task<IActionResult> ConvertHtmlToPdfSamuray([FromBody] List<ApiModel> lstTimeSheet)
        {
            string htmlBaseSamuray = @"<!DOCTYPE html><html lang=""en""><head><meta charset=""UTF-8""><meta name=""viewport"" content=""width=device-width, initial-scale=1.0""><title>Timesheet</title><style>body{font-family:Arial,sans-serif;margin:20px;}.container{width:100%;max-width:800px;margin:0 auto;border:1px solid #000;padding:20px;}.header,.footer{text-align:center;}.header img{width:100px;height:auto;}.header,.footer,.details,.timesheet,.signature{margin-bottom:20px;}.details,.timesheet{width:100%;}.details td,.timesheet td,.timesheet th{border:1px solid #000;padding:5px;text-align:center;}.timesheet th{background-color:#D8D8D8;}.no-border{border:none;}.signature td{border:none;padding:10px;}.total-row td{border-top:2px solid #000;font-weight:bold;}</style></head><body><div class=""container""><div class=""header""><img src=""logo.png"" alt=""ABM Logo""><p>TEL: (905) 948-1400<br>FAX: (905) 948-1300<br>EMAIL: info@aragonbuilding.com<br>TOLL FREE: 1-866-9ARAGON</p><p>* PLEASE FAX OR EMAIL YOUR TIME SHEET EVERY SECOND MONDAY *</p></div><table class=""details""><tr><td class=""no-border"">NAME: <strong>Alex Perera</strong></td><td class=""no-border"">EMPLOYEE No: ____________</td></tr></table><table class=""timesheet""><thead><tr><th>Date</th><th>Time In</th><th>Time Out</th><th>Location</th><th>Total Hrs</th></tr></thead><tbody>@filas</tbody><tr class=""total-row""><td colspan=""4"">TOTAL</td><td> @th </td></tr></table><table class=""signature""><tr><td>SIGNATURE: @firma </td><td>SUPERVISOR: <strong>MOE</strong></td></tr></table></div></body></html>";

            string html = htmlBaseSamuray;
            string filasHtml = "";
            string total = "";
            foreach (ApiModel model in lstTimeSheet)
            {
                filasHtml += "<tr>";
                filasHtml += "<td>" + model.Date + "</td>";
                filasHtml += "<td>" + model.TimeIn + "</td>";
                filasHtml += "<td>" + model.TimeOut + "</td>";
                filasHtml += "<td>2 Westmeath Lane</td>";
                if (model.Horas == 24.ToString())
                {
                    filasHtml += "<td>" + "0" + "</td>";
                }
                else
                {
                    filasHtml += "<td>" + model.Horas + "</td>";
                }

                filasHtml += "</tr>";
                total = model.Total;
            }
            html = htmlBaseSamuray.Replace("@filas", filasHtml)
                                  .Replace("@th", total)
                                  .Replace("@firma", "Alex Perera");

            string nombrePdf = "TimeSheet" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".pdf";
            SelectPdf.HtmlToPdf oHtmlToPdf = new SelectPdf.HtmlToPdf();

            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();

            return File(
                pdf,
                "application/pdf",
                nombrePdf
            );





        }





    }

    public class HtmlModel
    {
        public string Cliente { get; set; }
        public string Negocio { get; set; }
        public string Filas { get; set; }
        public string Total { get; set; }
    }

    public class ApiModel
    {
        public string Date { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Horas { get; set; }
        public string Total { get; set; }

    }
}

