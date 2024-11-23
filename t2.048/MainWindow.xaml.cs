using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;


namespace t2._048
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int lastInt;

        static int Score;
        static int cash_score;


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
                cash_score = Convert.ToInt32(button.Content);
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
                return;
            }
            else if (sender is StackPanel stackPanel)
            {
                Border roundedBorder = new Border
                {
                    CornerRadius = new CornerRadius(5),
                    Background = GetColorForCard(lastInt),
                    Width = 57,
                    Height = 85,
                };


                TextBlock t = new TextBlock
                {
                    FontSize = 20,
                    Text = lastInt.ToString(),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Foreground = Brushes.White
                };

                roundedBorder.Child = t;

                // Сбрасываем lastInt и добавляем элемент
                lastInt = 0;

                foreach (var child in stackPanel.Children)
                {
                    if (child is FrameworkElement element)
                    {
                        // Удаляем старые элементы, если они есть
                        if (element.Name == "h1" || element.Name == "h2" || element.Name == "h3" || element.Name == "h4")
                        {
                            stackPanel.Children.Clear();
                            break;
                        }
                    }
                }

                Score += cash_score;
                score.Text = Score.ToString();
                stackPanel.Children.Add(roundedBorder);

                // Обновляем размеры элементов
                Minimized(stackPanel.Children.Count, stackPanel, score.Text);
            }
        }

        static void Minimized(int count, StackPanel stack, string score)
        {
            for (int i = 0; i < count; i++)
            {
                if (stack.Children[i] is Border border)
                {
                    if (i < count - 1)
                    {
                        border.Height = 31.25;
                        border.CornerRadius = new CornerRadius(5, 5, 0, 0);
                    }
                    else if (i == count - 1)
                    {
                        border.Height = 85;

                    }
                }
            }
            SumCards(stack);
        }

        static void SumCards(StackPanel stackPanel)
        {
            if (stackPanel.Children.Count < 2) return;

            while (stackPanel.Children.Count > 1) // Пока есть минимум два элемента
            {
                var lastChild = stackPanel.Children[stackPanel.Children.Count - 1] as Border;
                var secondLastChild = stackPanel.Children[stackPanel.Children.Count - 2] as Border;

                if (lastChild.Child is TextBlock lastTextBlock &&
                    secondLastChild.Child is TextBlock secondLastTextBlock &&
                    lastTextBlock.Text == secondLastTextBlock.Text)
                {
                    // Удваиваем значение текста во втором последнем элементе
                    secondLastTextBlock.Text = (int.Parse(secondLastTextBlock.Text) * 2).ToString();

                    // Применяем новые параметры к второму последнему элементу
                    secondLastChild.Background = GetColorForCard(int.Parse(secondLastTextBlock.Text));
                    secondLastChild.Height = 85; // Устанавливаем высоту на 85 после слияния
                    secondLastChild.CornerRadius = new CornerRadius(5);

                    // Удаляем последний элемент
                    stackPanel.Children.Remove(lastChild);
                }
                else
                {
                    break; // Прерываем, если элементы не равны
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