using System.Text.RegularExpressions;
using System;

namespace Core
{
    public class PersonalInfo
    {
        public PersonalInfo(string id, string password)
        {
            try
            {
                ID = id;
                Password = password;
                Money = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(@"PersonalInfo Constructor Exception: {0}", e);
            }
        }

        private string id;
        public string ID 
        {
            get => id;
            set 
            {
                if (Regex.IsMatch(value, @"[0-9]{16}", RegexOptions.IgnoreCase))
                {
                    id = value;
                }
                else
                {
                    throw new InvalidOperationException("ID Format Exception");
                }
            } 
        }

        private string password;
        public string Password
        {
            get => password;
            set
            {

                if (Regex.IsMatch(value, @"[0-9]{4}", RegexOptions.IgnoreCase))
                {
                    password = value;
                }
                else
                {
                    throw new InvalidOperationException("Password Format Exception");
                }
            }
        }

        private int money;
        public int Money
        {
            get => money;
            set
            {
                if (value >= 0)
                {
                    money = value;
                }
                else
                {
                    throw new InvalidOperationException("Money Value Exception");
                }
            }
        }

        public bool GiveMoney(int money)
        {
            Money += money;
            return true;
        }

        public bool TakeMoney(int money)
        {
            try
            {
                Money -= money;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
