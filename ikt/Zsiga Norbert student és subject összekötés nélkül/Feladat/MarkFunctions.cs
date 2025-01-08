namespace Feladat;

public static class MarkFunctions
{
    private static DateTime minDate = DataService.CreateCustomDate((DateTime.Now.Month > 7 ? DateTime.Now.Year : DateTime.Now.Year - 1), "09-01");
    private static DateTime maxDate = DataService.CreateCustomDate((DateTime.Now.Month < 7 ? DateTime.Now.Year : DateTime.Now.Year + 1), "06-15");
    public static async Task<uint> GetMarkIdAsync(ApplicationDbContext dbContext, ulong studentId)
    {
        List<MarkEntity> marks = await dbContext.Marks.Where(x => x.StudentId == studentId).ToListAsync();
        List<string> fullmarks = GetMarksWithEveryProperties(marks);

        Console.Clear();

        int selectedIndex = Menus.ReusableMenu(fullmarks);

        if (selectedIndex == -1)
        {
            return 0;
        }

        return marks[selectedIndex].MarkId;
    }

    public static async Task AddNewMarkAsync(ApplicationDbContext dbContext)
    {
        do
        {
            ulong studentId = await StudentFunctions.GetStudentIdAsync(dbContext);
            if (studentId == 0) 
            { 
                return; 
            }

            uint subjectId = await SubjectFunctions.SelectNewOrExistingSubjectAsync(dbContext);
            if (subjectId == 0) 
            { 
                return; 
            }

            Console.Clear();

            MarkEntity mark = new MarkEntity()
            {
                Date = ExtendentConsole.ReadDateTime($"Kérem a dátumot({minDate} és {maxDate} között): ", minDate, maxDate),
                Mark = (uint)ExtendentConsole.ReadInteger(1, 5, "Kérem a beírandó jegyet: "),
                StudentId = studentId,
                SubjectId = subjectId
            };

            await dbContext.Marks.AddAsync(mark);
            await dbContext.SaveChangesAsync();
        } while (true); 
    }

    public static async Task DeleteMarkAsync(ApplicationDbContext dbContext)
    {
        ulong studentId = await StudentFunctions.GetStudentIdAsync(dbContext);
        if (studentId == 0)
        {
            return;
        }

        uint SelectedMarkId = await GetMarkIdAsync(dbContext, studentId);
        if (SelectedMarkId == 0) 
        { 
            return;
        }

        MarkEntity mark = await dbContext.Marks.FirstAsync(x => x.MarkId == SelectedMarkId);

        dbContext.Marks.Remove(mark);
        await dbContext.SaveChangesAsync();
    }

    public static async Task ModifyMarkAsync(ApplicationDbContext dbContext)
    {
        ulong studentId = await StudentFunctions.GetStudentIdAsync(dbContext);
        if (studentId == 0)
        {
            return;
        }

        uint selectedMarkId = await GetMarkIdAsync(dbContext, studentId);
        if (selectedMarkId == 0) 
        { 
            return; 
        }

        Console.Clear();

        int whatToModify = Menus.ReusableMenu(["jegy", "Dátum"]);
        if (whatToModify == -1) 
        {
            return; 
        }

        List<MarkEntity> marks = await dbContext.Marks.ToListAsync();         

        switch (whatToModify)
        {
            case 0:
                {
                    marks.First(x => x.MarkId == selectedMarkId).Mark = (uint)ExtendentConsole.ReadInteger(1, 5, "Kérem a módosítot jegyet: ");
                    break;
                }
            case 1:
                {
                    marks.First(x => x.MarkId == selectedMarkId).Date = ExtendentConsole.ReadDateTime($"Kérem a módosított dátumot({minDate} és {maxDate} között): ", minDate, maxDate);
                    break;
                }
        }
        await dbContext.SaveChangesAsync();
    }

    public static string PutEveryMarkIntoString(StudentEntity student)
    {
        var groupedMarks = student.Marks.GroupBy(x => x.Subject.Name);

        StringBuilder sb = new StringBuilder();
        foreach (var subject in groupedMarks)
        {
            sb.AppendLine($"\n{subject.Key}: ");
            foreach (var mark in subject)
            {
                sb.AppendLine($"- {mark.Date} : {mark.Mark}");
            }
        }
        return sb.ToString();
    }

    public static List<string> GetMarksWithEveryProperties(List<MarkEntity> marks)
    {
        string temp = "";
        List<string> result = new List<string>();

        foreach (MarkEntity mark in marks)
        {
            temp = $"{mark.Mark}, {mark.Date}, {mark.Subject.Name}";
            result.Add(temp);
        }
        return result;
    }
}
