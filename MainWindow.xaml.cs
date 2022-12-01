using System.Windows;
using System;
using Microsoft.Win32;
using System.Threading.Tasks;
using PDFEditor;

namespace WPFEstraiFogliPDF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            //display initial infos to the user
            tbkInfo.Text = "The software look in every page of the origin file. " +
                           "For each page look if any of the text in \"Text to look for\" is in the text of the page. " +
                           "If any of the text is in the page it copies the page to the destination file.";

            base.OnInitialized(e);
        }

        /// <summary>
        /// Shows file dialog and returns the selected file.
        /// </summary>
        /// <param name="filter">Filter to apply to the dialog. <see cref="FileDialog.Filter"/></param>
        /// <returns>Selected path.</returns>
        private string BrowseFiles(string filter)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = filter;
            fileDialog.ShowDialog();
            return fileDialog.FileName;
        }

        /// <summary>
        /// Export action.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnExport_Click(object sender, RoutedEventArgs e)
        {
            //grab variables form the UI
            var originPath = tbxOrigin.Text;
            var destinationPath = tbxDestination.Text;
            var textPath = tbxText.Text;

            //notify extraction in progress and disables the UI
            tbkInfo.Text = "Extraction in progress...";
            IsEnabled= false;

            try
            {
                //extract the files on a different thread.
                await Task.Run(() => PDFEdit.ExtractPages(new Uri(originPath), new Uri(destinationPath), new Uri(textPath)));

                //notify the successful extraction
                tbkInfo.Text = "Extraction succeed.";
            }
            catch(Exception ex)  
            {
                //notify errors
                tbkInfo.Text = ex.Message;
            }
            finally
            {
                //renable the interface
                IsEnabled = true;
            }
             
        }

        /// <summary>
        /// Open dilog for browsing the origin .pdf file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseOrigin_Click(object sender, RoutedEventArgs e)
        {
            tbxOrigin.Text = BrowseFiles("PDF file (*.pdf)|*.pdf");
        }

        /// <summary>
        /// Open dilog for browsing the file .txt with the text to select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseText_Click(object sender, RoutedEventArgs e)
        {
            tbxText.Text = BrowseFiles("TXT file (*.txt)|*.txt");
        }

        /// <summary>
        /// Open dilog for browsing the destination .pdf file path.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowseDestiantion_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog() 
            { 
                Filter = "PDF file (*.pdf)|*.pdf",
                CheckFileExists = false,
                Multiselect = false,
                AddExtension = true
            };

            tbxDestination.Text = fileDialog.ShowDialog() ?? false ? fileDialog.FileName : string.Empty;
             
        }
    }
}
