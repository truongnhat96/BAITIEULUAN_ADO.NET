using BAITIEULUAN.DataContext;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;

namespace BAITIEULUAN
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public ObservableCollection<WorkItem> Tasks { get; set; }
        public Home(string userId)
        {
            InitializeComponent();
            Uid = userId;
            LoadData();
        }

        private void LoadData()
        {
            Tasks = new ObservableCollection<WorkItem>();
            var table = DataProvider.Instance.ExecuteQuery("SELECT * FROM WORK");
            foreach (DataRow row in table.Rows)
            {
                var item = new WorkItem
                {
                    TaskName = row["Name"].ToString(),
                    StartDate = ((DateTime)row["Starting"]).ToShortDateString(),
                    EndDate = ((DateTime)row["DeadLine"]).ToShortDateString(),
                    IsCompleted = (bool)row["Complete"]
                };
                Tasks.Add(item);
            }
            lvTasks.ItemsSource = Tasks;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditWork work = new EditWork(Uid);
            work.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if(lvTasks.SelectedItem is WorkItem selectedTask)
            {
                // Ví dụ xóa
                DataProvider.Instance.ExecuteNonQuery("DELETE FROM WORK WHERE Name = @Name", new object[] { selectedTask.TaskName });
                MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                Tasks.Remove(selectedTask);
                lvTasks.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn công việc cần xóa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvTasks.SelectedItem is WorkItem selectedTask)
            {
                // Ví dụ cập nhật: đảo trạng thái hiện tại
                DataProvider.Instance.ExecuteNonQuery("UPDATE WORK SET Complete = @Complete WHERE Name = @Name", new object[] { selectedTask.IsCompleted, selectedTask.TaskName });
                MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                lvTasks.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn công việc cần cập nhật", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }

    public class WorkItem
    {
        public string TaskName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
