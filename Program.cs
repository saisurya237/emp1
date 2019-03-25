using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EFemp;

namespace EFemp
{
    class Program
    {
        static void Main(string[] args)
        {
            int Flag = 1;
            int choice;
            while (Flag == 1)
            {
                Console.WriteLine("Choose an option from the below: - \n");
                Console.WriteLine("\n1) Insert from CSV into DATABASE \n2) Get all employee Details according to your location");
                Console.WriteLine("3) Get all employee details greater than the date given by user \n4)Age of a person \n5) exit \n");
                choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        string userchoice = Program.Csvinsert();
                        if (userchoice == "n")
                        {
                            Flag = 0;
                        }
                        break;
                    case 2:
                        Console.WriteLine("Enter the location to filter \n");
                        string location = Console.ReadLine();
                        Program.Locationmapping(location);
                        break;
                    case 3:
                        Console.WriteLine("Enter the Date of Joining to filter \n");
                        System.DateTime dateofjoining = Convert.ToDateTime(Console.ReadLine());
                        Program.Datecomparioson(dateofjoining);
                        break;
                    case 4:
                        Console.WriteLine("Enter the EID and Name to calculate the age. \n");
                        int empid = int.Parse(Console.ReadLine());
                        var employeeage = Program.CalculateAge(empid);
                        Console.WriteLine(employeeage + " \n");
                        break;
                    case 5:
                        break;
                }
                if(choice == 5)
                {
                    Flag = 0;
                    break;
                }
            }
        }
        private static int CalculateAge(int empid)
        {
            EmployeeEntities _repo = new EmployeeEntities();
            EMP emp = new EMP();
            int age = 0;
            emp.EID = empid;
            var query = _repo.EMPs.Where(s => s.EID == emp.EID ).Select(s => s.DOB).Single();
            //Console.WriteLine(query.DOB);
            System.DateTime dateOfBirth = Convert.ToDateTime(query);
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        public static void Datecomparioson(System.DateTime dateofjoining)
        {
            EmployeeEntities _repo = new EmployeeEntities();
            EMP emp = new EMP();

            var query = _repo.EMPs.Where(s => s.DOJ > emp.DOJ).ToList();
            foreach (var employeeid in query)
            {
                string formateddateofbirth = String.Format("{0:MM/dd/yyyy}", employeeid.DOB);
                string formateddateofjoining = String.Format("{0:MM/dd/yyyy}", employeeid.DOJ);
                Console.WriteLine($"\nEID : {employeeid.EID} \nNAME : {employeeid.ENAME} \nDOB : {formateddateofbirth} \nLocation : {employeeid.LOCATION} \nDOJ : {formateddateofjoining} \n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------\n");
            }
        }
        public static void Locationmapping(string location)
        {
            EmployeeEntities _repo = new EmployeeEntities();
            EMP emp = new EMP();
     
            var query = _repo.EMPs.Where(s => s.LOCATION == location).ToList();
            foreach(var employeeid in query)
            {
                string formateddateofbirth = String.Format("{0:MM/dd/yyyy}", employeeid.DOB);
                string formateddateofjoining = String.Format("{0:MM/dd/yyyy}", employeeid.DOJ);
                Console.WriteLine($"\nEID : {employeeid.EID} \nNAME : {employeeid.ENAME} \nDOB : {formateddateofbirth} \nLocation : {employeeid.LOCATION} \nDOJ : {formateddateofjoining} \n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------\n");
            }
        }
        public static string Csvinsert()
        {
            List<MyMappedCSVFile> values = File.ReadAllLines(@"C:\Users\Thatodesaisurya\Desktop\neudesiccsharp\Records.csv")
                                           .Skip(1)
                                           .Select(v => MyMappedCSVFile.FromCsv(v))
                                           .ToList();
            foreach (var rowl in values)
            {
                EmployeeEntities _repo = new EmployeeEntities();
                EMP emp = new EMP();
                Console.WriteLine(rowl);
                emp.EID = rowl.EID;
                emp.ENAME = rowl.Name;
                emp.DOB = rowl.DOB;
                emp.LOCATION = rowl.location;
                emp.DOJ = rowl.DOJ;
                try
                {
                    _repo.EMPs.Add(emp);
                    _repo.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString() + "Warning");
                }
            }
            Console.WriteLine("\n Want to enter another option ? y/n");
            string choice2 = Console.ReadLine();
            return choice2;
        }
    

        public class MyMappedCSVFile
        {
            public string Name { get; set; }
            public System.DateTime DOB { get; set; }
            public int EID { get; set; }
            public string location { get; set; }
            public System.DateTime DOJ { get; set; }

            public override string ToString()
            {
                return "Person: " + EID + " " + Name + " " + DOB + " " + location + " " + DOJ;
            }
            public static MyMappedCSVFile FromCsv(string csvLine)
            {
                string[] values = csvLine.Split(',');
                MyMappedCSVFile obj = new MyMappedCSVFile();
                obj.EID = Convert.ToInt32(values[0]);
                obj.Name = Convert.ToString(values[2]);
                obj.DOB = Convert.ToDateTime(values[10]);
                obj.location = Convert.ToString(values[34]);
                obj.DOJ = Convert.ToDateTime(values[14]);
                return obj;
            }
        }
    }
}