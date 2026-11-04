using System.Text.Json;

namespace Assignment_Group_Project.Scripts.Objects
{
    public class DataStorage
    {
        private const string FilePath = "data.json";

        public AppData Load()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, JsonSerializer.Serialize(new AppData()));
            }

            string json = File.ReadAllText(FilePath);

            try
            {
                return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
            }
            catch (JsonException)
            {
                Console.WriteLine("Filen data.json är trasig. Rätta den och starta om.");
                Environment.Exit(1);
                return new AppData();
            }
        }

        public void Save(AppData data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(FilePath, JsonSerializer.Serialize(data, options));
        }
    }
}