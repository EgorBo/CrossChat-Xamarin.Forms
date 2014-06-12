using System;
using System.Reflection;
using Crosschat.Client.iOS.CustomRenderers;
using Crosschat.Client.Views.Controls;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ChatListView), typeof(ChatListViewRenderer))]

namespace Crosschat.Client.iOS.CustomRenderers
{
    /// <summary>
    /// Thanks to MartinBooth for the example how to create DynamicRowHeightListViewRenderer
    /// http://forums.xamarin.com/discussion/comment/56693#Comment_56693
    /// </summary>
    public class ChatListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            var table = (UITableView)this.Control;
            table.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            table.Source = new ListViewDataSourceWrapper(this.GetFieldValue<UITableViewSource>(typeof(ListViewRenderer), "dataSource"));
        }
    }

    public class ListViewDataSourceWrapper : UITableViewSource
    {
        private readonly UITableViewSource _underlyingTableSource;

        public ListViewDataSourceWrapper(UITableViewSource underlyingTableSource)
        {
            this._underlyingTableSource = underlyingTableSource;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            return this.GetCellInternal(tableView, indexPath);
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return this._underlyingTableSource.RowsInSection(tableview, section);
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return this._underlyingTableSource.GetHeightForHeader(tableView, section);
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {
            return this._underlyingTableSource.GetViewForHeader(tableView, section);
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return this._underlyingTableSource.NumberOfSections(tableView);
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            this._underlyingTableSource.RowSelected(tableView, indexPath);
        }

        public override string[] SectionIndexTitles(UITableView tableView)
        {
            return this._underlyingTableSource.SectionIndexTitles(tableView);
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            return this._underlyingTableSource.TitleForHeader(tableView, section);
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var uiCell = (BubbleCell)GetCellInternal(tableView, indexPath);

            uiCell.SetNeedsLayout();
            uiCell.LayoutIfNeeded();

            return uiCell.GetHeight(tableView);
        }

        private UITableViewCell GetCellInternal(UITableView tableView, NSIndexPath indexPath)
        {
            return this._underlyingTableSource.GetCell(tableView, indexPath);
        }
    }

    public static class PrivateExtensions
    {
        public static T GetFieldValue<T>(this object @this, Type type, string name)
        {
            var field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField);
            return (T)field.GetValue(@this);
        }

        public static T GetPropertyValue<T>(this object @this, Type type, string name)
        {
            var property = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            return (T)property.GetValue(@this);
        }
    }
}