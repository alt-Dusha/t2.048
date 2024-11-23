using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace t2._048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lastInt;

        public MainWindow()
        {
            InitializeComponent();
            FirstButton.Content = RandomIndex();
            FirstButton.Background = GetColorForCard(int.Parse(FirstButton.Content.ToString()));
            SecondButton.Content = RandomIndex();
            SecondButton.Background = GetColorForCard(int.Parse(SecondButton.Content.ToString()));
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
                    button.Background = GetColorForCard(int.Parse(button.Content.ToString()));
                }
                else
                {
                    lastInt = Convert.ToInt32(button.Content);
                    button.Content = FirstButton.Content;
                    button.Background = GetColorForCard(int.Parse(button.Content.ToString()));
                    FirstButton.Content = RandomIndex();
                    FirstButton.Background = GetColorForCard(int.Parse(FirstButton.Content.ToString()));
                }
            }
        }

        private void stack1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lastInt == 0)
            {

            }
            else if (sender is StackPanel stackPanel)
            {
                TextBlock t = new TextBlock
                {
                    Background = GetColorForCard(lastInt),
                    FontSize = 20,
                    Width = 57,
                    Height = 85,
                    Text = lastInt.ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
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
                Minimized(stackPanel.Children.Count, stackPanel);
            }
        }

        static void Minimized(int i, StackPanel stack)
        {
            for (int j = 0; j < i; j++)
            {
                if (stack.Children[j] is TextBlock element)
                {
                    if (j < i - 1)
                    {
                        element.Height = 31.25;
                    }
                    else if (j == i - 1)
                    {
                        element.Height = 85;
                    }
                }
            }
            SumCards(stack);
        }

        static void SumCards(StackPanel stackPanel)
        {
            if (stackPanel.Children.Count < 2) return;
            else
            {
                while (stackPanel.Children.Count > 1) // Пока есть минимум два элемента
                {
                    var lastChild = stackPanel.Children[stackPanel.Children.Count - 1] as TextBlock;
                    var secondLastChild = stackPanel.Children[stackPanel.Children.Count - 2] as TextBlock;
                    if (lastChild.Text == secondLastChild.Text)
                    {
                        secondLastChild.Height = 85;
                        secondLastChild.Text = (int.Parse(secondLastChild.Text) * 2).ToString();
                        secondLastChild.Background = GetColorForCard(int.Parse(secondLastChild.Text));
                        stackPanel.Children.Remove(lastChild);
                    }
                    else
                    {
                        break; // Прерываем, если элементы не равны
                    }
                }
            }
        }

        //Полностью лаконичное и закоченное пространство, если и дорабатывать то в другой жизни
        static SolidColorBrush GetColorForCard(int i)
        {
            int cash = 0;
            string[] colors = { "#6FEEB0", "#A3D88B", "#FF6B6B", "#FF3E5B", "#EAD2AC", "#9B59B6", "#B9FBC0", "#76E6D5", "#00A8E1", "#5C6BC0", "#F27D4C" };
            for (int j = 2; j != 2024; j *= 2)
            {
                if (i == j)
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[cash]));
                }
                else
                {
                    cash++;
                }
            }
            return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colors[cash]));
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