using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace t2._048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lastInt;
        int c1 = 0;
        int c2 = 0;
        int c3 = 0;
        int c4 = 0;
        string[] nameInLine1 = new string[10];
        string[] nameInLine2 = new string[10];
        string[] nameInLine3 = new string[10];
        string[] nameInLine4 = new string[10];


        public MainWindow()
        {
            InitializeComponent();
            FirstButton.Content = RandomIndex();
            SecondButton.Content = RandomIndex();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (lastInt != 0)
            {
                
            }
            else if (sender is Button button)
            {
                if (button.Name == "FirstButton")
                {
                    lastInt = Convert.ToInt32(button.Content);
                    button.Content = RandomIndex();
                }
                else
                {
                    lastInt = Convert.ToInt32(button.Content);
                    button.Content = FirstButton.Content;
                    FirstButton.Content = RandomIndex();
                }
            }
        }

        private string[] ArrayForLineAdd(string nameForCard, string nameStack)
        {
            if (nameStack == "stack1")
            {
                for (int i =0;  i < 9; i++)
                {
                    if (nameInLine1[i] == null || nameInLine1[i] == "")
                    {
                        nameInLine1[i] = nameForCard;
                        nameInLine1[9] = i.ToString();
                        return nameInLine1;
                    }
                }
                return nameInLine1;
            }
            else if (nameStack == "stack2")
            {
                for (int i = 0; i < 9; i++)
                {
                    if (nameInLine2[i] == null || nameInLine2[i] == "")
                    {
                        nameInLine2[i] = nameForCard;
                        nameInLine2[9] = i.ToString();
                        return nameInLine2;
                    }
                }
                return nameInLine2;
            }
            else if (nameStack == "stack3")
            {
                for (int i = 0; i < 9; i++)
                {
                    if (nameInLine3[i] == null || nameInLine3[i] == "")
                    {
                        nameInLine3[i] = nameForCard;
                        nameInLine3[9] = i.ToString();
                        return nameInLine3;
                    }
                }
                return nameInLine3;
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if (nameInLine4[i] == null || nameInLine4[i] == "")
                    {
                        nameInLine4[i] = nameForCard;
                        nameInLine4[9] = i.ToString();
                        return nameInLine4;
                    }
                }
                return nameInLine4;
            }
        }



        private void stack1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string nameForCard;
            if (lastInt == 0)
            {

            }
            else if (sender is StackPanel stackPanel)
            {
                //Формирование нового имени для Карты в столбце(записываются в массив)
                if (stackPanel.Name == "stack1") { nameForCard = stackPanel.Name + c1; c1++; }
                else if (stackPanel.Name == "stack2") { nameForCard = stackPanel.Name + c2; c2++; }
                else if (stackPanel.Name == "stack3") { nameForCard = stackPanel.Name + c3; c3++; }
                else { nameForCard = stackPanel.Name + c4; c4++; }
                string stackPanelName = stackPanel.Name;
                TextBlock t = new TextBlock
                {
                    Background = GetColorForCard(lastInt),
                    FontSize = 20,
                    Width = 57,
                    Height = 85,
                    Text = lastInt.ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Name = nameForCard
                };
                lastInt = 0;
                foreach (var child in stackPanel.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        //Нужно только для того что бы убрали большие текстбоксы и вместо них уже вставляли новое значение
                        if (element.Name == "h1" || element.Name == "h2" || element.Name == "h3" || element.Name == "h4")
                        {
                            stackPanel.Children.Clear();
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                stackPanel.Children.Add(t);
                Minimized(ArrayForLineAdd(nameForCard, stackPanel.Name), stackPanel);
            }
        }

        static void Minimized(string[] arrayName, StackPanel stack)
        {
            int elementCount = 0;
            foreach (string name in arrayName)
            {
                if (!string.IsNullOrEmpty(name))
                    elementCount++;
                else
                    break;
            }

            for (int i = 0; i < stack.Children.Count; i++)
            {
                if (stack.Children[i] is FrameworkElement element)
                {
                    if (i < elementCount - 1)
                    {
                        SumCards(arrayName, stack);
                        element.Height = 31.25;
                    }
                    else if (i == elementCount - 1)
                    {
                        element.Height = 85;
                    }
                }
            }
        }

        static void SumCards(string[] nameCards, StackPanel stack)
        {
            int elementCount = 0;
            int lastCardIndex = Convert.ToInt32(nameCards[9]);
            for (int i = lastCardIndex; i > 0; i--)
            {
                if (nameCards[i] == nameCards[i - 1])
                {
                    nameCards[i - 1] = (Convert.ToInt32(nameCards[i]) * 2).ToString();
                    nameCards[i] = "";
                    nameCards[9] = (Convert.ToInt32(nameCards[9]) - 1).ToString();
                }
            }
            if (stack.Children.Count > 0)
            {
                if (stack.Children[stack.Children.Count - 2] is TextBlock textBlock)
                {
                    textBlock.Text = nameCards[9];
                }
                stack.Children.RemoveAt(stack.Children.Count - 1);
            }
        }

        //Полностью лаконичное и закоченное пространство, если и дорабатывать то в другой жизни
        private SolidColorBrush GetColorForCard(int lastInt)
        {
            if (lastInt == 2)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0084ab"));
            }
            else if (lastInt == 4)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1c4693"));
            }
            else if (lastInt == 8)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d318d"));
            }
            else
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5e2c8a"));
            }
        }

        //Нечего дорабатывать, максимум - подкрут значений
        static int RandomIndex()
        {
            int[] integers = { 2, 4, 8, 16 };
            Random rnd = new Random();
            int t = rnd.Next(101);
            if (t > 60) 
            {
                return integers[0];
            }
            else if (t > 30)
            {
                return integers[1];
            }
            else if (t > 10)
            {
                return integers[2];
            }
            else
            {
                return integers[3];
            }

        }
    }
}