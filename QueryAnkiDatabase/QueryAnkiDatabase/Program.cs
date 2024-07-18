namespace QueryAnkiDatabase;

internal class Program
{
    // TODO Replace with your Anki database file path to test
    const string AnkiDatabaseFilePath = @"c:\Users\windo\AppData\Roaming\Anki2\Usuario 1\collection.anki2";

    static void Main(string[] args)
    {
        // Try query Anki database using `Microsoft.Data.Sqlite` library -> it will work because we can register the custom collation
        TryReadNoteTypeNames_Microsoft_Data_Sqlite();

        // For reference, try query Anki database using `System.Data.SQLite` library -> it will fail due to missing `unicase` collation
        TryReadNoteTypeNames_System_Data_Sqlite();

    }

    private static void TryReadNoteTypeNames_Microsoft_Data_Sqlite()
    {
        using var connection = new Microsoft.Data.Sqlite.SqliteConnection($"Data Source={AnkiDatabaseFilePath};");
        connection.Open();

        // Anki registers the following collation to compare strings:
        // https://github.com/seanmonstar/unicase
        // You can try to mimic that library's behavior, or just work around the problem using a similar, case-insensitive comparison like:
        connection.CreateCollation("unicase", (x, y) => String.Compare(x, y, StringComparison.OrdinalIgnoreCase));

        var query = $@"SELECT DISTINCT notetypes.name FROM notetypes";
        using var command = new Microsoft.Data.Sqlite.SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var noteTypeName = reader.GetString(0);
            Console.WriteLine($"Note type: {noteTypeName}");
        }
    }

    private static void TryReadNoteTypeNames_System_Data_Sqlite()
    {
        using var connection = new System.Data.SQLite.SQLiteConnection($"Data Source={AnkiDatabaseFilePath};Version=3;");
        connection.Open();

        var query = $@"SELECT DISTINCT notetypes.name FROM notetypes";
        using var command = new System.Data.SQLite.SQLiteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var noteTypeName = reader.GetString(0);
            Console.WriteLine($"Note type: {noteTypeName}");
        }
    }

}
