using ChineseKreta.Database;
using ChineseKreta.Database.Entities;
using Custom.Library.ConsoleExtensions;
using Feladat;
using Microsoft.EntityFrameworkCore;

public static class DataService
{
    #region student
    public static async Task AddNewStudentsAsync(ApplicationDbContext dbContext)
    {
        bool nomore = false;
        do
        {
            Console.Clear();
            string input =await ConsoleFunctions.ReadStudentNameAsync(dbContext);
            if (input.ToLower() == "e") 
            { 
                break; 
            }

            uint address = await Menus.SelectNewOrExistingAddressCompleteAsync(dbContext);
            if(address == 0) return;
            var student = new StudentEntity()
            {
                Name = input,
                BirthDay = ExtendentConsole.ReadDateTime("Kérem a születési dátumát: "),
                MothersName = ExtendentConsole.ReadString("Kérem az anyja nevét: "),
                AddressId = address
            };
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
        }while(!nomore);
    }

    

    public static async Task ModifyStudentsDataAsync(ApplicationDbContext dbContext)
    {
        uint studentNeedsModifyId = await ConsoleFunctions.GetStudentIdAsync(dbContext);
        if (studentNeedsModifyId == 0)
        {
            return;
        }
        do
        {
            Console.Clear();

            int modificationType = Menus.ReusableMenu(["Név módosítás", "Születésnap módosítása","Anyja nevének módosítása", "Lakcím módosítás"]);
            if (modificationType == -1) break;
            string newData;
            var student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsModifyId);
            switch (modificationType)
            {

                case 0:
                    {
                        newData = await ConsoleFunctions.ReadStudentNameAsync(dbContext);
                        if (newData.ToLower() == "e")
                        {
                            break;
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
                        student.AddressId = await ConsoleFunctions.GetAddressIdCompleteAsync(dbContext);
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

            uint studentNeedsDeleteID = await ConsoleFunctions.GetStudentIdAsync(dbContext);
            if (studentNeedsDeleteID == 0)
            {
                return;
            }
            dbContext.Students.Remove(await dbContext.Students.FirstAsync(x => x.EducationalID == studentNeedsDeleteID));
            await dbContext.SaveChangesAsync();

        } while (true);
    }
    #endregion

    #region subject
    public static async Task AddNewSubjectAsync(ApplicationDbContext dbContext)
    {
        string input = null;
        SubjectEntity subject;
            input = ExtendentConsole.ReadString("Kérem a tantárgy nevét vagy a feladat végesztével a 'e' gomb lenyomása: ").ToLower();
            if (input.ToLower() != "e")
            {
                if (!dbContext.Subjects.Any(x => x.Name == input))
                {
                    subject = new SubjectEntity() {Name = input };
                await dbContext.Subjects.AddAsync(subject);
                await dbContext.SaveChangesAsync();
                }
            }
         
    }

    public static async Task DeleteSubjectsAsync(ApplicationDbContext dbContext)
    {
        do
        {
            Console.Clear();

            uint subjectId =await ConsoleFunctions.GetSubjectId(dbContext);
            if (subjectId == 0)
            { 
                return; 
            }

            dbContext.Subjects.Remove(await dbContext.Subjects.FirstAsync(x => x.Id == subjectId));
            await dbContext.SaveChangesAsync();

        } while (true);
    }

    

    public static async Task ModifySubjectNameAsync(ApplicationDbContext dbContext)
    {
        List<SubjectEntity> subjects = await dbContext.Subjects.ToListAsync();
        List<string> subjectNames = subjects.Select(x => x.Name).ToList();
        int selectedSubjectIndex = Menus.ReusableMenu(subjectNames);
        if (selectedSubjectIndex == 0)
        { return ;
        }

        string newName = "";
        do
        {
           newName = ExtendentConsole.ReadString("Kérem a tantárgy módosított nevét: ").ToLower();
        } while (subjectNames.Any(x => x == newName));
        subjects.First(x => x.Name == subjectNames[selectedSubjectIndex -1]).Name = newName;
        await dbContext.SaveChangesAsync();
    }
    #endregion

    #region mark
    public static async Task AddNewMarkAsync(ApplicationDbContext applicationDb)
    {
       uint studentId =await ConsoleFunctions.GetStudentIdAsync(applicationDb);
        if(studentId == 0) { return ; }

        uint subjectId = await Menus.SelectNewOrExistingSubjectAsync(applicationDb);
            if(subjectId == 0) { return ; }
            Console.Clear();

        MarkEntity mark = new MarkEntity() { 
            Date = ExtendentConsole.ReadDateTime("Kérem a dátumot: "),
            Mark = (uint)ExtendentConsole.ReadInteger(1, 5, "Kérem a beírandó jegyet: "),
            StudentId = studentId,
            SubjectId = subjectId
        };
        await applicationDb.Marks.AddAsync(mark);
        await applicationDb.SaveChangesAsync();
    }

    public static async Task DeleteMarkAsync(ApplicationDbContext dbContext)
    {
        uint SelectedMarkId =await ConsoleFunctions.GetMarkIdAsync(dbContext);
        dbContext.Marks.Remove(await dbContext.Marks.FirstAsync(x => x.MarkId == SelectedMarkId));
        await dbContext.SaveChangesAsync();
    }

    public static async Task ModifyMarkAsync(ApplicationDbContext dbContext)
    {
        uint selectedMarkId =await ConsoleFunctions.GetMarkIdAsync(dbContext);
        if(selectedMarkId == 0) return;

        int whatToModify = Menus.ReusableMenu(["jegy", "Dátum"]);
        if( whatToModify == -1) return;

        List<MarkEntity> marks =await dbContext.Marks.ToListAsync();

        switch (whatToModify)
        { 
            case 0:
                {
                    marks.First(x => x.MarkId == selectedMarkId).Mark = (uint)ExtendentConsole.ReadInteger(1, 5, "Kérem a módosítot jegyet: ");
                    break;
                }
            case 1:
                {
                    marks.First(x => x.MarkId == selectedMarkId).Date = ExtendentConsole.ReadDateTime("Kérem a módosított dátumot: ");
                    break;
                }
        }
        await dbContext.SaveChangesAsync();
    }
    #endregion

    #region address
    public static async Task DeleteAddressAsync(ApplicationDbContext dbContext)
    { 
        List<AddressEntity> addresses =await dbContext.Addresses.Include(a => a.Street).ThenInclude(a => a.City).ToListAsync();
        List<string> addressNames = new List<string>();
        string temp = "";
        foreach (AddressEntity address in addresses) 
        {
            temp = $"{address.Street.City.Name} {address.Street.Name} {address.Address}.";
            addressNames.Add(temp);
        }
        int selectedAddressNumber = Menus.ReusableMenu(addressNames);
        dbContext.Remove(addresses[selectedAddressNumber]);
        dbContext.SaveChanges();
    }

    public static async Task ModifyAddressAsync(ApplicationDbContext dbContext)
    {
        List<AddressEntity> addresses = await dbContext.Addresses.Include(a => a.Street).ThenInclude(a => a.City).ToListAsync();
        List<string> addressNames = new List<string>();
        string temp = "";
        foreach (AddressEntity address in addresses)
        {
            temp = $"{address.Street.City.Name} {address.Street.Name} {address.Address}.";
            addressNames.Add(temp);
        }
        int selectedAddressNumber = Menus.ReusableMenu(addressNames);
        addresses[selectedAddressNumber].Address = ExtendentConsole.ReadString("Kérem az új címet: ");
        dbContext.SaveChanges();
    }

    #endregion

}