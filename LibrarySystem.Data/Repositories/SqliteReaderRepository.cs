using LibrarySystem.Core.Models;
using LibrarySystem.Data.Repositories;
using Microsoft.Data.Sqlite;

namespace LibrarySystem.Data.Repositories;

/// <summary>
/// SQLite-реализация репозитория читателей.
/// </summary>
public sealed class SqliteReaderRepository : IReaderRepository
{
    private readonly SqliteConnectionFactory _factory;

    public SqliteReaderRepository(SqliteConnectionFactory factory)
    {
        _factory = factory;
    }

    public List<Reader> GetAll()
    {
        var readers = new List<Reader>();
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, LastName, FirstName, MiddleName, Phone, Address
            FROM Readers
            ORDER BY LastName, FirstName";
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            readers.Add(new Reader
            {
                Id = reader.GetInt32(0),
                LastName = reader.GetString(1),
                FirstName = reader.GetString(2),
                MiddleName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Address = reader.IsDBNull(5) ? "" : reader.GetString(5)
            });
        }
        return readers;
    }

    public Reader? GetById(int id)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, LastName, FirstName, MiddleName, Phone, Address
            FROM Readers
            WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Reader
            {
                Id = reader.GetInt32(0),
                LastName = reader.GetString(1),
                FirstName = reader.GetString(2),
                MiddleName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Address = reader.IsDBNull(5) ? "" : reader.GetString(5)
            };
        }
        return null;
    }

    public List<Reader> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAll();

        var readers = new List<Reader>();
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            SELECT Id, LastName, FirstName, MiddleName, Phone, Address
            FROM Readers
            WHERE LastName LIKE $q OR FirstName LIKE $q OR MiddleName LIKE $q OR Id::TEXT LIKE $q
            ORDER BY LastName, FirstName";
        cmd.Parameters.AddWithValue("$q", $"%{query}%");
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            readers.Add(new Reader
            {
                Id = reader.GetInt32(0),
                LastName = reader.GetString(1),
                FirstName = reader.GetString(2),
                MiddleName = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                Address = reader.IsDBNull(5) ? "" : reader.GetString(5)
            });
        }
        return readers;
    }

    public void Add(Reader reader)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO Readers (LastName, FirstName, MiddleName, Phone, Address)
            VALUES ($ln, $fn, $mn, $phone, $addr);
            SELECT last_insert_rowid();";
        cmd.Parameters.AddWithValue("$ln", reader.LastName);
        cmd.Parameters.AddWithValue("$fn", reader.FirstName);
        cmd.Parameters.AddWithValue("$mn", reader.MiddleName);
        cmd.Parameters.AddWithValue("$phone", reader.Phone);
        cmd.Parameters.AddWithValue("$addr", reader.Address);
        reader.Id = Convert.ToInt32(cmd.ExecuteScalar());
    }

    public void Delete(int id)
    {
        using var conn = _factory.CreateConnection();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Readers WHERE Id = $id";
        cmd.Parameters.AddWithValue("$id", id);
        cmd.ExecuteNonQuery();
    }
}