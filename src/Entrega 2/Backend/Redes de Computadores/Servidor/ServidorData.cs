using System.Text.Json;

public static class SensorDataStore
{
    private static readonly List<string> sensorData = new List<string>();
    private static readonly object locker = new object();

    public static void Add(string json)
    {
        lock (locker)
        {
            sensorData.Add(json);
            if (sensorData.Count > 50)
                sensorData.RemoveAt(0); // Mantém os 50 últimos
        }
    }

    public static string GetAllAsHtml()
    {
        lock (locker)
        {
            var html = "<html><head><meta http-equiv='refresh' content='2'>" +
                       "<style>body{font-family:Arial;background:#f5f5f5;padding:20px} .box{background:#fff;padding:10px;border:1px solid #ddd;margin:10px}</style>" +
                       "</head><body><h2>📊 Dados dos Sensores</h2><div>";

            foreach (var item in sensorData)
            {
                try
                {
                    var doc = JsonDocument.Parse(item);
                    var obj = doc.RootElement;
                    html += "<div class='box'>";
                    foreach (var prop in obj.EnumerateObject())
                    {
                        html += $"{prop.Name}: {prop.Value}<br/>";
                    }
                    html += "</div>";
                }
                catch
                {
                    html += $"<div class='box'><pre>{item}</pre></div>"; // fallback se não for JSON válido
                }
            }

            html += "</div></body></html>";
            return html;
        }
    }

}
