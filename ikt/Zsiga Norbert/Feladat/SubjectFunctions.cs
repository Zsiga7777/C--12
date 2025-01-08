using Kreta.Database.Migrations;

namespace Kreta.ConsoleApp;

public static class SubjectFunctions
{
    public static async Task<uint> SelectNewOrExistingSubjectAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        Console.Clear();
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő tantárgy használata a tanulónál", "Adatbázisban létező tantárgy hozzáadása a tanulóhoz", "új tantárgy hozzáadása a tanulóhoz"]);

        if (input == -1) 
        { 
            return 0; 
        }
        uint selectedSubjectId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedSubjectId = await GetSubjectIdForSpecificStudentAsync(dbContext, studentId);
                    break;
                }
            case 1:
                {
                    selectedSubjectId = await AddSubjectToSpecificStudentAsync(dbContext, studentId);
                    break;
                }
            case 2:
                {
                    if (await AddNewSubjectToSpecificStudentAsync(dbContext, studentId))
                    {
                        List<SubjectEntity> subjects = await dbContext.Subjects.OrderBy(x => x.Id).ToListAsync();
                        selectedSubjectId = subjects.Last().Id;
                    }
                    break;
                }
        }
        return selectedSubjectId;
    }

    public static async Task<uint> AddNewOrExistingSubjectToStudentAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        Console.Clear();
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Adatbázisban létező tantárgy hozzáadása a tanulóhoz", "új tantárgy hozzáadása a tanulóhoz"]);

        if (input == -1)
        {
            return 0;
        }
        uint selectedSubjectId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedSubjectId = await AddSubjectToSpecificStudentAsync(dbContext, studentId);
                    break;
                }
            case 1:
                {
                    if (await AddNewSubjectToSpecificStudentAsync(dbContext, studentId))
                    {
                        List<SubjectEntity> subjects = await dbContext.Subjects.OrderBy(x => x.Id).ToListAsync();
                        selectedSubjectId = subjects.Last().Id;
                    }
                    break;
                }
        }
        return selectedSubjectId;
    }

    public static async Task<uint> GetSubjectId(ApplicationDbContext dbContext)
    {
        Console.Clear();
        Console.WriteLine("\nA tantárgyak: ");

        List<SubjectEntity> subjects = await dbContext.Subjects.ToListAsync();
        List<string> subjectNames = await dbContext.Subjects.Select(x => x.Name).ToListAsync();
        int indexOfSubjectName = Menus.ReusableMenu(subjectNames);

        if (indexOfSubjectName == -1)
        {
            return 0;
        }

        return subjects[indexOfSubjectName].Id;
    }

    public static async Task<uint> GetSubjectIdForSpecificStudentAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        Console.Clear();
        Console.WriteLine("\nA tantárgyak: ");

        StudentEntity student = await dbContext.Students.Include(x => x.Subjects).FirstAsync(x => x.EducationalID == studentId);
        List<SubjectEntity> subjects = student.Subjects.ToList();

        List<string> subjectNames = subjects.Select(x => x.Name).ToList();
        int indexOfSubjectName = Menus.ReusableMenu(subjectNames);

        if (indexOfSubjectName == -1)
        {
            return 0;
        }

        return subjects[indexOfSubjectName].Id;
    }
    public static async Task<bool> AddNewSubjectAsync(ApplicationDbContext dbContext)
    {
        List<SubjectEntity> subjects = await dbContext.Subjects.ToListAsync();
        string subjectName =await ReadSubjectNameAsync(subjects);
        if (subjectName == "")
        { 
            return false;
        }

        SubjectEntity subject = new SubjectEntity() { Name = subjectName };

        Console.Clear();

        await dbContext.Subjects.AddAsync(subject);
        await dbContext.SaveChangesAsync();
        return true;        
    }
    public static async Task<uint> AddSubjectToSpecificStudentAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        int input = 0;

        StudentEntity student = await dbContext.Students.Include(x => x.Subjects).FirstAsync(x => x.EducationalID == studentId);
        List<SubjectEntity> databaseSubjects = await dbContext.Subjects.ToListAsync();

        Console.Clear();

        input = Menus.ReusableMenu(databaseSubjects.Select(x => x.Name).ToList());

        if (input == -1)
        {
            return 0;
        }
        if (student.Subjects == null)
        { 
            student.Subjects = new List<SubjectEntity>() { databaseSubjects[input] };
        }
        else
        {
            if (!student.Subjects.Contains(databaseSubjects[input]))
            { 
                student.Subjects.Add(databaseSubjects[input]);
            }
        }
        
        await dbContext.SaveChangesAsync();

        return databaseSubjects[input].Id;
    }

    public static async Task<bool> AddNewSubjectToSpecificStudentAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        bool isNewSubjectAdded = await AddNewSubjectAsync(dbContext);
        if (isNewSubjectAdded)
        {
            StudentEntity student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentId);
            List<SubjectEntity> subjects = await dbContext.Subjects.OrderBy(x => x.Id).ToListAsync();

            if (student.Subjects == null)
            { 
                student.Subjects= new List<SubjectEntity>()
                { 
                    subjects.Last()
                };
            }
            else
            {
                student.Subjects.Add(subjects.Last());
            }

            
            await dbContext.SaveChangesAsync();
        }
        return isNewSubjectAdded;
    }

    public static async Task DeleteSubjectAsync(ApplicationDbContext dbContext)
    {
            Console.Clear();

            uint subjectId = await GetSubjectId(dbContext);
            if (subjectId == 0)
            {
                return;
            }
        SubjectEntity subject = await dbContext.Subjects.FirstAsync(x => x.Id == subjectId);

            dbContext.Subjects.Remove(subject);
            await dbContext.SaveChangesAsync();
    }

    public static async Task DeleteSubjectFromStudentAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        Console.Clear();

        uint subjectId = await GetSubjectIdForSpecificStudentAsync(dbContext, studentId);
        if (subjectId == 0)
        {
            return;
        }
        SubjectEntity subject = await dbContext.Subjects.FirstAsync(x => x.Id == subjectId);

        StudentEntity student = await dbContext.Students.FirstAsync(x => x.EducationalID == studentId);
        await MarkFunctions.DeleteMarksFromOneSubjectAndOneStudentAsync(dbContext, studentId, subjectId);

        student.Subjects.Remove(subject);
        await dbContext.SaveChangesAsync();
    }

    public static async Task ModifySubjectNameAsync(ApplicationDbContext dbContext)
    {
        List<SubjectEntity> subjects = await dbContext.Subjects.ToListAsync();
        List<string> subjectNames = subjects.Select(x => x.Name).ToList();
        int selectedSubjectIndex = Menus.ReusableMenu(subjectNames);

        if (selectedSubjectIndex == -1)
        {
            return;
        }

        string newName = await ReadSubjectNameAsync(subjects);

        subjects[selectedSubjectIndex].Name = newName;
        await dbContext.SaveChangesAsync();
    }

    public static async Task<string> ReadSubjectNameAsync(List<SubjectEntity> subjects)
    {
        string name = "";
        do
        {
            Console.Clear();
            name = ExtendentConsole.ReadString("Kérem a tantárgy nevét: ").ToLower();
            if (subjects.Any(x => x.Name == name))
            {
                Console.WriteLine("Ilyen tantárgy már létezik.");
                await Task.Delay(2000);
            }
            else if (name == "e")
            {
                return "";
            }
        } while (subjects.Any(x => x.Name == name));

        return name;
    }

}
