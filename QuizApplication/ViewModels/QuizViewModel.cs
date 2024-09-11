using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuizApplication.Models;

public class QuizViewModel : BaseViewModel
{
    private readonly List<Category> _categories;
    private readonly Random _random = new();

    private string _resultMessage;
    public string ResultMessage
    {
        get => _resultMessage;
        set => SetProperty(ref _resultMessage, value);
    }


    public ObservableCollection<Category> Categories { get; set; } = new();
    public ObservableCollection<Question> Questions { get; set; } = new();
    public ICommand SelectCategoryCommand { get; }
    public ICommand CheckAnswerCommand { get; }
    public ICommand StartQuizCommand { get; }


    private int _score;
    public int Score
    {
        get => _score;
        private set => SetProperty(ref _score, value);
    }

    private Category _selectedCategory;
    public Category SelectedCategory
    {
        get => _selectedCategory;
        set
        {
            SetProperty(ref _selectedCategory, value);
            if (value != null)
            {
                LoadQuestions();
            }
        }
    }

    private bool _isQuizRunning;
    public bool IsQuizRunning
    {
        get => _isQuizRunning;
        set => SetProperty(ref _isQuizRunning, value);
    }

    private bool _quizEnded;
    public bool QuizEnded
    {
        get => _quizEnded;
        set => SetProperty(ref _quizEnded, value);
    }

    private Question _currentQuestion;
    public Question CurrentQuestion
    {
        get => _currentQuestion;
        set => SetProperty(ref _currentQuestion, value);
    }

    public ICommand ContinueCommand { get; }
    private bool _isCategorySelectionVisible;
    public bool IsCategorySelectionVisible
    {
        get => _isCategorySelectionVisible;
        set => SetProperty(ref _isCategorySelectionVisible, value);
    }

    public QuizViewModel()
    {
        
        SelectCategoryCommand = new Command<Category>(category =>
        {
            SelectedCategory = category;
            IsCategorySelectionVisible = false; 
            LoadQuestions(); 
        });
        CheckAnswerCommand = new Command<int>(CheckAnswer);
        StartQuizCommand = new Command(LoadQuestions);
        ContinueCommand = new Command(ResetQuiz);


        
        IsCategorySelectionVisible = true;

        
        _categories = LoadCategories();
        foreach (var category in _categories)
        {
            Categories.Add(category);
        }
    }

    private int _totalQuestions;
    private int _correctAnswers;



    private void LoadQuestions()
    {
        Questions.Clear();
        if (SelectedCategory == null) return;

        var selectedQuestions = SelectedCategory.Questions.OrderBy(x => _random.Next()).Take(5).ToList();
        foreach (var question in selectedQuestions)
        {
            Questions.Add(question);
        }

        
        CurrentQuestion = Questions.FirstOrDefault();
        IsCategorySelectionVisible = false;
        IsQuizRunning = true;
        QuizEnded = false;

        
        _totalQuestions = Questions.Count;
        _correctAnswers = 0;
    }

    private void CheckAnswer(int selectedOptionIndex)
    {
        if (CurrentQuestion != null)
        {
            if (CurrentQuestion.CorrectOptionIndex == selectedOptionIndex)
            {
                Score += 1;
                _correctAnswers += 1;
            }

            var nextQuestion = Questions.Skip(1).FirstOrDefault();
            Questions.Remove(CurrentQuestion);
            CurrentQuestion = nextQuestion;

            if (CurrentQuestion == null)
            {
                OnQuizEnded();
            }
        }
    }

    private void OnQuizEnded()
    {
        IsQuizRunning = false;
        QuizEnded = true;

        
        var percentage = (_correctAnswers / (float)_totalQuestions) * 100;
        ResultMessage = $"Your score: {Score}/{_totalQuestions} ({percentage:F2}%)";
    }


    private void ResetQuiz()
    {
        
        Score = 0;
        IsQuizRunning = false;
        QuizEnded = false;

        
        IsCategorySelectionVisible = true;

        
        CurrentQuestion = null;
    }



    private List<Category> LoadCategories()
    {
        
        return new List<Category>
        {
           new Category
        {
            Name = "Geography",
            Questions = new List<Question>
            {
                new Question { Text = "What is the capital city of France?", Options = new List<string> { "Paris", "Rome", "Madrid", "Berlin" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which river is the longest in the world?", Options = new List<string> { "Amazon", "Nile", "Yangtze", "Mississippi" }, CorrectOptionIndex = 1 },
                new Question { Text = "Mount Everest is located in which mountain range?", Options = new List<string> { "Himalayas", "Alps", "Andes", "Rockies" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which country has the most islands?", Options = new List<string> { "Sweden", "Canada", "Norway", "Finland" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the smallest country in the world by land area?", Options = new List<string> { "Monaco", "Vatican City", "San Marino", "Liechtenstein" }, CorrectOptionIndex = 1 },
                new Question { Text = "Which ocean is the largest?", Options = new List<string> { "Atlantic", "Indian", "Pacific", "Arctic" }, CorrectOptionIndex = 2 },
                new Question { Text = "Which country is known as the Land of the Rising Sun?", Options = new List<string> { "China", "Japan", "Thailand", "South Korea" }, CorrectOptionIndex = 1 },
                new Question { Text = "Which desert is the largest in the world?", Options = new List<string> { "Sahara", "Gobi", "Arabian", "Kalahari" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the capital of Australia?", Options = new List<string> { "Sydney", "Melbourne", "Canberra", "Brisbane" }, CorrectOptionIndex = 2 },
                new Question { Text = "Which continent is the least populated?", Options = new List<string> { "Africa", "Europe", "Antarctica", "South America" }, CorrectOptionIndex = 2 }
            }
        },

       
        new Category
        {
            Name = "Biology",
            Questions = new List<Question>
            {
                new Question { Text = "What is the largest organ in the human body?", Options = new List<string> { "Liver", "Heart", "Skin", "Brain" }, CorrectOptionIndex = 2 },
                new Question { Text = "Which animal is known as the King of the Jungle?", Options = new List<string> { "Lion", "Tiger", "Elephant", "Gorilla" }, CorrectOptionIndex = 0 },
                new Question { Text = "What process do plants use to make their food?", Options = new List<string> { "Respiration", "Photosynthesis", "Digestion", "Fermentation" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is the basic unit of life?", Options = new List<string> { "Atom", "Cell", "Molecule", "Organism" }, CorrectOptionIndex = 1 },
                new Question { Text = "Which bird is known for its ability to mimic human speech?", Options = new List<string> { "Parrot", "Sparrow", "Penguin", "Ostrich" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the process by which cells divide to form new cells?", Options = new List<string> { "Mitosis", "Meiosis", "Photosynthesis", "Respiration" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which part of the plant conducts photosynthesis?", Options = new List<string> { "Root", "Stem", "Leaf", "Flower" }, CorrectOptionIndex = 2 },
                new Question { Text = "What type of blood cell carries oxygen?", Options = new List<string> { "Red Blood Cell", "White Blood Cell", "Platelet", "Plasma" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which organ in the human body is primarily responsible for detoxification?", Options = new List<string> { "Kidney", "Liver", "Heart", "Lung" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is the name of the process by which organisms adapt to their environment?", Options = new List<string> { "Evolution", "Development", "Digestion", "Adaptation" }, CorrectOptionIndex = 0 }
            }
        },

        
        new Category
        {
            Name = "History",
            Questions = new List<Question>
            {
                new Question { Text = "Who was the first President of the USA?", Options = new List<string> { "George Washington", "Thomas Jefferson", "John Adams", "James Madison" }, CorrectOptionIndex = 0 },
                new Question { Text = "When did World War II end?", Options = new List<string> { "1944", "1945", "1946", "1947" }, CorrectOptionIndex = 1 },
                new Question { Text = "Which empire was ruled by Julius Caesar?", Options = new List<string> { "Roman Empire", "Ottoman Empire", "British Empire", "Mongol Empire" }, CorrectOptionIndex = 0 },
                new Question { Text = "What year did the Titanic sink?", Options = new List<string> { "1912", "1915", "1918", "1920" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who discovered America?", Options = new List<string> { "Christopher Columbus", "Ferdinand Magellan", "Vasco da Gama", "Hernán Cortés" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which ancient civilization built the pyramids?", Options = new List<string> { "Egyptian", "Aztec", "Inca", "Mayan" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who was the leader of the Soviet Union during World War II?", Options = new List<string> { "Joseph Stalin", "Vladimir Lenin", "Nikita Khrushchev", "Leon Trotsky" }, CorrectOptionIndex = 0 },
                new Question { Text = "What was the primary cause of the American Civil War?", Options = new List<string> { "Slavery", "Economic Differences", "Territorial Expansion", "Political Disputes" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who was the first Emperor of China?", Options = new List<string> { "Qin Shi Huang", "Han Wudi", "Liu Bei", "Gaozu" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which war was fought between the North and South in the United States?", Options = new List<string> { "American Revolution", "War of 1812", "American Civil War", "Spanish-American War" }, CorrectOptionIndex = 2 }
            }
        },

        
        new Category
        {
            Name = "Mathematics",
            Questions = new List<Question>
            {
                new Question { Text = "What is 2 + 2?", Options = new List<string> { "3", "4", "5", "6" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is the square root of 16?", Options = new List<string> { "2", "4", "8", "16" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is 7 * 6?", Options = new List<string> { "42", "36", "48", "54" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the value of π (Pi) approximately?", Options = new List<string> { "3.14", "2.71", "1.61", "4.14" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the next prime number after 7?", Options = new List<string> { "11", "13", "17", "19" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is 50 divided by 5?", Options = new List<string> { "10", "15", "20", "25" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the value of 2^3?", Options = new List<string> { "6", "8", "10", "12" }, CorrectOptionIndex = 1 },
                new Question { Text = "How many sides does a hexagon have?", Options = new List<string> { "5", "6", "7", "8" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is 15% of 200?", Options = new List<string> { "25", "30", "35", "40" }, CorrectOptionIndex = 1 },
                new Question { Text = "What is the value of 9 - 4?", Options = new List<string> { "3", "4", "5", "6" }, CorrectOptionIndex = 2 }
            }
        },

       
        new Category
        {
            Name = "Physics",
            Questions = new List<Question>
            {
                new Question { Text = "What is the speed of light in a vacuum?", Options = new List<string> { "300,000 km/s", "150,000 km/s", "450,000 km/s", "600,000 km/s" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who is known as the father of modern physics?", Options = new List<string> { "Albert Einstein", "Isaac Newton", "Niels Bohr", "Galileo Galilei" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the unit of force in the International System of Units?", Options = new List<string> { "Newton", "Joule", "Watt", "Pascal" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the law of inertia?", Options = new List<string> { "An object in motion stays in motion", "For every action, there is an equal and opposite reaction", "Energy cannot be created or destroyed", "Force equals mass times acceleration" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the symbol for the element gold?", Options = new List<string> { "Au", "Ag", "Fe", "Hg" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who formulated the laws of motion and universal gravitation?", Options = new List<string> { "Isaac Newton", "Albert Einstein", "James Clerk Maxwell", "Michael Faraday" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the process of converting a liquid to a gas called?", Options = new List<string> { "Evaporation", "Condensation", "Sublimation", "Freezing" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the measure of the average kinetic energy of particles in a substance?", Options = new List<string> { "Temperature", "Pressure", "Density", "Volume" }, CorrectOptionIndex = 0 },
                new Question { Text = "What type of wave is sound?", Options = new List<string> { "Mechanical", "Electromagnetic", "Transverse", "Longitudinal" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the formula for calculating work done?", Options = new List<string> { "Force × Distance", "Mass × Acceleration", "Power × Time", "Energy × Time" }, CorrectOptionIndex = 0 }
            }
        },

        
        new Category
        {
            Name = "Chemistry",
            Questions = new List<Question>
            {
                new Question { Text = "What is the chemical symbol for water?", Options = new List<string> { "H2O", "O2", "CO2", "NaCl" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which element is the most abundant in the Earth's crust?", Options = new List<string> { "Oxygen", "Silicon", "Aluminum", "Iron" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the pH of a neutral solution?", Options = new List<string> { "7", "0", "14", "3" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the process of breaking down a compound by heat?", Options = new List<string> { "Decomposition", "Synthesis", "Combustion", "Neutralization" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which gas is most commonly used in welding?", Options = new List<string> { "Argon", "Oxygen", "Nitrogen", "Carbon Dioxide" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the main component of natural gas?", Options = new List<string> { "Methane", "Ethane", "Propane", "Butane" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the formula for table salt?", Options = new List<string> { "NaCl", "KCl", "MgO", "CaCO3" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which element is commonly used in batteries?", Options = new List<string> { "Lithium", "Gold", "Platinum", "Copper" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the process of mixing a solid into a liquid to form a solution?", Options = new List<string> { "Dissolution", "Evaporation", "Condensation", "Filtration" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the most reactive metal in the periodic table?", Options = new List<string> { "Francium", "Cesium", "Potassium", "Sodium" }, CorrectOptionIndex = 0 }
            }
        },

        
        new Category
        {
            Name = "Literature",
            Questions = new List<Question>
            {
                new Question { Text = "Who wrote 'To Kill a Mockingbird'?", Options = new List<string> { "Harper Lee", "Mark Twain", "Ernest Hemingway", "J.D. Salinger" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the title of the first Harry Potter book?", Options = new List<string> { "Harry Potter and the Philosopher's Stone", "Harry Potter and the Chamber of Secrets", "Harry Potter and the Prisoner of Azkaban", "Harry Potter and the Goblet of Fire" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who is the author of 'Pride and Prejudice'?", Options = new List<string> { "Jane Austen", "Charles Dickens", "William Shakespeare", "George Orwell" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which novel starts with the line 'It was the best of times, it was the worst of times'?", Options = new List<string> { "A Tale of Two Cities", "Great Expectations", "Moby-Dick", "The Catcher in the Rye" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who wrote '1984'?", Options = new List<string> { "George Orwell", "Aldous Huxley", "Ray Bradbury", "Philip K. Dick" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the protagonist in 'The Great Gatsby'?", Options = new List<string> { "Jay Gatsby", "Nick Carraway", "Tom Buchanan", "George Wilson" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which Shakespeare play features the character of Macbeth?", Options = new List<string> { "Macbeth", "Hamlet", "Othello", "King Lear" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who wrote 'The Catcher in the Rye'?", Options = new List<string> { "J.D. Salinger", "F. Scott Fitzgerald", "Ernest Hemingway", "John Steinbeck" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the famous detective created by Arthur Conan Doyle?", Options = new List<string> { "Sherlock Holmes", "Hercule Poirot", "Miss Marple", "Philip Marlowe" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which novel is known for the phrase 'All animals are equal, but some animals are more equal than others'?", Options = new List<string> { "Animal Farm", "Brave New World", "1984", "The Road" }, CorrectOptionIndex = 0 }
            }
        },

       
        new Category
        {
            Name = "Music",
            Questions = new List<Question>
            {
                new Question { Text = "Who composed the 'Four Seasons'?", Options = new List<string> { "Antonio Vivaldi", "Johann Sebastian Bach", "Ludwig van Beethoven", "Wolfgang Amadeus Mozart" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the term for a musical composition for a solo instrument?", Options = new List<string> { "Sonata", "Symphony", "Concerto", "Quartet" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which composer is known for his 'Moonlight Sonata'?", Options = new List<string> { "Ludwig van Beethoven", "Johann Strauss", "Franz Liszt", "Pyotr Ilyich Tchaikovsky" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the term for a piece of music written for an orchestra?", Options = new List<string> { "Symphony", "Sonata", "Opera", "Chamber Music" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which band is known for the album 'Abbey Road'?", Options = new List<string> { "The Beatles", "The Rolling Stones", "Led Zeppelin", "Pink Floyd" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the musical symbol for a silence of one beat?", Options = new List<string> { "Rest", "Note", "Pause", "Clef" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which instrument is known for having keys and pedals?", Options = new List<string> { "Piano", "Guitar", "Violin", "Trumpet" }, CorrectOptionIndex = 0 },
                new Question { Text = "What type of music is characterized by improvisation and swing?", Options = new List<string> { "Jazz", "Classical", "Rock", "Country" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who is known as the 'King of Pop'?", Options = new List<string> { "Michael Jackson", "Elvis Presley", "Prince", "Madonna" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the term for a piece of music written for two performers?", Options = new List<string> { "Duo", "Trio", "Quartet", "Quintet" }, CorrectOptionIndex = 0 }
            }
        },

       
        new Category
        {
            Name = "Sport",
            Questions = new List<Question>
            {
                new Question { Text = "Which sport is known as the 'king of sports'?", Options = new List<string> { "Football", "Basketball", "Tennis", "Cricket" }, CorrectOptionIndex = 0 },
                new Question { Text = "How many players are on a standard soccer team?", Options = new List<string> { "11", "7", "9", "5" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which country won the FIFA World Cup in 2018?", Options = new List<string> { "France", "Croatia", "Brazil", "Germany" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who holds the record for the most home runs in Major League Baseball?", Options = new List<string> { "Barry Bonds", "Hank Aaron", "Babe Ruth", "Sammy Sosa" }, CorrectOptionIndex = 0 },
                new Question { Text = "In which sport would you perform a slam dunk?", Options = new List<string> { "Basketball", "Football", "Volleyball", "Soccer" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which athlete is known for the 'Fosbury Flop' high jump technique?", Options = new List<string> { "Dick Fosbury", "Jesse Owens", "Usain Bolt", "Carl Lewis" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the distance of a marathon race?", Options = new List<string> { "42.195 kilometers", "21.097 kilometers", "10 kilometers", "5 kilometers" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which country is known for originating the sport of judo?", Options = new List<string> { "Japan", "China", "Brazil", "South Korea" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who is considered the greatest swimmer of all time, with the most Olympic gold medals?", Options = new List<string> { "Michael Phelps", "Ian Thorpe", "Mark Spitz", "Ryan Lochte" }, CorrectOptionIndex = 0 },
                new Question { Text = "In which sport would you use a shuttlecock?", Options = new List<string> { "Badminton", "Tennis", "Squash", "Table Tennis" }, CorrectOptionIndex = 0 }
            }
        },

        
        new Category
        {
            Name = "Cinematography",
            Questions = new List<Question>
            {
                new Question { Text = "Who directed 'Pulp Fiction'?", Options = new List<string> { "Quentin Tarantino", "Martin Scorsese", "Steven Spielberg", "Francis Ford Coppola" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which film won the Academy Award for Best Picture in 1994?", Options = new List<string> { "Forrest Gump", "Shawshank Redemption", "Pulp Fiction", "The Lion King" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who starred as Jack Dawson in 'Titanic'?", Options = new List<string> { "Leonardo DiCaprio", "Brad Pitt", "Johnny Depp", "Tom Cruise" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the fictional African country in 'Black Panther'?", Options = new List<string> { "Wakanda", "Genosha", "Narnia", "Elbonia" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who won the Academy Award for Best Actress in 2019?", Options = new List<string> { "Renee Zellweger", "Charlize Theron", "Scarlett Johansson", "Saoirse Ronan" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which film franchise features a character named 'James Bond'?", Options = new List<string> { "James Bond", "Mission Impossible", "Die Hard", "Bourne" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which movie is famous for the quote 'I'll be back'?", Options = new List<string> { "The Terminator", "Die Hard", "RoboCop", "Predator" }, CorrectOptionIndex = 0 },
                new Question { Text = "What is the name of the fictional hotel in 'The Shining'?", Options = new List<string> { "Overlook Hotel", "Shining Hotel", "Grand Budapest Hotel", "Hotel California" }, CorrectOptionIndex = 0 },
                new Question { Text = "Who directed the 'Lord of the Rings' trilogy?", Options = new List<string> { "Peter Jackson", "James Cameron", "Ridley Scott", "Sam Raimi" }, CorrectOptionIndex = 0 },
                new Question { Text = "Which film won the Best Animated Feature at the 2020 Oscars?", Options = new List<string> { "Soul", "Toy Story 4", "Frozen II", "Onward" }, CorrectOptionIndex = 0 }
            }
        },

        new Category
        {
            Name = "Mixed",
            Questions = new List<Question>
                {
                    
                    new Question { Text = "In Counter-Strike: Global Offensive, which weapon has the most effective range at long distances?", Options = new List<string> { "AK-47", "AWP", "M4A4", "Desert Eagle" }, CorrectOptionIndex = 1 },
                    new Question { Text = "In which map is the bomb located in Counter-Strike?", Options = new List<string> { "Dust2", "Inferno", "Mirage", "Overpass" }, CorrectOptionIndex = 0 }, 
                    new Question { Text = "Who wrote the novel '1984'?", Options = new List<string> { "Aldous Huxley", "George Orwell", "Ray Bradbury", "J.D. Salinger" }, CorrectOptionIndex = 1 },
                    new Question { Text = "In which book does the character 'Hobbit' appear?", Options = new List<string> { "Hobbit", "Lord of the Rings", "Chronicles of Narnia", "Harry Potter" }, CorrectOptionIndex = 0 },

                    
                    new Question { Text = "Who painted 'The Last Supper'?", Options = new List<string> { "Vincent van Gogh", "Pablo Picasso", "Leonardo da Vinci", "Claude Monet" }, CorrectOptionIndex = 2 },
                    new Question { Text = "Which art style is associated with Salvador Dalí?", Options = new List<string> { "Impressionism", "Surrealism", "Cubism", "Expressionism" }, CorrectOptionIndex = 1 },

                    
                    new Question { Text = "What does the acronym URL stand for?", Options = new List<string> { "Uniform Resource Locator", "Universal Resource Locator", "Uniform Resource Link", "Universal Resource Link" }, CorrectOptionIndex = 0 },
                    new Question { Text = "Which of the following protocols is used for secure data transmission over the internet?", Options = new List<string> { "HTTP", "FTP", "HTTPS", "SMTP" }, CorrectOptionIndex = 2 },

                    
                    new Question { Text = "What is the main programming language used for creating web pages?", Options = new List<string> { "Python", "Java", "HTML", "C#" }, CorrectOptionIndex = 2 },
                    new Question { Text = "What does the acronym CPU stand for?", Options = new List<string> { "Central Processing Unit", "Central Power Unit", "Computer Processing Unit", "Central Programming Unit" }, CorrectOptionIndex = 0 },

                    
                    new Question { Text = "What is the English translation of the word 'book'?", Options = new List<string> { "Book", "Notebook", "Novel", "Manuscript" }, CorrectOptionIndex = 0 },
                    new Question { Text = "What does the English expression 'break the ice' mean?", Options = new List<string> { "To break ice", "To start a conversation to ease tension", "To interrupt something", "To break something" }, CorrectOptionIndex = 1 },

                    
                    new Question { Text = "Complete the proverb: 'No rose without...'", Options = new List<string> { "Thorns", "Flowers", "Buds", "Leaves" }, CorrectOptionIndex = 0 },
                    new Question { Text = "Complete the proverb: 'Where there are six cooks, there is...'", Options = new List<string> { "A better soup", "Nothing", "The best food", "Everyone cooking" }, CorrectOptionIndex = 1 },

                    
                    new Question { Text = "What is the name of the leader of the counter-terrorist team in Counter-Strike series?", Options = new List<string> { "Counter", "Strike", "G-Man", "CT" }, CorrectOptionIndex = 3 },
                    new Question { Text = "What is the main objective of the terrorists in Counter-Strike?", Options = new List<string> { "Protect the VIP", "Plant the bomb", "Eliminate the counter-terrorist team", "Gather information" }, CorrectOptionIndex = 1 },

                    
                    new Question { Text = "In which novel does the character 'Hannibal Lecter' appear?", Options = new List<string> { "Silence of the Lambs", "Dracula", "Black Swan", "Crime and Punishment" }, CorrectOptionIndex = 0 },
                    new Question { Text = "Who is the author of 'Master and Margarita'?", Options = new List<string> { "Fyodor Dostoevsky", "Mikhail Bulgakov", "Anton Chekhov", "Alexander Solzhenitsyn" }, CorrectOptionIndex = 1 },

                    
                    new Question { Text = "Which painting is known as 'Girl with a Pearl Earring'?", Options = new List<string> { "Mona Lisa", "Sierotka Marysia", "Sunflowers", "Portrait of the Girl with a Pearl Earring" }, CorrectOptionIndex = 3 },
                    new Question { Text = "Which painter is known for the series 'Water' and 'Grass'?", Options = new List<string> { "Claude Monet", "Edgar Degas", "Pierre-Auguste Renoir", "Élisabeth Louise Vigée Le Brun" }, CorrectOptionIndex = 0 },
                }
        }
        };
    }
}

