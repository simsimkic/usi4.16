using System.Windows;

namespace ZdravoCorp.ViewModels.DoctorWindows
{
    public partial class DoctorsRoomWindow : Window
    {

        public Room Room { get; private set; }

        public DoctorsRoomWindow()
        {
            InitializeComponent();
            Room = new Room("", 0);
            foreach (Room room in RoomRepository.Instance.Rooms)
            {
                if (room.Type == RoomType.ExaminatingRoom) RoomsComboBox.Items.Add(room);
            }
        }

        private void FormSubmitted(object sender, RoutedEventArgs e)
        {
            if (RoomsComboBox.SelectedItem != null) Room = (Room) RoomsComboBox.SelectedItem;
            DialogResult = true;
        }
    }
}
