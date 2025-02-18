using Serilog;

namespace SparkApi.Services
{
    public class ImportTimestampService
    {
        private const string FilePath = "lastImport.txt";

        public void WriteImportTimestamp(DateTime timestamp)
        {
            try
            {
                File.WriteAllText(FilePath, timestamp.ToString("o"));
            }
            catch (Exception ex)
            {
                Log.Error($"Error writing to {FilePath}: {ex.Message}");
            }
        }

        public DateTime? ReadImportTimestamp()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    Log.Error($"Can't find file on {FilePath}");
                }

                var content = File.ReadAllText(FilePath);
                if (DateTime.TryParse(content, out DateTime parsedTimestamp))
                {
                    return parsedTimestamp;
                }
                else
                {
                    Log.Error($"The file {FilePath} does not contain a valid DateTime.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error reading from {FilePath}: {ex.Message}");
                return null;
            }
        }
    }
}
