using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;

namespace t2._048_android
{
    public partial class MainPage : ContentPage
    {
        private readonly int[] integers = { 2, 4, 8, 16 };
        private readonly Random random = new Random();
        private List<VerticalStackLayout> columns;
        private VerticalStackLayout? selectedColumn;

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

            UpdateInitialButtons();
        }

        private void UpdateInitialButtons()
        {
            FirstButton.Text = RandomIndex().ToString();
            FirstButton.BackgroundColor = GetColorForCard(int.Parse(FirstButton.Text));
            FirstButton.CornerRadius = 5;

            SecondButton.Text = RandomIndex().ToString();
            SecondButton.BackgroundColor = GetColorForCard(int.Parse(SecondButton.Text));
            SecondButton.CornerRadius = 5;
        }

        private int RandomIndex()
        {
            int t = random.Next(0, 101); // 0-100
            
            if (t > 60)
                return integers[0];
            else if (t > 30)
                return integers[1];
            else if (t > 10)
                return integers[2];
            else
                return integers[3];
        }

        private void MergeCards(VerticalStackLayout stackLayout)
        {
            while (stackLayout.Children.Count > 1)
            {
                var lastChild = stackLayout.Children[^1] as Border;
                var secondLastChild = stackLayout.Children[^2] as Border;

                if (lastChild?.Content is Label lastLabel &&
                    secondLastChild?.Content is Label secondLastLabel &&
                    lastLabel.Text == secondLastLabel.Text)
                {
                    // Double the value
                    int newValue = int.Parse(secondLastLabel.Text) * 2;
                    secondLastLabel.Text = newValue.ToString();

                    // Update styling
                    secondLastChild.BackgroundColor = GetColorForCard(newValue);
                    secondLastChild.HeightRequest = 85;
                    secondLastChild.StrokeShape = new RoundRectangle 
                    { 
                        CornerRadius = new CornerRadius(5) 
                    };

                    // Remove merged card
                    stackLayout.Children.Remove(lastChild);
                }
                else
                {
                    break;
                }
            }
        }

        private Color GetColorForCard(int value)
        {
            string[] colors = { "#6FEEB0", "#A3D88B", "#FF6B6B", "#FF3E5B", "#EAD2AC", 
                               "#9B59B6", "#B9FBC0", "#76E6D5", "#00A8E1", "#5C6BC0", "#F27D4C" };
            
            int index = 0;
            for (int j = 2; j <= 2024; j *= 2)
            {
                if (value == j && index < colors.Length)
                    return Color.FromArgb(colors[index]);
                index++;
            }
            return Colors.Gray;
        }

        private void ColumnTapped(object? sender, TappedEventArgs e)
        {
            if (sender is VerticalStackLayout tappedColumn)
            {
                var parentBorder = tappedColumn.Parent as Border;

                // Reset previous selection
                if (selectedColumn?.Parent is Border previousBorder)
                {
                    previousBorder.BackgroundColor = Colors.Transparent;
                }

                selectedColumn = tappedColumn;

                // Highlight new selection
                if (parentBorder != null)
                {
                    parentBorder.BackgroundColor = Color.FromArgb("#303030");
                }
            }
        }

         private void ButtonClicked(object sender, EventArgs e)
        {
            if (selectedColumn == null)
            {
                DisplayAlert("Внимание", "Выберите столбец для размещения карточки", "OK");
                return;
            }
            
            if (sender is Button button)
            {
                var border = new Border
                {
                    Content = new Label 
                    { 
                        Text = button.Text,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.White
                    },
                    BackgroundColor = GetColorForCard(int.Parse(button.Text)),
                    HeightRequest = 85,
                    WidthRequest = 55,
                    Margin = new Thickness(2),
                    StrokeShape = new RoundRectangle 
                    { 
                        CornerRadius = new CornerRadius(5)
                    }
                };
        
                // Добавляем только в выбранный столбец
                selectedColumn.Children.Add(border);
                MergeCards(selectedColumn);
        
                // Обновляем кнопку
                button.Text = RandomIndex().ToString();
                button.BackgroundColor = GetColorForCard(int.Parse(button.Text));
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
    }

}
