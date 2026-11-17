using System.Text.Json;

namespace Assignment_Group_Project.Scripts.Objects
{
    public static class DataStorage
    {
        // Load data
        public static T Load<T>(string FilePath)
            where T : new()
        {
            if (!File.Exists(FilePath))
            {
                Save(FilePath, new T());
            }

            string json = File.ReadAllText(FilePath);

            try
            {
                return JsonSerializer.Deserialize<T>(json)
                       ?? new T();
            }
            catch (JsonException)
            {
                throw new InvalidDataException(
                    $"Invalid JSON data in {FilePath}");
            }
        }

        // Save data
        public static void Save<T>(string FilePath, T Data)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json =
                JsonSerializer.Serialize(Data, options);

            File.WriteAllText(FilePath, json);
        }




        //private const string FilePath = "data.json";

        //public AppData Load()
        //{
        //    if (!File.Exists(FilePath))
        //    {
        //        File.WriteAllText(FilePath, JsonSerializer.Serialize(new AppData()));
        //    }

        //    string json = File.ReadAllText(FilePath);

        //    try
        //    {
        //        return JsonSerializer.Deserialize<AppData>(json) ?? new AppData();
        //    }
        //    catch (JsonException)
        //    {
        //        Console.WriteLine("Filen data.json är trasig. Rätta den och starta om.");
        //        Environment.Exit(1);
        //        return new AppData();
        //    }
        //}

        //public void Save(AppData data)
        //{
        //    var options = new JsonSerializerOptions { WriteIndented = true };
        //    File.WriteAllText(FilePath, JsonSerializer.Serialize(data, options));
        //}
    }
}