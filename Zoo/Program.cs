using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo
{
    internal class Program
    {
        static void Main()
        {
            Zoo zoo = new Zoo();

            zoo.Work();
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries = new List<Aviary>();
        private List<Animal> _animals;

        public Zoo()
        {
            _animals = FillAnimals();
            _aviaries = FillAviaries();
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

                int input = ReadInt();
                int index = input - 1;

                if (input == exitNumber)
                {
                    isWork = false;
                }

                if (IsIndexInRange(index))
                {
                    _aviaries[index].WriteInfo();
                }

                Console.Clear();
            }
        }

        private int ReadInt()
        {
            int number;

            while (int.TryParse(ReadString(), out number) == false)
                Console.WriteLine("Некорректный ввод. Введите число.");

            return number;
        }

        private bool IsIndexInRange(int index)
        {
            if (index < 0 || index >= _aviaries.Count)
            {
                return false;
            }

            return true;
        }

        private List<Aviary> FillAviaries()
        {
            Random random = new Random();
            List<Aviary> aviaries = new List<Aviary>();

            int minAnimalQuantity = 2;
            int maxAnimalQuantity = 5;

            foreach (Animal animal in _animals)
            {
                Aviary aviary = FindAviary(animal);
                int animalQuantity = random.Next(minAnimalQuantity, maxAnimalQuantity + 1);

                for (int i = 0; i < animalQuantity; i++)
                {
                    aviary.AddAnimal(animal.Clone());
                }

                aviaries.Add(aviary);
            }

            return aviaries;
        }

        private Aviary FindAviary(Animal animal)
        {
            //Aviary aviary = _aviaries.FirstOrDefault(matchedAviary => matchedAviary.AnimalName == animal.Name);

            foreach (Aviary aviary1 in _aviaries)
            {
                if (aviary1.Name == animal.Name)
                {
                    return aviary1;
                }
            }

            return new Aviary(animal.Name);

            //if (aviary == null)
            //{
            //    return new Aviary();
            //}

            //return aviary;
        }

        private List<Animal> FillAnimals()
        {
            List<Animal> animals = new List<Animal>();

            foreach (Gender gender in Enum.GetValues(typeof(Gender)))
            {
                animals.Add(new Tiger("Тигр", "Я тигр. Ррррр...", gender));
                animals.Add(new Hedgehog("Ежик", "Do you know the way?", gender));
                animals.Add(new Giraffe("Жираф", "Хрум-хрум...", gender));
                animals.Add(new Titmouse("Синица", "Чик-чирик, мазафака.", gender));
            }

            return animals;
        }

        private string ReadString()
        {
            Console.Write("Введите команду: ");

            return Console.ReadLine();
        }
    }

    class Aviary
    {
        private List<Animal> _animals = new List<Animal>();

        public Aviary(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string AnimalName => _animals.First().Name;

        public void AddAnimal(Animal animal) =>
            _animals.Add(animal);

        public void WriteInfo()
        {
            int maleAnimalValue = _animals.Count(animal => animal.Gender == Gender.Male);
            int femaleAnimalValue = _animals.Count - maleAnimalValue;
            Console.WriteLine($"{_animals.Count}  {maleAnimalValue}  {femaleAnimalValue}\n\n\n");
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

    abstract class Animal
    {
        protected Animal(string name, string sound, Gender gender)
        {
            Name = name;
            Sound = sound;
            Gender = gender;
        }

        public string Name { get; }
        public string Sound { get; }
        public Gender Gender { get; }

        public abstract Animal Clone();
    }

    class Tiger : Animal
    {
        public Tiger(string name, string sound, Gender gender) : base(name, sound, gender) { }

        public override Animal Clone() =>
            new Tiger(Name, Sound, Gender);
    }

    class Hedgehog : Animal
    {
        public Hedgehog(string name, string sound, Gender gender) : base(name, sound, gender) { }

        public override Animal Clone() =>
            new Hedgehog(Name, Sound, Gender);
    }

    class Giraffe : Animal
    {
        public Giraffe(string name, string sound, Gender gender) : base(name, sound, gender) { }

        public override Animal Clone() =>
            new Giraffe(Name, Sound, Gender);
    }

    class Titmouse : Animal
    {
        public Titmouse(string name, string sound, Gender gender) : base(name, sound, gender) { }

        public override Animal Clone() =>
            new Titmouse(Name, Sound, Gender);
    }

    enum Gender
    {
        Male,
        Female
    }
}
