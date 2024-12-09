namespace Feladat;

public static class StudentFunctions
{
    public static async Task WriteStudentData(ApplicationDbContext dbContext)
    {
        var studentsData = await dbContext.Students.Include(x => x.Address).ThenInclude(x => x.Street).ThenInclude(x => x.City).ToListAsync();
        uint studentIndex = await GetStudentIdAsync(dbContext);

        if (studentIndex == 0) return;

        StudentEntity student = studentsData.First(x => x.EducationalID == studentIndex);

        Console.Clear();

        Console.WriteLine($"neve: {student.Name}\nanyja neve: {student.MothersName}\nszületési ideje: {student.BirthDay}\nlakhelye:{student.Address.Street.City.Name} {student.Address.Street.Name} {student.Address.Address} ");
        await Task.Delay(10000);
    }

    public static async Task<uint> GetStudentIdAsync(ApplicationDbContext dbContext)
    {
        Console.WriteLine("\nA tanulók nevei: ");

        int temp = Menus.ReusableMenu(await dbContext.Students.Select(x => x.Name).ToListAsync());

        if (temp == -1) return 0;

        return dbContext.Students.ElementAt(temp).EducationalID;
    }

    public static async Task<string> ReadStudentNameAsync(ApplicationDbContext dbContext)
    {
        string input = ExtendentConsole.ReadString("Kérem a tanuló nevét vagy a feladat végeztével a 'e' gomb lenyomását: ");
        if (input.ToLower() == "e") { return input; }

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

    public static async Task AddNewStudentsAsync(ApplicationDbContext dbContext)
    {
        do
        {
            Console.Clear();
            string input = await ReadStudentNameAsync(dbContext);

            if (input.ToLower() == "e")
            {
                return;
            }

            uint address = await AddressFunctions.SelectNewOrExistingAddressCompleteAsync(dbContext);

            if (address == 0) return;
            var student = new StudentEntity()
            {
                Name = input,
                BirthDay = ExtendentConsole.ReadDateTime("Kérem a születési dátumát: "),
                MothersName = ExtendentConsole.ReadString("Kérem az anyja nevét: "),
                AddressId = address
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
        } while (true);
    }

    public static async Task ModifyStudentsDataAsync(ApplicationDbContext dbContext)
    {
        uint studentNeedsModifyId = await GetStudentIdAsync(dbContext);
        if (studentNeedsModifyId == 0)
        {
            return;
        }
        do
        {
            Console.Clear();

            int modificationType = Menus.ReusableMenu(["Név módosítás", "Születésnap módosítása", "Anyja nevének módosítása", "Lakcím módosítás"]);
            if (modificationType == -1) return;

            string newData;
            var student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsModifyId);

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
                        student.BirthDay = ExtendentConsole.ReadDateTime("Kérem az új születésnapot: ");
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

                        if (newAddressId == 0) { return; }

                        student.AddressId = newAddressId;
                        break;
                    }
            }
            await dbContext.SaveChangesAsync();
        } while (true);

    }

    public static async Task DeleteStudentsDataAsync(ApplicationDbContext dbContext)
    {
        do
        {
            Console.Clear();

            uint studentNeedsDeleteID = await GetStudentIdAsync(dbContext);
            if (studentNeedsDeleteID == 0)
            {
                return;
            }

            dbContext.Students.Remove(await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsDeleteID));
            await dbContext.SaveChangesAsync();

        } while (true);
    }
}
