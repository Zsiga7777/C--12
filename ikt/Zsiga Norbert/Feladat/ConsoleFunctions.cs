
namespace Feladat
{
    public class ConsoleFunctions
    {
        public static async Task WriteStudentData(List<ChineseKreta.Database.Entities.StudentEntity> studentDatas)
        {
            int studentIndex = GetStudentIndex(studentDatas);
            if (studentIndex == -1) return;
            Console.WriteLine(studentDatas[studentIndex]);
            await Task.Delay(10000);
        }

        public static bool AskIfWantTocontinue()
        {
            bool nomore = false;
            char iterationTemp = ' ';
            Console.WriteLine("Kívánja folytatni? Amennyiben nem, nyomje le az 'e' gombot, ellenkező esetben nyomjon entert: ");
            iterationTemp = Console.ReadKey().KeyChar;
            if (iterationTemp == 'e')
            {
                nomore = true;
            }
            return nomore;
        }

        public static int GetStudentIndex(List<ChineseKreta.Database.Entities.StudentEntity> studentDatas)
        {
            Console.WriteLine("\nA tanulók nevei: ");
            return Menus.ReusableMenu(studentDatas.Select(x => x.Name).ToList()); ;
        }

        public static string GetSubjectName(Dictionary<string, ICollection<int>> subjecFolder)
        {
            Console.WriteLine("\nA tantárgyak: ");
            int indexOfSubject = Menus.ReusableMenu(subjecFolder.Keys.ToList());
            if (indexOfSubject == -1)
            {
                return "";
            }
            string subject = subjecFolder.Keys.ToList()[indexOfSubject];


            return subject;
        }

        public static int GetMarkIndex(List<int> marks)
        {
            int index = Menus.ReusableMenu(marks);
            return index;
        }

        public static void WriteMenu<T>(List<T> options, int selected)
        {
            Console.Clear();
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selected)
                {
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(options[i]);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{options[i]}");
                }
            }
        }

    }
}
