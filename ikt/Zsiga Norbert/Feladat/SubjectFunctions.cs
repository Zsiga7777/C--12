namespace Feladat;

public static class SubjectFunctions
{
    public static async Task<uint> SelectNewOrExistingSubjectAsync(ApplicationDbContext dbContext)
    {
        Console.WriteLine("Válasston lehetőséget: ");
        int input = Menus.ReusableMenu(["Meglévő tantárgy használata", "Új tantárgy hozzáadása"]);

        if (input == -1) { return 0; }
        uint selectedSubjectId = 0;

        switch (input)
        {
            case 0:
                {
                    selectedSubjectId = await GetSubjectId(dbContext);
                    break;
                }
            case 1:
                {
                    if (await AddNewSubjectAsync(dbContext))
                    {
                        selectedSubjectId = dbContext.Subjects.OrderBy(x => x.Id).Last().Id;
                    }
                    else
                    {
                        selectedSubjectId = 0;
                    }
                    break;
                }
        }
        return selectedSubjectId;
    }

    public static async Task<uint> GetSubjectId(ApplicationDbContext dbContext)
    {
        Console.WriteLine("\nA tantárgyak: ");

        List<SubjectEntity> subjects = await dbContext.Subjects.ToListAsync();
        List<string> subjectNames = await dbContext.Subjects.Select(x => x.Name).ToListAsync();
        int indexOfSubjectName = Menus.ReusableMenu(subjectNames);

        if (indexOfSubjectName == -1)
        {
            return 0;
        }

        return subjects.First(x => x.Name == subjectNames[indexOfSubjectName]).Id;
    }
    public static async Task<bool> AddNewSubjectAsync(ApplicationDbContext dbContext)
    {
        string input = null;
        SubjectEntity subject;

        input = ExtendentConsole.ReadString("Kérem a tantárgy nevét vagy a feladat végesztével a 'e' gomb lenyomása: ").ToLower();
        if (input.ToLower() != "e")
        {
            if (!dbContext.Subjects.Any(x => x.Name == input))
            {
                subject = new SubjectEntity() { Name = input };
                await dbContext.Subjects.AddAsync(subject);
                await dbContext.SaveChangesAsync();
                return true;
            }
        }
        return false;
    }

    public static async Task DeleteSubjectsAsync(ApplicationDbContext dbContext)
    {
        do
        {
            Console.Clear();

            uint subjectId = await GetSubjectId(dbContext);
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
        {
            return;
        }

        string newName = "";
        do
        {
            newName = ExtendentConsole.ReadString("Kérem a tantárgy módosított nevét: ").ToLower();
        } while (subjectNames.Any(x => x == newName));

        subjects.First(x => x.Name == subjectNames[selectedSubjectIndex - 1]).Name = newName;
        await dbContext.SaveChangesAsync();
    }
}
