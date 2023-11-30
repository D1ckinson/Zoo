using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Пользователь запускает приложение и перед ним находится меню,
//в котором он может выбрать, к какому вольеру подойти.
//При приближении к вольеру, пользователю выводится информация о том,
//что это за вольер, сколько животных там обитает, их пол и какой звук издает животное.
//Вольеров в зоопарке может быть много, в решении нужно создать минимум 4 вольера.

namespace Zoo
{
    internal class Program
    {
        static void Main()
        {

        }
    }

    class Zoo
    {
        private List<Aviary> _aviaries;
        private List<string> _animalsName;

        public void AddAnimal()
        {
            //закончил здесь
        }
    }

    class Aviary
    {
        private List<Animal> _animals;

        private string _animalName;
        private string _animalSound;
        private string _maleSex;
        private string _femaleSex;

        public Aviary(string animalName, string animalSound, string maleSex, string femaleSex)
        {
            _animalName = animalName;
            _animalSound = animalSound;
            _maleSex = maleSex;
            _femaleSex = femaleSex;
        }

        public void AddAnimal(Animal animal)
        {
            if (animal.Name != _animalName)
                return;

            _animals.Add(animal);
        }

        public void WriteInfo()
        {
            int maleAnimalValue = _animals.Count(animal => animal.Sex == _maleSex);
            int femaleAnimalValue = _animals.Count(animal => animal.Sex == _femaleSex);

            Console.Clear();

            Console.WriteLine($"В этом вольере живет {_animalName}.\n" +
                $"Количество самцов в вольере - {maleAnimalValue}.\n" +
                $"Количество самок в вольере - {femaleAnimalValue}.\n" +
                $"{_animalName} издает звук {_animalSound}.");

            Console.ReadKey(true);
        }
    }

    class Animal
    {
        public Animal(string name, string sex, string animalSound)
        {
            Name = name;
            Sex = sex;
            AnimalSound = animalSound;
        }

        public string Name { get; private set; }
        public string Sex { get; private set; }
        public string AnimalSound { get; private set; }
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
            string[] actionDescriptions;
            Action[] actionsArray;

            Dictionary<string, Action> actions = new Dictionary<string, Action>
            {

            };

            return actions;
        }
    }
}
