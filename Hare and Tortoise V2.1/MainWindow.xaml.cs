using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Hare_and_Tortoise_V2._1.MainWindow;

namespace Hare_and_Tortoise_V2._1
{
    public partial class MainWindow : Window
    {
        //Declare Lists for the various inputs from the user
        List<string> nameOfAnimals = new List<string>();
        List<double> minSpeeds = new List<double>();
        List<double> maxSpeeds = new List<double>();
        List<double> endurances = new List<double>();
        //Set constant for endurance recovery
        const int enduranceRecoveryMultiplier = 3;
        public List<string> Items { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            
            //Set the options for the drop down comboBox
            comboBox.ItemsSource = Items;
            Items = new List<string>
            {
                "Wins",
                "Alphabetical"
            };
        }
        public void SubmitAnimal(object sender, RoutedEventArgs e)
        {
            //Take the input from the TextBox/Sliders and add to the lists
            string animalName = animalInput.Text;
            double minSpeed = minSpeedSlider.Value;
            double maxSpeed = maxSpeedSlider.Value;
            double endurance = enduranceSlider.Value;
            //Ensure that the length of the animal name < 20 characters
            if (animalName.Length > 20 || animalName == "")
            {
                MessageBox.Show("Please enter a valid animal name under 20 characters.");
            }
            else
            {
                nameOfAnimals.Add(animalName);
                minSpeeds.Add(minSpeed);
                maxSpeeds.Add(maxSpeed);
                endurances.Add(endurance);
            }

            //Set a limit to the length of the Lists that are held and displayed by the ListBoxes
            //Display the Lists in ListBoxes to the user
            int maxItemsToDisplay = 12;
            List<string> limitedAnimalNames = nameOfAnimals.Take(maxItemsToDisplay).ToList();
            animalList.ItemsSource = limitedAnimalNames;
            List<double> limitedMinSpeeds = minSpeeds.Take(maxItemsToDisplay).ToList();
            minSpeedList.ItemsSource = limitedMinSpeeds;
            List<double> limitedMaxSpeeds = maxSpeeds.Take(maxItemsToDisplay).ToList();
            maxSpeedList.ItemsSource = limitedMaxSpeeds;
            List<double> limitedEndurances = endurances.Take(maxItemsToDisplay).ToList();
            enduranceList.ItemsSource = limitedEndurances;
        }
        private void ClearSelection(object sender, RoutedEventArgs e)
        {
            //Remove all set values inside of the listboxes and empty the lists themselves
            animalList.ItemsSource = null;
            minSpeedList.ItemsSource = null;
            maxSpeedList.ItemsSource = null;
            enduranceList.ItemsSource = null;
            nameOfAnimals.Clear();
            minSpeeds.Clear();
            maxSpeeds.Clear();
            endurances.Clear();
        }
        private void LoadFromFile(object sender, RoutedEventArgs e)
        {
            //Read the Lists from the various json files that
            //they are saved in from the last time the game was ran
            string filePath = "animals.json";
            string json = File.ReadAllText(filePath);
            List<string> nameOfAnimals = JsonSerializer.Deserialize<List<string>>(json);
            filePath = "minSpeeds.json";
            json = File.ReadAllText(filePath);
            List<double> minSpeeds = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "maxSpeeds.json";
            json = File.ReadAllText(filePath);
            List<double> maxSpeeds = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "endurances.json";
            json = File.ReadAllText(filePath);
            List<double> endurances = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "trackLength.json";
            json = File.ReadAllText(filePath);
            int trackLength = JsonSerializer.Deserialize<int>(json);

            //Set a limit to the length of the Lists that are held and displayed by the ListBoxes
            //Display the Lists in ListBoxes to the user
            int maxItemsToDisplay = 12;
            List<string> limitedAnimalNames = nameOfAnimals.Take(maxItemsToDisplay).ToList();
            animalList.ItemsSource = limitedAnimalNames;
            List<double> limitedMinSpeeds = minSpeeds.Take(maxItemsToDisplay).ToList();
            minSpeedList.ItemsSource = limitedMinSpeeds;
            List<double> limitedMaxSpeeds = maxSpeeds.Take(maxItemsToDisplay).ToList();
            maxSpeedList.ItemsSource = limitedMaxSpeeds;
            List<double> limitedEndurances = endurances.Take(maxItemsToDisplay).ToList();
            enduranceList.ItemsSource = limitedEndurances;
            trackLengthSlider.Value = trackLength;
        }
        private void StartGame(object sender, RoutedEventArgs e)
        {
            //Make a new leaderboard file if the game has not been previously ran
            string FileName = "leaderboard.txt";
            if (!File.Exists(FileName))
            {
                File.Create(FileName).Close();
            }

            //Taking inputs for the track length/number of races to occur
            int trackLength = Convert.ToInt32(trackLengthSlider.Value);
            double numRaces = numRacesSlider.Value;
            int racesDone = 0;

            //Write the values from the Lists to json files
            string filePath = "animals.json";
            string json = JsonSerializer.Serialize(animalList.ItemsSource);
            File.WriteAllText(filePath, json);
            filePath = "minSpeeds.json";
            json = JsonSerializer.Serialize(minSpeedList.ItemsSource);
            File.WriteAllText(filePath, json);
            filePath = "maxSpeeds.json";
            json = JsonSerializer.Serialize(maxSpeedList.ItemsSource);
            File.WriteAllText(filePath, json);
            filePath = "endurances.json";
            json = JsonSerializer.Serialize(enduranceList.ItemsSource);
            File.WriteAllText(filePath, json);
            filePath = "trackLength.json";
            json = JsonSerializer.Serialize(trackLength);
            File.WriteAllText(filePath, json);

            //Re-Reading the values back to assign as Lists for the game
            filePath = "animals.json";
            json = File.ReadAllText(filePath);
            List<string> nameOfAnimals = JsonSerializer.Deserialize<List<string>>(json);
            filePath = "minSpeeds.json";
            json = File.ReadAllText(filePath);
            List<double> minSpeeds = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "maxSpeeds.json";
            json = File.ReadAllText(filePath);
            List<double> maxSpeeds = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "endurances.json";
            json = File.ReadAllText(filePath);
            List<double> endurances = JsonSerializer.Deserialize<List<double>>(json);
            filePath = "trackLength.json";
            json = File.ReadAllText(filePath);
            trackLength = JsonSerializer.Deserialize<int>(json);
            List<Wins> wins = ReadDataFromFile("leaderboard.txt");

            //Actual game code
            //The game will repeat whilst the number of races is not satisfied
            //Each round will repeat until a race has been completed
            //Each animal moves based on a random number between their min/max speeds
            //Remove how far they moved from their endurance
            //If they have no endurance remaining add 3x their min speed to endurance
            //and don't move that round
            while (racesDone < numRaces)
            {
                bool winner = false;
                List<double> distCovered = Enumerable.Repeat(0.0, 12).ToList();
                List<bool> resting = Enumerable.Repeat<bool>(false, 12).ToList();
                List<bool> hasCoveredDist = Enumerable.Repeat<bool>(false, 12).ToList();
                while (winner != true)
                {
                    for (int i = 0; i < nameOfAnimals.Count; i++)
                    {
                        if (resting[i] == false)
                        {
                            Random dist = new Random();
                            int min = Convert.ToInt32(minSpeeds[i]);
                            int max = Convert.ToInt32(maxSpeeds[i]);
                            distCovered[i] = (distCovered[i] + dist.Next(min, max));
                            endurances[i] = (endurances[i] - dist.Next(min, max));
                            if (endurances[i] <= 0)
                            {
                                resting[i] = true;
                                endurances[i] = (min * enduranceRecoveryMultiplier);
                            }
                            if (distCovered[i] >= trackLength)
                            {
                                hasCoveredDist[i] = true;
                            }
                        }
                        else
                        {
                            distCovered[i] = distCovered[i];
                            resting[i] = false;
                        }
                    }
                    for (int i = 0; i < nameOfAnimals.Count; i++)
                    {
                        if (hasCoveredDist[i] == true)
                        {
                            winner = true;
                        }
                    }
                }
                //Use LINQ lamda functions to get the index pos of each item
                //that is the largest in distCovered
                //Store these index values, and if there is more than one,
                //#announce a draw and display which animals drew
                double maxValue = distCovered.Max();
                List<int> indicesOfMaxValue = distCovered
                    .Select((value, index) => new { Value = value, Index = index })
                    .Where(item => item.Value == maxValue)
                    .Select(item => item.Index)
                    .ToList();
                string output;
                if (indicesOfMaxValue.Count == 1)
                {
                    int indexOfMaxValue = indicesOfMaxValue[0];
                    output = "The Winner is: " + nameOfAnimals[indexOfMaxValue];
                    bool nameFound = false;
                    if (wins == null)
                    {
                        wins = new List<Wins>();
                    }
                    if (wins.Count != 0)
                    {
                        for (int i = 0; i < wins?.Count; i++)
                        {
                            if (wins[i].NameClass == nameOfAnimals[indexOfMaxValue])
                            {
                                wins[i].WinsClass++;
                                nameFound = true;
                                break;
                            }
                        }
                        if (!nameFound)
                        {
                            wins.Add(new Wins(nameOfAnimals[indexOfMaxValue], 1));
                        }
                    }
                    else
                    {
                        wins.Add(new Wins(nameOfAnimals[indexOfMaxValue], 1));
                    }
                }
                else
                {
                    output = "Draw between: " + string.Join(", ", indicesOfMaxValue.Select(index => nameOfAnimals[index]));
                }
                //Display who won in a message box and write the new winner into the leaderboard file
                MessageBox.Show(output);
                filePath = "leaderboard.txt";
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (Wins win in wins)
                    {
                        writer.WriteLine(win.ToString());
                    }
                }
                racesDone++;
            }
        }
        //Custom data structure used to saving/storing winners and how many wins they have
        public class Wins
        {
            public string NameClass { get; set; }
            public int WinsClass { get; set; }

            public Wins() { }
            public Wins(string nameNew, int winsNew)
            {
                NameClass = nameNew;
                WinsClass = winsNew;
            }
            //Store the winners/num of wins in this format
            public override string ToString()
            {
                return $"{NameClass}: {WinsClass}";
            }
        }

        //Detect when the combobox is changed and update how the leaderboard is sorted and displayed
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<Wins> wins = ReadDataFromFile("leaderboard.txt");
            if (comboBox.SelectedItem != null)
            {
                ComboBoxItem selectedComboBoxItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedComboBoxItem != null)
                {
                    string selectedOption = selectedComboBoxItem.Content.ToString();
                    if (selectedOption == "Wins")
                    {
                        wins = wins?.OrderByDescending(a => a.WinsClass).ToList();
                        leaderboard.ItemsSource = wins;
                    }
                    else if (selectedOption == "Alphabetical")
                    {
                        wins = wins?.OrderBy(a => a.NameClass).ToList();
                        leaderboard.ItemsSource = wins;
                    }
                }
            }
        }
        //Method used to read data from the leaderboard file and split my custom data type
        //into two parts so it can be displayed correctly
        static List<Wins> ReadDataFromFile(string filePath)
        {
            List<Wins> winsList = new List<Wins>();
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines)
                {
                    string[] parts = line.Split(':');
                    if (parts.Length == 2)
                    {
                        string name = parts[0].Trim();
                        if (int.TryParse(parts[1], out int wins))
                        {
                            winsList.Add(new Wins(name, wins));
                        }
                    }
                }
            }
            return winsList;
        }
    }
}