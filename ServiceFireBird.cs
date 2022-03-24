internal static class ServiceFireBird
{
    public static byte[] GetBytes(string fileName)
    {
        Image imageToConvert = Image.FromFile(fileName);
        using var ms = new MemoryStream();
        imageToConvert.Save(ms, format: imageToConvert.RawFormat);
        return ms.ToArray();
    }

    public static async Task<int> GetIdByFullName(string fullName)
    {
        string firstName = fullName.Split(' ')[0];
        string Name = fullName.Split(' ')[1];
        string lastName = fullName.Split(' ')[2];

        using FbConnection connection = new("database=192.168.250.72:Pers;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;");
        await connection.OpenAsync();
        await using FbCommand command = new($"select id from sotr where famil = '{firstName}' and name = '{Name}' and otch = '{lastName}'", connection);
        FbDataReader reader = await command.ExecuteReaderAsync();
        while (reader.Read())
        {
            if (reader.HasRows) 
                return reader.GetInt32(0);   
        }
        return 0;
    }

    public static async Task UpdateImage(byte[] picbyte, int id)
    {

        using FbConnection connection = new("database=192.168.250.72:Pers;user=sysdba;password=Vtlysq~Bcgjkby2020;Charset=win1251;");
        await connection.OpenAsync();
        using FbCommand command = new("UPDATE SOTR SET PHOTO = @photo where id = @id", connection);

        using var cmd = connection.CreateCommand();
        cmd.CommandText = "UPDATE SOTR SET PHOTO = @photo where id = @id";
        cmd.Parameters.AddWithValue("@photo", picbyte);
        cmd.Parameters.AddWithValue("@id", id);
        _ = await cmd.ExecuteNonQueryAsync();
    }
}

