using System;

namespace DesktopApp.Service
{
    public class MessagingService
    {
        private static readonly MessagingService instance = new MessagingService();

        public static MessagingService Instance => instance;

        public event Action<int> CourseSelected;
        public event Action<int> GroupSelected;
        public event Action<int> StudentSelected;

        public String LastCalledMethod { get; private set; }

        public void ChangeSelectedCourse(int category)
        {
            CourseSelected?.Invoke(category);

            LastCalledMethod = "ChangeSelectedCourse";
            
        }

        public void ChangeSelectedGroup(int category)
        {
            GroupSelected?.Invoke(category);
            LastCalledMethod = "ChangeSelectedGroup";
        }

        public void ChangeSelectedStudent(int category)
        {
            StudentSelected?.Invoke(category);
            LastCalledMethod = "ChangeSelectedStudent";
        }

        public void DeselectAll()
        {
            if (LastCalledMethod != null)
            {
                switch (LastCalledMethod)
                {
                    case "ChangeSelectedCourse":
                        CourseSelected?.Invoke(-1);
                        break;
                    case "ChangeSelectedGroup":
                        GroupSelected?.Invoke(-1);
                        break;
                    case "ChangeSelectedStudent":
                        StudentSelected?.Invoke(-1);
                        break;
                    default:
                       
                        break;
                }
            }
        }
    }
}