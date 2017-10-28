using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using osnE.Interop;
namespace osnE
{
	/// <summary>
	/// Interaction logic for ListItem.xaml
	/// </summary>
	public partial class ListItem : Window
	{
        int sortOrder=0;
        string optionText = "";
        
        public ListItem(int sortOrder)
        {
            this.InitializeComponent();
            this.txtSearchString.Text = "";
            this.sortOrder = sortOrder;
            this.Visibility = System.Windows.Visibility.Hidden ;
        }

        public void SetTextAndShow(List<Inline> text)
        {
            //this.optionText = text;
            this.txtSearchString.Text = "";
            this.txtSearchString.Inlines.Clear();
            if (text!=null && text.Count > 0)
                this.txtSearchString.Inlines.AddRange(text);

            this.optionText = this.txtSearchString.Text;
            this.Show();
            this.Visibility = System.Windows.Visibility.Visible;
        }
        public void SetTextAndShow(string text)
        {
            this.optionText = text;
            this.txtSearchString.Text = text;
            this.Show();
            this.Visibility = System.Windows.Visibility.Visible;
        }
        public void ClearTextAndHide()
        {
            this.optionText = "";
            this.txtSearchString.Text = "";
            this.txtSearchString.Inlines.Clear();
            this.Visibility = System.Windows.Visibility.Hidden;
        }
        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            this.Top = this.sortOrder * this.ActualHeight;
        }
        
	}
}