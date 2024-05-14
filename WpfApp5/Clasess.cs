using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp5
{
    public static class ClassChangePage
    {
        public static Frame frame1 = new Frame();
    }
    public class GlobalVar
    {
        public static string PanelLogin;
        public static bool ThemeNegr = false;
        public static bool FlagToAction = true;
        public static List<DateTime> selectionChangedInitTimes = new List<DateTime>();
        public static List<string> selectionChangedInitData = new List<string>();
        public static List<string> selectionChangedInitFirstName = new List<string>();
        public static List<string> selectionChangedInitSurrName = new List<string>();
        public static List<string> selectionChangedInitLastName = new List<string>();
        public static List<string> selectionChangedInitRootPass = new List<string>();
        public static bool StatusAuth;
        public static bool AdminReg;
        public static bool UserReg;
        public static bool EditorReg;
    }
    public class GlobalMethods
    {
        public static string GetNameAdmin(string name)
        {
            SportEntities dataBase = new SportEntities();
            // Получить объект пользователя из БД по login:
            Admins user = dataBase.Admins.FirstOrDefault(u => u.alogin == name);

            // Если у пользователя есть имя, то вернуть его:
            if (user != null && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.SecondName))
            {
                return $"{user.FirstName} {user.SecondName}";
            }
            else
            {
                return "Администратор";
            }
        }

        public static string GetNameEditor(string name)
        {
            SportEntities dataBase = new SportEntities();
            // Получить объект пользователя из БД по login:
            Editors user = dataBase.Editors.FirstOrDefault(u => u.alogin == name);

            // Если у пользователя есть имя, то вернуть его:
            if (user != null && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.SecondName))
            {
                return $"{user.FirstName} {user.SecondName}";
            }
            else
            {
                return "Редактор";
            }
        }

        public static string GetNameUser(string name)
        {
            SportEntities dataBase = new SportEntities();
            // Получить объект пользователя из БД по login:
            Users user = dataBase.Users.FirstOrDefault(u => u.alogin == name);

            // Если у пользователя есть имя, то вернуть его:
            if (user != null && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.SecondName))
            {
                return $"{user.FirstName} {user.SecondName}";
            }
            else
            {
                return "Пользователь";
            }
        }
    }
}
