using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoo
{
    internal class Program
    {
        static void Main()
        {
            int animalsQuantity = 5;

            List<Animal> allAnimals = new List<Animal>()
            {
                new Tiger(Gender.Male),
                new Hedgehog(Gender.Male),
                new Giraffe (Gender.Male),
                new Titmouse(Gender.Male),
            };
            Zoo zoo = new Zoo();
            AnimalFabric animalFabric = new AnimalFabric();

            for (int i = 0; i < allAnimals.Count; i++)
            {
                List<Animal> animals = animalFabric.CreateAnimals(animalsQuantity, allAnimals[i]);

                zoo.AddAnimals(animals);
            }

            ActionBuilder actionBuilder = new ActionBuilder(zoo);
            Menu menu = new Menu(actionBuilder.GetActions());

            Console.CursorVisible = false;

            menu.Work();
        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries = new List<Aviary>();

        public void AddAnimals(List<Animal> animals) =>
            animals.ForEach(animal => AddAnimal(animal));

        public List<Action> GetAviariesWriteInfoActions()
        {
            if (_aviaries == null)
                return new List<Action>();

            List<Action> actions = new List<Action>();

            _aviaries.ForEach(aviary => actions.Add(aviary.WriteInfo));

            return actions;
        }

        private void AddAnimal(Animal animal)
        {
            Aviary aviary = _aviaries.FirstOrDefault(requiredAviary => requiredAviary.IsAnimalSuitable(animal));

            if (aviary == null)
            {
                aviary = new Aviary();

                _aviaries.Add(aviary);
            }

            aviary.AddAnimal(animal);
        }
    }

    class Aviary
    {
        private List<Animal> _animals = new List<Animal>();

        private bool IsSomeoneIn => _animals.Count > 0;

        public void AddAnimal(Animal animal) =>
            _animals.Add(animal);

        public bool IsAnimalSuitable(Animal animal) =>
            IsSomeoneIn == false || _animals.First().Name == animal.Name;

        public void WriteInfo()
        {
            if (IsSomeoneIn == false)
            {
                Console.WriteLine("Вольер пуст.");

                return;
            }

            int maleAnimalValue = _animals.Count(animal => animal.Gender == Gender.Male);
            int femaleAnimalValue = _animals.Count(animal => animal.Gender == Gender.Female);

            string animalName = _animals.First().Name;
            string animalSound = _animals.First().Sound;

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
        protected Animal(Gender gender) =>
            Gender = gender;

        public string Name { get; protected set; }
        public Gender Gender { get; protected set; }
        public string Sound { get; protected set; }
    }

    class AnimalFabric
    {
        private Random _random = new Random();

        public List<Animal> CreateAnimals(int quantity, Animal animal)
        {
            List<Animal> animals = new List<Animal>();

            Gender[] genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToArray();

            for (int i = 0; i < quantity; i++)
            {
                int index = _random.Next(genders.Length);

                Gender gender = genders[index];

                animals.Add(Create(animal, gender));
            }

            return animals;
        }

        private Animal Create(Animal animal, Gender gender)
        {
            switch (animal)
            {
                case Tiger _:
                    return new Tiger(gender);

                case Hedgehog _:
                    return new Hedgehog(gender);

                case Giraffe _:
                    return new Giraffe(gender);

                case Titmouse _:
                    return new Titmouse(gender);

                default:
                    return null;
            }
        }
    }

    class Tiger : Animal
    {
        public Tiger(Gender gender) : base(gender)
        {
            Name = "Тигр";
            Sound = "Я тигр. Ррррр...";
        }
    }

    class Hedgehog : Animal
    {
        public Hedgehog(Gender gender) : base(gender)
        {
            Name = "Ежик";
            Sound = "Do you know the way?";
        }
    }

    class Giraffe : Animal
    {
        public Giraffe(Gender gender) : base(gender)
        {
            Name = "Жираф";
            Sound = "Хрум-хрум...";
        }
    }

    class Titmouse : Animal
    {
        public Titmouse(Gender gender) : base(gender)
        {
            Name = "Синица";
            Sound = "Чик-чирик, мазафака.";
        }
    }

    class Menu
    {
        private const ConsoleKey MoveSelectionUp = ConsoleKey.UpArrow;
        private const ConsoleKey MoveSelectionDown = ConsoleKey.DownArrow;
        private const ConsoleKey ConfirmSelection = ConsoleKey.Enter;

        private ConsoleColor _backgroundColor = ConsoleColor.White;
        private ConsoleColor _foregroundColor = ConsoleColor.Black;

        private int _itemIndex = 0;
        private bool _isRunning;
        private string[] _items;

        private Dictionary<string, Action> _actions = new Dictionary<string, Action>();

        public Menu(Dictionary<string, Action> actions)
        {
            _actions = actions;
            _actions.Add("Выход", Exit);
            _items = _actions.Keys.ToArray();
        }

        private int ItemIndex
        {
            get
            {
                return _itemIndex;
            }

            set
            {
                int lastIndex = _items.Length - 1;

                if (value > lastIndex)
                    value = lastIndex;

                if (value < 0)
                    value = 0;

                _itemIndex = value;
            }
        }

        public void Work()
        {
            _isRunning = true;

            while (_isRunning)
            {
                DrawMenu();

                ReadKey();
            }
        }

        private void ReadKey()
        {
            switch (Console.ReadKey(true).Key)
            {
                case MoveSelectionDown:
                    ItemIndex++;
                    break;

                case MoveSelectionUp:
                    ItemIndex--;
                    break;

                case ConfirmSelection:
                    _actions[_items[_itemIndex]].Invoke();
                    break;
            }
        }

        public void DrawMenu()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < _items.Length; i++)
                if (i == ItemIndex)
                    WriteColoredText(_items[i]);
                else
                    Console.WriteLine(_items[i]);
        }

        private void WriteColoredText(string text)
        {
            Console.ForegroundColor = _foregroundColor;
            Console.BackgroundColor = _backgroundColor;

            Console.WriteLine(text);

            Console.ResetColor();
        }

        private void Exit() =>
            _isRunning = false;
    }

    class ActionBuilder
    {
        private Zoo _zoo;

        public ActionBuilder(Zoo zoo) =>
            _zoo = zoo;

        public Dictionary<string, Action> GetActions()
        {
            List<Action> writeInfoActions = _zoo.GetAviariesWriteInfoActions();

            Dictionary<string, Action> actions = new Dictionary<string, Action>();

            for (int i = 0; i < writeInfoActions.Count; i++)
            {
                int aviaryCount = i + 1;

                actions.Add($"Подойти к {aviaryCount} вольеру.", writeInfoActions[i]);
            }

            return actions;
        }
    }

    enum Gender
    {
        Male,
        Female
    }
}
