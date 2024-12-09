namespace Feladat;

public static class MarkFunctions
{
    public static async Task<uint> GetMarkIdAsync(ApplicationDbContext dbContext)
    {
        List<MarkEntity> marks = await dbContext.Marks.Include(x => x.Student).Include(x => x.Subject).ToListAsync();
        List<string> fullmarks = new List<string>();
        string temp = "";

        foreach (MarkEntity mark in marks)
        {
            temp = $"{mark.Mark}, {mark.Date}, {mark.Subject.Name}, {mark.Student.Name}";
            fullmarks.Add(temp);
        }

        int selectedIndex = Menus.ReusableMenu(fullmarks);

        if (selectedIndex == -1) { return 0; }

        return marks[selectedIndex].MarkId;
    }

    public static async Task AddNewMarkAsync(ApplicationDbContext applicationDb)
    {
        uint studentId = await StudentFunctions.GetStudentIdAsync(applicationDb);
        if (studentId == 0) { return; }

        uint subjectId = await SubjectFunctions.SelectNewOrExistingSubjectAsync(applicationDb);
        if (subjectId == 0) { return; }

        Console.Clear();

        MarkEntity mark = new MarkEntity()
        {
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
        uint SelectedMarkId = await GetMarkIdAsync(dbContext);
        if (SelectedMarkId == 0) { return; }

        dbContext.Marks.Remove(await dbContext.Marks.FirstAsync(x => x.MarkId == SelectedMarkId));
        await dbContext.SaveChangesAsync();
    }

    public static async Task ModifyMarkAsync(ApplicationDbContext dbContext)
    {
        uint selectedMarkId = await GetMarkIdAsync(dbContext);
        if (selectedMarkId == 0) return;

        int whatToModify = Menus.ReusableMenu(["jegy", "Dátum"]);
        if (whatToModify == -1) return;

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
                    marks.First(x => x.MarkId == selectedMarkId).Date = ExtendentConsole.ReadDateTime("Kérem a módosított dátumot: ");
                    break;
                }
        }
        await dbContext.SaveChangesAsync();
    }
}
