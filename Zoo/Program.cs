using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo
{
    internal class Program
    {
        static void Main()
        {
            ZooFactory zooFactory = new ZooFactory();
            Zoo zoo = zooFactory.Create();

            zoo.Work();
        }
    }

    class ZooFactory
    {
        private AviaryFactory _aviaryFactory = new AviaryFactory();

        public Zoo Create()
        {
            List<Aviary> aviaries = new List<Aviary>();

            foreach (string name in _aviaryFactory.Names)
            {
                Aviary aviary = _aviaryFactory.Create(name);
                aviaries.Add(aviary);
            }

            return new Zoo(aviaries);
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries = new List<Aviary>();

        public Zoo(List<Aviary> aviaries)
        {
            _aviaries = aviaries;
        }

        public void Work()
        {
            bool isWork = true;

            while (isWork)
            {
                for (int i = 0; i < _aviaries.Count; i++)
                {
                    int aviaryNumber = i + 1;
                    Console.WriteLine($"{aviaryNumber} - подойти к {aviaryNumber} вольеру");
                }

                int exitNumber = _aviaries.Count + 1;
                Console.WriteLine($"{exitNumber} - выход");

                int input = UserUtils.ReadInt("Введите команду: ");
                int index = input - 1;

                if (input == exitNumber)
                {
                    isWork = false;
                }

                if (UserUtils.IsIndexInRange(index, _aviaries.Count))
                {
                    _aviaries[index].WriteInfo();
                }

                Console.Clear();
            }
        }
    }

    class AviaryFactory
    {
        private AnimalFactory _animalFactory = new AnimalFactory();

        public List<string> Names => _animalFactory.Names;

        public Aviary Create(string name) =>
            new Aviary(_animalFactory.Create(name));
    }

    class Aviary
    {
        private List<Animal> _animals;

        public Aviary(List<Animal> animals)
        {
            _animals = animals;
        }

        public void WriteInfo()
        {
            int maleAnimalValue = _animals.Count(animal => animal.Gender == Gender.Male);
            int femaleAnimalValue = _animals.Count - maleAnimalValue;

            Animal tempAnimal = _animals.First();
            string animalName = tempAnimal.Name;
            string animalSound = tempAnimal.Sound;

            Console.Clear();
            Console.WriteLine($"В этом вольере живет {animalName}.\n" +
                $"Количество самцов в вольере - {maleAnimalValue}.\n" +
                $"Количество самок в вольере - {femaleAnimalValue}.\n" +
                $"{animalName} издает звук: \"{animalSound}\".\n" +
                $"Нажмите любую клавишу чтобы вернутся.");

            Console.ReadKey(true);
            Console.Clear();
        }
    }

    class AnimalFactory
    {
        private Dictionary<string, string> _animals;

        public AnimalFactory()
        {
            _animals = FillAnimals();
        }

        public List<string> Names => _animals.Keys.ToList();

        public List<Animal> Create(string name)
        {
            List<Animal> animals = new List<Animal>();
            int animalsQuantity = 10;

            Gender[] genders = (Gender[])Enum.GetValues(typeof(Gender));

            for (int i = 0; i < animalsQuantity; i++)
            {
                int index = UserUtils.GenerateRandomValue(genders.Length);

                Gender gender = genders[index];
                string sound = _animals[name];

                animals.Add(new Animal(name, sound, gender));
            }

            return animals;
        }

        private Dictionary<string, string> FillAnimals() =>
            new Dictionary<string, string>
            {
                ["Тигр"] = "Мяу",
                ["Ежик"] = "Я ежик",
                ["Жираф"] = "Я жираф"
            };
    }

    class Animal
    {
        public Animal(string name, string sound, Gender gender)
        {
            Name = name;
            Sound = sound;
            Gender = gender;
        }

        public string Name { get; }
        public string Sound { get; }
        public Gender Gender { get; }
    }

    enum Gender
    {
        Male,
        Female
    }

    static class UserUtils
    {
        private static Random s_random = new Random();

        public static int ReadInt(string text)
        {
            int number;

            while (int.TryParse(ReadString(text), out number) == false)
                Console.WriteLine("Некорректный ввод. Введите число.");

            return number;
        }

        public static string ReadString(string text)
        {
            Console.Write(text);

            return Console.ReadLine();
        }

        public static int GenerateRandomValue(int max) =>
            s_random.Next(max);

        public static bool IsIndexInRange(int index, int maxIndex) =>
            (index < 0 || index >= maxIndex) == false;
    }
}
