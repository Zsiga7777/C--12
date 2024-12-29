namespace Feladat;

public static class StudentFunctions
{
    private static DateTime minBornDate = DataService.CreateCustomDate(DateTime.Now.Year - 80, DateTime.Now.Month, DateTime.Now.Day);
    private static DateTime maxBornDate = DataService.CreateCustomDate(DateTime.Now.Year - 6, DateTime.Now.Month, DateTime.Now.Day);
    public static async Task WriteStudentData(ApplicationDbContext dbContext)
    {
        var studentsData = await dbContext.Students.Include(x => x.Address)
                                                    .ThenInclude(x => x.Street)
                                                    .ThenInclude(x => x.City)
                                                    .Include(x => x.Marks)
                                                    .ThenInclude(x => x.Subject).ToListAsync();
        ulong studentId = await GetStudentIdAsync(dbContext);

        if (studentId == 0)  
        {
            return;
        }

        StudentEntity student = studentsData.First(x => x.EducationalID == studentId);

        Console.Clear();
        string stringifiedMarks = MarkFunctions.PutEveryMarkIntoString(student);
        Console.WriteLine($"neve: {student.Name}\noktatási azonosítója: {student.EducationalID}\nanyja neve: {student.MothersName}\nszületési ideje: {student.BirthDay}\nlakhelye:{student.Address.Street.City.Name} {student.Address.Street.Name} {student.Address.Address}\nTantárgyai:\n{stringifiedMarks} ");
        
        await Task.Delay(10000);
    }

    public static async Task<ulong> GetStudentIdAsync(ApplicationDbContext dbContext)
    {
        Console.WriteLine("\nA tanulók nevei: ");

        int temp = Menus.ReusableMenu(await dbContext.Students.Select(x => x.Name).ToListAsync());

        if (temp == -1) 
        { 
            return 0; 
        }

        return dbContext.Students.ElementAt(temp).EducationalID;
    }

    public static async Task<string> ReadStudentNameAsync(ApplicationDbContext dbContext)
    {
        string input = ExtendentConsole.ReadString("Kérem a tanuló nevét: ");
        if (input.ToLower() == "e") 
        { 
            return input; 
        }

        int counter = 0;
        while (await dbContext.Students.AnyAsync(x => x.Name == input))
        {
            if (counter == 0)
            {
                counter++;
                input = input + $"{counter}";
            }
            else
            {
                input = input.Remove(counter);
                counter++;
                input = input + $"{counter}";
            }
        }
        return input;
    }

    public static async Task<ulong> ReadStudentIdAsync(ApplicationDbContext dbContext)
    {
        ulong input = 0;

        do
        {         
            input = ExtendentConsole.ReadUlong("Kérem a oktatási azonosítóját(11 számjegy):  ", 9999999999, 100000000000);
        } while (await dbContext.Students.AnyAsync(x => x.EducationalID == input));
            
        return input;
    }

    public static async Task AddNewStudentsAsync(ApplicationDbContext dbContext)
    {
            Console.Clear();
            string input = await ReadStudentNameAsync(dbContext);

            if (input.ToLower() == "e")
            {
                return;
            }

            ulong id = await ReadStudentIdAsync(dbContext);

            uint address = await AddressFunctions.SelectNewOrExistingAddressCompleteAsync(dbContext);

        Console.Clear();
        if (address == 0) 
        { 
            return; 
        }

            var student = new StudentEntity()
            {
                EducationalID = id,
                Name = input,
                BirthDay = ExtendentConsole.ReadDateTime($"Kérem a születési dátumát({minBornDate} és {maxBornDate} között): ", minBornDate, maxBornDate),
                MothersName = ExtendentConsole.ReadString("Kérem az anyja nevét: "),
                AddressId = address
                
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
    }

    public static async Task ModifyStudentsDataAsync(ApplicationDbContext dbContext)
    {
        ulong studentNeedsModifyId = await GetStudentIdAsync(dbContext);
        if (studentNeedsModifyId == 0)
        {
            return;
        }
       
            Console.Clear();

            int modificationType = Menus.ReusableMenu(["Név módosítás", "Születésnap módosítása", "Anyja nevének módosítása", "Lakcím módosítás"]);
        if (modificationType == -1) 
        { 
            return; 
        }

            string newData;
            var student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsModifyId);
        Console.Clear();
        switch (modificationType)
            {
                case 0:
                    {
                        newData = await ReadStudentNameAsync(dbContext);
                        if (newData.ToLower() == "e")
                        {
                            return;
                        }
                        student.Name = newData;
                        break;
                    }
                case 1:
                    {
                        student.BirthDay = ExtendentConsole.ReadDateTime($"Kérem az új születésnapot({minBornDate} és {maxBornDate} között): ", minBornDate, maxBornDate);
                        break;
                    }
                case 2:
                    {
                        student.MothersName = ExtendentConsole.ReadString("Kérem az anyja módosított nevét: ");
                        break;
                    }
                case 3:
                    {
                        uint newAddressId = await AddressFunctions.SelectNewOrExistingAddressCompleteAsync(dbContext);

                        if (newAddressId == 0) 
                        { 
                            return; 
                        }

                        student.AddressId = newAddressId;
                        break;
                    }
            }
            await dbContext.SaveChangesAsync();

    }

    public static async Task DeleteStudentsDataAsync(ApplicationDbContext dbContext)
    {
            Console.Clear();

            ulong studentNeedsDeleteID = await GetStudentIdAsync(dbContext);
            if (studentNeedsDeleteID == 0)
            {
                return;
            }

        StudentEntity student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsDeleteID);

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();
    }
}
