using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Core;
using Helpers;

namespace Bank
{
    public class MainModel
    {
        public MainModel()
        {
            InitPersonalInfos();
            InitCash();
        }

        #region Constants
        private const int MaxCountOfCash = 10000;
        #endregion

        #region Fields
        private List<PersonalInfo> personalInfoList;
        private PersonalInfo selectedInfo;

        private List<Cash> cashListGlobal;
        #endregion

        #region Properties
        private int _countOfCash;
        private int _cash;
        #endregion

        #region Methods
        private void InitPersonalInfos()
        {
            personalInfoList = new List<PersonalInfo>
            {
                new PersonalInfo("0000000000000000", "0000"),
                new PersonalInfo("1111111111111111", "1111"),
                new PersonalInfo("2222222222222222", "2222"),
                new PersonalInfo("3333333333333333", "3333"),
                new PersonalInfo("4444444444444444", "4444"),
                new PersonalInfo("5555555555555555", "5555"),
                new PersonalInfo("6666666666666666", "6666"),
                new PersonalInfo("7777777777777777", "7777"),
                new PersonalInfo("8888888888888888", "8888"),
                new PersonalInfo("9999999999999999", "9999"),
            };
        }

        private void InitCash()
        {
            _countOfCash = 0;
            _cash = 0;

            cashListGlobal = new List<Cash>();

            for (int i = 0; i < 1000; i++)
            {
                AddCash(Cash.Turquoise);
                AddCash(Cash.Orange);
                AddCash(Cash.Green);
                AddCash(Cash.Violet);
                AddCash(Cash.Cyan);
                AddCash(Cash.Blue);
                AddCash(Cash.Red);
            }
        }

        private void AddCash(Cash cash)
        {
            cashListGlobal.Add(cash);

            _countOfCash++;
            _cash += (int)cash;
        }

        private bool CheckInfo(string id, string password, out string outInfo)
        {
            outInfo = string.Empty;

            if (!Regex.IsMatch(id, @"^[0-9]{16}\z", RegexOptions.IgnoreCase))
            {
                outInfo = StringHelper.IncorrectIdPatternTitle;
                return false;
            }

            if (!Regex.IsMatch(password, @"^[0-9]{4}\z", RegexOptions.IgnoreCase))
            {
                outInfo = StringHelper.IncorrectPasswordPatternTitle;
                return false;
            }

            foreach (var personalInfo in personalInfoList)
            {
                if (personalInfo.ID == id && personalInfo.Password == password)
                {
                    selectedInfo = personalInfo;
                    return true;
                }
            }

            outInfo = StringHelper.IncorrectPasswordTitle;
            return false;
        }

        public string Balance(string id, string password)
        {
            if (!CheckInfo(id, password, out string outInfo))
            {
                return outInfo;
            }

            return string.Format($"Баланс: {selectedInfo.Money}");
        }

        public string AddInfo(string id, string password)
        {
            if (!Regex.IsMatch(id, @"^[0-9]{16}\z", RegexOptions.IgnoreCase))
            {
                return string.Format($"{StringHelper.NoSuccessTitle}, {StringHelper.IncorrectIdPatternTitle}"); ;
            }

            if (!Regex.IsMatch(password, @"^[0-9]{4}\z", RegexOptions.IgnoreCase))
            {
                return string.Format($"{StringHelper.NoSuccessTitle}, {StringHelper.IncorrectPasswordPatternTitle}"); ;
            }

            foreach (var personalInfo in personalInfoList)
            {
                if (personalInfo.ID == id)
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, карта уже зарегистрирована");
                }
            }

            personalInfoList.Add(new PersonalInfo(id, password));
            return string.Format($"{StringHelper.SuccessTitle}");
        }

        private bool CanTakeMoney(int money, out List<int> cashIndexList)
        {
            cashIndexList = new List<int>();

            Array values = Enum.GetValues(typeof(Cash));
            Array.Reverse(values);

            foreach (int value in values)
            {
                for (int i = 0; i < cashListGlobal.Count; i++)
                {
                    if ((int)cashListGlobal[i] == value && money >= value)
                    {
                        money -= value;
                        cashIndexList.Add(i);

                        if (money / value == 0)
                        {
                            break;
                        }
                    }
                }
            }

            cashIndexList.Sort();
            return money == 0;
        }

        private bool RemoveCash(List<int> cashIndexList)
        {
            try
            {
                int indexOffset = 0;

                foreach (int cashIndex in cashIndexList)
                {
                    _countOfCash--;
                    _cash -= (int)cashListGlobal[cashIndex - indexOffset];
                    cashListGlobal.RemoveAt(cashIndex - indexOffset);
                    indexOffset++;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("RemoveCash Exception: {0}", e);
                return false;
            }
        }

        public string TakeMoney(string id, string password, string input)
        {
            if (!CheckInfo(id, password, out string outInfo))
            {
                return outInfo;
            }

            if (int.TryParse(input, out int money))
            {
                if (_cash < money)
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, в банкомате недостаточно денег: {_cash}");
                }

                if (money % 50 != 0)
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, сумма должна быть кратна 50");
                }

                if (!CanTakeMoney(money, out List<int> cashList))
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, в банкомате недостаточно купюр для набора необходимой суммы");
                }

                if (selectedInfo.TakeMoney(money))
                {
                    if (!RemoveCash(cashList))
                    {
                        selectedInfo.GiveMoney(money);
                        return string.Format($"{StringHelper.NoSuccessTitle}, RemoveCash Exception");
                    }

                    return string.Format($"{StringHelper.SuccessTitle}, баланс: {selectedInfo.Money}");
                }
                else
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, баланс: {selectedInfo.Money}");
                }
            }
            else
            {
                return StringHelper.IncorrectNumberTitle;
            }
        }

        private bool StringToListOfCash(string input, out List<Cash> cashList, out int sum)
        {
            cashList = new List<Cash>();
            sum = 0;
            string[] words = input.Split(new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
           
            foreach (string word in words)
            {
                if (int.TryParse(word, out int cash))
                {
                    if (Enum.IsDefined(typeof(Cash), cash))
                    {
                        cashList.Add((Cash)cash);
                        sum += cash;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public string GiveMoney(string id, string password, string input)
        {
            if (!CheckInfo(id, password, out string outInfo))
            {
                return outInfo;
            }

            if (StringToListOfCash(input, out List<Cash> cashList, out int sum))
            {
                if (_countOfCash + cashList.Count > MaxCountOfCash)
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, в банкомате слишком много купюр: {_countOfCash}, максимум: {MaxCountOfCash}");
                }

                if (selectedInfo.GiveMoney(sum))
                {
                    foreach (Cash cash in cashList)
                    {
                        AddCash(cash);
                    }

                    return string.Format($"{StringHelper.SuccessTitle}, баланс: {selectedInfo.Money}");
                }
                else
                {
                    return string.Format($"{StringHelper.NoSuccessTitle}, баланс: {selectedInfo.Money}");
                }
            }
            else
            {
                return StringHelper.IncorrectCashTitle;
            }
        }
        #endregion
    }
}
