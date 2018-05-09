using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnetSchoolTask3
{
    class Program
    {
        static void Main(string[] args)
        {
            int april_summ = 0;
            string No_Credit_April = "";

            int Biggest_Debit = 0;
            int Biggest_Credit = 0;
            string Biggest_Debit_Client = "";
            string Biggest_Credit_Client = "";

            string Client_May_First = "";
            int First_May_Debit = 0;
            int First_May_Credit = 0;
            Result result = new Result(); //Объект типа Result для записывания результатов в json
            int Container_For_Balance = 0;
            
            using (FileStream fs = new FileStream(@"bankClients.json", FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        JObject obj = JObject.Load(reader);
                        
                        BankClient bankClient = obj.ToObject<BankClient>();

                        foreach (var operation in bankClient.Operations)
                        {

                            bool No_Credit_April_Test = false;
                             var op_type = operation.OperationType;
                             var date = operation.Date;
                             var amount = operation.Amount;
                             var name = bankClient.FirstName;
                             var last_name = bankClient.LastName;
                             var middle_name = bankClient.MiddleName;
                             var first_deposit = bankClient.Operations[0].Date;
                            if (date.Month == 4) // Сумму дебетовых и кредитовых операций за апрель месяц по всем клиентам
                             {
                                april_summ += amount;
                             }
                            if (date.Month == 4 & op_type == "Credit") //  Информацию по клиентам, которые не снимали деньги в апреле.
                            {
                                No_Credit_April_Test = true;
                                if (No_Credit_April_Test == false)
                                {
                                    No_Credit_April += $"{name} {last_name} {middle_name} Первый вклад {first_deposit} ; {Environment.NewLine}";
                                }
                                
                            }
                            if(Biggest_Debit < amount & op_type =="Debit")  //Информацию о клиенте с самой большой суммой дебетовых операций
                            {
                                Biggest_Debit = amount;
                                Biggest_Debit_Client = $"{bankClient.FirstName} {bankClient.LastName} {bankClient.MiddleName} Первый вклад {first_deposit} ; {Environment.NewLine}";
                            }
                            if (Biggest_Credit < amount & op_type == "Credit") //Информацию о клиенте с самой большой суммой кредитовых операций
                            {
                                Biggest_Credit = amount;
                                Biggest_Credit_Client = $"{bankClient.FirstName} {bankClient.LastName} {bankClient.MiddleName} Первый вклад {first_deposit} ; {Environment.NewLine}";
                            }
                            if (date.Month == 5 & date.Day == 1 /*& date.Hour == 0 & date.Minute == 0*/ ) //Информацию о клиенте с самым крупным остатком на счёте на 1 мая 00:00
                            {
                                if(op_type == "Debit")
                                {
                                    First_May_Debit = amount;
                                }
                                if (op_type == "Credit")
                                {
                                    First_May_Credit = amount;
                                }
                                
                                if(Container_For_Balance < Balance(First_May_Debit, First_May_Credit)) //Сравниваем балансы
                                {
                                    Container_For_Balance = Balance(First_May_Debit, First_May_Credit);
                                    Client_May_First = $"{bankClient.FirstName} {bankClient.LastName} {bankClient.MiddleName} Первый вклад {first_deposit} ; {Environment.NewLine}";
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Сумма операций всех клиентов за апрель месяц: {april_summ}");
                Console.WriteLine($"Информация по клиентам, которые не снимали деньги в апреле: {No_Credit_April}");
                Console.WriteLine($"Информация по клиентам с самой большой суммой дебетовых операций: {Biggest_Debit_Client}");
                Console.WriteLine($"Информация по клиентам с самой большой суммой кредитовых операций: {Biggest_Credit_Client}");
                Console.WriteLine($"Информация о клиентe с самым крупным остатком на счете на 1 мая 00:00 : {Client_May_First}");
                Console.WriteLine("Press ENTER to close the console");
                Console.ReadLine();
            }


            result.First_Task = april_summ;
            result.Second_Task = No_Credit_April;
            result.Third_Task = Biggest_Debit_Client;
            result.Fourth_Task = Biggest_Credit_Client;
            result.Fifth_Task = Client_May_First;
            using (StreamWriter file = File.CreateText(@"result.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, result);
            }

        }
        public static int Balance(int Debit,int Kredit) //Метод для вычисления баланса
        {
            return Debit - Kredit;
        }
    }
}
