using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Globalization;
using SessionDb.Migrations;

namespace SessionDb;

public static class SessionDatabase
{
    static SessionDatabase()
    {
        _ = typeof(Migrations.InitialCreate);
        _ = typeof(Migrations.AddEegFlag);
    }

    public const string DefaultFileName = "sessions.db";

    public static SessionContext Open(string databasePath)
    {
        if (string.IsNullOrWhiteSpace(databasePath))
            throw new ArgumentException("A database path must be supplied.", nameof(databasePath));

        string fullPath = Path.GetFullPath(databasePath);
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        SessionContextOptions options = BuildOptions(fullPath);
        var context = new SessionContext(options);

        InitializeDatabase(context);
        return context;
    }

    public static SessionContextOptions BuildOptions(string databasePath)
    {
        var builder = new DbContextOptionsBuilder<SessionContext>();
        builder.UseSqlite($"Data Source={databasePath}");
#if DEBUG
        builder.EnableDetailedErrors();
        builder.EnableSensitiveDataLogging();
#endif
        return new SessionContextOptions(builder.Options);
    }

    private static void InitializeDatabase(SessionContext context)
    {
        if (context is null)
            throw new ArgumentNullException(nameof(context));

        PrepareLegacyDatabaseIfNeeded(context);

        context.Database.Migrate();

        SessionSeed.EnsureSeedData(context);
    }

    private static void PrepareLegacyDatabaseIfNeeded(SessionContext context)
    {
        if (!context.Database.IsSqlite())
            return;

        using DbConnection connection = context.Database.GetDbConnection();
        bool shouldClose = connection.State != System.Data.ConnectionState.Open;
        if (shouldClose)
            connection.Open();

        try
        {
            bool sessionsTableExists = TableExists(connection, "Sessions");
            if (!sessionsTableExists)
                return;

            bool historyTableExists = TableExists(connection, "__EFMigrationsHistory");
            if (!historyTableExists)
            {
                EnsureColumn(connection, "Sessions", "EEG", "INTEGER NOT NULL DEFAULT 0");
                EnsureMigrationsHistory(connection);
                EnsureMigrationRecorded(connection, "20240913120000_InitialCreate");
                EnsureMigrationRecorded(connection, "20240915090000_AddEegFlag");
            }
        }
        finally
        {
            if (shouldClose && connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
    }

    private static bool TableExists(DbConnection connection, string tableName)
    {
        using DbCommand command = connection.CreateCommand();
        command.CommandText = "SELECT 1 FROM sqlite_master WHERE type = 'table' AND name = $name LIMIT 1;";
        DbParameter parameter = command.CreateParameter();
        parameter.ParameterName = "$name";
        parameter.Value = tableName;
        command.Parameters.Add(parameter);

        object? result = command.ExecuteScalar();
        return result is not null && result != DBNull.Value;
    }

    private static void EnsureColumn(DbConnection connection, string tableName, string columnName, string columnDefinition)
    {
        if (ColumnExists(connection, tableName, columnName))
            return;

        using DbCommand command = connection.CreateCommand();
        command.CommandText = $"ALTER TABLE \"{tableName}\" ADD COLUMN \"{columnName}\" {columnDefinition};";
        command.ExecuteNonQuery();
    }

    private static bool ColumnExists(DbConnection connection, string tableName, string columnName)
    {
        using DbCommand command = connection.CreateCommand();
        command.CommandText = $"PRAGMA table_info(\"{tableName}\");";

        using DbDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            if (!reader.IsDBNull(1) &&
                string.Equals(reader.GetString(1), columnName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static void EnsureMigrationsHistory(DbConnection connection)
    {
        using DbCommand command = connection.CreateCommand();
        command.CommandText =
            "CREATE TABLE IF NOT EXISTS \"__EFMigrationsHistory\" (" +
            "\"MigrationId\" TEXT NOT NULL CONSTRAINT \"PK___EFMigrationsHistory\" PRIMARY KEY, " +
            "\"ProductVersion\" TEXT NOT NULL);";
        command.ExecuteNonQuery();
    }

    private static void EnsureMigrationRecorded(DbConnection connection, string migrationId)
    {
        using DbCommand selectCommand = connection.CreateCommand();
        selectCommand.CommandText =
            "SELECT 1 FROM \"__EFMigrationsHistory\" WHERE \"MigrationId\" = $id LIMIT 1;";
        DbParameter parameter = selectCommand.CreateParameter();
        parameter.ParameterName = "$id";
        parameter.Value = migrationId;
        selectCommand.Parameters.Add(parameter);

        object? result = selectCommand.ExecuteScalar();
        if (result is not null && result != DBNull.Value)
            return;

        using DbCommand insertCommand = connection.CreateCommand();
        insertCommand.CommandText =
            "INSERT INTO \"__EFMigrationsHistory\" (\"MigrationId\", \"ProductVersion\") VALUES ($id, $version);";

        DbParameter idParameter = insertCommand.CreateParameter();
        idParameter.ParameterName = "$id";
        idParameter.Value = migrationId;
        insertCommand.Parameters.Add(idParameter);

        DbParameter versionParameter = insertCommand.CreateParameter();
        versionParameter.ParameterName = "$version";
        versionParameter.Value = GetProductVersion();
        insertCommand.Parameters.Add(versionParameter);

        insertCommand.ExecuteNonQuery();
    }

    private static string GetProductVersion()
    {
        Version? version = typeof(DbContext).Assembly.GetName().Version;
        if (version is null)
            return "0.0.0";

        if (version.Build >= 0)
            return string.Create(CultureInfo.InvariantCulture, $"{version.Major}.{version.Minor}.{version.Build}");

        return string.Create(CultureInfo.InvariantCulture, $"{version.Major}.{version.Minor}.0");
    }
}
