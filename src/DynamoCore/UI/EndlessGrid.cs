using Dynamo.Controls;
using Dynamo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Dynamo.Controls
{
    public class EndlessGrid : Canvas
    {
        private ItemsControl itemsControl;

        public EndlessGrid()
        {
            this.RenderTransform = new TranslateTransform();
            this.Loaded += EndlessGrid_Loaded;
        }

        /// <summary>
        /// Create EndlessGrid component that contains ItemsControl for binding
        /// of GridLine from ViewModel
        /// </summary>
        void EndlessGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Create ItemsControl in Canvas to bind the grid line onto it
            this.itemsControl = new ItemsControl();
            this.Children.Add(itemsControl);

            // Set Item Panel to Canvas instead of StackPanel
            FrameworkElementFactory factoryPanel = new FrameworkElementFactory(typeof(Canvas));
            factoryPanel.SetValue(StackPanel.IsItemsHostProperty, true);
            ItemsPanelTemplate template = new ItemsPanelTemplate();
            template.VisualTree = factoryPanel;
            itemsControl.ItemsPanel = template;

            // Background Transparent in order to catch mouse event
            this.Background = Brushes.Transparent;

            // Call ViewModel to create content for Grid Lines
            ((EndlessGridViewModel)this.DataContext).RunCommand.Execute(null);

            CreateBinding();
        }

        private void CreateBinding()
        {
            // Visibility Binding
            this.itemsControl.SetBinding(FrameworkElement.VisibilityProperty, new Binding("FullscreenWatchShowing")
            {
                Converter = new InverseBoolToVisibilityConverter(),
                Mode = BindingMode.OneWay
            });

            // Size Binding
            this.SetBinding(EndlessGrid.WidthProperty, new Binding("Width")
            {
                Mode = BindingMode.OneWay
            });
            this.SetBinding(EndlessGrid.HeightProperty, new Binding("Height")
            {
                Mode = BindingMode.OneWay
            });

            // GridLine binds to ItemsControl
            this.itemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("GridLines")
            {
                Mode = BindingMode.OneWay
            });

            // TranslationTransform
            this.SetBinding(EndlessGrid.RenderTransformProperty, new Binding("Transform")
            {
                Mode = BindingMode.OneWay
            });
        }
    }
}
