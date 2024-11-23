using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;

namespace t2._048_android
{
    public partial class MainPage : ContentPage
    {
        private int lastInt;
        private static int Score;
        private static int cash_score;
        private Button? activeButton;
        private readonly int[] integers = { 2, 4, 8, 16 };
        private readonly Random random = new Random();
        private List<VerticalStackLayout> columns;
        private VerticalStackLayout? selectedColumn;
        private const int MAX_CARDS_PER_COLUMN = 9;

        private const int GAME_TIME_SECONDS = 300; // 5 minutes
        private IDispatcherTimer gameTimer;
        private int remainingSeconds;

        public MainPage()
        {
            InitializeComponent();

            // Адаптация под тач-интерфейс
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += ButtonClicked;
            FirstButton.GestureRecognizers.Add(tapGesture);
            SecondButton.GestureRecognizers.Add(tapGesture);

            columns = new List<VerticalStackLayout> 
            { 
                Column1Stack, 
                Column2Stack, 
                Column3Stack, 
                Column4Stack
            };

            foreach (var column in columns)
            {
                var collumTapGesture = new TapGestureRecognizer();
                collumTapGesture.Tapped += ColumnTapped;
                column.GestureRecognizers.Add(collumTapGesture);
            }

            InitializeTimer();
            StartNewGame();
        }

        private void InitializeTimer()
        {
            gameTimer = Application.Current.Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += Timer_Tick;
        }

        private void StartNewGame()
        {
            remainingSeconds = GAME_TIME_SECONDS;
            UpdateTimerDisplay();
            gameTimer.Start();

            // Reset other game elements
            Score = 0;
            score.Text = "0";
            UpdateInitialButtons();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            remainingSeconds--;
            UpdateTimerDisplay();

            if (remainingSeconds <= 0)
            {
                gameTimer.Stop();
                await DisplayAlert("Время вышло!", $"Вы не успели собрать 2048.\nВаш счет: {Score}", "Новая игра");
                ResetGame();
            }
        }

        private void UpdateTimerDisplay()
        {
            var minutes = remainingSeconds / 60;
            var seconds = remainingSeconds % 60;
            timerLabel.Text = $"{minutes:00}:{seconds:00}";
        }

        private int RandomIndex()
        {
            int t = random.Next(0, 101); // 0-100

            if (t > 60)
                return integers[0];      // 2
            else if (t > 30)
                return integers[1];      // 4
            else if (t > 10)
                return integers[2];      // 8
            else
                return integers[3];      // 16
        }

        private void UpdateInitialButtons()
        {
            FirstButton.Text = RandomIndex().ToString();
            FirstButton.BackgroundColor = GetColorForCard(int.Parse(FirstButton.Text));

            SecondButton.Text = RandomIndex().ToString();
            SecondButton.BackgroundColor = GetColorForCard(int.Parse(SecondButton.Text));
        }

        private async Task MergeCards(VerticalStackLayout stack)
        {
            if (stack.Children.Count < 2) return;

            while (stack.Children.Count > 1)
            {
                var lastCard = stack.Children[^1] as Border;
                var prevCard = stack.Children[^2] as Border;

                if (lastCard?.Content is Label lastLabel && 
                    prevCard?.Content is Label prevLabel && 
                    lastLabel.Text == prevLabel.Text)
                {
                    int newValue = int.Parse(prevLabel.Text) * 2;
                    prevLabel.Text = newValue.ToString();
                    prevCard.BackgroundColor = GetColorForCard(newValue);
                    
                    stack.Children.Remove(lastCard);
                    
                    // Проверка победы
                    if (newValue == 2048)
                    {
                        gameTimer.Stop();
                        await DisplayAlert("Поздравляем!", $"Вы собрали 2048!\nВаш счет: {Score}", "Новая игра");
                        ResetGame();
                        return;
                    }
                }
                else break;
            }
        }

        private Color GetColorForCard(int value)
        {
            string[] colors = { "#2350FC", "#91FC23", "#6C23FC", "#EADD00", 
                               "#6D4FA7", "#FC4623", "#00BFFF", "#FCC223", 
                               "#4F63A7", "#50727D", "#0000FF" };
            
            int index = 0;
            for (int j = 2; j <= 2024; j *= 2)
            {
                if (value == j && index < colors.Length)
                    return Color.FromArgb(colors[index]);
                index++;
            }
            return Color.FromArgb(colors[^1]);
        }

        private void ColumnTapped(object? sender, TappedEventArgs e)
        {
            if (sender is VerticalStackLayout tappedColumn)
            {
                var parentBorder = tappedColumn.Parent as Border;

                // Перезапускаем прошлый столбец
                if (selectedColumn?.Parent is Border previousBorder)
                {
                    previousBorder.BackgroundColor = Colors.Transparent;
                }

                selectedColumn = tappedColumn;

                // Подсвечиваем выбранный столбец
                if (parentBorder != null)
                {
                    parentBorder.BackgroundColor = Color.FromArgb("#303030");
                }
            }
        }

        private void ClearColumnSelection()
        {
            if (selectedColumn?.Parent is Border previousBorder)
            {
                previousBorder.BackgroundColor = Colors.Transparent;
            }
            selectedColumn = null;
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {
            if (selectedColumn == null)
            {
                await DisplayAlert("Внимание", "Выберите столбец для размещения карточки", "OK");
                return;
            }

            if (columns.All(col => col.Children.Count >= MAX_CARDS_PER_COLUMN))
            {
                await DisplayAlert("Игра окончена", "Все столбцы заполнены", "OK");
                ResetGame();
                return;
            }
            else if (selectedColumn.Children.Count >= MAX_CARDS_PER_COLUMN)
            {
                await DisplayAlert("Внимание", "Этот столбец заполнен. Выберите другой столбец", "OK");
                ClearColumnSelection();
                return;
            }
            

            if (sender is Button button)
            {
                activeButton = button;
                cash_score = int.Parse(button.Text);
                lastInt = cash_score;

                // Анимация кнопки
                button.TranslationY = -20;
                await button.TranslateTo(button.TranslationX, 0, 200, Easing.CubicOut);

                var border = new Border
                {
                    Content = new Label 
                    { 
                        Text = lastInt.ToString(),
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White,
                        FontSize = 20
                    },
                    BackgroundColor = GetColorForCard(lastInt),
                    HeightRequest = 85,
                    WidthRequest = 57,
                    StrokeShape = new RoundRectangle { CornerRadius = 5 },
                    Stroke = Colors.Black,
                    StrokeThickness = 1
                };

                selectedColumn.Children.Add(border);
                
                // Обновление очков
                Score += cash_score;
                score.Text = Score.ToString();

                // Минимизация и слияние карточек
                await MergeCards(selectedColumn);
                MinimizeCards(selectedColumn);

                // Обновление кнопки
                button.Text = RandomIndex().ToString();
                button.BackgroundColor = GetColorForCard(int.Parse(button.Text));
            }
        }

        private void MinimizeCards(VerticalStackLayout stack)
        {
            int count = stack.Children.Count;
            
            // Если карта одна - оставляем полный размер
            if (count == 1)
            {
                if (stack.Children[0] is Border border)
                {
                    border.HeightRequest = 85;
                    border.Margin = new Thickness(0);
                    border.StrokeShape = new RoundRectangle { CornerRadius = 5 };
                }
                return;
            }

            // Обрабатываем множество карт
            for (int i = 0; i < count; i++)
            {
                if (stack.Children[i] is Border border)
                {
                    // Отступы
                    border.Margin = i == 0 
                        ? new Thickness(0) 
                        : new Thickness(0, -4, 0, 0);

                    // Размеры и скругления
                    if (i < count - 1)
                    {
                        border.HeightRequest = 31.25;
                        border.StrokeShape = new RoundRectangle 
                        { 
                            CornerRadius = new CornerRadius(5, 5, 0, 0) 
                        };
                    }
                    else
                    {
                        border.HeightRequest = 85;
                        border.StrokeShape = new RoundRectangle { CornerRadius = 5 };
                    }
                }
            }
        }

        private void FirstButton_Clicked(object sender, EventArgs e)
        {
            ButtonClicked(e, new TappedEventArgs(FirstButton));
        }

        private void SecondButton_Clicked(object sender, EventArgs e)
        {
            ButtonClicked(e, new TappedEventArgs(SecondButton));
        }

        private void ResetGame()
        {
            gameTimer.Stop();
            StartNewGame();

            foreach (var column in columns)
            {
                column.Children.Clear();
            }
            ClearColumnSelection();
        }
    }

}
