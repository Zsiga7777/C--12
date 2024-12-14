namespace Feladat;

public static class DataService
{
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
}
