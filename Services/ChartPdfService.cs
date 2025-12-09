// // Services/ChartPdfService.cs
// using System;
// using System.Linq;
// using System.Collections.Generic;
// using System.Text.Json;
// using System.Threading.Tasks;
// using PuppeteerSharp;
// using PuppeteerSharp.Media;
// using finance_management_backend.Models;

// namespace finance_management_backend.Services
// {
//     public class ChartPdfService
//     {
//         private readonly BrowserFetcher _browserFetcher;

//         public ChartPdfService()
//         {
//             // BrowserFetcher used as fallback/downloader (prefer to download at startup in Program.cs)
//             _browserFetcher = new BrowserFetcher();
//         }

//         public async Task<byte[]> GeneratePdfAsync(IEnumerable<AssessmentOfAdequacy> items)
//         {
//             var list = items.ToList();

//             // Labels: include "No" field for pie/donut identification
//             var labelsWithNo = list.Select(x => $"{x.No} - {x.Process}").ToArray();
//             var labels = list.Select(x => x.Process).ToArray(); // plain process names for other charts
//             var scores = list.Select(x => x.DesignAdequacyScore).ToArray();

//             // Constant array for max score (10) to compare against
//             var maxScores = labels.Select(_ => 10).ToArray();

//             // JSON payloads for embedding into HTML/JS
//             var labelsWithNoJson = JsonSerializer.Serialize(labelsWithNo);
//             var labelsJson = JsonSerializer.Serialize(labels);
//             var scoresJson = JsonSerializer.Serialize(scores);
//             var maxScoresJson = JsonSerializer.Serialize(maxScores);

//             // HTML template: each chart on its own page; Horizontal/Vertical/Radar compare to Max (10).
//             var html = $@"
// <!doctype html>
// <html>
// <head>
//   <meta charset='utf-8'>
//   <meta name='viewport' content='width=device-width, initial-scale=1'>
//   <title>Assessment Of Adequacy - Charts</title>
//   <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
//   <style>
//     @page {{ size: A4; margin: 15mm; }}
//     body {{ font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; }}
//     .page {{
//       width: 100%;
//       height: 100%;
//       page-break-after: always;
//       display: flex;
//       flex-direction: column;
//       align-items: center;
//       justify-content: center;
//       padding: 10mm;
//       box-sizing: border-box;
//     }}
//     h1, h2 {{ margin: 0; padding: 0; }}
//     .chart-wrapper {{ width: 100%; max-width: 1100px; }}
//     canvas {{ width: 100% !important; height: 520px !important; background: #fff; border-radius: 6px; box-shadow: 0 0 6px rgba(0,0,0,0.08); }}
//     .small-canvas {{ height: 420px !important; }}
//   </style>
// </head>
// <body>

//   <!-- Horizontal Bar (compare actual vs total) -->
//   <div class='page'>
//     <h2>Design Adequacy — Horizontal Bar (Actual vs Total)</h2>
//     <div class='chart-wrapper'>
//       <canvas id='horizontalBar'></canvas>
//     </div>
//   </div>

//   <!-- Vertical Bar (compare actual vs total) -->
//   <div class='page'>
//     <h2>Design Adequacy — Vertical Bar (Actual vs Total)</h2>
//     <div class='chart-wrapper'>
//       <canvas id='verticalBar'></canvas>
//     </div>
//   </div>

//   <!-- Radar (compare actual vs total) -->
//   <div class='page'>
//     <h2>Design Adequacy — Radar (Actual vs Total)</h2>
//     <div class='chart-wrapper'>
//       <canvas id='radarChart' class='small-canvas'></canvas>
//     </div>
//   </div>

//   <!-- Line (with horizontal line at 10) -->
//   <div class='page'>
//     <h2>Design Adequacy — Line Chart (with total = 10)</h2>
//     <div class='chart-wrapper'>
//       <canvas id='lineChart'></canvas>
//     </div>
//   </div>

//   <!-- Pie (older UI: labels show No - Process) -->
//   <div class='page'>
//     <h2>Design Adequacy — Pie</h2>
//     <div class='chart-wrapper'>
//       <canvas id='pieChart' class='small-canvas'></canvas>
//     </div>
//   </div>

//   <!-- Donut (older UI: labels show No - Process) -->
//   <div class='page'>
//     <h2>Design Adequacy — Donut</h2>
//     <div class='chart-wrapper'>
//       <canvas id='donutChart' class='small-canvas'></canvas>
//     </div>
//   </div>

// <script>
//   const labelsWithNo = {labelsWithNoJson}; // used for pie/donut labels: 'No - Process'
//   const labels = {labelsJson};             // used for other charts
//   const scores = {scoresJson};
//   const maxScores = {maxScoresJson};

//   // Colors requested: blue for actual, orange for total (bars/radar/line)
//   const colorBlue = 'rgba(54,162,235,0.85)';
//   const colorBlueBorder = 'rgba(54,162,235,1)';
//   const colorOrange = 'rgba(255,159,64,0.85)';
//   const colorOrangeBorder = 'rgba(255,159,64,1)';

//   // Generate a distinct color per process (good-looking HSL palette)
//   function generatePalette(n, alpha) {{
//     if (alpha === undefined) alpha = 0.85;
//     const colors = [];
//     const borders = [];
//     for (let i = 0; i < n; i++) {{
//       const hue = Math.round((i * 360) / n); // distribute around hue wheel
//       // main color (slightly bright) - use string concatenation to avoid C# interpolation issues
//       colors.push('hsla(' + hue + ', 70%, 50%, ' + alpha + ')');
//       // border color (darker)
//       borders.push('hsla(' + hue + ', 70%, 40%, 1)');
//     }}
//     return {{ colors: colors, borders: borders }};
//   }}

//   const palette = generatePalette(labelsWithNo.length);
//   const sliceColors = palette.colors;
//   const sliceBorders = palette.borders;

//   // Horizontal Bar (indexAxis: 'y') with Actual and Total datasets
//   new Chart(document.getElementById('horizontalBar'), {{
//     type: 'bar',
//     data: {{
//       labels: labels,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: colorBlue,
//           borderColor: colorBlueBorder,
//           borderWidth: 1
//         }},
//         {{
//           label: 'Total design adequacy score (10)',
//           data: maxScores,
//           backgroundColor: colorOrange,
//           borderColor: colorOrangeBorder,
//           borderWidth: 1
//         }}
//       ]
//     }},
//     options: {{
//       indexAxis: 'y',
//       responsive: true,
//       scales: {{
//         x: {{ suggestedMin: 0, suggestedMax: 10 }}
//       }},
//       plugins: {{
//         legend: {{ position: 'top' }}
//       }}
//     }}
//   }});

//   // Vertical Bar with Actual and Total
//   new Chart(document.getElementById('verticalBar'), {{
//     type: 'bar',
//     data: {{
//       labels: labels,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: colorBlue,
//           borderColor: colorBlueBorder,
//           borderWidth: 1
//         }},
//         {{
//           label: 'Total design adequacy score (10)',
//           data: maxScores,
//           backgroundColor: colorOrange,
//           borderColor: colorOrangeBorder,
//           borderWidth: 1
//         }}
//       ]
//     }},
//     options: {{
//       responsive: true,
//       scales: {{
//         y: {{ suggestedMin: 0, suggestedMax: 10 }}
//       }},
//       plugins: {{
//         legend: {{ position: 'top' }}
//       }}
//     }}
//   }});

//   // Radar: Actual vs Total
//   new Chart(document.getElementById('radarChart'), {{
//     type: 'radar',
//     data: {{
//       labels: labels,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: colorBlue,
//           borderColor: colorBlueBorder,
//           fill: true,
//           tension: 0.1
//         }},
//         {{
//           label: 'Total design adequacy score (10)',
//           data: maxScores,
//           backgroundColor: 'rgba(255,159,64,0.15)',
//           borderColor: colorOrangeBorder,
//           fill: false,
//           borderDash: [5, 5]
//         }}
//       ]
//     }},
//     options: {{
//       responsive: true,
//       elements: {{ line: {{ tension: 0.1 }} }},
//       scales: {{
//         r: {{ suggestedMin: 0, suggestedMax: 10 }}
//       }},
//       plugins: {{
//         legend: {{ position: 'top' }}
//       }}
//     }}
//   }});

//   // Line: actual + horizontal line at 10 (second dataset)
//   new Chart(document.getElementById('lineChart'), {{
//     type: 'line',
//     data: {{
//       labels: labels,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: colorBlue,
//           borderColor: colorBlueBorder,
//           fill: false,
//           tension: 0.2,
//           pointRadius: 3
//         }},
//         {{
//           label: 'Total design adequacy score (10)',
//           data: labels.map(() => 10),
//           type: 'line',
//           fill: false,
//           borderColor: colorOrangeBorder,
//           borderWidth: 2,
//           borderDash: [6,4],
//           pointRadius: 0,
//           tension: 0
//         }}
//       ]
//     }},
//     options: {{
//       responsive: true,
//       scales: {{
//         y: {{ suggestedMin: 0, suggestedMax: 10 }}
//       }},
//       plugins: {{ legend: {{ position: 'top' }} }}
//     }}
//   }});

//   // Pie: older UI but with distinct slice colors and labelsWithNo
//   new Chart(document.getElementById('pieChart'), {{
//     type: 'pie',
//     data: {{
//       labels: labelsWithNo,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: sliceColors,
//           borderColor: sliceBorders,
//           borderWidth: 1
//         }}
//       ]
//     }},
//     options: {{
//       responsive: true,
//       plugins: {{
//         legend: {{ position: 'right' }}
//       }}
//     }}
//   }});

//   // Donut: older UI but with distinct slice colors and labelsWithNo
//   new Chart(document.getElementById('donutChart'), {{
//     type: 'doughnut',
//     data: {{
//       labels: labelsWithNo,
//       datasets: [
//         {{
//           label: 'Process design adequacy score',
//           data: scores,
//           backgroundColor: sliceColors,
//           borderColor: sliceBorders,
//           borderWidth: 1
//         }}
//       ]
//     }},
//     options: {{
//       responsive: true,
//       cutout: '50%',
//       plugins: {{
//         legend: {{ position: 'right' }}
//       }}
//     }}
//   }});

//   // Notify Puppeteer that charts are rendered
//   window.onload = () => {{ window.__chartsRendered = true; }};
// </script>
// </body>
// </html>
// ";

//             // Attempt to ensure Chromium is available (fallback). Don't depend on returned type.
//             try
//             {
//                 await _browserFetcher.DownloadAsync();
//             }
//             catch
//             {
//                 // swallow - if download fails here, Puppeteer may use a system-installed Chromium
//             }

//             var launchOptions = new LaunchOptions
//             {
//                 Headless = true,
//                 Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
//             };

//             // Launch, render, print PDF
//             using var browser = await Puppeteer.LaunchAsync(launchOptions);
//             using var page = await browser.NewPageAsync();

//             await page.SetContentAsync(html, new NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Load } });

//             // Wait for the in-page flag (or fallback)
//             try
//             {
//                 await page.WaitForFunctionAsync("() => window.__chartsRendered === true", new WaitForFunctionOptions { Timeout = 8000 });
//             }
//             catch
//             {
//                 await Task.Delay(1000);
//             }

//             var pdfOptions = new PdfOptions
//             {
//                 Format = PaperFormat.A4,
//                 PrintBackground = true,
//                 MarginOptions = new MarginOptions
//                 {
//                     Top = "10mm",
//                     Bottom = "10mm",
//                     Left = "10mm",
//                     Right = "10mm"
//                 }
//             };

//             var pdfBytes = await page.PdfDataAsync(pdfOptions);
//             return pdfBytes;
//         }
//     }
// }

// Services/ChartPdfService.cs
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using finance_management_backend.Models;

namespace finance_management_backend.Services
{
    public class ChartPdfService
    {
        private readonly BrowserFetcher _browserFetcher;

        public ChartPdfService()
        {
            _browserFetcher = new BrowserFetcher();
        }

        public async Task<byte[]> GeneratePdfAsync(IEnumerable<AssessmentOfAdequacy> items)
        {
            var list = items.OrderByDescending(x => x.No).ToList(); // REVERSED ORDER HERE

            var labelsWithNo = list.Select(x => $"{x.No} - {x.Process}").ToArray();
            var fullLabels = list.Select(x => x.Process).ToArray();
            var shortLabels = fullLabels.Select(label =>
                label.Length > 15 ? label.Substring(0, 15).Trim() + "…" : label
            ).ToArray();

            var scores = list.Select(x => x.DesignAdequacyScore).ToArray();
            var maxScores = Enumerable.Repeat(10, list.Count).ToArray();

            var labelsWithNoJson = JsonSerializer.Serialize(labelsWithNo);
            var shortLabelsJson = JsonSerializer.Serialize(shortLabels);
            var scoresJson = JsonSerializer.Serialize(scores);
            var maxScoresJson = JsonSerializer.Serialize(maxScores);

            // Table rows in reverse order (same order as list now)
            var tableRows = string.Join("", list.Select(item => 
                $"<tr><td>{item.Process}</td><td>{item.No}</td><td>{item.DesignAdequacyScore}</td><td>10</td></tr>"
            ));

            var html = $@"
<!doctype html>
<html>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>Assessment Of Adequacy - Charts</title>
  <script src='https://cdn.jsdelivr.net/npm/chart.js'></script>
  <style>
    @page {{ size: A4; margin: 15mm; }}
    body {{ font-family: Arial, Helvetica, sans-serif; margin: 0; padding: 0; }}
    .page {{
      width: 100%;
      min-height: 100vh;
      page-break-after: always;
      display: flex;
      flex-direction: column;
      padding: 10mm;
      box-sizing: border-box;
    }}
    h1, h2 {{ margin: 10px 0; text-align: center; }}
    table {{
      width: 100%;
      border-collapse: collapse;
      margin: 20px 0;
      font-size: 14px;
    }}
    th, td {{
      border: 1px solid #333;
      padding: 10px;
      text-align: left;
    }}
    th {{
      background-color: #f0f0f0;
      font-weight: bold;
    }}
    .chart-wrapper {{ width: 100%; max-width: 1100px; margin: 20px auto; }}
    canvas {{ 
      width: 100% !important; 
      height: 520px !important; 
      background: #fff; 
      border-radius: 8px; 
      box-shadow: 0 2px 10px rgba(0,0,0,0.1); 
    }}
    .small-canvas {{ height: 420px !important; }}
  </style>
</head>
<body>

  <!-- Page 1: Summary Table (Reverse Order) -->
  <div class='page'>
    <h1>Assessment of Adequacy - Summary</h1>
    <table>
      <thead>
        <tr>
          <th>Process</th>
          <th>Process ID</th>
          <th>Process Design Adequacy Score</th>
          <th>Total Process Design Adequacy Score</th>
        </tr>
      </thead>
      <tbody>
        {tableRows}
      </tbody>
    </table>
  </div>

  <!-- Charts start from Page 2 -->
  <div class='page'>
    <h2>Design Adequacy — Horizontal Bar (Actual vs Total)</h2>
    <div class='chart-wrapper'>
      <canvas id='horizontalBar'></canvas>
    </div>
  </div>

  <div class='page'>
    <h2>Design Adequacy — Vertical Bar (Actual vs Total)</h2>
    <div class='chart-wrapper'>
      <canvas id='verticalBar'></canvas>
    </div>
  </div>

  <div class='page'>
    <h2>Design Adequacy — Radar (Actual vs Total)</h2>
    <div class='chart-wrapper'>
      <canvas id='radarChart' class='small-canvas'></canvas>
    </div>
  </div>

  <div class='page'>
    <h2>Design Adequacy — Line Chart (with Total = 10)</h2>
    <div class='chart-wrapper'>
      <canvas id='lineChart'></canvas>
    </div>
  </div>

  <div class='page'>
    <h2>Design Adequacy — Pie Chart</h2>
    <div class='chart-wrapper'>
      <canvas id='pieChart' class='small-canvas'></canvas>
    </div>
  </div>

  <div class='page'>
    <h2>Design Adequacy — Donut Chart</h2>
    <div class='chart-wrapper'>
      <canvas id='donutChart' class='small-canvas'></canvas>
    </div>
  </div>

<script>
  const labelsWithNo = {labelsWithNoJson};
  const labels = {shortLabelsJson};
  const scores = {scoresJson};
  const maxScores = {maxScoresJson};

  const colorBlue = 'rgba(54, 162, 235, 0.85)';
  const colorBlueBorder = 'rgba(54, 162, 235, 1)';
  const colorOrange = 'rgba(255, 159, 64, 0.85)';
  const colorOrangeBorder = 'rgba(255, 159, 64, 1)';

  function generatePalette(n, alpha = 0.85) {{
    const colors = [];
    const borders = [];
    for (let i = 0; i < n; i++) {{
      const hue = Math.round((i * 360) / n);
      colors.push(`hsla(${{hue}}, 70%, 55%, ${{alpha}})`);
      borders.push(`hsla(${{hue}}, 70%, 40%, 1)`);
    }}
    return {{ colors, borders }};
  }}

  const palette = generatePalette(labelsWithNo.length);
  const sliceColors = palette.colors;
  const sliceBorders = palette.borders;

  new Chart(document.getElementById('horizontalBar'), {{
    type: 'bar',
    data: {{ labels: labels, datasets: [
      {{ label: 'Actual Score', data: scores, backgroundColor: colorBlue, borderColor: colorBlueBorder, borderWidth: 1 }},
      {{ label: 'Maximum Score (10)', data: maxScores, backgroundColor: colorOrange, borderColor: colorOrangeBorder, borderWidth: 1 }}
    ]}},
    options: {{ indexAxis: 'y', responsive: true, maintainAspectRatio: false,
      scales: {{ x: {{ suggestedMin: 0, suggestedMax: 10, ticks: {{ stepSize: 2 }} }} }},
      plugins: {{ legend: {{ position: 'top' }} }}
    }}
  }});

  new Chart(document.getElementById('verticalBar'), {{
    type: 'bar',
    data: {{ labels: labels, datasets: [
      {{ label: 'Actual Score', data: scores, backgroundColor: colorBlue, borderColor: colorBlueBorder, borderWidth: 1 }},
      {{ label: 'Maximum Score (10)', data: maxScores, backgroundColor: colorOrange, borderColor: colorOrangeBorder, borderWidth: 1 }}
    ]}},
    options: {{ responsive: true, maintainAspectRatio: false,
      scales: {{ y: {{ suggestedMin: 0, suggestedMax: 10, ticks: {{ stepSize: 2 }} }} }},
      plugins: {{ legend: {{ position: 'top' }} }}
    }}
  }});

  new Chart(document.getElementById('radarChart'), {{
    type: 'radar',
    data: {{ labels: labels, datasets: [
      {{ label: 'Actual Score', data: scores, backgroundColor: 'rgba(54,162,235,0.2)', borderColor: colorBlueBorder, fill: true }},
      {{ label: 'Maximum Score (10)', data: maxScores, backgroundColor: 'rgba(255,159,64,0.1)', borderColor: colorOrangeBorder, borderDash: [6,4], fill: false }}
    ]}},
    options: {{ responsive: true, maintainAspectRatio: false,
      scales: {{ r: {{ suggestedMin: 0, suggestedMax: 10, ticks: {{ stepSize: 2 }} }} }},
      plugins: {{ legend: {{ position: 'top' }} }}
    }}
  }});

  new Chart(document.getElementById('lineChart'), {{
    type: 'line',
    data: {{ labels: labels, datasets: [
      {{ label: 'Actual Score', data: scores, borderColor: colorBlueBorder, backgroundColor: colorBlue, fill: false, tension: 0.2, pointRadius: 4 }},
      {{ label: 'Maximum Score (10)', data: maxScores, borderColor: colorOrangeBorder, borderWidth: 3, borderDash: [8,5], pointRadius: 0, fill: false }}
    ]}},
    options: {{ responsive: true, maintainAspectRatio: false,
      scales: {{ y: {{ suggestedMin: 0, suggestedMax: 10, ticks: {{ stepSize: 2 }} }} }},
      plugins: {{ legend: {{ position: 'top' }} }}
    }}
  }});

  new Chart(document.getElementById('pieChart'), {{
    type: 'pie',
    data: {{ labels: labelsWithNo, datasets: [{{ data: scores, backgroundColor: sliceColors, borderColor: sliceBorders, borderWidth: 2 }}] }},
    options: {{ responsive: true, maintainAspectRatio: false, plugins: {{ legend: {{ position: 'right' }} }} }}
  }});

  new Chart(document.getElementById('donutChart'), {{
    type: 'doughnut',
    data: {{ labels: labelsWithNo, datasets: [{{ data: scores, backgroundColor: sliceColors, borderColor: sliceBorders, borderWidth: 2 }}] }},
    options: {{ responsive: true, maintainAspectRatio: false, cutout: '60%', plugins: {{ legend: {{ position: 'right' }} }} }}
  }});

  window.__chartsRendered = true;
</script>
</body>
</html>";

            // Fix closing script tag
            html = html.Replace("</script>", "</" + "script>");

            try { await _browserFetcher.DownloadAsync(); } catch { }

            var launchOptions = new LaunchOptions
            {
                Headless = true,
                Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--disable-gpu" }
            };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(html);

            try
            {
                await page.WaitForFunctionAsync("() => window.__chartsRendered === true", new WaitForFunctionOptions { Timeout = 10000 });
            }
            catch
            {
                await Task.Delay(1500);
            }

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                PreferCSSPageSize = true,
                MarginOptions = new MarginOptions { Top = "10mm", Bottom = "10mm", Left = "10mm", Right = "10mm" }
            };

            return await page.PdfDataAsync(pdfOptions);
        }
    }
}