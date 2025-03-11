using BAITIEULUAN.DataContext;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BAITIEULUAN
{
    /// <summary>
    /// Interaction logic for EditWork.xaml
    /// </summary>
    public partial class EditWork : Window
    {
        public ObservableCollection<TaskItem> Tasks { get; set; }
        public EditWork(string userId)
        {
            InitializeComponent();
            Uid = userId;
            LoadData();
        }

        private void LoadData()
        {
            Tasks = new ObservableCollection<TaskItem>();
            var table = DataProvider.Instance.ExecuteQuery("SELECT * FROM WORK");
            foreach (DataRow row in table.Rows)
            {
                var item = new TaskItem
                {
                    TaskName = row["Name"].ToString(),
                    StartDate = DateTime.Parse(row["Starting"].ToString()),
                    EndDate = DateTime.Parse(row["DeadLine"].ToString()),
                };
                Tasks.Add(item);
            }
            lvTasks.ItemsSource = Tasks;
            dpStartDate.SelectedDate = DateTime.Now;
            dpEndDate.SelectedDate = DateTime.Now;
        }

        private void lvTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvTasks.SelectedItem is TaskItem selectedTask)
            {
                txtTaskName.Text = selectedTask.TaskName;
                dpStartDate.SelectedDate = selectedTask.StartDate;
                dpEndDate.SelectedDate = selectedTask.EndDate;
            }
        }

        private void btnAddtask_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTaskName.Text) && dpStartDate.SelectedDate.HasValue && dpEndDate.SelectedDate.HasValue)
            {
                Tasks.Add(new TaskItem
                {
                    TaskName = txtTaskName.Text,
                    StartDate = dpStartDate.SelectedDate.Value,
                    EndDate = dpEndDate.SelectedDate.Value
                });
                DataProvider.Instance.ExecuteNonQuery($@"INSERT INTO Work VALUES (@Name, @Start, @End, 0, @UserId)", new object[] { txtTaskName.Text, dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value, Convert.ToInt32(Uid) });
                MessageBox.Show("Thêm công việc thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearInputFields();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnUpdatetask_Click(object sender, RoutedEventArgs e)
        {
            if (lvTasks.SelectedItem is TaskItem selectedTask)
            {
                if (!string.IsNullOrWhiteSpace(txtTaskName.Text) && dpStartDate.SelectedDate.HasValue && dpEndDate.SelectedDate.HasValue)
                {
                    selectedTask.TaskName = txtTaskName.Text;
                    selectedTask.StartDate = dpStartDate.SelectedDate.Value;
                    selectedTask.EndDate = dpEndDate.SelectedDate.Value;
                    DataProvider.Instance.ExecuteNonQuery($@"UPDATE Work SET Name = @Name, Starting = @Start, DeadLine = @End WHERE Name = @OldName", new object[] { txtTaskName.Text, dpStartDate.SelectedDate.Value, dpEndDate.SelectedDate.Value, selectedTask.TaskName });
                    MessageBox.Show("Sửa công việc thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn công việc cần sửa.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ClearInputFields()
        {
            txtTaskName.Clear();
            dpStartDate.SelectedDate = DateTime.Now;
            dpEndDate.SelectedDate = DateTime.Now;
        }

    }

    public class TaskItem : INotifyPropertyChanged
    {
        private string taskName;
        public string TaskName
        {
            get => taskName;
            set { taskName = value; OnPropertyChanged(nameof(TaskName)); }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set { startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
            set { endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
