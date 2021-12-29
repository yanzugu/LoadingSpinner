# LoadingSpinner

![2021-12-29 19-16-36](https://user-images.githubusercontent.com/58894500/147657422-46962cd0-6f39-4ec8-86c5-8c3dca6d6b9b.gif)

Example Code

```
<Window x:Class="WpfApp1.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:spinner="clr-namespace:LoadingSpinner.Src;assembly=LoadingSpinner"
        xmlns:local="clr-namespace:WpfApp1"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Window.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ResourceDictionary Source="/LoadingSpinner;component/Src/Styles/SpinStyle.xaml"/>
            </ResourceDictionary.MergedDictionaries>

            <LinearGradientBrush x:Key="LinearGradientBrush1">
                <GradientStop Color="Aqua" Offset="0"/>
                <GradientStop Color="Violet" Offset="1"/>
            </LinearGradientBrush>
        </ResourceDictionary>
    </Window.Resources>
    
    <StackPanel Orientation="Horizontal" VerticalAlignment="Center" HorizontalAlignment="Center">
        <spinner:CircularProgressBar SegmentColor="{StaticResource LinearGradientBrush1}"
                                     IsShowPercentage="True"
                                     Margin="8">
            <spinner:CircularProgressBar.Triggers>
                <EventTrigger RoutedEvent="Loaded">
                    <BeginStoryboard>
                        <Storyboard Storyboard.TargetProperty="Percentage">
                            <DoubleAnimation From="0" To="100" Duration="0:0:10"
                                             RepeatBehavior="Forever"/>
                        </Storyboard>
                    </BeginStoryboard>
                </EventTrigger>
            </spinner:CircularProgressBar.Triggers>
        </spinner:CircularProgressBar>
        <spinner:CircularProgressBar SpinStyle="{StaticResource SpinStyle1}"
                                     SegmentColor="{StaticResource LinearGradientBrush1}"
                                     Margin="8"/>
        <spinner:CircularProgressBar SpinStyle="{StaticResource SpinStyle2}"
                                     SegmentColor="{StaticResource LinearGradientBrush1}"
                                     Margin="8"/>
        <spinner:CircularProgressBar SpinStyle="{StaticResource SpinStyle3}"
                                     SegmentColor="{StaticResource LinearGradientBrush1}"
                                     Margin="8"/>
        <spinner:CircularProgressBar SpinStyle="{StaticResource SpinStyle4}"
                                     SegmentColor="{StaticResource LinearGradientBrush1}"
                                     Margin="8"/>
    </StackPanel>
</Window>

```
