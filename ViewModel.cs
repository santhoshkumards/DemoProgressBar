using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Threading;

namespace MVVMDemo
{
    public class ViewModel : ViewModelBase
    {
        private Student _student;
        private ObservableCollection<Student> _students;
        private ICommand _SubmitCommand;

        public Student Student
        {
            get
            {
                return _student;
            }
            set
            {
                _student = value;
                NotifyPropertyChanged("Student");
            }
        }
        public ObservableCollection<Student> Students
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
                NotifyPropertyChanged("Students");
            }
        }

        public ICommand SubmitCommand
        {
            get
            {
                if (_SubmitCommand == null)
                {
                    _SubmitCommand = new RelayCommand(param => this.Submit(),
                        null);
                }
                return _SubmitCommand;
            }
        }

        public ViewModel()
        {
            Student = new Student();
            Students = new ObservableCollection<Student>();
            Students.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Students_CollectionChanged);
        }

        void Students_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Students");
        }

        private void Submit()
        {
            PctComplete = 0.0;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += (s, ea) =>
            {
                PctComplete += 1.0;
                if (PctComplete >= 100.0)
                    timer.Stop();
            };
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);  // 2/sec
            timer.Start();
            BindStudent();
        }
        private void BindStudent()
        {
            Student.JoiningDate = DateTime.Today.Date;
            Students.Add(Student);
            Student = new Student();
        }

        private double pctComplete = 0.0;
        public double PctComplete
        {
            get { return pctComplete; }
            set
            {
                if (pctComplete != value)
                {
                    pctComplete = value;
                    OnPropertyChanged("PctComplete");
                }
            }
        }
    }
}
