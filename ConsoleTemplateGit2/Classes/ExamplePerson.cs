using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTemplateGit2
{
    public class Person
    {
        public string FirstName;
        public string LastName;
        public int Age;
        public int Height;
        public int Weight;
        public double BMI;

        public static List<Person> GeneratePeople(int? number = null)
        {
            var bransches = TextfilesIO.ImportBransches("ExamplePeople.csv");

            var peoplelist = new List<Person>();
            int height;
            int weight;
            double bmi;

            int length;
            if (number == null) { length = bransches.Count; }
            else { length = (int)number; }

            for (int i = 0; i < length; i++)
            {
                height = int.Parse(bransches[i][3]);
                weight = int.Parse(bransches[i][4]);
                bmi = weight / (Math.Pow((height / 100), 2));

                var newPerson = new Person
                {
                    FirstName = bransches[i][0],
                    LastName = bransches[i][1],
                    Age = int.Parse(bransches[i][2]),
                    Height = height,
                    Weight = weight,
                    BMI = Math.Round(bmi)
                };
                peoplelist.Add(newPerson);

            }

            return peoplelist;
        }

    }
}
