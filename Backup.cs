/*namespace VACalculatorApp;

public class Backup
{
foreach (var percentage in _selectedPercentages)
{
    var percentFrame = new Frame
    {
        BackgroundColor = Color.FromArgb("#E6F2FF"),
        Padding = new Thickness(10, 5),
        CornerRadius = 5,
        BorderColor = Color.FromArgb("#0078D7"),
        Margin = new Thickness(0, 0, 8, 8)
    };
        
    var layout = new HorizontalStackLayout();
        
    var labelPercent = new Label
    {
        Text = $"{percentage}%",
        FontSize = 16,
        TextColor = Colors.Black,
        FontAttributes = FontAttributes.Bold,
        VerticalOptions = LayoutOptions.Center
    };
        
    var percentSelectedButton = new Button
    {
        Text = "×",
        FontSize = 14,
        FontAttributes = FontAttributes.Bold,
        BackgroundColor = Color.FromArgb("#FF5252"),
        TextColor = Colors.White,
        Margin = new Thickness(5, 0, 0, 0),
        HeightRequest = 20,
        WidthRequest = 20,
        CornerRadius = 10,
        Padding = new Thickness(0),  
        LineBreakMode = LineBreakMode.NoWrap,  
        VerticalOptions = LayoutOptions.Center, 
        HorizontalOptions = LayoutOptions.Center, 
        CommandParameter = percentage

    };
    percentSelectedButton.Clicked += RemovePercentage_Clicked;
        
    layout.Children.Add(labelPercent);
    layout.Children.Add(percentSelectedButton);
    percentFrame.Content = layout;
        
    SfChipGroupPercentContainer.Children.Add(percentFrame);
}
}*/