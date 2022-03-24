// Регистрация кодировки win 1251 (Потому что в Net. 5 её не существует, нужно регистровать провайдер вручную )
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Encoding.GetEncoding(1252);
string Path = @"D:\TEST-IMAGE\";

// Проходимся по директории 
IEnumerable<string> allfiles = Directory.EnumerateFiles(Path);
//Параллельное выполнение 
Parallel.ForEach(allfiles, async filename =>
{
    // Обрезаем путь и берем только файл
    string file = filename.Split(Path)[1];
    // Обрезаем формат файла
    string fullName = file.Split('.')[0];
    // поиск человека
    int IdPerson = await ServiceFireBird.GetIdByFullName(fullName);

    if (IdPerson != 0)
    {
        // Конвертируем в массив байтов
        byte[] photo = await Task.Run(() => ServiceFireBird.GetBytes(filename));
        // Загружаем на старую базу
        await ServiceFireBird.UpdateImage(photo, IdPerson);
        Console.BackgroundColor = ConsoleColor.Green;
        Console.WriteLine(filename);
        Console.BackgroundColor = ConsoleColor.White;
    }
});
Console.WriteLine("Завершено!");
Console.ReadKey();